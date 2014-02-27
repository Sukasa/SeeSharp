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
    ///     Render configuration as selected by user
    /// </summary>
    public sealed class RenderConfiguration
    {
        /// <summary>
        ///     Minimum blocklight level to render
        /// </summary>
        /// <remarks>
        ///      MinLightLevel is the minimum light level to use when adjusting block colours for ambient and sky light.  This might be lowered or raised to simulate time-of-day, or to set a minimum illumination for cave rendering.  This value will be between 0 and 15.
        /// </remarks>
        /// <seealso cref="Colour.LightLevel"/>
        public int MinLightLevel;

        /// <summary>
        ///     What dimension is being rendered.
        /// </summary>
        /// <remarks>
        ///     Dimension contains the folder name for the world dimension being rendered (DIM1, DIM-1, etc).  The overworld is <see cref="String.Empty"/>
        /// </remarks>
        public String Dimension;

        /// <summary>
        ///     The file path the world is stored at
        /// </summary>
        /// <remarks>
        ///     This will always be set, and is the file path of the folder containing the world data, e.g. C:\Users\YourName\AppData\Roaming\.minecraft\saves\World1
        /// </remarks>
        public String WorldPath;

        /// <summary>
        ///     What file to save the output map as. 
        /// </summary>
        /// <remarks>
        ///     Where to save the renderer output, expressed as a file.  The file is not actually created for the renderer, and should be created as part of the render process.
        /// </remarks>
        public String SaveFilename;

        /// <summary>
        ///     Where to save sign-scan data
        /// </summary>
        /// <remarks>
        ///     The file path to save the XML document listing all the sign exports processed by See Sharp.
        /// </remarks>
        public String ScanFilename;

        /// <summary>
        ///     Where to save basic world metrics data
        /// </summary>
        /// <remarks>
        ///     The file path to save the XML document detailing the world's base metrics for the current dimension
        /// </remarks>
        public String MetricsFilename;

        /// <summary>
        ///     Which map renderer to use
        /// </summary>
        public String RendererName;

        /// <summary>
        ///     Whether the render should run in one thread, or multiple threads.
        /// </summary>
        /// <remarks>
        ///     See Sharp supports both single-threaded rendering and multi-threaded rendering.  If the user has not specified single-threaded rendering, this value will be true.  If it is false, the renderer should NOT create any threads for its operation.
        /// </remarks>
        public bool EnableMultithreading;

        /// <summary>
        ///     Set to true during initialization and rendering if the render is for the Gui preview window
        /// </summary>
        /// <remarks>
        ///     IsPreview is set true when the plugin is being configured and initialized for a preview render instead of a full render.
        /// </remarks>
        public bool IsPreview;

        /// <summary>
        ///     If <see cref="EnableMultithreading"/> is set, maximum number of threads to use
        /// </summary>
        /// <remarks>
        ///     The MaxThreads parameter instructs the rendering plugin on the maximum number of high-processing-load threads to spawn, as per the user configuration.  If multithreading should be disabled, this will be set to one.
        /// </remarks>
        public int MaxThreads;

        /// <summary>
        ///     The block colour palette. 
        /// </summary>
        /// <remarks>
        ///     The Palette parameter contains a reference to the <see cref="SeeSharp.Palette.BlockPalette"/> object containing the currently-configured colour palette.
        /// </remarks>
        /// <seealso cref="SeeSharp.Palette.BlockPalette"/>
        public BlockPalette Palette;

        /// <summary>
        ///     World metrics for the dimension being rendered
        /// </summary>
        /// <remarks>
        ///     The Metrics parameter provides metrics information about the current dimension being rendered.
        /// </remarks>
        ///  <seealso cref="SeeSharp.WorldMetrics"/>
        public WorldMetrics Metrics;

        /// <summary>
        ///     The chunk provider for the world.
        /// </summary>
        /// <remarks>
        ///     The Chunks parameter is the core object by which the renderer can get the data it needs to perform its intended function.
        /// </remarks>
        /// <seealso cref="Substrate.RegionChunkManager"/>
        public RegionChunkManager Chunks;

        /// <summary>
        ///     If true, the user has selected a specific region to be rendered, instead of the whole world
        /// </summary>
        /// <remarks>
        ///     RenderSubregion is set when a render mask for the world has been specified instead of the entire world.  It can be used to handle the whole-world case in the event that it allows for faster rendering, or as a hint as to the render process to be expected.
        /// </remarks>
        ///  <seealso cref="SubregionChunks"/>
        public bool RenderSubregion;

        /// <summary>
        ///     The rectangle mask of which chunks to render
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         The SubregionChunks mask is an inclusive rectangle measured in chunks which demarcates a region of the world to render.  The renderer should not attempt to render world chunks outside of this range, and not all chunks inside this range are guaranteed to exist.
        ///     </para>
        ///     <para>
        ///         This rectangle measures in chunks and is zero-based; Rendering (0, 0) to (1, 1) is 2x2, not 1x1.  Be sure to allocate resources correctly to account for this.
        ///     </para>
        ///  </remarks>
        public Rectangle SubregionChunks;

        /// <summary>
        ///     List of key-value render option pairs.
        /// </summary>
        /// <remarks>
        ///     The AdvancedRenderOptions param is a list of Key-Value pairs generated either from the command line, or via user interaction with the advanced settings dialog specific to the renderer.
        /// </remarks>
        /// <seealso cref="SeeSharp.Rendering.RendererConfigForm"/>
        /// <seealso cref="SeeSharp.Rendering.IRenderer"/>
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
