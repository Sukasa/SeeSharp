using System;
using System.ComponentModel;
using SeeSharp.Rendering;

namespace SeeSharp.Gui
{
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal struct RendererListEntry
    {
        public readonly String RendererName;
        private readonly String _RendererFriendlyName;
        public RendererListEntry(String Name)
        {
            RendererName = Name;
            _RendererFriendlyName = RendererManager.Instance().GetFriendlyName(Name);
        }
        public override string ToString()
        {
            return _RendererFriendlyName;
        }
    }
}
