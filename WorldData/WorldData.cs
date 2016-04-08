using System;
using System.Reflection;
using System.IO;
using Substrate;
using Substrate.Nbt;
using Substrate.Core;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SeeSharp.WorldData
{
    class WorldData
    {
        public IList<string> LoadedMods
        {
            get
            {
                return _LoadedMods.AsReadOnly();
            }
        }
        public IDictionary<string, int> BlockIDs { get; protected set; }

        private List<string> _LoadedMods;

        public Boolean IsModPresent(string ModId)
        {
            return _LoadedMods.Contains(ModId);
        }

        public WorldData(string WorldPath)
        {
            _LoadedMods = new List<string>();
            BlockIDs = new Dictionary<string, int>();

            NBTFile LevelFile = new NBTFile(Path.Combine(WorldPath, "level.dat"));
            NbtTree LevelTree;

            using (Stream nbtstr = LevelFile.GetDataInputStream())
            {
                LevelTree = new NbtTree(nbtstr);
            }

            _LoadedMods.Add("minecraft");

            if (LevelTree.Root.ContainsKey("FML"))
            {
                TagNodeList IDList = (TagNodeList)(((TagNodeCompound)LevelTree.Root["FML"])["ItemData"]);
                TagNodeList ModList = (TagNodeList)(((TagNodeCompound)LevelTree.Root["FML"])["ModList"]);
                
                foreach(TagNodeCompound Entry in IDList) {
                    string Key = ((TagNodeString)Entry["K"]).Data.Trim(' ', '',''); // Non-visible control characters in those last two entries
                    int Value = ((TagNodeInt)Entry["v"]).Data;

                    BlockIDs.Add(Key, Value);
                }

                foreach (TagNodeCompound Entry in ModList)
                {
                    _LoadedMods.Add(((TagNodeString)Entry["ModId"]).Data);
                }
            }
            else
            {
                // Load default vanilla stuff here
                StreamReader TR = new StreamReader(Properties.Resources.BaseIDs);
                string[] Line;

                while (!TR.EndOfStream)
                {
                    Line = TR.ReadLine().Split(' ');
                    BlockIDs.Add(Line[1], int.Parse(Line[0]));
                }
            }
        }
    }
}
