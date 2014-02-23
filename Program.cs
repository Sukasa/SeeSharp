using System;
using System.Windows.Forms;

namespace SeeSharp
{
    public sealed class SeeSharpMain
    {
        private SeeSharpMain()
        {
            // *** Empty
        }
        [STAThread]
        public static int Main(string[] args)
        {
            return (new SeeSharpMain()).ProgramMain(args);
        }

        int ProgramMain(string[] args)
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



