using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;

namespace SeeSharp.Palette
{
    internal class PaletteManager
    {
        private static PaletteManager _Instance;

        private readonly List<PaletteFile> _Palettes = new List<PaletteFile>();
        private ReadOnlyCollection<PaletteFile> _PalettesReadOnly;

        public ReadOnlyCollection<PaletteFile> AllPalettes
        {
            get
            {
                if (_PalettesReadOnly == null)
                    _PalettesReadOnly = new ReadOnlyCollection<PaletteFile>(_Palettes);
                return _PalettesReadOnly;
            }
        }

        private PaletteManager()
        {
            // *** Load palettes here
            Reload();

        }

        public void AutoConfig(String WorldPath, String RendererInternalName)
        {
            foreach (PaletteFile File in _Palettes)
                File.Selected = File.Selected || System.IO.File.Exists(WorldPath + Path.DirectorySeparatorChar + File.AssociatedCfgFile) || RendererInternalName == File.AssociatedRenderer;
        }

        public void Reload()
        {

            List<String> PaletteFilenames = new List<String>();
            _Palettes.Clear();

            List<String> Folders = new List<string> {Path.GetDirectoryName((Assembly.GetExecutingAssembly().Location))};

            for (int I = 0; I < Folders.Count; I++)
            {
                String Folder = Folders[I];
                Folders.AddRange(Directory.GetDirectories(Folder));
                PaletteFilenames.AddRange(Directory.EnumerateFiles(Folder, "*.pal"));
            }

                foreach (string File in PaletteFilenames)
                    _Palettes.Add(new PaletteFile(File));

            _PalettesReadOnly = null;
        }

        public static PaletteManager Instance()
        {
            return _Instance ?? (_Instance = new PaletteManager());
        }
    }
}
