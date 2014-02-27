using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using SeeSharp.Palette;
using SeeSharp.Rendering;
using Substrate;

namespace SeeSharp
{
    internal sealed class Renderer : IRenderer
    {
        // *** Error Codes
        public const Int32 ErrorNoMemory = -1;  // *** ERROR: Couldn't allocate bitmap.  Fatal, because w/o a bitmap wtf are you rendering to?
        public const Int32 ErrorBadChunk = 1;   // *** ERROR: There was a bad chunk that couldn't be rendered.  Not a fatal error.
        public const Int32 ErrorErrors = 2;     // *** ERROR: There were errors in the render!  Makes sense in context.


        // *** Configuration data
        private BlockPalette ColourPalette = new BlockPalette();
        private WorldMetrics Metrics;
        private RenderConfiguration Config;
        private RegionChunkManager Chunks;


        // *** Rendering data
        private Bitmap OutputMap;
        private BitmapData RenderTarget;
        private Int32 Stride;
        private Func<AlphaBlockCollection, int, int, int> RenderStartY;
        private CancellationTokenSource Cancellation = new CancellationTokenSource();
        ParallelOptions RenderingParallelOptions = new ParallelOptions();
        private ProgressUpdateEventArgs ProgressUpdateEventData = new ProgressUpdateEventArgs();
        private int PauseRendering; // *** Will erroneously say it's not assigned to when in debug configuration.
        private int RenderableChunks;


        // *** Progress Update Data
        string NumFormat;
        private int ProcessedChunks = 0;
        private bool CorruptChunks = false;
        private bool PendingRender = false;


        // *** Events
        public event ProgressUpdateHandler ProgressUpdate;
        public event RenderingErrorHandler RenderError;


        // *** Constructor
        public Renderer()
        {
        }


        // *** Render start determiners
        public int GetStartRenderYNormal(AlphaBlockCollection Blocks, int X, int Z)
        {
            int Y = Blocks.GetHeight(X, Z);
            if (Y > 255)
                Y = 255;
            if (Y == 0)
                return ColourPalette.DepthOpacities[Blocks.GetID(X, Y, Z)][Blocks.GetData(X, Y, Z)] == 0 ? -1 : 0;
            return Y;
        }
        public int GetStartRenderYCave(AlphaBlockCollection Blocks, int X, int Z)
        {
            int Y = 255;
            if (Y > 255)
                Y = 255;

            while (Y >= 0 && ColourPalette.FastPalette[0][Blocks.GetID(X, Y, Z)][Blocks.GetData(X, Y, Z)].Colour.A == 0)
                Y--;
            while (Y >= 0 && ColourPalette.FastPalette[0][Blocks.GetID(X, Y, Z)][Blocks.GetData(X, Y, Z)].Colour.A != 0)
                Y--;
            while (Y >= 0 && ColourPalette.FastPalette[0][Blocks.GetID(X, Y, Z)][Blocks.GetData(X, Y, Z)].Colour.A == 0)
                Y--;

            if (Y == 0)
                return ColourPalette.DepthOpacities[Blocks.GetID(X, Y, Z)][Blocks.GetData(X, Y, Z)] == 0 ? -1 : 0;
            return Y;
        }
        public int GetStartRenderYCaveAlternate(AlphaBlockCollection Blocks, int X, int Z)
        {
            int Y = 255;
            if (Y > 255)
                Y = 255;

            while (Y >= 0 && ColourPalette.FastPalette[0][Blocks.GetID(X, Y, Z)][Blocks.GetData(X, Y, Z)].Colour.A < 255) // Find first renderable fully opaque block
                Y--;
            while (Y >= 0 && ColourPalette.FastPalette[0][Blocks.GetID(X, Y, Z)][Blocks.GetData(X, Y, Z)].Colour.A != 0)
                Y--;
            while (Y >= 0 && ColourPalette.FastPalette[0][Blocks.GetID(X, Y, Z)][Blocks.GetData(X, Y, Z)].Colour.A == 0)
                Y--;

            if (Y == 0)
                return ColourPalette.DepthOpacities[Blocks.GetID(X, Y, Z)][Blocks.GetData(X, Y, Z)] == 0 ? -1 : 0;

            return Y;
        }


        // *** Rendering Functions
        public void Render()
        {

            // *** Define the chunk query, in order to cleanly support subregions
            IEnumerable<ChunkRef> ChunkProvider = from ChunkRef Chunk in Chunks
                                                  where !Config.RenderSubregion || Config.SubregionChunks.ContainsPoint(Chunk.X, Chunk.Z)
                                                  select Chunk;

            PendingRender = false;

            if (Config.RenderSubregion)
                foreach (ChunkRef Chunk in ChunkProvider)
                    RenderableChunks++;

            if (Cancellation.IsCancellationRequested)
            {
                OutputMap.Dispose();
                OutputMap = null;
                return;
            }

            // *** And render
            if (Config.EnableMultithreading)
            {
                Thread UpdateThread = new Thread(DisplayProgress);
                UpdateThread.Start();

                RenderingParallelOptions.CancellationToken = Cancellation.Token;
                RenderingParallelOptions.MaxDegreeOfParallelism = Config.MaxThreads;

                try
                {
                    Parallel.ForEach<ChunkRef>(ChunkProvider, RenderingParallelOptions, RenderChunk);
                    UpdateThread.Join();
                }
                catch (OperationCanceledException)
                {

                }
            }
            else
            {
                foreach (ChunkRef Chunk in ChunkProvider)
                {
                    RenderChunk(Chunk, null);
                    if (Cancellation.IsCancellationRequested)
                        break;
                    DoProgressUpdate();
                }
            }

            OutputMap.UnlockBits(RenderTarget);

            if (Cancellation.IsCancellationRequested)
            {
                OutputMap.Dispose();
                OutputMap = null;
                return;
            }

            if (!Config.IsPreview)
            {
                OutputMap.Save(Config.SaveFilename);
                OutputMap.Dispose();
                OutputMap = null;
            }

            // *** If a chunk failed to render, let the user know.  Unless we aborted, because there's no point then.
            if (CorruptChunks)
            {
                RenderingErrorEventArgs e = new RenderingErrorEventArgs();
                e.ErrorCode = ErrorErrors;
                e.IsFatal = true;
                e.UserErrorMessage = "WARNING: At least one potentially corrupt chunk was encountered during rendering.\r\nThe map image may contain missing sections as a result.";

                if (RenderError != null)
                    RenderError.Invoke(this, e);
            }
        }
        void RenderChunk(ChunkRef Chunk, ParallelLoopState LoopState)
        {
            Interlocked.Increment(ref ProcessedChunks);
#if !DEBUG
            try
            {
#endif
            if (LoopState != null && Cancellation.IsCancellationRequested)
                LoopState.Stop();

            if (LoopState != null && LoopState.IsStopped)
                return;

            while (PauseRendering > 0)
                Thread.Sleep(50);

            AlphaBlockCollection Blocks = Chunk.Blocks;

            for (int X = 0; X < 16; X++)
            {
                for (int Z = 0; Z < 16; Z++)
                {
                    // *** Start by finding the topmost block to render
                    int EndY = RenderStartY(Blocks, X, Z);
                    int Y = EndY;
                    int RenderVal = 255;

                    if (Y < 0)
                        continue; // *** No valid renderable blocks in this column, so continue with the next column

                    // *** Drill into the column to determine how many blocks down to render
                    while (RenderVal > 0)
                    {
                        RenderVal -= ColourPalette.DepthOpacities[Blocks.GetID(X, Y, Z)][Blocks.GetData(X, Y, Z)];
                        if (Y == 0) // *** If we've hit the bottom of the map, don't try and keep going.
                            break;  // *** It wouldn't end well.
                        Y--;
                    }

                    Colour SetColour = Colour.Transparent; // *** What colour to set the current column's pixel to.
                    Colour TempColour; // *** Working pixel colour

                    // *** The Block-Metadata palette for this column's biome
                    MiniPaletteEntry[][] BiomePalette = ColourPalette.FastPalette[Chunk.Biomes.GetBiome(X, Z)];


                    for (; Y <= EndY; Y++) // *** Now render up from the lowest block to the starting block
                    {
                        // *** For each block we render, grab its palette entry.
                        MiniPaletteEntry Entry = BiomePalette[Blocks.GetID(X, Y, Z)][Blocks.GetData(X, Y, Z)];

                        // *** If it has an associated .EntityColours list, then it needs special consideration to get its colour
                        if (Entry.EntityColours != 0)
                        {
                            PaletteEntry Entry2 = ColourPalette.GetPaletteEntry(Entry.EntityColours).Find((E) => E.IsMatch(Blocks.GetData(X, Y, Z), Blocks.SafeGetTileEntity(X, Y, Z)));
                            if (Entry2 != null)
                                TempColour = Entry2.Color;
                            else
                                TempColour = Entry.Colour;
                        }
                        else // *** No special consideration, just grab the colour in the palette
                            TempColour = Entry.Colour;

                        if (TempColour.A == 0)
                            continue; // *** If we're trying to render air, let's not.

                        // *** Blend in our working colour to the column's pixel, after applying altitude and light-level blends.
                        SetColour.Blend(TempColour.Copy().LightLevel((uint)Math.Max(Config.MinLightLevel, Blocks.GetBlockLight(X, Math.Min(Y + 1, 255), Z))).Altitude(Y));
                    }

                    if (SetColour.A > 0) // *** If our pixel isn't just transparent, then write it out to the target bitmap
                        Marshal.WriteInt32(RenderTarget.Scan0 + (Stride * (((Chunk.Z - Config.SubregionChunks.Y) << 4) + Z)) + ((((Chunk.X - Config.SubregionChunks.X) << 4) + X) << 2), (int)SetColour.FullAlpha().Color);
                }
            }
#if !DEBUG // *** When not running in debug mode, chunks that fail to render should NOT crash everything.
            }
            catch (Exception ex)
            {
                Interlocked.Increment(ref PauseRendering);

                CorruptChunks = true;
                RenderingErrorEventArgs e = new RenderingErrorEventArgs();
                e.ErrorException = ex;
                e.IsFatal = false;
                e.UserErrorMessage = "A chunk failed to render";
                e.ErrorCode = ErrorBadChunk;

                if (RenderError != null)
                    RenderError.Invoke(this, e);

                Interlocked.Decrement(ref PauseRendering);
            }
#endif
        }


        // *** Progress Updating
        void DoProgressUpdate()
        {
            ProgressUpdateEventData.ProgressShortDescription = "Rendering";
            if (ProgressUpdate != null)
            {
                ProgressUpdateEventData.Progress = (float)ProcessedChunks / (float)RenderableChunks;
                if (RenderableChunks > 0)
                    ProgressUpdateEventData.ProgressDescription = String.Format("Rendered {0} of {1} chunks ({2}%)", ProcessedChunks, RenderableChunks, (100 * ProcessedChunks) / RenderableChunks);
                else
                    ProgressUpdateEventData.ProgressDescription = "No chunks to render";
                ProgressUpdate.Invoke(this, ProgressUpdateEventData);
            }
        }
        void DisplayProgress()
        {

            while (true)
            {
                DoProgressUpdate();

                if (ProcessedChunks >= RenderableChunks)
                    return;

                if (Cancellation.IsCancellationRequested)
                    return;

                Thread.Sleep(100);
            }
        }


        // ** Initialization and control
        public void Configure(RenderConfiguration Configuration)
        {
            Config = Configuration;

            ColourPalette = Config.Palette;
            Metrics = Config.Metrics;

            Chunks = Configuration.Chunks;

            if (Config.AdvancedRenderOptions.Exists((x) => x.Key == "Mode"))
            {
                switch (Config.AdvancedRenderOptions.Find((x) => x.Key == "Mode").Value)
                {
                    case "c":
                        RenderStartY = GetStartRenderYCave;
                        break;
                    case "C":
                        RenderStartY = GetStartRenderYCaveAlternate;
                        break;
                    default:
                        RenderStartY = GetStartRenderYNormal;
                        break;
                }
            }
            else
                RenderStartY = GetStartRenderYNormal;
        }
        public void Abort()
        {
            Cancellation.Cancel();
            if (PendingRender)
            {
                OutputMap.Dispose();
                OutputMap = null;
            }
        }
        public void Initialize()
        {
            // *** Perform basic init.  Set up the bitmap, cache stride, etc

            RenderableChunks = Config.RenderSubregion ? 0 : Metrics.NumberOfChunks;
            NumFormat = "D" + Math.Ceiling(Math.Log10(RenderableChunks + 1)).ToString();

            try
            {
                OutputMap = new Bitmap((Config.SubregionChunks.Width + 1) * 16, (Config.SubregionChunks.Height + 1) * 16, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                PendingRender = true;
            }
            catch (OutOfMemoryException)
            {
                RenderingErrorEventArgs e = new RenderingErrorEventArgs();
                e.ErrorCode = ErrorNoMemory;
                e.IsFatal = true;
                e.UserErrorMessage = "Failed to allocate dimension bitmap: memory unavailable.  Try rendering a smaller area";

                if (RenderError != null)
                    RenderError.Invoke(this, e);
            }
            catch (Exception ex)
            {
                RenderingErrorEventArgs e = new RenderingErrorEventArgs();
                e.ErrorException = ex;
                e.ErrorCode = ErrorNoMemory;
                e.ShowInnerException = true;
                e.IsFatal = true;
                e.UserErrorMessage = "Failed to allocate dimension bitmap.";

                if (RenderError != null)
                    RenderError.Invoke(this, e);
            }

            RenderTarget = OutputMap.LockBits(new Rectangle(0, 0, OutputMap.Width, OutputMap.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Stride = RenderTarget.Stride;
        }
        public bool IsAborting
        {
            get { return Cancellation.IsCancellationRequested; }
        }
        public RendererConfigForm ConfigurationForm
        {
            get { return new StandardRendererConfig(); }
        }


        // *** Preview
        public Bitmap Preview()
        {
            Render();
            if (Cancellation.IsCancellationRequested)
                return new Bitmap(512, 512, PixelFormat.Format32bppArgb);
            return OutputMap;
        }


        // *** Identification and labelling
        public string RendererName
        {
            get { return "Standard"; }
        }
        public string RendererFriendlyName
        {
            get { return "Standard Renderer"; }
        }

        public void PrintHelpInfo()
        {
            Console.WriteLine("  Mode [ ]: . . . . . . . Render Mode");
            Console.WriteLine("        n:. . . . . . . . Normal rendering (default)");
            Console.WriteLine("        c:. . . . . . . . Cave rendering");
            Console.WriteLine("        C:. . . . . . . . Alternate cave rendering (ignores leaves, glass, etc)");
        }
    }
}
