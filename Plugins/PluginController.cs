using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

// TODO: Make this use AppDomains, so that I can unload DLLs after loading them and checking for information.  This will allow me to update the DLLs (plugins) from within the app.

namespace SeeSharp.Plugins
{
    /// <summary>
    ///     Plugin controller for See Sharp.  
    /// </summary>
    public class PluginController
    {
        internal List<Type> Plugins;
        internal List<Type> AvailableComponents;

        internal List<UpdateInfo> Updates;
        // ReSharper disable once InconsistentNaming
        // *** Screw you ReSharper, I even added "DLL" as an acceptable abbreviation >8(
        internal List<String> LoadablePluginDLLs;

        internal AppDomain ProxyDomain;
        private static PluginController _Instance;

        private PluginController()
        {
        }

        internal static PluginController Instance()
        {
            if (_Instance == null)
                _Instance = new PluginController();
            return _Instance;
        }

        /// <summary>
        ///     Scan the plugin directory for valid (only one IPlugin instantiation, etc) plugins, using CheckPlugin() to validate each
        /// </summary>
        internal void Update(bool Rescan = false)
        {


            // Provide 'do you want to update?' prompt

            DialogResult Result = DialogResult.No;

            if (Rescan)
                Result = DialogResult.Abort;


            // If they selected yes, update all plugins

            if (Result == DialogResult.Yes)
            {
                // Update Plugins
                if(!Rescan)
                    Update(true);
            }


            // Now load all plugins

        }


        internal void ScanPlugins()
        {

            if (!Directory.Exists(Assembly.GetExecutingAssembly().Location + Path.PathSeparator + "Plugins"))
                return;

            AppDomainSetup ProxySetup = new AppDomainSetup
            {
                DisallowCodeDownload = true,
                DisallowBindingRedirects = false,
                ApplicationBase = Environment.CurrentDirectory,
                DisallowApplicationBaseProbing = false
            };


            ProxyDomain = null;

            try
            {
                ProxyDomain = AppDomain.CreateDomain("Plugin-Domain", null, ProxySetup);

                PluginLoader Loader = (PluginLoader)ProxyDomain.CreateInstanceAndUnwrap(Assembly.GetExecutingAssembly().FullName, typeof(PluginLoader).FullName);

                Loader.Scan();

                LoadablePluginDLLs = Loader.GetPluginsReadyToLoad;
                Updates = Loader.Updates;

            }
            finally
            {
                if (ProxyDomain != null)
                    AppDomain.Unload(ProxyDomain);
            }
        }

        internal void CheckUpdates(Form MainWindow)
        {
            
            
        }

        internal void UpdatePlugins(Form MainWindow)
        {

        }

        internal void LoadPlugins()
        {
            // *** Load plugins into main AppDomain
            foreach (String File in LoadablePluginDLLs)
            {
                Assembly PluginDLL = Assembly.LoadFrom(File);
                foreach (Type T in PluginDLL.GetTypes())
                {
                    if (typeof(IComponent).IsAssignableFrom(T))
                        AvailableComponents.Add(T);
                }
            }
        }




        /// <summary>
        ///     Gets the instantiated plugins in the form of an IEnumerable&lt;IPlugin&gt;
        /// </summary>
        /// <returns></returns>
        internal IEnumerable<IPlugin> InstantiateAllPlugins()
        {
            return from Type InstantiateType in Plugins select (IPlugin)Activator.CreateInstance(InstantiateType);
        }

        /// <summary>
        ///     Update a plugin using the supplied Source URL / Destination Filename / Description  Tuple
        /// </summary>
        internal void UpdatePlugin(Tuple<String, String, String, String> UpdateData)
        {
            // TODO
        }

        /// <summary>
        ///     Returns a list of all plugin classes that implement a specific interface
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Type> GetComponentsOfType<T>() where T : IComponent
        {
            return from Type Test in Plugins where typeof(T).IsAssignableFrom(Test) select Test;
        }

        /// <summary>
        ///     Take an enumerable collection of types, and instantiate each one and return the result as an enumerable collection of isntantiated types
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="List"></param>
        /// <returns></returns>
        public IEnumerable<T> InstantiateTypeList<T>(IEnumerable<Type> List) where T : IComponent
        {
            return from Type Test in List select (T)Activator.CreateInstance(Test);
        }

        public IEnumerable<T> InstantiateComponentsOfType<T>() where T : IComponent
        {
            return InstantiateTypeList<T>(GetComponentsOfType<T>());
        }
    }
}
