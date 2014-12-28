using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using SeeSharp.Palette;
using SeeSharp.Plugins;
using Substrate;
// ReSharper disable PossibleMultipleEnumeration

namespace SeeSharp.Rendering
{
    internal sealed class Renderer : IRenderer
    {
        // *** Error Codes
        private const Int32 ErrorNoMemory = -1;  // *** ERROR: Couldn't allocate bitmap.  Fatal, because w/o a bitmap wtf are you rendering to?
        // ReSharper disable once UnusedMember.Local
        private const Int32 ErrorBadChunk = 1;   // *** ERROR: There was a bad chunk that couldn't be rendered.  Not a fatal error.  Only used in Release config.
        private const Int32 ErrorErrors = 2;     // *** ERROR: There were errors in the render!  Makes sense in context.


        // *** Configuration data
        private BlockPalette _ColourPalette = new BlockPalette();
        private WorldMetrics _Metrics;
        private RenderConfiguration _Config;
        private RegionChunkManager _Chunks;


        // *** Rendering data
        private Bitmap _OutputMap;
        private BitmapData _RenderTarget;
        private Int32 _Stride;
        private Func<AlphaBlockCollection, int, int, int> _RenderStartY;
        private readonly CancellationTokenSource _Cancellation = new CancellationTokenSource();
        private readonly ParallelOptions _RenderingParallelOptions = new ParallelOptions();
        private readonly ProgressUpdateEventArgs _ProgressUpdateEventData = new ProgressUpdateEventArgs();

        // ** Chunk Stuff
        private int _RenderableChunks;


        // *** Progress Update Data
        private int _ProcessedChunks;
        private bool _PendingRender;
        private int _ActiveRenderThreads;


        // *** Not used in release mode
#pragma warning disable 649
        private int _PauseRendering;
        private bool _CorruptChunks;
#pragma warning restore 649

        // *** Events
        public event ProgressUpdateHandler ProgressUpdate;
        public event RenderingErrorHandler RenderError;


        // *** Render start determiners
        private int GetStartRenderYNormal(AlphaBlockCollection Blocks, int X, int Z)
        {
            int Y = Blocks.GetHeight(X, Z);
            if (Y > 255)
                Y = 255;
            if (Y == 0)
                return _ColourPalette.DepthOpacities[Blocks.GetID(X, Y, Z)][Blocks.GetData(X, Y, Z)] == 0 ? -1 : 0;
            return Y;
        }
        private int GetStartRenderYCave(AlphaBlockCollection Blocks, int X, int Z)
        {
            int Y = 255;
            if (Y > 255)
                Y = 255;

            while (Y >= 0 && _ColourPalette.FastPalette[0][Blocks.GetID(X, Y, Z)][Blocks.GetData(X, Y, Z)].A == 0)
                Y--;
            while (Y >= 0 && _ColourPalette.FastPalette[0][Blocks.GetID(X, Y, Z)][Blocks.GetData(X, Y, Z)].A != 0)
                Y--;
            while (Y >= 0 && _ColourPalette.FastPalette[0][Blocks.GetID(X, Y, Z)][Blocks.GetData(X, Y, Z)].A == 0)
                Y--;

            if (Y == 0)
                return _ColourPalette.DepthOpacities[Blocks.GetID(X, Y, Z)][Blocks.GetData(X, Y, Z)] == 0 ? -1 : 0;
            return Y;
        }
        private int GetStartRenderYCaveAlternate(AlphaBlockCollection Blocks, int X, int Z)
        {
            int Y = 255;
            if (Y > 255)
                Y = 255;

            while (Y >= 0 && _ColourPalette.FastPalette[0][Blocks.GetID(X, Y, Z)][Blocks.GetData(X, Y, Z)].A < 255) // Find first renderable fully opaque block
                Y--;
            while (Y >= 0 && _ColourPalette.FastPalette[0][Blocks.GetID(X, Y, Z)][Blocks.GetData(X, Y, Z)].A != 0)
                Y--;
            while (Y >= 0 && _ColourPalette.FastPalette[0][Blocks.GetID(X, Y, Z)][Blocks.GetData(X, Y, Z)].A == 0)
                Y--;

            if (Y == 0)
                return _ColourPalette.DepthOpacities[Blocks.GetID(X, Y, Z)][Blocks.GetData(X, Y, Z)] == 0 ? -1 : 0;

            return Y;
        }


        // *** Rendering Functions
        public void Render()
        {

            // *** Define the chunk query, in order to cleanly support subregions
            IEnumerable<ChunkRef> ChunkProvider = from ChunkRef Chunk in _Chunks
                                                  where !_Config.RenderSubregion || _Config.SubregionChunks.ContainsPoint(Chunk.X, Chunk.Z)
                                                  select Chunk;

            _PendingRender = false;

            // *** If rendering a subregion, then wipe the _RenderableChunks var and recalculate it from the number of chunks in the subregion, not the world
            if (_Config.RenderSubregion)
            {
                _RenderableChunks = ChunkProvider.Count();
            }

            // *** If rendering was cancelled between init and now, abort.
            if (_Cancellation.IsCancellationRequested)
            {
                _OutputMap.Dispose();
                _OutputMap = null;
                return;
            }

            // *** Render
            if (_Config.EnableMultithreading)
            {
                Thread UpdateThread = new Thread(DisplayProgress);
                UpdateThread.Start();

                _RenderingParallelOptions.CancellationToken = _Cancellation.Token;
                _RenderingParallelOptions.MaxDegreeOfParallelism = _Config.MaxThreads;

                try
                {
                    Parallel.ForEach(ChunkProvider, _RenderingParallelOptions, RenderChunk);
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
                    if (_Cancellation.IsCancellationRequested)
                        break;
                    DoProgressUpdate();
                }
            }

            _OutputMap.UnlockBits(_RenderTarget);

            if (_Cancellation.IsCancellationRequested)
            {
                _OutputMap.Dispose();
                _OutputMap = null;
                return;
            }

            // *** If this is not a preview, then make sure the output directory exists then save the output bitmap
            if (!_Config.IsPreview)
            {
                // ReSharper disable once AssignNullToNotNullAttribute
                Directory.CreateDirectory(Path.GetDirectoryName(_Config.SaveFilename));
                _OutputMap.Save(_Config.SaveFilename);
                _OutputMap.Dispose();
                _OutputMap = null;
            }

            // *** If a chunk failed to render, let the user know.  Unless we aborted, because there's no point then.
            if (_CorruptChunks)
            {
                RenderingErrorEventArgs E = new RenderingErrorEventArgs
                {
                    ErrorCode = ErrorErrors,
                    IsFatal = true,
                    UserErrorMessage =
                        "WARNING: At least one potentially corrupt chunk was encountered during rendering.\r\nThe map image may contain missing sections as a result."
                };

                if (RenderError != null)
                    RenderError.Invoke(this, E);
            }
        }
        // ReSharper disable once FunctionComplexityOverflow
        // *** I do this because this function is the 'hottest' block of code in the program, and saving cycles during execution is ridiculously important here.
        void RenderChunk(ChunkRef Chunk, ParallelLoopState LoopState)
        {
            // *** Track how many chunks have been processed, for user feedback
            Interlocked.Increment(ref _ProcessedChunks);
            Interlocked.Increment(ref _ActiveRenderThreads);
#if !DEBUG
            // *** In release mode, gracefully handle bad chunks.  Explode in debug mode so I can track down the issue.
            try
            {
#endif

            // *** Cancellation logic for parallel processing    
            if (LoopState != null && _Cancellation.IsCancellationRequested)
                LoopState.Stop();

            if (LoopState != null && LoopState.IsStopped)
            {
                Interlocked.Decrement(ref _ActiveRenderThreads);
                return;
            }

            // *** Hold off on rendering if the user needs to attend to an issue
            while (_PauseRendering > 0)
                Thread.Sleep(50);

            // *** Load the chunk from disk here
            AlphaBlockCollection Blocks = Chunk.Blocks;

            for (int X = 0; X < 16; X++)
            {
                for (int Z = 0; Z < 16; Z++)
                {
                    // *** Start by finding the topmost block to render
                    int EndY = _RenderStartY(Blocks, X, Z);
                    int Y = EndY;
                    int RenderVal = 255;

                    if (Y < 0)
                        continue; // *** No valid renderable blocks in this column, so continue with the next column

                    // *** Drill into the column to determine how many blocks down to render
                    while (RenderVal > 0)
                    {
                        RenderVal -= _ColourPalette.DepthOpacities[Blocks.GetID(X, Y, Z)][Blocks.GetData(X, Y, Z)];
                        if (Y == 0) // *** If we've hit the bottom of the map, don't try and keep going.
                            break;  // *** It wouldn't end well.
                        Y--;
                    }

                    Colour SetColour = Colour.Transparent; // *** What colour to set the current column's pixel to.

                    // *** The Block-Metadata palette for this column's biome
                    Colour[][] BiomePalette = _ColourPalette.FastPalette[Chunk.Biomes.GetBiome(X, Z)];


                    for (; Y <= EndY; Y++) // *** Now render up from the lowest block to the starting block
                    {
                        // *** For each block we render, grab its palette entry.
                        Colour Entry = BiomePalette[Blocks.GetID(X, Y, Z)][Blocks.GetData(X, Y, Z)];
                        Colour TempColour; // *** Working pixel colour

                        // *** If it has an associated entity colours list, then it needs special consideration to get its colour
                        if ((Entry.Color & 0xFFFF0000U) == 0x00FF0000U)
                        {
                            PaletteEntry Entry2 = _ColourPalette.GetPaletteEntry((int)(Entry.Color & 0x0000FFFFU)).First(e => e.IsMatch(Blocks.GetData(X, Y, Z), Blocks.SafeGetTileEntity(X, Y, Z)));
                            if (Entry2 != null)
                                TempColour = Entry2.Color;
                            else
                                TempColour = Entry;
                        }
                        else // *** No special consideration, just grab the colour in the palette
                            TempColour = Entry;

                        if (TempColour.A == 0)
                            continue; // *** If we're trying to render air, let's not.

                        // *** Blend in our working colour to the column's pixel, after applying altitude and light-level blends.
                        SetColour.Blend(TempColour.Copy().LightLevel((uint)Math.Max(_Config.MinLightLevel, Blocks.GetBlockLight(X, Math.Min(Y + 1, 255), Z))).Altitude(Y));
                    }

                    if (SetColour.A > 0) // *** If our pixel isn't just transparent, then write it out to the target bitmap
                        Marshal.WriteInt32(_RenderTarget.Scan0 + (_Stride * (((Chunk.Z - _Config.SubregionChunks.Y) << 4) + Z)) + ((((Chunk.X - _Config.SubregionChunks.X) << 4) + X) << 2), (int)SetColour.FullAlpha().Color);
                }
            }
#if !DEBUG // *** When not running in debug mode, chunks that fail to render should NOT crash everything.
            }
            catch (Exception Ex)
            {
                Interlocked.Increment(ref _PauseRendering);

                _CorruptChunks = true;
                RenderingErrorEventArgs E = new RenderingErrorEventArgs
                {
                    ErrorException = Ex,
                    IsFatal = false,
                    UserErrorMessage = "A chunk failed to render",
                    ErrorCode = ErrorBadChunk
                };

                if (RenderError != null)
                    RenderError.Invoke(this, E);

                Interlocked.Decrement(ref _PauseRendering);
            }
#endif
            Interlocked.Decrement(ref _ActiveRenderThreads);
        }


        // *** Progress Updating
        void DoProgressUpdate()
        {
            _ProgressUpdateEventData.ProgressShortDescription = "Rendering";
            if (ProgressUpdate != null)
            {
                _ProgressUpdateEventData.Progress = _ProcessedChunks / (float)_RenderableChunks;
                if (_RenderableChunks > 0)
                {
                    _ProgressUpdateEventData.ProgressDescription = String.Format("Rendered {0} of {1} chunks ({2}%)",
                        _ProcessedChunks, _RenderableChunks, (100*_ProcessedChunks)/_RenderableChunks);
                }
                else
                {
                    _ProgressUpdateEventData.ProgressDescription = "No chunks to render";
                }
                ProgressUpdate.Invoke(this, _ProgressUpdateEventData);
            }
        }
        void DisplayProgress()
        {

            while (true)
            {
                DoProgressUpdate();

                if (_ProcessedChunks >= _RenderableChunks)
                    return;

                if (_Cancellation.IsCancellationRequested)
                    return;

                Thread.Sleep(100);
            }
        }


        // ** Initialization and control
        public void Configure(RenderConfiguration Configuration)
        {
            _Config = Configuration;

            _ColourPalette = _Config.Palette;
            _Metrics = _Config.Metrics;

            _Chunks = Configuration.Chunks;

            if (_Config.AdvancedRenderOptions.Exists(x => x.Key == "Mode"))
            {
                switch (_Config.AdvancedRenderOptions.Find(x => x.Key == "Mode").Value)
                {
                    case "c":
                        _RenderStartY = GetStartRenderYCave;
                        break;
                    case "C":
                        _RenderStartY = GetStartRenderYCaveAlternate;
                        break;
                    default:
                        _RenderStartY = GetStartRenderYNormal;
                        break;
                }
            }
            else
                _RenderStartY = GetStartRenderYNormal;
        }
        public void Abort()
        {
            _Cancellation.Cancel();

            if (!_PendingRender)
                return;

            // *** Wait for renderers to exit before disposing
            while(_ActiveRenderThreads > 0)
                Thread.Sleep(10);

            _OutputMap.Dispose();
            _OutputMap = null;
        }
        public void Initialize()
        {
            // *** Perform basic init.  Set up the bitmap, cache stride, etc

            _RenderableChunks = _Config.RenderSubregion ? 0 : _Metrics.NumberOfChunks;

            try
            {
                _OutputMap = new Bitmap((_Config.SubregionChunks.Width + 1) * 16, (_Config.SubregionChunks.Height + 1) * 16, PixelFormat.Format32bppArgb);
                _PendingRender = true;
            }
            catch (OutOfMemoryException)
            {
                RenderingErrorEventArgs E = new RenderingErrorEventArgs
                {
                    ErrorCode = ErrorNoMemory,
                    IsFatal = true,
                    UserErrorMessage =
                        "Failed to allocate dimension bitmap: memory unavailable.  Try rendering a smaller area"
                };

                if (RenderError != null)
                    RenderError.Invoke(this, E);
            }
            catch (Exception Ex)
            {
                RenderingErrorEventArgs E = new RenderingErrorEventArgs
                {
                    ErrorException = Ex,
                    ErrorCode = ErrorNoMemory,
                    ShowInnerException = true,
                    IsFatal = true,
                    UserErrorMessage = "Failed to allocate dimension bitmap."
                };

                if (RenderError != null)
                    RenderError.Invoke(this, E);
            }

            _RenderTarget = _OutputMap.LockBits(new Rectangle(0, 0, _OutputMap.Width, _OutputMap.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            _Stride = _RenderTarget.Stride;
        }
        public bool IsAborting
        {
            get { return _Cancellation.IsCancellationRequested; }
        }
        public RendererConfigForm ConfigurationForm
        {
            get { return new StandardRendererConfig(); }
        }


        // *** Preview
        public Bitmap Preview()
        {
            Render();
            if (_Cancellation.IsCancellationRequested)
                return new Bitmap(512, 512, PixelFormat.Format32bppArgb);
            return _OutputMap;
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
