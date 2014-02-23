using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SeeSharp.Palette;
using Substrate;
using System.Drawing;

namespace SeeSharp
{

    public sealed class RenderConfiguration
    {
        public int MinLightLevel;
        public String Dimension;

        public String WorldPath;
        public String SaveFilename;
        public String ScanFilename;
        public String MetricsFilename;
        public String RendererName;

        public bool EnableMultithreading;
        public bool IsPreview;
        public int MaxThreads;

        public BlockPalette Palette;
        public WorldMetrics Metrics;
        public RegionChunkManager Chunks;

        public bool RenderSubregion;
        public Rectangle SubregionChunks;

        public List<Tuple<String, String>> AdvancedRenderOptions;

        public RenderConfiguration() : this(null, null)
        {
            // *** Nothing here
        }

        public RenderConfiguration(BlockPalette UsePalette, WorldMetrics UseMetrics)
        {
            MinLightLevel = 15;
            WorldPath = "";
            SaveFilename = "";
            ScanFilename = "";
            MetricsFilename = "";
            Dimension = "";
            EnableMultithreading = false;
            Palette = UsePalette;
            Metrics = UseMetrics;
            RendererName = "Standard";
            MaxThreads = Environment.ProcessorCount;
            AdvancedRenderOptions = new List<Tuple<string, string>>();
            SubregionChunks = new Rectangle();
            RenderSubregion = false;
        }
    }
}
