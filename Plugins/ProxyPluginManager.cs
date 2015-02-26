using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SeeSharp.Plugins
{
    /// <summary>
    ///     Proxy plugin manager that runs in the plugin AppDomain
    /// </summary>
    class PluginLoader : MarshalByRefObject
    {
        private readonly List<UpdateInfo> _Updates = new List<UpdateInfo>();  // Valid updates - Tuple of <Name>, <UpdateURL.dll>, <LocalFilename.dll>, Description
        // ReSharper disable once InconsistentNaming
        // *** Screw you ReSharper, I even added "DLL" as an acceptable abbreviation >8(
        private readonly List<String> _LoadablePluginDLLs = new List<String>();

        internal List<String> GetPluginsReadyToLoad
        {
            get
            {
                return _LoadablePluginDLLs;
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


                Type UpdateableType = Check.GetTypes().First(x => typeof(IUpdatable).IsAssignableFrom(x));

                if (UpdateableType != null)
                {
                    IUpdatable UpdateInfoSource = (IUpdatable)Activator.CreateInstance(UpdateableType);

                    UpdateInfo Info = new UpdateInfo
                    {
                        InternalVersion = UpdateInfoSource.InternalVersion,
                        PluginFilename = File,
                        UpdateXmlSource = UpdateInfoSource.XmlDataURL
                    };


                    _Updates.Add(Info);
                }


                _LoadablePluginDLLs.Add(File);

            }

        }

        /// <summary>
        ///     Validate that a given .DLL plugin is usable and meets the validation requirements
        /// </summary>
        /// <returns>
        ///     TRUE if the plugin is valid and should be loaded, or FALSE otherwise.
        /// </returns>
        private static bool CheckPlugin(Assembly File)
        {
            int NumMatches = File.GetTypes().Count(x => typeof(IPlugin).IsAssignableFrom(x));
            int NumUpdates = File.GetTypes().Count(x => typeof(IUpdatable).IsAssignableFrom(x));

            return NumMatches == 1 && NumUpdates <= NumMatches;
        }
    }
}
