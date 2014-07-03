using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;
using SeeSharp.Plugins;

namespace SeeSharp
{
    /// <summary>
    ///     See Sharp win32 entrypoint class
    /// </summary>
    /// <remarks>
    ///     SeeSharpMain is the core entrypoint class for See Sharp.  This class is normally hidden from Intellisense, as it is not useful.
    /// </remarks>
    [Browsable(false)]  // This class isn't really used or useful for people writing plugins, so hide it.
    [EditorBrowsable(EditorBrowsableState.Never)]
    public sealed class SeeSharpMain : IPlugin
    {
        internal SeeSharpMain()
        {
            // *** Empty
        }

        /// <summary>
        ///     Win32 main entry point
        /// </summary>
        /// <param name="args">
        ///     Command-line arguments
        /// </param>
        /// <returns>
        ///     Program return code
        /// </returns>
        [STAThread]
        public static int Main(string[] args)
        {
            return (new SeeSharpMain()).ProgramMain(args);
        }

        private int ProgramMain(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("No command-line parameters; opening Gui...");
                Application.EnableVisualStyles();
                Application.Run(new Gui.frmMain());
            }
            else
            {
                return (new CLIMain()).HandleCLI(args);
            }


            return 0;
        }

        public string Name
        {
            get { return "See Sharp"; }
        }

        public string Description
        {
            get { return "Base Components"; }
        }

        public string Author
        {
            get { return "Sukasa"; }
        }

        public string DisplayedVersion
        {
            get { return Assembly.GetExecutingAssembly().GetName().Version.ToString(); }
        }

        public string Homepage
        {
            get { return ""; }
        }
    }
}



