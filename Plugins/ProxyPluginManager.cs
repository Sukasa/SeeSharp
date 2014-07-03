using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace SeeSharp.Plugins
{
    /// <summary>
    ///     Proxy plugin manager that runs in the plugin AppDomain
    /// </summary>
    class PluginLoader : MarshalByRefObject
    {
        private List<UpdateInfo> _Updates = new List<UpdateInfo>();  // Valid updates - Tuple of <Name>, <UpdateURL.dll>, <LocalFilename.dll>, Description
        private List<String> LoadablePluginDLLs = new List<String>();

        internal List<String> GetPluginsReadyToLoad
        {
            get
            {
                return LoadablePluginDLLs;
            }
        }
        internal List<UpdateInfo> Updates
        {
            get
            {
                return _Updates;
            }
        }

        /// <summary>
        ///     Scan the plugin directory for valid (only one IPlugin instantiation, etc) plugins, using CheckPlugin() to validate each
        /// </summary>
        internal void Scan()
        {
            if (!Directory.Exists(Assembly.GetExecutingAssembly().Location + Path.PathSeparator + "Plugins"))
                return;

            foreach (String File in Directory.EnumerateFiles(Assembly.GetExecutingAssembly().Location + Path.PathSeparator + "Plugins", "*.dll"))
            {
                Assembly Check = Assembly.LoadFrom(File);

                if (!CheckPlugin(Check))
                    continue;


                Type IUpdateableType = Check.GetTypes().First(x => typeof(IUpdatable).IsAssignableFrom(x));

                if (IUpdateableType != null)
                {
                    IUpdatable UpdateInfoSource = (IUpdatable)Activator.CreateInstance(IUpdateableType);

                    UpdateInfo Info = new UpdateInfo();

                    Info.InternalVersion = UpdateInfoSource.InternalVersion;
                    Info.PluginFilename = File;
                    Info.UpdateXmlSource = UpdateInfoSource.XmlDataURL;

                    _Updates.Add(Info);
                }


                LoadablePluginDLLs.Add(File);

            }

        }

        /// <summary>
        ///     Validate that a given .DLL plugin is usable and meets the validation requirements
        /// </summary>
        /// <returns>
        ///     TRUE if the plugin is valid and should be loaded, or FALSE otherwise.
        /// </returns>
        private bool CheckPlugin(Assembly File)
        {
            int NumMatches = 0;
            int NumUpdates = 0;
            foreach (Type TestType in File.GetTypes())
                if (typeof(IPlugin).IsAssignableFrom(TestType))
                    NumMatches++;
            foreach (Type TestType in File.GetTypes())
                if (typeof(IUpdatable).IsAssignableFrom(TestType))
                    NumUpdates++;
            return NumMatches == 1 && NumUpdates <= NumMatches;
        }
    }
}
