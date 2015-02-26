using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Xml;
using SeeSharp.Palette;
using SeeSharp.Plugins;
using SeeSharp.Rendering;
using SeeSharp.Signs;
using Substrate;

namespace SeeSharp
{
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    //    Main handler class that processes "command-line mode" for See Sharp.
    internal class CliMain
    {
        readonly Stopwatch _StepTimer = new Stopwatch();
        readonly List<String> _LoadAdditionalPalettes = new List<string>();
        readonly WorldMetrics _Metrics = new WorldMetrics();
        readonly BlockPalette _ColourPalette = new BlockPalette();

        RenderConfiguration _Configuration;
        IRenderer _Renderer;

        public int HandleCli(string[] Args)
        {
            _Configuration = new RenderConfiguration(_ColourPalette, _Metrics);

            // *** Function returns TRUE if the command line is invalid and the program should NOT proceed.
            if (HandleCommandLineArguments(Args))
                return 0;


            // *** World path is mandatory.
            if (_Configuration.WorldPath == "")
            {
                Console.WriteLine("A world must be specified.");
                ShowHelp();
                return 0;
            }

            // *** Load the world
            Console.WriteLine("Opening world file:");
            Console.WriteLine("    " + _Configuration.WorldPath + "...");
            AnvilWorld World;


            // *** Try and open the world.  If it fails, handle it gracefully
            try
            {
                World = AnvilWorld.Open(_Configuration.WorldPath);
            }
            catch (Exception Ex)
            {
                Console.WriteLine("World could not be opened for reading:");
                Console.WriteLine(Ex.ToString());
                Console.WriteLine("Please check that the world exists and is valid.");
                return 0;
            }


            // *** Read world and compute metrics (size, chunk count, more?)
            Console.WriteLine("Determining world boundaries");
            _StepTimer.Reset();
            _StepTimer.Start();
            _Metrics.Measure(World.GetChunkManager(_Configuration.Dimension));
            _StepTimer.Stop();
            Console.WriteLine("Took " + _StepTimer.Elapsed.ToString("m\\ms\\sff"));
            Console.WriteLine("Boundaries: (" + _Metrics.MinX.ToString() + ", " + _Metrics.MinZ.ToString() + ") to (" + _Metrics.MaxX.ToString() + ", " + _Metrics.MaxZ.ToString() + ")");


            // *** Render dimension, if applicable
            if (_Configuration.SaveFilename != "")
            {
                _StepTimer.Reset();
                _StepTimer.Start();

                // *** Load palettes.  Start with palettes in EXE directory, then append all palettes in the force-load list.
                Console.WriteLine("Loading Palettes...");
                // ReSharper disable once AssignNullToNotNullAttribute
                // *** Safe to assume our EXE will never not be in a folder.
                foreach (String PalFile in Directory.EnumerateFiles(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "*.pal").Concat(_LoadAdditionalPalettes))
                {
                    string PaletteFile = PalFile;

                    if (!PaletteFile.EndsWith(".pal") && !File.Exists(PaletteFile)) //Append ".pal" to filename if it's been accidentally omitted
                        PaletteFile += ".pal";

                    if (File.Exists(PaletteFile))
                    {
                        try
                        {
                            _ColourPalette.LoadPalette(PaletteFile);
                        }
                        catch (BlockPalette.PaletteExecutionException Ex)
                        {
                            Console.WriteLine(Ex.Message);
                        }
                        catch (Exception Ex)
                        {
                            Console.WriteLine("Failed to load palette " + Path.GetFileNameWithoutExtension(PaletteFile) + ":");
                            if (Ex is BlockPalette.PaletteExecutionException)
                                Console.WriteLine(Ex.Message);
                            else
                                Console.WriteLine("Internal Error");
                        }
                    }
                    else
                        Console.WriteLine("Skipped missing file " + PaletteFile);
                }

                _ColourPalette.AssembleLookupTables();

                Console.WriteLine("Palettes loaded.  Took " + _StepTimer.Elapsed.ToString("m\\ms\\sff"));
                _StepTimer.Reset();

                _StepTimer.Start();
                _Renderer = new Renderer();

                // *** Set up the progress indicator
                Console.WriteLine("Using " + (_Configuration.EnableMultithreading ? "multi-threaded" : "single-threaded") + " " + _Renderer.RendererFriendlyName);


                _Renderer.ProgressUpdate += OnRenderProgressUpdate;
                _Renderer.RenderError += OnRenderError;

                _Configuration.Chunks = World.GetChunkManager(_Metrics.Dimension);
                if (!_Configuration.RenderSubregion)
                    _Configuration.SubregionChunks = new Rectangle(_Metrics.MinX, _Metrics.MinZ, (_Metrics.MaxX - _Metrics.MinX), (_Metrics.MaxZ - _Metrics.MinZ));
                _Renderer.Configure(_Configuration);
                _Renderer.Initialize();

                if (!_Renderer.IsAborting)
                    _Renderer.Render();

                _StepTimer.Stop();

                if (_Renderer.IsAborting)
                    Console.WriteLine("\r\nMap export failed.");
                else
                    Console.WriteLine("\r\nMap export complete.  Took " + _StepTimer.Elapsed.ToString("m\\ms\\sff"));
            }


            // *** Produce sign information file, if applicable
            if (_Configuration.ScanFilename != "")
            {
                _StepTimer.Reset();
                _StepTimer.Start();

                SignExporter Exporter = new SignExporter();
                Exporter.Process(World.GetChunkManager(_Metrics.Dimension), _Metrics, _Configuration.ScanFilename);

                _StepTimer.Stop();
                Console.WriteLine("Took " + _StepTimer.Elapsed.ToString("m\\ms\\sff"));
            }


            // *** Write world metrics, if applicable
            if (_Configuration.MetricsFilename != "")
            {
                WriteMetricsFile(World);
            }

#if DEBUG
            System.Threading.Thread.Sleep(5500); // *** If debug mode, pause at the end to give me time to read any messages
#endif
            return 0;
        }

        void OnRenderProgressUpdate(object Sender, ProgressUpdateEventArgs E)
        {
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(E.ProgressDescription);
        }

        void OnRenderError(object Sender, RenderingErrorEventArgs E)
        {
            Console.WriteLine(E.UserErrorMessage);
            if (E.ShowInnerException)
                Console.WriteLine(E.ErrorException.Message);
            if (E.IsFatal)
                ((IRenderer)Sender).Abort();
        }

        void WriteMetricsFile(AnvilWorld World)
        {

            XmlWriter Writer = XmlWriter.Create(_Configuration.MetricsFilename);

            Writer.WriteStartElement("Metrics");
            Writer.WriteAttributeString("Dimension", _Metrics.Dimension);
            Writer.WriteAttributeString("Name", World.Level.LevelName);

            {

                Writer.WriteStartElement("World");
                {
                    Writer.WriteElementString("Filesize", World.Level.SizeOnDisk.ToString());
                    Writer.WriteElementString("Seed", World.Level.RandomSeed.ToString());
                    Writer.WriteElementString("LastPlayed", World.Level.LastPlayed.ToString());
                    Writer.WriteElementString("IsHardcore", World.Level.Hardcore.ToString());
                    Writer.WriteElementString("GameType", World.Level.GameType.ToString());
                    Writer.WriteElementString("GenerateStructures", World.Level.UseMapFeatures.ToString());
                    Writer.WriteElementString("VersionNumber", World.Level.Version.ToString());

                    Writer.WriteStartElement("SpawnPoint");
                    {
                        Writer.WriteElementString("X", World.Level.Spawn.X.ToString());
                        Writer.WriteElementString("Y", World.Level.Spawn.Y.ToString());
                        Writer.WriteElementString("Z", World.Level.Spawn.Z.ToString());
                    }
                    Writer.WriteEndElement();

                }
                Writer.WriteEndElement();

                Writer.WriteStartElement("Size");
                {
                    Writer.WriteElementString("WidthChunks", (_Metrics.MaxX - _Metrics.MinX + 1).ToString());
                    Writer.WriteElementString("HeightChunks", (_Metrics.MaxZ - _Metrics.MinZ + 1).ToString());
                }
                Writer.WriteEndElement();

                Writer.WriteStartElement("Extents");
                {
                    Writer.WriteElementString("X1", _Metrics.MinX.ToString());
                    Writer.WriteElementString("Y1", _Metrics.MinZ.ToString());

                    Writer.WriteElementString("X2", _Metrics.MaxX.ToString());
                    Writer.WriteElementString("Y2", _Metrics.MaxZ.ToString());
                }
                Writer.WriteEndElement();

                Writer.WriteElementString("ChunkCount", _Metrics.NumberOfChunks.ToString());
            }

            Writer.WriteEndElement();

            Writer.Flush();
            Writer.Close();

            Console.WriteLine("Wrote metrics file to " + Path.GetFileName(_Configuration.MetricsFilename));
        }

        // ReSharper disable once FunctionComplexityOverflow
        // *** It's a freaking switch.  Not exactly overly complex.
        bool HandleCommandLineArguments(IList<string> Args)
        {
            bool DoShowHelp = false;
            for (int X = 0; X < Args.Count; X++)
            {
                switch (Args[X])
                {
                    case "-l":
                    case "--light-Level":
                        _Configuration.MinLightLevel = int.Parse(Args[++X]);
                        if (_Configuration.MinLightLevel < 0 || _Configuration.MinLightLevel > 15)
                        {
                            Console.WriteLine("The Minlight (--light-level) value must be between 0 and 15, inclusive.  Will use a Minlight value of 15 (full daylight).");
                            _Configuration.MinLightLevel = 15;
                        }
                        break;
                    case "-w":
                    case "--world":
                        _Configuration.WorldPath = Args[++X];
                        break;
                    case "-s":
                    case "--signs":
                        _Configuration.ScanFilename = Args[++X];
                        break;
                    case "-m":
                    case "--metrics":
                        _Configuration.MetricsFilename = Args[++X];
                        break;
                    case "-p":
                    case "--palette":
                        _LoadAdditionalPalettes.Add(Args[++X]);
                        break;
                    case "-o":
                    case "--output":
                        _Configuration.SaveFilename = Args[++X];
                        break;
                    case "-T":
                    case "--multi-thread":
                        _Configuration.EnableMultithreading = true;
                        break;
                    case "-t":
                    case "--max-threads":
                        _Configuration.MaxThreads =  int.Parse(Args[++X]);
                        break;
                    case "-d":
                    case "--dimension":
                        _Metrics.Dimension = Args[++X];
                        _Configuration.Dimension = Args[X];
                        break;
                    case "-S":
                    case "--subregion":
                        _Configuration.RenderSubregion = true;
                        break;
                    case "-mX":
                    case "--subregion-min-x":
                        int NewLeft = int.Parse(Args[++X]);
                        _Configuration.SubregionChunks.Width += _Configuration.SubregionChunks.Left - NewLeft;
                        _Configuration.SubregionChunks.X = NewLeft;
                        break;
                    case "-mZ":
                    case "--subregion-min-Z":
                        int NewTop = int.Parse(Args[++X]);
                        _Configuration.SubregionChunks.Height += _Configuration.SubregionChunks.Top - NewTop;
                        _Configuration.SubregionChunks.Y = NewTop;
                        break;
                    case "-MX":
                    case "--subregion-max-x":
                        _Configuration.SubregionChunks.Width = int.Parse(Args[++X]) - _Configuration.SubregionChunks.Left;
                        break;
                    case "-MZ":
                    case "--subregion-max-Z":
                        _Configuration.SubregionChunks.Height = int.Parse(Args[++X]) - _Configuration.SubregionChunks.Top;
                        break;
                    case "-r":
                    case "--render-core":
                        _Configuration.RendererName = Args[++X];
                        break;
                    case "-R":
                    case "--render-option":
                            String Option = Args[++X];
                            _Configuration.AdvancedRenderOptions.Add(new KeyValuePair<string, string>(Option, Args[++X]));
                        break;
                    case "-ls":
                    case "--list-renderers":
                        Console.Write("Renderers:");
                        foreach (String RendererName in RendererManager.Instance().AvailableRendererCodes)
                        {
                            Console.WriteLine(RendererName + " - " + RendererManager.Instance().GetFriendlyName(RendererName));
                        }
                        break;
                    case "-?":
                    case "-h":
                    case "--help":
                        DoShowHelp = true;
                        break;
                    default:
                        Console.WriteLine("Error: Unknown command line parameter " + Args[X]);
                        ShowHelp();
                        return true;
                }
            }
            if (DoShowHelp)
            {
                ShowHelp();
                return true;
            }
            return false;
        }

        void ShowHelp()
        {
            Console.WriteLine("SeeSharp Syntax:  SeeSharp ( params )");
            Console.WriteLine("    -?, -h: . . . . . . Show this help and exit.");
            Console.WriteLine("    -w world/path:  . . Path to save folder.  Mandatory.");
            Console.WriteLine("    -o output/path: . . Where to save exported map image.  Omit to skip this step.");
            Console.WriteLine("    -T :. . . . . . . . Use multithreaded renderer.");
            Console.WriteLine("    -m output/path: . . Where to save world metrics.    Omit to skip this step.");
            Console.WriteLine("    -s output/path: . . Where to save sign information.    Omit to skip this step.");
            Console.WriteLine("    -p palette/path:. . Add an additional palette to the renderer, ex for custom blocks");
            Console.WriteLine("    -l [0..15]: . . . . Minimum light value to render.  Defaults to 15 if skipped.");
            Console.WriteLine("    -d #: . . . . . . . Dimension to render.  Defaults to 0 / Overworld.");
            Console.WriteLine("    -r Renderer-Name: . Which renderer to use.  'Standard' is built in");
            Console.WriteLine("    -R Option Value:. . Advanced renderer option");

            IRenderer Renderer = RendererManager.Instance().InstantiateRenderer(!String.IsNullOrEmpty(_Configuration.RendererName) ? _Configuration.RendererName : "");
            Console.WriteLine("");
            Console.WriteLine(Renderer.RendererFriendlyName + " advanced options: ");
            Renderer.PrintHelpInfo();
        }

    }
}
