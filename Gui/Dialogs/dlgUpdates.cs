using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SeeSharp.Gui.Dialogs
{
    internal partial class DlgUpdates : Form
    {
        public DlgUpdates()
        {
            InitializeComponent();
        }

        internal DialogResult UpdateDialog(Form OwnerForm, IEnumerable<Tuple<String, String, String, String>> Updates)
        {
            dgUpdates.Rows.Clear();
            String UpdateList = "";

            foreach (Tuple<String, String, String, String> Update in Updates) //Tuple of <Name>, <UpdateURL.dll>, <LocalFilename.dll>, <Description>
            {
                dgUpdates.Rows.Add(Update.Item1, Update.Item4);
                UpdateList += Update.Item1 + ", ";
            }

            lblPluginList.Text = UpdateList.TrimEnd(',', ' ');

            return ShowDialog(OwnerForm);
        }

        private void ResizeMoreInfo(object Sender, EventArgs E) // Handles btnMoreInfo.Click
        {
            if ((string)btnMoreInfo.Tag == "expand")
            {
                btnMoreInfo.Tag = "contract";
                btnMoreInfo.Text = "Less Info...";
                Height += dgUpdates.Height;
            }
            else
            {
                btnMoreInfo.Tag = "expand";
                btnMoreInfo.Text = "More Info...";
                Height -= dgUpdates.Height;
            }
        }
    }
}
