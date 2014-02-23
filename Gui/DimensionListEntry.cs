using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SeeSharp.Gui
{
    internal struct DimensionListEntry
    {
        public String Name;
        public String Value;
        public DimensionListEntry(String Name, String Value)
        {
            this.Name = Name;
            this.Value = Value;
        }
        public override string ToString()
        {
            return Name;
        }
    }
}
