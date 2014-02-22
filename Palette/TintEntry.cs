using System;

namespace SeeSharp
{

    public class TintEntry : IComparable<TintEntry>
    {
        public int BiomeID;
        public int BlockID;
        public int Metadata;

        public Colour Tint;

        public bool IsSupplantedBy(TintEntry NewEntry)
        {
            if (NewEntry.BlockID != BlockID)
                return false;

            if (Metadata != NewEntry.Metadata && NewEntry.Metadata != -1)
                return false;

            return true;
        }

        public TintEntry(int BiomeID, int BlockID, int MetaData, int Red, int Green, int Blue, int Alpha)
        {
            this.BiomeID = BiomeID;
            this.BlockID = BlockID;
            this.Metadata = MetaData;
            this.Tint = new Colour { A = (byte)Alpha, R = (byte)Red, G = (byte)Green, B = (byte)Blue };
        }

        public bool IsMatch(int CheckBlockID, int CheckMetaData)
        {
            if (CheckBlockID != BlockID)
                return false;

            if (Metadata >= 0 && CheckMetaData != Metadata)
                return false;

            return true;
        }

        int IComparable<TintEntry>.CompareTo(TintEntry OtherEntry)
        {
            if (BlockID < OtherEntry.BlockID)
                return -1;

            if (BlockID > OtherEntry.BlockID)
                return 1;

            if (OtherEntry.Metadata == -1 && Metadata != -1)
                return -1;

            if (OtherEntry.Metadata != -1 && Metadata == -1)
                return 1;

            return 0;
        }
    }

}