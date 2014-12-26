using System;
using System.ComponentModel;
using SeeSharp.Rendering;

namespace SeeSharp.Gui
{
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
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
