using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SeeSharp.Rendering;

namespace SeeSharp.Gui
{
    internal struct RendererListEntry
    {
        public String RendererName;
        public String RendererFriendlyName;
        public RendererListEntry(String Name)
        {
            RendererName = Name;
            RendererFriendlyName = RendererManager.Instance().GetFriendlyName(Name);
        }
        public override string ToString()
        {
            return RendererFriendlyName;
        }
    }
}
