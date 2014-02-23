using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Xml;
using Substrate;
using Substrate.TileEntities;

namespace SeeSharp
{
    internal class SignExporter
    {

        Dictionary<String, Type> SignMap = new Dictionary<String, Type>();
        List<SignBase> ExportableSigns = new List<SignBase>();


        Dictionary<String, Type> CreateSignMap()
        {
            Dictionary<String, Type> SignMap = new Dictionary<String, Type>();
            List<Type> Plugins = new List<Type>();
            string FolderPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + Path.DirectorySeparatorChar + "Signs";
            if (!Directory.Exists(FolderPath))
                return SignMap;
            foreach (System.IO.FileInfo DllFile in new System.IO.DirectoryInfo(FolderPath).GetFiles("*.dll"))
            {
                try
                {
                    foreach (Type ClassDef in Assembly.LoadFrom(DllFile.FullName).GetTypes().Where((System.Type ClassType) => typeof(SignBase).IsAssignableFrom(ClassType)))
                    {
                        try
                        {
                            Plugins.Add(ClassDef);
                        }
                        catch
                        {
                            // *** Handle a broken or incompatible Class
                            continue;
                        }
                    }
                }
                catch
                {
                    // *** Handle a broken or incompatible DLL
                    continue;
                }
            }

            foreach (Type ClassDef in Plugins)
            {
                try
                {
                    SignBase Sign = (SignBase)Activator.CreateInstance(ClassDef, true);
                    SignMap.Add("[" + Sign.SignType() + "]", ClassDef);
                    Console.WriteLine("Registered sign type " + ClassDef.Name);
                } catch (Exception ex) {
                    Console.WriteLine("Failure registering plugin class " + ClassDef.AssemblyQualifiedName);
                    Console.WriteLine("    " + ex.Message);
                }
            }

            return SignMap;
        }

        public void Process(RegionChunkManager Chunks, WorldMetrics Metrics, String Filename)
        {
            // *** Accumulate list of sign types
            SignMap = CreateSignMap();


            int NumLen = Metrics.NumberOfChunks.ToString().Length;
            int NumCount = Metrics.NumberOfChunks;
            string NumFormat = "D" + NumLen.ToString();
            int ProcessedChunks = 0;
            Console.Write("Scanned " + 0.ToString(NumFormat) + " of " + Metrics.NumberOfChunks.ToString() + " chunks (0%)");
            Point ProcessedCountPoint = new Point(8, Console.CursorTop);
            Point PercentageUpdatePoint = new Point(Console.CursorLeft - 3, Console.CursorTop);


            // *** Find all signs in the world and scan
            foreach (ChunkRef Chunk in Chunks)
            {
                ProcessedChunks++;
                Console.SetCursorPosition(ProcessedCountPoint.X, ProcessedCountPoint.Y);
                Console.Write(ProcessedChunks.ToString(NumFormat));
                
                Console.SetCursorPosition(PercentageUpdatePoint.X, PercentageUpdatePoint.Y);
                Console.Write(((100 * ProcessedChunks) / NumCount).ToString() + "%)");

                ProcessChunk(Chunk);
            }
            Console.WriteLine("");
            Console.WriteLine("Scan completed.  Saving sign data...");

            XmlWriter Writer = XmlWriter.Create(Filename);

            Writer.WriteStartElement("Signs");

            // *** Curly braces used to ensure that start and end element counts are matched.
            foreach (SignBase Sign in ExportableSigns)
            {

                Writer.WriteStartElement("Sign");
                {
                    Writer.WriteAttributeString("Type", Sign.SignType());

                    Writer.WriteStartElement("Location");
                    {
                        Writer.WriteElementString("X", Sign.Location.X.ToString());
                        Writer.WriteElementString("Y", Sign.Location.Y.ToString());
                    }
                    Writer.WriteEndElement();

                    List<KeyValuePair<String, String>> Params = new List<KeyValuePair<string, string>>();

                    Sign.ExportParameters(Params);

                    foreach (KeyValuePair<String, String> Param in Params)
                    {
                        Writer.WriteStartElement(Param.Key);
                        {
                            Writer.WriteString(Param.Value);
                        }
                        Writer.WriteEndElement();
                    }

                }
                Writer.WriteEndElement();

            }

            Writer.WriteEndElement();
            
            Writer.Flush();
            Writer.Close();

            Console.WriteLine("Wrote sign data to file " + Path.GetFileName(Filename));

        }

        void ProcessChunk(ChunkRef Chunk)
        {
            AlphaBlockCollection Blocks = Chunk.Blocks;
            for (int ChunkX = 0; ChunkX < 16; ChunkX++)
            {
                for (int ChunkZ = 0; ChunkZ < 16; ChunkZ++)
                {
                    for (int ChunkY = 0; ChunkY < 128; ChunkY++)
                    {
                        try
                        {
                            TileEntity Entity = Blocks.SafeGetTileEntity(ChunkX, ChunkY, ChunkZ);
                            if (Entity is TileEntitySign)
                                ParseSignInfo((TileEntitySign)Entity, ChunkX, ChunkY, ChunkZ, Chunk);
                        }
                        catch
                        {

                        }
                    }
                }
            }
        }

        SignBase CreateSign(string Text1)
        {
            if (SignMap.ContainsKey(Text1))
                return (SignBase)Activator.CreateInstance(SignMap[Text1], true);
            return null;
        }

        void ParseSignInfo(TileEntitySign SignEntity, int X, int Y, int Z, ChunkRef Chunk)
        {
            // *** Try to create a sign from the entity.  NULL will be returned if the sign doesn't map to anything.
            SignBase Sign = CreateSign(SignEntity.Text1);
            
            if (Sign == null)
                return;

            // *** Pre-set the Location var...
            Sign.Location = new Point(SignEntity.X, SignEntity.Z);

            // *** ...and pass control to the sign's validation/setup function.
            if (!Sign.CreateFrom(SignEntity))
                return;

            // *** Now scan for and process any "rider" signs that might add additional information.
            TileEntity Ent;

            // *** This is kind of ugly but oh well
            do {
                Ent = Chunk.Blocks.SafeGetTileEntity(X, --Y, Z);
            } while (Y > 0 && Ent is TileEntitySign && Sign.AddSign((TileEntitySign)Ent));

            if (Sign.IsValid())
                ExportableSigns.Add(Sign);

        }
    }
}
