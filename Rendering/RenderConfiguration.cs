using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SeeSharp.Palette;
using Substrate;
using System.Drawing;

namespace SeeSharp
{
    /// <summary>
    ///     Render configuration.  This will reflect the settings the user selected in the render dialog, or the command line
    /// </summary>
    public sealed class RenderConfiguration
    {
        /// <summary>
        ///     Minimum blocklight level to render at.  This might be lowered or raised to simulate time-of-day, or to set a minimum illumination for cave rendering.
        /// </summary>
        public int MinLightLevel;
        /// <summary>
        ///     What dimension is being rendered.
        /// </summary>
        /// <remarks>
        ///     The overworld is <see cref="String.Empty"/>
        /// </remarks>
        public String Dimension;

        /// <summary>
        ///     The file path the world is stored at
        /// </summary>
        public String WorldPath;

        /// <summary>
        ///     What file to save the output map as. 
        /// </summary>
        public String SaveFilename;

        /// <summary>
        ///     Where to save sign-scan data, if that data is to be exported
        /// </summary>
        public String ScanFilename;

        /// <summary>
        ///     Where to save basic world metrics data to
        /// </summary>
        public String MetricsFilename;


        /// <summary>
        ///     Which map renderer to use
        /// </summary>
        public String RendererName;

        /// <summary>
        ///     Whether the render should run in one thread, or multiple threads.
        /// </summary>
        public bool EnableMultithreading;

        /// <summary>
        ///     Set to true during initialization and rendering if the render is for the Gui preview window
        /// </summary>
        public bool IsPreview;

        /// <summary>
        ///     If <see cref="EnableMultithreading"/> is set, how many threads to use, maximum.
        /// </summary>
        public int MaxThreads;

        /// <summary>
        ///     The block colour palette. 
        /// </summary>
        /// <seealso cref="SeeSharp.Palette.BlockPalette"/>
        public BlockPalette Palette;

        /// <summary>
        ///     World metrics for the dimension being rendered
        /// </summary>
        ///  <seealso cref="SeeSharp.WorldMetrics"/>
        public WorldMetrics Metrics;

        /// <summary>
        ///     The chunk provider for the world.
        /// </summary>
        /// <seealso cref="Substrate.RegionChunkManager"/>
        public RegionChunkManager Chunks;

        /// <summary>
        ///     If true, the user has selected a specific region to be rendered, instead of the whole world
        /// </summary>
        /// <remarks>
        ///     Will be true during a preview render, along with the render mask. 
        /// </remarks>
        ///  <seealso cref="SubregionChunks"/>
        public bool RenderSubregion;

        /// <summary>
        ///     The rectangle mask of which chunks to render
        /// </summary>
        /// <remarks>
        ///     This rectangle is inclusive, so remeber that your render window is (Width+1) * (Height+1)
        ///  </remarks>
        public Rectangle SubregionChunks;

        /// <summary>
        ///     List of key-value render option pairs.
        /// </summary>
        /// <remarks>
        ///     The meanings of each key and its values are dependent on and unique to the renderer being used.
        /// </remarks>
        public List<KeyValuePair<String, String>> AdvancedRenderOptions;

        internal RenderConfiguration()
            : this(null, null)
        {
            // *** Nothing here
        }

        internal RenderConfiguration(BlockPalette UsePalette, WorldMetrics UseMetrics)
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
            AdvancedRenderOptions = new List<KeyValuePair<string, string>>();
            SubregionChunks = new Rectangle();
            RenderSubregion = false;
        }
    }
}
