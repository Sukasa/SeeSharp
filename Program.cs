using System;
using System.Windows.Forms;

namespace SeeSharp
{
    /// <summary>
    ///     See Sharp win32 entrypoint class
    /// </summary>
    public sealed class SeeSharpMain
    {
        private SeeSharpMain()
        {
            // *** Empty
        }

        /// <summary>
        ///     Win32 main entry point
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
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



