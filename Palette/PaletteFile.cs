using System;
using System.Collections.Generic;
using System.IO;

namespace SeeSharp.Palette
{
    internal class PaletteFile
    {
        public string Name;
        public string Version;
        public string PalettePath;
        public string Author;
        public string Description;

        public string AssociatedCfgFile;
        public string AssociatedRenderer;

        public bool Selected;

        public Dictionary<Tuple<int, int>, String> IdAutoConfig = new Dictionary<Tuple<int,int>,string>();
        public List<PaletteEntry> PaletteEntries = new List<PaletteEntry>();

        public PaletteFile(String Filename)
        {
            Name = Path.GetFileName(Filename);
            Version = String.Empty;
            Author = "Unknown";
            Description = Filename;

            PalettePath = Filename;

            ReadMetadata();
        }

        public void ReadMetadata()
        {
            StreamReader S = File.OpenText(PalettePath);

            String Line;

            while (!String.IsNullOrEmpty(Line = S.ReadLine()) && Line.TrimStart().StartsWith("#"))
            {
                Line = Line.TrimStart(' ', '#');
                String[] LineParts = Line.Split('=');
                if (LineParts.Length < 2)
                    continue;

                switch (LineParts[0].ToLower())
                {
                    case "name":
                        Name = LineParts[1];
                        Selected = true;
                        break;
                    case "version":
                        Version = LineParts[1];
                        Selected = true;
                        break;
                    case "author":
                        Author = LineParts[1];
                        Selected = true;
                        break;
                    case "desc":
                    case "description":
                        Description = LineParts[1];
                        Selected = true;
                        break;
                    case "renderer":
                        AssociatedRenderer = LineParts[1];
                        Selected = true;
                        break;
                    case "config":
                        AssociatedCfgFile = LineParts[1];
                        Selected = true;
                        break;
                }
            }
            S.Close();
        }
    }
}
