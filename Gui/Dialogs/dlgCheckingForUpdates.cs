using System;
using System.Windows.Forms;

namespace SeeSharp.Gui.Dialogs
{
    internal partial class DlgCheckingForUpdates : Form
    {
        internal DlgCheckingForUpdates()
        {
            InitializeComponent();
        }

        internal void SetProgressAndLabel(int Progress, int ProgressMax, String Label)
        {
            pbProgress.Maximum = ProgressMax;
            pbProgress.Value = Progress;
            lblCurrentlyChecking.Text = Label;
            
        }
    }
}
