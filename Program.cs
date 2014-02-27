using System;
using System.ComponentModel;
using System.Windows.Forms;

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
    public sealed class SeeSharpMain
    {
        private SeeSharpMain()
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

    }
}



