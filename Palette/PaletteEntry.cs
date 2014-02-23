using System;
using Substrate;

namespace SeeSharp
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
            IDMetadata,
            /// <summary>
            ///     Entity entry - the block is distinguished from similar blocks by specific data in its NBT structure
            /// </summary>
            IDEntity
        }

        /// <summary>
        ///     What kind of entry this is, whether a basic BlockID-Metadata entry, or BlockID-Metadata-Entity entry
        /// </summary>
        public EntryType PaletteEntryType = EntryType.IDMetadata;
        /// <summary>
        ///     The base colour for the block
        /// </summary>
        public Colour Color;
        /// <summary>
        ///     The depth opacity for a block.  Lower depth opacities cause the renderer to draw farther down past this block
        /// </summary>
        public int DepthOpacity;

        /// <summary>
        ///     Block ID this palette entry applies to
        /// </summary>
        public int BlockID;
        /// <summary>
        ///     Metadata this palette entry applies to, or -1 if it applies to all 16 metadatas for this entry's Block ID.
        /// </summary>
        public int Metadata;

        private string EntityTag;
        private string EntityTagCheckValue;

        internal PaletteEntry(int BlockID, int MetaData, int Opacity, int Red, int Green, int Blue, int Alpha)
        {
            this.BlockID = BlockID;
            this.Metadata = MetaData;
            this.Color = new Colour { A = (byte)Alpha, B = (byte)Blue, G = (byte)Green, R = (byte)Red };
            this.DepthOpacity = Opacity;
        }

        internal PaletteEntry(int BlockID, int MetaData, string ValueKey, string ValueRef, int Opacity, int Red, int Green, int Blue, int Alpha)
            : this(BlockID, MetaData, Opacity, Red, Green, Blue, Alpha)
        {
            PaletteEntryType = EntryType.IDEntity;
            this.EntityTag = ValueKey;
            this.EntityTagCheckValue = ValueRef;
        }

        internal bool IsSupplantedBy(PaletteEntry NewEntry)
        {
            if (NewEntry.BlockID != BlockID)
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

            if (EntityTag != null)
            {
                if (!Entity.Source.Keys.Contains(EntityTag))
                    return false;

                if (Entity.Source[EntityTag].ToTagString() != EntityTagCheckValue)
                    return false;
            }
            else if (PaletteEntryType == EntryType.IDEntity)
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
            if (BlockID < OtherEntry.BlockID)
                return -1;

            if (BlockID > OtherEntry.BlockID)
                return 1;

            if (OtherEntry.PaletteEntryType == EntryType.IDEntity && PaletteEntryType == EntryType.IDMetadata)
                return 1;

            if (OtherEntry.PaletteEntryType == EntryType.IDMetadata && PaletteEntryType == EntryType.IDEntity)
                return -1;

            if (OtherEntry.Metadata == -1 && Metadata != -1)
                return -1;

            if (OtherEntry.Metadata != -1 && Metadata == -1)
                return 1;

            return 0;
        }
    }
}