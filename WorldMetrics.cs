using System;
using System.Collections.Generic;
using Substrate;


namespace SeeSharp
{
    /// <summary>
    ///     World Metrics class.  Contains information about the world, and provides a way to access cached data about the world such as mineral distribution wealths, etc.
    /// </summary>
    public sealed class WorldMetrics
    {
        /// <summary>
        ///     Minimum X extent (Westmost chunk X-value) of the world
        /// </summary>
        public int MinX;
        /// <summary>
        ///     Minimum Z extent (Northernmost chunk Z-value) of the world
        /// </summary>
        public int MinZ;
        /// <summary>
        ///     Maximum X extent (Eastmost chunk X-value) of the world
        /// </summary>
        public int MaxX;
        /// <summary>
        ///     Maximum Z extent (Southmost chunk Z-value) of the world
        /// </summary>
        public int MaxZ;

        /// <summary>
        ///     Number of chunks in the world
        /// </summary>
        /// <remarks>
        ///     In the event a Subregion is used, this number will not update to reflect the number of chunks within the subregion 
        /// </remarks>
        public int NumberOfChunks;
        /// <summary>
        ///     Which dimension the metrics apply to
        /// </summary>
        public String Dimension = "";

        internal WorldMetrics() { }
        internal WorldMetrics(RegionChunkManager Chunks)
        {
            Measure(Chunks);
        }
        internal WorldMetrics(AnvilWorld World, String Dimension)
        {
            this.Dimension = Dimension;
            Measure(World);
        }


        internal void Measure(IEnumerable<ChunkRef> Chunks)
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
        internal void Measure(AnvilWorld World)
        {
            Measure(World.GetChunkManager(Dimension));
        }
    }
}
