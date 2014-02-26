using System;
using System.Collections.Generic;
using System.IO;

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
            public PaletteExecutionException() : base() { }
            public PaletteExecutionException(string message) : base(message) { }
            public PaletteExecutionException(string message, Exception inner) : base(message, inner) { }
        }

        private Dictionary<int, List<PaletteEntry>> PaletteEntries = new Dictionary<int, List<PaletteEntry>>();
        private Dictionary<int, List<TintEntry>> BiomeTints = new Dictionary<int, List<TintEntry>>();

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
        public MiniPaletteEntry[][][] FastPalette = new MiniPaletteEntry[BiomesMax][][];

        internal BlockPalette()
        {
            for (int X = 0; X < BiomesMax; X++)
            {
                FastPalette[X] = new MiniPaletteEntry[BlocksMax][];
                for (int Y = 0; Y < BlocksMax; Y++)
                    FastPalette[X][Y] = new MiniPaletteEntry[MetadataMax];
            }
            for (int X = 0; X < BlocksMax; X++)
                DepthOpacities[X] = new int[MetadataMax];

        }

        internal PaletteToken[] TokenizePaletteFile(string[] PaletteFileLines)
        {
            List<PaletteToken> Tokens = new List<PaletteToken>();
            int LineNumber = 0;
            foreach (String Line in PaletteFileLines)
            {
                LineNumber++;
                String[] Parts = Line.Split(' ');
                if (Line.Length < 1)
                    continue;
                foreach (String Part in Parts)
                {
                    switch (Part[0])
                    {
                        case 'e':   // Various commands
                        case 't':
                        case '.':
                        case '=':
                            if (Part.Length > 1)
                            {
                                Tokens.Add(new PaletteToken(LineNumber, PaletteToken.TokenType.Constant, Part));
                                break;
                            }
                            Tokens.Add(new PaletteToken(LineNumber, PaletteToken.TokenType.Command, Part.Substring(0, 1)));
                            break;
                        case '>':   // Comment command.  Be sure to handle a missing space there!
                            Tokens.Add(new PaletteToken(LineNumber, PaletteToken.TokenType.Command, ">"));
                            if (Part.Length > 1)
                                Tokens.Add(new PaletteToken(LineNumber, PaletteToken.TokenType.Constant, Part.Substring(1)));
                            break;
                        case '#':   // EOL Comment
                            goto NextLine;
                        case '$':   // Variable
                            Tokens.Add(new PaletteToken(LineNumber, PaletteToken.TokenType.Variable, Part));
                            break;
                        default:    // Anything else will just be a constant
                            Tokens.Add(new PaletteToken(LineNumber, PaletteToken.TokenType.Constant, Part));
                            break;
                    }
                }
            NextLine:
                if (Tokens.Count == 0 || Tokens[Tokens.Count - 1].Type != PaletteToken.TokenType.Newline)
                    Tokens.Add(new PaletteToken(LineNumber, PaletteToken.TokenType.Newline));
            }

            return Tokens.ToArray();
        }

        internal string Resolve(Dictionary<string, string> Variables, PaletteToken Token)
        {
            if (Token.Type == PaletteToken.TokenType.Constant)
                return Token.TokenData;
            if (!Variables.ContainsKey(Token.TokenData))
                throw new PaletteExecutionException("Unknown Variable " + Token.TokenData + " at line " + Token.Line.ToString());
            return Variables[Token.TokenData];
        }

        internal int ResolveInteger(PaletteToken Token, Dictionary<string, string> Variables)
        {
            String S = Resolve(Variables, Token);
            return (S == "*" ? -1 : int.Parse(S));
        }

        internal bool IsValidNumericToken(object Data, Dictionary<string, string> Variables)
        {
            if (Data is PaletteToken)
                Data = Resolve(Variables, (PaletteToken)Data);
            if (Data is int)
                return true;
            int t;
            return Data is string && ((string)Data == "*" || int.TryParse((string)Data, out t)); // "*" resolves to -1 in practice, so return that it has a valid numeric value.
        }

        internal bool IsValidStringToken(object Data, Dictionary<string, string> Variables)
        {
            if (Data is PaletteToken)
                Data = Resolve(Variables, (PaletteToken)Data);
            if (Data is string)
                return true;

            return false;
        }

        internal void AssembleLookupTables()
        {

            // *** Assemble palette & depth opacity lookup arrays
            foreach (int Key in PaletteEntries.Keys)
            {
                List<PaletteEntry> Entries = PaletteEntries[Key];
                foreach (PaletteEntry Entry in Entries)
                {
                    for (int X = 0; X < 256; X++)
                    {
                        if (Entry.PaletteEntryType == PaletteEntry.EntryType.IDEntity)
                            if (Entry.Metadata != -1)
                            {
                                if (FastPalette[X][Key][Entry.Metadata].EntityColours == null)
                                    FastPalette[X][Key][Entry.Metadata].EntityColours = new List<PaletteEntry>();

                                FastPalette[X][Key][Entry.Metadata].EntityColours.Add(Entry);
                                DepthOpacities[Entry.BlockID][Entry.Metadata] = Entry.DepthOpacity;
                            }
                            else
                                for (int Meta = 0; Meta < 16; Meta++)
                                {
                                    if (FastPalette[X][Key][Meta].EntityColours == null)
                                        FastPalette[X][Key][Meta].EntityColours = new List<PaletteEntry>();

                                    FastPalette[X][Key][Meta].EntityColours.Add(Entry);
                                    DepthOpacities[Entry.BlockID][Meta] = Entry.DepthOpacity;
                                }
                        else
                        {
                            if (Entry.Metadata != -1)
                            {
                                FastPalette[X][Key][Entry.Metadata].Colour = Entry.Color;
                                DepthOpacities[Entry.BlockID][Entry.Metadata] = Entry.DepthOpacity;
                            }
                            else
                            {
                                for (int Meta = 0; Meta < 16; Meta++)
                                {
                                    if (FastPalette[X][Key][Meta].Colour.A == 0)
                                        FastPalette[X][Key][Meta].Colour = Entry.Color;
                                    DepthOpacities[Entry.BlockID][Meta] = Entry.DepthOpacity;
                                }
                            }
                        }
                    }
                }
            }


            // *** Assemble tint lookups
            foreach (int Key in BiomeTints.Keys)
            {
                List<TintEntry> Entries = BiomeTints[Key];
                int M = 0;
                foreach (TintEntry Entry in Entries)
                {
                    if (Entry.Metadata > -1)
                    {
                        FastPalette[Key][Entry.BlockID][Entry.Metadata].Colour.Blend(Entry.Tint);
                        M |= (1 << Entry.Metadata);
                    }
                    else
                    {
                        for (int X = 0; X < 16; X++)
                            if ((M & (1 << X)) == 0)
                                FastPalette[Key][Entry.BlockID][X].Colour.Blend(Entry.Tint);
                    }
                }
            }




        }

        // TODO Refactor this because it's horrible
        internal void ExecutePalette(PaletteToken[] Tokens)
        {
            List<PaletteEntry> ProspectiveAdditions = new List<PaletteEntry>();
            List<TintEntry> ProspectiveTintAdditions = new List<TintEntry>();
            Dictionary<string, string> Variables = new Dictionary<string, string>();

            // *** Loop through all the tokens and execute them as necessary
            for (int x = 0; x < Tokens.Length; x++)
            {
                PaletteToken Token = Tokens[x];
                switch (Token.Type)
                {
                    case PaletteToken.TokenType.Command:
                        switch (Token.TokenData)
                        {
                            case "=":
                                if (Tokens[x - 1].Type != PaletteToken.TokenType.Variable)
                                {
                                    // *** Throw Error - Cannot assign to non-variable
                                    throw new PaletteExecutionException("Cannot assign to non-variable at line " + Tokens[x].Line.ToString());
                                }
                                if ((Tokens[x + 1].Type != PaletteToken.TokenType.Variable) && (Tokens[x + 1].Type != PaletteToken.TokenType.Constant))
                                {
                                    // *** Throw Error - Can only assign from variables or constants
                                    throw new PaletteExecutionException("Invalid assignment source at line " + Tokens[x].Line.ToString());
                                }

                                Variables[Tokens[x - 1].TokenData] = Resolve(Variables, Tokens[x + 1]);
                                x++;
                                break;
                            case ">":
                                Console.Write("  ");
                                for (++x; Tokens[x].Type != PaletteToken.TokenType.Newline; x++)
                                    Console.Write((Tokens[x].Type == PaletteToken.TokenType.Constant ? Tokens[x].TokenData : Resolve(Variables, Tokens[x])) + " ");
                                Console.WriteLine("");
                                break;
                            case ".":
                                for (int y = 1; y < 8; y++)
                                {
                                    if (Tokens[x + y].Type == PaletteToken.TokenType.Newline)
                                        throw new PaletteExecutionException("Too few arguments to command at line " + Tokens[x].Line.ToString());
                                }
                                if (!IsValidNumericToken(Tokens[x + 1], Variables) || !IsValidNumericToken(Tokens[x + 2], Variables) || !IsValidNumericToken(Tokens[x + 3], Variables) ||
                                    !IsValidNumericToken(Tokens[x + 4], Variables) || !IsValidNumericToken(Tokens[x + 5], Variables) || !IsValidNumericToken(Tokens[x + 6], Variables) ||
                                    !IsValidNumericToken(Tokens[x + 7], Variables))
                                    throw new PaletteExecutionException("Illegal argument to command at line " + Tokens[x].Line.ToString());

                                ProspectiveAdditions.Add(new PaletteEntry(ResolveInteger(Tokens[x + 1], Variables), ResolveInteger(Tokens[x + 2], Variables), ResolveInteger(Tokens[x + 3], Variables),
                                                                          ResolveInteger(Tokens[x + 4], Variables), ResolveInteger(Tokens[x + 5], Variables), ResolveInteger(Tokens[x + 6], Variables),
                                                                          ResolveInteger(Tokens[x + 7], Variables)));
                                x += 7;
                                break;
                            case "e":
                                for (int y = 1; y < 10; y++)
                                {
                                    if (Tokens[x + y].Type == PaletteToken.TokenType.Newline)
                                        throw new PaletteExecutionException("Too few arguments to command at line " + Tokens[x].Line.ToString());
                                }
                                if (!IsValidNumericToken(Tokens[x + 1], Variables) || !IsValidNumericToken(Tokens[x + 2], Variables) || !IsValidNumericToken(Tokens[x + 5], Variables) ||
                                    !IsValidNumericToken(Tokens[x + 6], Variables) || !IsValidNumericToken(Tokens[x + 7], Variables) || !IsValidNumericToken(Tokens[x + 5], Variables) ||
                                    !IsValidNumericToken(Tokens[x + 9], Variables))
                                    throw new PaletteExecutionException("Illegal argument to command at line " + Tokens[x].Line.ToString());

                                ProspectiveAdditions.Add(new PaletteEntry(ResolveInteger(Tokens[x + 1], Variables), ResolveInteger(Tokens[x + 2], Variables), Resolve(Variables, Tokens[x + 3]),
                                                                          Resolve(Variables, Tokens[x + 4]), ResolveInteger(Tokens[x + 5], Variables), ResolveInteger(Tokens[x + 6], Variables),
                                                                          ResolveInteger(Tokens[x + 7], Variables), ResolveInteger(Tokens[x + 5], Variables), ResolveInteger(Tokens[x + 6], Variables)));
                                x += 9;
                                break;
                            case "t": // *** t Biome Block Meta R G B A
                                for (int y = 1; y < 8; y++)
                                {
                                    if (Tokens[x + y].Type == PaletteToken.TokenType.Newline)
                                        throw new PaletteExecutionException("Too few arguments to command at line " + Tokens[x].Line.ToString());
                                }
                                if (!IsValidNumericToken(Tokens[x + 1], Variables) || !IsValidNumericToken(Tokens[x + 2], Variables) || !IsValidNumericToken(Tokens[x + 3], Variables) ||
                                    !IsValidNumericToken(Tokens[x + 4], Variables) || !IsValidNumericToken(Tokens[x + 5], Variables) || !IsValidNumericToken(Tokens[x + 6], Variables) ||
                                    !IsValidNumericToken(Tokens[x + 7], Variables))

                                    throw new PaletteExecutionException("Illegal argument to command at line " + Tokens[x].Line.ToString());

                                ProspectiveTintAdditions.Add(new TintEntry(ResolveInteger(Tokens[x + 1], Variables), ResolveInteger(Tokens[x + 2], Variables), ResolveInteger(Tokens[x + 3], Variables),
                                                                           ResolveInteger(Tokens[x + 4], Variables), ResolveInteger(Tokens[x + 5], Variables), ResolveInteger(Tokens[x + 6], Variables),
                                                                           ResolveInteger(Tokens[x + 7], Variables)));
                                x += 7;
                                break;
                        }
                        break;
                    case PaletteToken.TokenType.Constant:
                        throw new PaletteExecutionException("Unexpected constant at line " + Tokens[x].Line.ToString());
                    case PaletteToken.TokenType.Variable:
                        //***  Do nothing
                        break;
                }
            }

            // *** Merge changes into main palette

            // *** Start by adding any lists needed to the dictionary and removing any outdated entries
            foreach (PaletteEntry Entry in ProspectiveAdditions)
            {
                if (!PaletteEntries.ContainsKey(Entry.BlockID))
                    PaletteEntries[Entry.BlockID] = new List<PaletteEntry>();

                List<PaletteEntry> Entries = PaletteEntries[Entry.BlockID];
                Entries.RemoveAll((X) => X.IsSupplantedBy(Entry));
            }

            // *** Then add all the new entries to the palette
            foreach (PaletteEntry Entry in ProspectiveAdditions)
            {
                PaletteEntries[Entry.BlockID].Add(Entry);
            }

            // *** Finally, sort the lists by specificity.  More specific palette entries will thus take rendering prevalence over broader catch-all entries.
            foreach (List<PaletteEntry> EntryList in PaletteEntries.Values)
                EntryList.Sort();


            // *** Now do the same for tints


            // *** Start by adding any lists needed to the dictionary and removing any outdated entries
            foreach (TintEntry Entry in ProspectiveTintAdditions)
            {
                if (!BiomeTints.ContainsKey(Entry.BiomeID))
                    BiomeTints[Entry.BiomeID] = new List<TintEntry>();
                List<TintEntry> Entries = BiomeTints[Entry.BiomeID];
                Entries.RemoveAll((X) => X.IsSupplantedBy(Entry));
            }

            // *** Then add all the new entries to the palette
            foreach (TintEntry Entry in ProspectiveTintAdditions)
                BiomeTints[Entry.BiomeID].Add(Entry);

            // *** Finally, sort the lists by specificity.  More specific palette entries will thus take rendering prevalence over broader catch-all entries.
            foreach (List<TintEntry> EntryList in BiomeTints.Values)
                EntryList.Sort();

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
            String[] Lines = System.IO.File.ReadAllLines(PaletteFile);
            Console.WriteLine("Loading palette " + Path.GetFileNameWithoutExtension(PaletteFile));

            PaletteToken[] Tokens = TokenizePaletteFile(Lines);

            ExecutePalette(Tokens);
        }
    }
}
