using System;
using Substrate;
using System.Collections.Generic;

namespace SeeSharp.Palette
{
    /// <summary>
    ///     Every block/metadata/entity combo has an associated full palette entry.  This entity stores depth-opacity, colour, metadata, entity tag, and custom data for each block.
    /// </summary>
    public class PaletteEntry : IComparable<PaletteEntry>
    {
        /// <summary>
        ///     What type of palette entry
        /// </summary>
        public enum EntryType
        {
            /// <summary>
            ///     Metadata entry - the block is identified by metadata and blockID
            /// </summary>
            IdMetadata,
            /// <summary>
            ///     Entity entry - the block is distinguished from similar blocks by specific data in its NBT structure
            /// </summary>
            IdEntity
        }

        /// <summary>
        ///     What kind of entry this is, whether a basic BlockId-Metadata entry, or BlockId-Metadata-Entity entry
        /// </summary>
        public readonly EntryType PaletteEntryType = EntryType.IdMetadata;
        /// <summary>
        ///     The base colour for the block
        /// </summary>
        public Colour Color;
        /// <summary>
        ///     The depth opacity for a block.  Lower depth opacities cause the renderer to draw farther down past this block
        /// </summary>
        public readonly int DepthOpacity;

        /// <summary>
        ///     Block ID this palette entry applies to
        /// </summary>
        public readonly int BlockId;
        /// <summary>
        ///     Metadata this palette entry applies to, or -1 if it applies to all 16 metadatas for this entry's Block ID.
        /// </summary>
        public readonly int Metadata;

        private readonly string _EntityTag;
        private readonly string _EntityTagCheckValue;

        private readonly Dictionary<String, String> _CustomData = new Dictionary<string, string>();

        /// <summary>
        ///     Return custom data about a block, or a default value if it is not defined
        /// </summary>
        /// <remarks>
        ///     GetCustomData() allows you to retrieve "Specialty" data about a block, such as value, name, 
        /// </remarks>
        /// <param name="Key">
        ///     Key for the data, e.g. "MaterialValue"
        /// </param>
        /// <param name="DefaultValue">
        ///     Default value if the key is not defined, e.g. "0" to match the example key.
        /// </param>
        /// <returns>
        ///     The stored data value for that key, or the default value if the key is not defined.
        /// </returns>
        public String GetCustomData(String Key, String DefaultValue = "")
        {
            return _CustomData.ContainsKey(Key) ? _CustomData[Key] : DefaultValue;
        }

        internal PaletteEntry(int BlockId, int MetaData, int Opacity, int Red, int Green, int Blue, int Alpha)
        {
            this.BlockId = BlockId;
            Metadata = MetaData;
            Color = new Colour { A = (byte)Alpha, B = (byte)Blue, G = (byte)Green, R = (byte)Red };
            DepthOpacity = Opacity;
        }

        internal PaletteEntry(int BlockId, int MetaData, string ValueKey, string ValueRef, int Opacity, int Red, int Green, int Blue, int Alpha)
            : this(BlockId, MetaData, Opacity, Red, Green, Blue, Alpha)
        {
            PaletteEntryType = EntryType.IdEntity;
            _EntityTag = ValueKey;
            _EntityTagCheckValue = ValueRef;
        }

        internal void AddCustomData(string DataKey, string DataValue)
        {
            _CustomData.Add(DataKey, DataValue);
        }

        internal bool IsSupplantedBy(PaletteEntry NewEntry)
        {
            if (NewEntry.BlockId != BlockId)
                return false;

            if (Metadata != NewEntry.Metadata && NewEntry.Metadata != -1)
                return false;

            return true;
        }

        /// <summary>
        ///     Returns whether this PaletteEntry object is a match to the supplied block information.  Note that this does NOT check Block ID due to how the Palette Dictionary is optimized.
        /// </summary>
        /// <param name="CheckMetaData">
        ///     The Block's Metadata value
        /// </param>
        /// <param name="Entity">
        ///     The block's TileEntity, if it has one.
        /// </param>
        /// <returns>
        ///     True if the block is a match, False if not
        /// </returns>
        internal bool IsMatch(int CheckMetaData, TileEntity Entity)
        {
            if (Metadata >= 0 && CheckMetaData != Metadata)
                return false;

            if (_EntityTag != null)
            {
                if (!Entity.Source.Keys.Contains(_EntityTag))
                    return false;

                if (Entity.Source[_EntityTag].ToTagString() != _EntityTagCheckValue)
                    return false;
            }
            else if (PaletteEntryType == EntryType.IdEntity)
                return false;

            return true;
        }

        /// <summary>
        ///     Sorting comparer between PaletteEntry objects
        /// </summary>
        /// <param name="OtherEntry">
        ///     The PaletteEntry object to compare to
        /// </param>
        /// <returns>
        ///     Sort Order
        /// </returns>
        /// <remarks>
        ///     This sorter arranges palette entries in order of descending specificity
        /// </remarks>
        int IComparable<PaletteEntry>.CompareTo(PaletteEntry OtherEntry)
        {
            if (BlockId < OtherEntry.BlockId)
                return -1;

            if (BlockId > OtherEntry.BlockId)
                return 1;

            if (OtherEntry.PaletteEntryType == EntryType.IdEntity && PaletteEntryType == EntryType.IdMetadata)
                return 1;

            if (OtherEntry.PaletteEntryType == EntryType.IdMetadata && PaletteEntryType == EntryType.IdEntity)
                return -1;

            if (OtherEntry.Metadata == -1 && Metadata != -1)
                return -1;

            if (OtherEntry.Metadata != -1 && Metadata == -1)
                return 1;

            return 0;
        }
    }
}