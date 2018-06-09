using System.Collections.Generic;

namespace SeeSharp.Palette
{
    class JsonPalette
    {
        public string Name;
        public string Version;
        public string Description;
        public string Author;
        public dynamic Defaults;
        public Dictionary<string, dynamic> Colors;
        public Dictionary<string, dynamic> Blocks;
        public Dictionary<string, dynamic> Biomes;


        public void Bake()
        {
            foreach (var Key in Blocks.Keys)
            {
                dynamic Block = Blocks[Key];

                if (Block.Color is string)
                {
                    if (Colors.ContainsKey(Block.Color))
                    {
                        Block.Color = Colors[Block.Color];
                    }
                    else
                    {
                        throw new PaletteException(string.Format("Block {0} references color \"{1}\" which is not defined", Key, Block.Color));
                    }
                } else
                {

                }
            }
        }
    }
}
