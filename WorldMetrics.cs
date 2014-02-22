using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Substrate;


namespace SeeSharp
{
    public sealed class WorldMetrics
    {
        public int MinX;
        public int MinZ;
        public int MaxX;
        public int MaxZ;

        public int NumberOfChunks;
        public String Dimension = "region";

        public WorldMetrics() { }
        public WorldMetrics(RegionChunkManager Chunks)
        {
            Measure(Chunks);
        }
        public WorldMetrics(AnvilWorld World, String Dimension)
        {
            this.Dimension = Dimension;
            Measure(World);
        }

        public void Measure(IEnumerable<ChunkRef> Chunks)
        {
            MinX = 0;
            MinZ = 0;
            MaxX = 0;
            MaxZ = 0;
            NumberOfChunks = 0;

            foreach (ChunkRef Chunk in Chunks)
            {
                MinX = Math.Min(MinX, Chunk.X);
                MaxX = Math.Max(MaxX, Chunk.X);
                MinZ = Math.Min(MinZ, Chunk.Z);
                MaxZ = Math.Max(MaxZ, Chunk.Z);
                NumberOfChunks++;
            }
        }
        public void Measure(AnvilWorld World)
        {
            Measure(World.GetChunkManager(Dimension));
        }
    }
}
