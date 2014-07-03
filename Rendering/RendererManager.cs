using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using SeeSharp.Plugins;

namespace SeeSharp.Rendering
{
    // *** Renderer Manager.  Loads rendering plugins and makes them available to higher-level code
    internal class RendererManager
    {
        private static RendererManager _Instance;
        private Dictionary<String, Tuple<String, Type>> Renderers = new Dictionary<string, Tuple<string, Type>>();

        private RendererManager()
        {
            LoadRenderers();
        }

        void LoadRenderers()
        {
            // TODO replace this with code that references and uses the plugin controller
            Renderers.Clear();
            Renderer CoreRenderer = new Renderer();
            Renderers.Add(CoreRenderer.RendererName, new Tuple<string, Type>(CoreRenderer.RendererFriendlyName, CoreRenderer.GetType()));

            if (!Directory.Exists(Assembly.GetExecutingAssembly().Location + "/Plugins"))
                return;

            foreach (String File in Directory.EnumerateFiles(Assembly.GetExecutingAssembly().Location + "/Plugins", "*.dll"))
                foreach (Type TestType in Assembly.LoadFile(File).GetTypes())
                    if (typeof(IRenderer).IsAssignableFrom(TestType))
                    {
                        IRenderer TestRenderer = (IRenderer)Activator.CreateInstance(TestType);
                        if (!Renderers.ContainsKey(TestRenderer.RendererFriendlyName))
                            Renderers.Add(TestRenderer.RendererName, new Tuple<string, Type>(TestRenderer.RendererFriendlyName, TestType));
                    }

            Renderers[""] = new Tuple<string, Type>("Standard Renderer", typeof(Renderer));
        }

        public static RendererManager Instance()
        {
           if (_Instance == null)
                _Instance = new RendererManager();
           return _Instance;
        }

        public String GetFriendlyName(String RendererName)
        {
            return Renderers[RendererName].Item1;
        }

        public IRenderer InstantiateRenderer(String RendererName)
        {
            return (IRenderer)Activator.CreateInstance(Renderers[RendererName].Item2);
        }

        public IEnumerable<String> AvailableRendererCodes
        {
            get
            {
                return Renderers.Keys;
            }
        }
    }
}
