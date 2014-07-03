using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SeeSharp.Gui.Dialogs
{
    internal partial class dlgCheckingForUpdates : Form
    {
        internal dlgCheckingForUpdates()
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
