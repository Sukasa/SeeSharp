using System;
using System.Reflection;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace SeeSharp.Palette
{
    internal class PaletteManager
    {
        private static PaletteManager _Instance;

        private List<PaletteFile> Palettes = new List<PaletteFile>();
        private ReadOnlyCollection<PaletteFile> _Palettes_ReadOnly;

        public ReadOnlyCollection<PaletteFile> AllPalettes
        {
            get
            {
                if (_Palettes_ReadOnly == null)
                    _Palettes_ReadOnly = new ReadOnlyCollection<PaletteFile>(Palettes);
                return _Palettes_ReadOnly;
            }
        }

        private PaletteManager()
        {
            // Load palettes here
            Reload();

        }

        public void AutoConfig(String WorldPath, String RendererInternalName)
        {
            foreach (PaletteFile File in Palettes)
                File.Selected = File.Selected || System.IO.File.Exists(WorldPath + Path.DirectorySeparatorChar + File.AssociatedCfgFile) || RendererInternalName == File.AssociatedRenderer;
        }

        public void Reload()
        {

            List<String> PaletteFilenames = new List<String>();
            Palettes.Clear();

            List<String> Folders = new List<string>();
            Folders.Add(Path.GetDirectoryName((Assembly.GetExecutingAssembly().Location)));

            for (int I = 0; I < Folders.Count; I++)
            {
                String Folder = Folders[I];
                Folders.AddRange(Directory.GetDirectories(Folder));
                PaletteFilenames.AddRange(Directory.EnumerateFiles(Folder, "*.pal"));
            }

                foreach (string File in PaletteFilenames)
                    Palettes.Add(new PaletteFile(File));

            _Palettes_ReadOnly = null;
        }

        public static PaletteManager Instance()
        {
            if (_Instance == null)
                _Instance = new PaletteManager();
            return _Instance;
        }
    }
}
