using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Xml;
using Substrate;
using SeeSharp.Palette;
using SeeSharp.Rendering;

namespace SeeSharp
{
    /// <summary>
    ///     Main handler class that processes "command-line mode" for See Sharp.
    /// </summary>
    internal class CLIMain
    {
        Stopwatch StepTimer = new Stopwatch();
        List<String> LoadAdditionalPalettes = new List<string>();
        WorldMetrics Metrics = new WorldMetrics();
        PaletteCore ColourPalette = new PaletteCore();

        RenderConfiguration Configuration;
        IRenderer Renderer;

        public int HandleCLI(string[] args)
        {
            Configuration = new RenderConfiguration(ColourPalette, Metrics);

            // *** Function returns TRUE if the command line is invalid and the program should NOT proceed.
            if (HandleCommandLineArguments(args))
                return 0;

            // *** World path is mandatory.
            if (Configuration.WorldPath == "")
            {
                Console.WriteLine("A world must be specified.");
                ShowHelp();
                return 0;
            }

            // *** Load the world
            Console.WriteLine("Opening world file:");
            Console.WriteLine("    " + Configuration.WorldPath + "...");
            AnvilWorld World;


            // *** Try and open the world.  If it fails, handle it gracefully
            try
            {
                World = Substrate.AnvilWorld.Open(Configuration.WorldPath);
            }
            catch (Exception e)
            {
                Console.WriteLine("World could not be opened for reading:");
                Console.WriteLine(e.ToString());
                Console.WriteLine("Please check that the world exists and is valid.");
                return 0;
            }


            // *** Read world and compute metrics (size, chunk count, more?)
            Console.WriteLine("Determining world boundaries");
            StepTimer.Reset();
            StepTimer.Start();
            Metrics.Measure(World.GetChunkManager(Configuration.Dimension));
            StepTimer.Stop();
            Console.WriteLine("Took " + StepTimer.Elapsed.ToString("m\\ms\\sff"));
            Console.WriteLine("Boundaries: (" + Metrics.MinX.ToString() + ", " + Metrics.MinZ.ToString() + ") to (" + Metrics.MaxX.ToString() + ", " + Metrics.MaxZ.ToString() + ")");


            // *** Render dimension, if applicable
            if (Configuration.SaveFilename != "")
            {
                StepTimer.Reset();
                StepTimer.Start();

                // Load palettes.  Start with palettes in EXE directory, then append all palettes in the force-load list.
                Console.WriteLine("Loading Palettes...");
                foreach (String PalFile in Directory.EnumerateFiles(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "*.pal").Concat(LoadAdditionalPalettes))
                {
                    string PaletteFile = PalFile;

                    if (!PaletteFile.EndsWith(".pal") && !File.Exists(PaletteFile)) //Append ".pal" to filename if it's been accidentally omitted
                        PaletteFile += ".pal";

                    if (File.Exists(PaletteFile))
                    {
                        try
                        {
                            ColourPalette.LoadPalette(PaletteFile);
                        }
                        catch (PaletteCore.PaletteExecutionException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Failed to load palette " + Path.GetFileNameWithoutExtension(PaletteFile) + ":");
                            if (ex is PaletteCore.PaletteExecutionException)
                                Console.WriteLine(ex.Message);
                            else
                                Console.WriteLine("Internal Error");
                        }
                    }
                    else
                        Console.WriteLine("Skipped missing file " + PaletteFile);
                }

                ColourPalette.AssembleLookupTables();

                Console.WriteLine("Palettes loaded.  Took " + StepTimer.Elapsed.ToString("m\\ms\\sff"));
                StepTimer.Reset();

                StepTimer.Start();
                Renderer = new Renderer();

                // Set up the progress indicator
                Console.WriteLine("Using " + (Configuration.EnableMultithreading ? "multi-threaded" : "single-threaded") + " " + Renderer.RendererFriendlyName);


                Renderer.ProgressUpdate += OnRenderProgressUpdate;
                Renderer.RenderError += OnRenderError;

                Configuration.Chunks = World.GetChunkManager(Metrics.Dimension);
                if (!Configuration.RenderSubregion)
                    Configuration.SubregionChunks = new Rectangle(Metrics.MinX, Metrics.MinZ, (Metrics.MaxX - Metrics.MinX), (Metrics.MaxZ - Metrics.MinZ));
                Renderer.Configure(Configuration);
                Renderer.Initialize();

                if (!Renderer.IsAborting)
                    Renderer.Render();

                StepTimer.Stop();

                if (Renderer.IsAborting)
                    Console.WriteLine("\r\nMap export failed.");
                else
                    Console.WriteLine("\r\nMap export complete.  Took " + StepTimer.Elapsed.ToString("m\\ms\\sff"));
            }


            // *** Produce sign information file, if applicable
            if (Configuration.ScanFilename != "")
            {
                StepTimer.Reset();
                StepTimer.Start();

                SignExporter Exporter = new SignExporter();
                Exporter.Process(World.GetChunkManager(Metrics.Dimension), Metrics, Configuration.ScanFilename);

                StepTimer.Stop();
                Console.WriteLine("Took " + StepTimer.Elapsed.ToString("m\\ms\\sff"));
            }


            // *** Write world metrics, if applicable
            if (Configuration.MetricsFilename != "")
            {
                WriteMetricsFile(World);
            }

#if DEBUG
            System.Threading.Thread.Sleep(5500); //If debug mode, pause at the end to give me time to read any messages
#endif
            return 0;
        }

        void OnRenderProgressUpdate(object sender, ProgressUpdateEventArgs e)
        {
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(e.ProgressDescription);
        }

        void OnRenderError(object sender, RenderingErrorEventArgs e)
        {
            Console.WriteLine(e.UserErrorMessage);
            if (e.ShowInnerException)
                Console.WriteLine(e.ErrorException.Message);
            if (e.IsFatal)
                ((IRenderer)sender).Abort();
        }

        void WriteMetricsFile(AnvilWorld World)
        {

            XmlWriter Writer = XmlWriter.Create(Configuration.MetricsFilename);

            Writer.WriteStartElement("Metrics");
            Writer.WriteAttributeString("Dimension", Metrics.Dimension.ToString());
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
                    Writer.WriteElementString("WidthChunks", (Metrics.MaxX - Metrics.MinX + 1).ToString());
                    Writer.WriteElementString("HeightChunks", (Metrics.MaxZ - Metrics.MinZ + 1).ToString());
                }
                Writer.WriteEndElement();

                Writer.WriteStartElement("Extents");
                {
                    Writer.WriteElementString("X1", Metrics.MinX.ToString());
                    Writer.WriteElementString("Y1", Metrics.MinZ.ToString());

                    Writer.WriteElementString("X2", Metrics.MaxX.ToString());
                    Writer.WriteElementString("Y2", Metrics.MaxZ.ToString());
                }
                Writer.WriteEndElement();

                Writer.WriteElementString("ChunkCount", Metrics.NumberOfChunks.ToString());
            }

            Writer.WriteEndElement();

            Writer.Flush();
            Writer.Close();

            Console.WriteLine("Wrote metrics file to " + Path.GetFileName(Configuration.MetricsFilename));
        }

        bool HandleCommandLineArguments(string[] args)
        {
            bool DoShowHelp = false;
            for (int x = 0; x < args.Length; x++)
            {
                switch (args[x])
                {
                    case "-l":
                    case "--light-Level":
                        Configuration.MinLightLevel = int.Parse(args[++x]);
                        if (Configuration.MinLightLevel < 0 || Configuration.MinLightLevel > 15)
                        {
                            Console.WriteLine("The Minlight (--light-level) value must be between 0 and 15, inclusive.  Will use a Minlight value of 15 (full daylight).");
                            Configuration.MinLightLevel = 15;
                        }
                        break;
                    case "-w":
                    case "--world":
                        Configuration.WorldPath = args[++x];
                        break;
                    case "-s":
                    case "--signs":
                        Configuration.ScanFilename = args[++x];
                        break;
                    case "-m":
                    case "--metrics":
                        Configuration.MetricsFilename = args[++x];
                        break;
                    case "-p":
                    case "--palette":
                        LoadAdditionalPalettes.Add(args[++x]);
                        break;
                    case "-o":
                    case "--output":
                        Configuration.SaveFilename = args[++x];
                        break;
                    case "-T":
                    case "--multi-thread":
                        Configuration.EnableMultithreading = true;
                        break;
                    case "-t":
                    case "--max-threads":
                        Configuration.MaxThreads =  int.Parse(args[++x]);
                        break;
                    case "-d":
                    case "--dimension":
                        Metrics.Dimension = args[++x];
                        Configuration.Dimension = args[x];
                        break;
                    case "-S":
                    case "--subregion":
                        Configuration.RenderSubregion = true;
                        break;
                    case "-mX":
                    case "--subregion-min-x":
                        int NewLeft = int.Parse(args[++x]);
                        Configuration.SubregionChunks.Width += Configuration.SubregionChunks.Left - NewLeft;
                        Configuration.SubregionChunks.X = NewLeft;
                        break;
                    case "-mZ":
                    case "--subregion-min-Z":
                        int NewTop = int.Parse(args[++x]);
                        Configuration.SubregionChunks.Height += Configuration.SubregionChunks.Top - NewTop;
                        Configuration.SubregionChunks.Y = NewTop;
                        break;
                    case "-MX":
                    case "--subregion-max-x":
                        Configuration.SubregionChunks.Width = int.Parse(args[++x]) - Configuration.SubregionChunks.Left;
                        break;
                    case "-MZ":
                    case "--subregion-max-Z":
                        Configuration.SubregionChunks.Height = int.Parse(args[++x]) - Configuration.SubregionChunks.Top;
                        break;
                    case "-r":
                    case "--render-core":
                        Configuration.RendererName = args[++x];
                        break;
                    case "-R":
                    case "--render-option":
                            String Option = args[++x];
                            Configuration.AdvancedRenderOptions.Add(new Tuple<string, string>(Option, args[++x]));
                        break;
                    case "-ls":
                    case "--list-renderers":
                        Console.Write("Renderers:");
                        foreach (String RendererName in RendererManager.Instance().AvailableRendererCodes)
                        {
                            Console.WriteLine(String.Format("{0} - {1}", RendererName, RendererManager.Instance().GetFriendlyName(RendererName)));
                        }
                        break;
                    case "-?":
                    case "-h":
                    case "--help":
                        DoShowHelp = true;
                        break;
                    default:
                        Console.WriteLine("Error: Unknown command line parameter " + args[x]);
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

            IRenderer Renderer = RendererManager.Instance().InstantiateRenderer(!String.IsNullOrEmpty(Configuration.RendererName) ? Configuration.RendererName : "");
            Console.WriteLine("");
            Console.WriteLine(Renderer.RendererFriendlyName + " advanced options: ");
            Renderer.PrintHelpInfo();
        }

    }
}
