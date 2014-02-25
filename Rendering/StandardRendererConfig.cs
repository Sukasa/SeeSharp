using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SeeSharp.Rendering
{
    class StandardRendererConfig : RendererConfigForm
    {
        private System.Windows.Forms.ComboBox listRenderModes;
        private System.Windows.Forms.Label label1;

        public override List<KeyValuePair<string, string>> ConfigStrings
        {
            get {
                List<KeyValuePair<String, String>> L = new List<KeyValuePair<string, string>>();
                String N = "";

                switch (listRenderModes.SelectedIndex)
                {
                    case 0:
                        N = "n";
                        break;
                    case 1:
                        N = "c";
                        break;
                    case 2:
                        N = "C";
                        break;

                }

                L.Add(new KeyValuePair<string, string>("Mode", N));

                return L;
            }
            set {
                if (value == null || !value.Exists((x) => true))
                    return;

                String Mode = (from KeyValuePair<String, String> T in value where T.Key == "Mode" select T.Value).First();

                switch (Mode)
                {
                    case "c":
                        listRenderModes.SelectedIndex = 1;
                        break;
                    case "C":
                        listRenderModes.SelectedIndex = 2;
                        break;
                    default:
                        listRenderModes.SelectedIndex = 0;
                        break;
                }
            
            }
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StandardRendererConfig));
            this.listRenderModes = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // listRenderModes
            // 
            this.listRenderModes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.listRenderModes.FormattingEnabled = true;
            this.listRenderModes.Items.AddRange(new object[] {
            "Regular",
            "Glass Ceilings",
            "Caves Only"});
            this.listRenderModes.Location = new System.Drawing.Point(87, 12);
            this.listRenderModes.Name = "listRenderModes";
            this.listRenderModes.Size = new System.Drawing.Size(149, 21);
            this.listRenderModes.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Render Type";
            // 
            // StandardRendererConfig
            // 
            this.ClientSize = new System.Drawing.Size(251, 44);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listRenderModes);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "StandardRendererConfig";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Advanced Config";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        internal StandardRendererConfig()
        {
            InitializeComponent();
            listRenderModes.SelectedIndex = 0;
        }
    }
}
