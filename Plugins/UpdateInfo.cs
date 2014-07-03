using System;
using System.Net;
using System.Xml;
using System.Timers;

namespace SeeSharp.Plugins
{
    internal struct UpdateInfo
    {
        internal String PluginName;
        internal String UpdateXmlSource;
        internal String PluginFilename;

        internal String UpdateFileSource;
        internal String UpdateDescription;

        internal int InternalVersion;
        internal bool UpdateAvailable;

        internal void DownloadXmlData()
        {
            try
            {
                
                XmlDocument Data = new XmlDocument();
                String XMLFile;

                using (WebClient Client = new WebClient())
                {
                    XMLFile = Client.DownloadString(UpdateFileSource);
                }

                Data.LoadXml(XMLFile);


            }
            catch
            {
                UpdateAvailable = false;
            }
        }
    }
}
