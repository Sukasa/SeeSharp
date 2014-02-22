using System;
using Substrate;

namespace SeeSharp
{
    public class PaletteEntry : IComparable<PaletteEntry>
    {
        public enum EntryType
        {
            IDMetadata,
            IDEntity
        }

        public EntryType PaletteEntryType = EntryType.IDMetadata;
        public Colour Color;
        public int DepthOpacity;

        public int BlockID;
        public int Metadata;

        private string EntityTag;
        private string EntityTagCheckValue;

        public PaletteEntry(int BlockID, int MetaData, int Opacity, int Red, int Green, int Blue, int Alpha)
        {
            this.BlockID = BlockID;
            this.Metadata = MetaData;
            this.Color = new Colour { A = (byte)Alpha, B = (byte)Blue, G = (byte)Green, R = (byte)Red };
            this.DepthOpacity = Opacity;
        }

        public PaletteEntry(int BlockID, int MetaData, string ValueKey, string ValueRef, int Opacity, int Red, int Green, int Blue, int Alpha)
            : this(BlockID, MetaData, Opacity, Red, Green, Blue, Alpha)
        {
            PaletteEntryType = EntryType.IDEntity;
            this.EntityTag = ValueKey;
            this.EntityTagCheckValue = ValueRef;
        }

        public bool IsSupplantedBy(PaletteEntry NewEntry)
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
        public bool IsMatch(int CheckMetaData, TileEntity Entity)
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