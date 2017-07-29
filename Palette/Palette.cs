using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SeeSharp.Palette
{
    /// <summary>
    ///     Block palette
    /// </summary>
    /// <remarks>
    ///     The main palette class.  This contains the block colour palette modified for biome colours.
    /// </remarks>
    public sealed class BlockPalette
    {
        public const int BlocksMax = 4096;
        public const int BiomesMax = 256;
        public const int MetadataMax = 16;

        internal class PaletteExecutionException : Exception
        {
            public PaletteExecutionException() { }
            public PaletteExecutionException(string Message) : base(Message) { }
            public PaletteExecutionException(string Message, Exception InnerException) : base(Message, InnerException) { }
        }

        private readonly Dictionary<int, List<PaletteEntry>> _PaletteEntries = new Dictionary<int, List<PaletteEntry>>();
        private readonly Dictionary<int, List<TintEntry>> _BiomeTints = new Dictionary<int, List<TintEntry>>();

        private readonly List<JsonPaletteFile> LoadedPalettes = new List<JsonPaletteFile>();

        // *** Lookup tables, for faster rendering
        /// <summary>
        ///     Depth opacity table for block/metadata.  Supports extended (0-4095) block IDs
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         The depth opacity table is used by the top-down standard renderer.  It determines how many blocks down to render from the surface, and is made available to third-party renderers.
        ///     </para>
        ///     <para>
        ///         This table is ordered [Block ID][Metadata ID]
        ///     </para>
        /// </remarks>
        public int[][] DepthOpacities = new int[BlocksMax][]; // *** Will store the *lowest* depth opacity for any given Block ID/Metadata.  The speedup from an array lookup far outweighs the potential cost of extra blocks being rendered.

        /// <summary>
        ///     Block colour values.    Supports extended (0-4095) block IDs
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         The FastPalette table is a jagged array designed for fast lookup of palette data.  It is designed for speed of access.
        ///     </para>
        ///     <para>
        ///         This table is ordered [Biome ID][Block ID][Metadata ID]
        ///     </para>
        /// </remarks>
        public Colour[][][] FastPalette = new Colour[BiomesMax][][];

        public List<PaletteEntry> GetPaletteEntry(Int32 Key)
        {
            if (Key == 0 || Key > _EntityEntries.Count)
                return null;
            return _EntityEntries[Key - 1];
        }

        private readonly List<List<PaletteEntry>> _EntityEntries = new List<List<PaletteEntry>>();

        internal BlockPalette()
        {
            for (int X = 0; X < BiomesMax; X++)
            {
                FastPalette[X] = new Colour[BlocksMax][];
                for (int Y = 0; Y < BlocksMax; Y++)
                    FastPalette[X][Y] = new Colour[MetadataMax];
            }
            for (int X = 0; X < BlocksMax; X++)
                DepthOpacities[X] = new int[MetadataMax];

        }
        internal void AssembleLookupTables()
        {

            // *** Assemble palette & depth opacity lookup arrays
            foreach (int Key in _PaletteEntries.Keys)
            {
                List<PaletteEntry> Entries = _PaletteEntries[Key];
                foreach (PaletteEntry Entry in Entries)
                {
                    for (int X = 0; X < 256; X++)
                    {
                        if (Entry.PaletteEntryType == PaletteEntry.EntryType.IdEntity)
                            if (Entry.Metadata != -1)
                            {
                                if (!FastPalette[X][Key][Entry.Metadata].IsEntityKey())
                                {
                                    _EntityEntries.Add(new List<PaletteEntry>());
                                    FastPalette[X][Key][Entry.Metadata].Color = 0x00FF0000U + (UInt32)(_EntityEntries.Count - 1);
                                }

                                GetPaletteEntry((int)(FastPalette[X][Key][Entry.Metadata].Color & 0xFFFF)).Add(Entry);
                                DepthOpacities[Entry.BlockId][Entry.Metadata] = Entry.DepthOpacity;
                            }
                            else
                                for (int Meta = 0; Meta < 16; Meta++)
                                {
                                    if (!FastPalette[X][Key][Meta].IsEntityKey())
                                    {
                                        _EntityEntries.Add(new List<PaletteEntry>());
                                        FastPalette[X][Key][Meta].Color = 0x00FF0000U + (UInt32)(_EntityEntries.Count - 1);
                                    }

                                    GetPaletteEntry((int)(FastPalette[X][Key][Meta].Color & 0xFFFF)).Add(Entry);
                                    DepthOpacities[Entry.BlockId][Meta] = Entry.DepthOpacity;
                                }
                        else
                        {
                            if (Entry.Metadata != -1)
                            {
                                FastPalette[X][Key][Entry.Metadata] = Entry.Color;
                                DepthOpacities[Entry.BlockId][Entry.Metadata] = Entry.DepthOpacity;
                            }
                            else
                            {
                                for (int Meta = 0; Meta < 16; Meta++)
                                {
                                    if (FastPalette[X][Key][Meta].A == 0)
                                        FastPalette[X][Key][Meta] = Entry.Color;
                                    DepthOpacities[Entry.BlockId][Meta] = Entry.DepthOpacity;
                                }
                            }
                        }
                    }
                }
            }


            // *** Assemble tint lookups
            foreach (int Key in _BiomeTints.Keys)
            {
                List<TintEntry> Entries = _BiomeTints[Key];
                int M = 0;
                foreach (TintEntry Entry in Entries)
                {
                    if (Entry.Metadata > -1)
                    {
                        FastPalette[Key][Entry.BlockId][Entry.Metadata].Blend(Entry.Tint);
                        M |= (1 << Entry.Metadata);
                    }
                    else
                    {
                        for (int X = 0; X < 16; X++)
                            if ((M & (1 << X)) == 0)
                                FastPalette[Key][Entry.BlockId][X].Blend(Entry.Tint);
                    }
                }
            }




        }
        
        /// <summary>
        ///     Load, Tokenize, and Execute a palette script.  If execution is successful, merge the new entries into the block colour palette.
        /// </summary>
        /// <param name="PaletteFile">
        ///     Path to the palette file to load
        /// </param>
        /// <remarks>
        ///     Throws a PaletteExecutionException if the palette script is invalid or malformed
        /// </remarks>
        internal void LoadPalette(String PaletteFile)
        {
            JsonSerializer SerDes = new JsonSerializer();
            JsonPaletteFile Palette = SerDes.Deserialize<JsonPaletteFile>(new JsonTextReader(File.OpenText(PaletteFile)));

            LoadedPalettes.Add(Palette);
        }
    }
}
