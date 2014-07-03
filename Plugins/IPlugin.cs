using System;

namespace SeeSharp.Plugins
{
    interface IPlugin : IComponent
    {
        String Name{ get; }
        String Description { get; }
        String Author { get; }
        String DisplayedVersion { get; }
        String Homepage { get; }
    }

    interface IUpdatable : IComponent
    {
        Int32 InternalVersion { get; }
        String XmlDataURL { get; }
    }
}
