using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeSharp.Palette
{
    class JsonPaletteFile
    {
        public string Name;
        public string Version;
        public string Description;
        public string Author;
        public dynamic Defaults;
        public Dictionary<string, dynamic> Colors;
        public Dictionary<string, dynamic> Blocks;
        public Dictionary<string, dynamic> Biomes;


    }
}
