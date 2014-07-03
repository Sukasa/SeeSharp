using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SeeSharp.Gui.Dialogs
{
    internal partial class dlgUpdates : Form
    {
        public dlgUpdates()
        {
            InitializeComponent();
        }

        internal DialogResult UpdateDialog(Form Owner, IEnumerable<Tuple<String, String, String, String>> Updates)
        {
            dgUpdates.Rows.Clear();
            String UpdateList = "";

            foreach (Tuple<String, String, String, String> Update in Updates) //Tuple of <Name>, <UpdateURL.dll>, <LocalFilename.dll>, <Description>
            {
                dgUpdates.Rows.Add(new object[] { Update.Item1, Update.Item4 });
                UpdateList += Update.Item1 + ", ";
            }

            lblPluginList.Text = UpdateList.TrimEnd(new char[] { ',', ' ' });

            return this.ShowDialog(Owner);
        }

        private void ResizeMoreInfo(object sender, EventArgs e) // Handles btnMoreInfo.Click
        {
            if ((string)btnMoreInfo.Tag == "expand")
            {
                btnMoreInfo.Tag = "contract";
                btnMoreInfo.Text = "Less Info...";
                this.Height += dgUpdates.Height;
            }
            else
            {
                btnMoreInfo.Tag = "expand";
                btnMoreInfo.Text = "More Info...";
                this.Height -= dgUpdates.Height;
            }
        }
    }
}
