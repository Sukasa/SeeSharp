using System;
using System.Collections.Generic;
using Substrate;


namespace SeeSharp
{
    /// <summary>
    ///     World metrics container class
    /// </summary>
    /// <remarks>
    ///     The world metrics class contains metrics information about the world including chunk extents, the dimension to which it applies, data maps, and similar metric information.
    /// </remarks>
    public sealed class WorldMetrics
    {
        /// <summary>
        ///     Minimum X extent (Westmost chunk X-value) of the world
        /// </summary>
        /// <remarks>
        ///     Minimum X extent (Westmost chunk X-value) of the world.
        ///     This value reflects the dimension as a whole, and is not modified to match any subregion masks in render configurations
        /// </remarks>
        public int MinX;

        /// <summary>
        ///     Minimum Z extent (Northernmost chunk Z-value) of the world
        /// </summary>
        /// <remarks>
        ///     Minimum Z extent (Northernmost chunk Z-value) of the world
        ///     This value reflects the dimension as a whole, and is not modified to match any subregion masks in render configurations
        /// </remarks>
        public int MinZ;

        /// <summary>
        ///     Maximum X extent (Eastmost chunk X-value) of the world
        /// </summary>
        /// <remarks>
        ///     Maximum X extent (Eastmost chunk X-value) of the world
        ///     This value reflects the dimension as a whole, and is not modified to match any subregion masks in render configurations
        /// </remarks>
        public int MaxX;

        /// <summary>
        ///     Maximum Z extent (Southmost chunk Z-value) of the world
        /// </summary>
        /// <remarks>
        ///     Maximum Z extent (Southmost chunk Z-value) of the world
        ///     This value reflects the dimension as a whole, and is not modified to match any subregion masks in render configurations
        /// </remarks>
        public int MaxZ;

        /// <summary>
        ///     Number of chunks in the world
        /// </summary>
        /// <remarks>
        ///     Number of chunks in the world
        ///     In the event a Subregion is used, this number will not update to reflect the number of chunks within the subregion 
        /// </remarks>
        public int NumberOfChunks;

        /// <summary>
        ///     Which dimension the metrics apply to
        /// </summary>
        /// <remarks>
        ///     This value will reflect the folder the dimension is stored in, or will be <see cref="String.Empty"/> for the overworld
        /// </remarks>
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
