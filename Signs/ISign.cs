using System;
using System.Drawing;
using System.Xml;
using SeeSharp.Plugins;
using Substrate.TileEntities;

namespace SeeSharp.Signs
{
    public interface ISign : IComponent
    {
        String SignTag { get; }
        Point Location { get; set; }
        bool IsValid { get; }
        XmlElement SignData { get; }

        void CreateFromSign(TileEntitySign Sign);
        bool AddSign(TileEntitySign Sign);
    }
}
