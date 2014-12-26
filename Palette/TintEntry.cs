using System;

namespace SeeSharp.Palette
{
    internal class TintEntry : IComparable<TintEntry>
    {
        public int BiomeId;
        public int BlockId;
        public int Metadata;

        public Colour Tint;

        public bool IsSupplantedBy(TintEntry NewEntry)
        {
            if (NewEntry.BlockId != BlockId)
                return false;

            if (Metadata != NewEntry.Metadata && NewEntry.Metadata != -1)
                return false;

            return true;
        }

        public TintEntry(int BiomeId, int BlockId, int MetaData, int Red, int Green, int Blue, int Alpha)
        {
            this.BiomeId = BiomeId;
            this.BlockId = BlockId;
            Metadata = MetaData;
            Tint = new Colour { A = (byte)Alpha, R = (byte)Red, G = (byte)Green, B = (byte)Blue };
        }

        public bool IsMatch(int CheckBlockId, int CheckMetaData)
        {
            if (CheckBlockId != BlockId)
                return false;


            return (!(Metadata >= 0 && CheckMetaData != Metadata));
        }

        int IComparable<TintEntry>.CompareTo(TintEntry OtherEntry)
        {
            if (BlockId < OtherEntry.BlockId)
                return -1;

            if (BlockId > OtherEntry.BlockId)
                return 1;

            if (OtherEntry.Metadata == -1 && Metadata != -1)
                return -1;

            if (OtherEntry.Metadata != -1 && Metadata == -1)
                return 1;

            return 0;
        }
    }

}