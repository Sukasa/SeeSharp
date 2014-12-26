using System;
using System.ComponentModel;

namespace SeeSharp.Gui
{
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
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
