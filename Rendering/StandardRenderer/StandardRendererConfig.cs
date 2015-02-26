using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SeeSharp.Rendering
{
    class StandardRendererConfig : RendererConfigForm
    {
        private System.Windows.Forms.ComboBox listRenderModes;
        private System.Windows.Forms.TrackBar tbMinY;
        private System.Windows.Forms.NumericUpDown nudMinY;
        private System.Windows.Forms.NumericUpDown nudMaxY;
        private System.Windows.Forms.TrackBar tbMaxY;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
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
                L.Add(new KeyValuePair<string, string>("MinY", nudMinY.Value.ToString()));
                L.Add(new KeyValuePair<string, string>("MaxY", nudMaxY.Value.ToString()));

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

                String ValueMax = (from KeyValuePair<String, String> T in value where T.Key == "MaxY" select T.Value).First();
                String ValueMin = (from KeyValuePair<String, String> T in value where T.Key == "MinY" select T.Value).First();

                if (ValueMax != null)
                    nudMaxY.Value = int.Parse(ValueMax);
                if (ValueMin != null)
                    nudMinY.Value = int.Parse(ValueMin);
            }
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StandardRendererConfig));
            this.listRenderModes = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbMinY = new System.Windows.Forms.TrackBar();
            this.nudMinY = new System.Windows.Forms.NumericUpDown();
            this.nudMaxY = new System.Windows.Forms.NumericUpDown();
            this.tbMaxY = new System.Windows.Forms.TrackBar();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.tbMinY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaxY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbMaxY)).BeginInit();
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
            this.listRenderModes.Location = new System.Drawing.Point(195, 6);
            this.listRenderModes.Name = "listRenderModes";
            this.listRenderModes.Size = new System.Drawing.Size(149, 21);
            this.listRenderModes.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(120, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Render Type";
            // 
            // tbMinY
            // 
            this.tbMinY.Location = new System.Drawing.Point(12, 33);
            this.tbMinY.Maximum = 255;
            this.tbMinY.Name = "tbMinY";
            this.tbMinY.Size = new System.Drawing.Size(282, 45);
            this.tbMinY.TabIndex = 2;
            this.tbMinY.TickFrequency = 4;
            this.tbMinY.ValueChanged += new System.EventHandler(this.tbMinY_ValueChanged);
            // 
            // nudMinY
            // 
            this.nudMinY.Location = new System.Drawing.Point(300, 33);
            this.nudMinY.Name = "nudMinY";
            this.nudMinY.Size = new System.Drawing.Size(44, 20);
            this.nudMinY.TabIndex = 3;
            this.nudMinY.ValueChanged += new System.EventHandler(this.nudMinY_ValueChanged);
            // 
            // nudMaxY
            // 
            this.nudMaxY.Location = new System.Drawing.Point(300, 84);
            this.nudMaxY.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudMaxY.Name = "nudMaxY";
            this.nudMaxY.Size = new System.Drawing.Size(44, 20);
            this.nudMaxY.TabIndex = 5;
            this.nudMaxY.Value = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudMaxY.ValueChanged += new System.EventHandler(this.nudMaxY_ValueChanged);
            // 
            // tbMaxY
            // 
            this.tbMaxY.Location = new System.Drawing.Point(12, 84);
            this.tbMaxY.Maximum = 255;
            this.tbMaxY.Name = "tbMaxY";
            this.tbMaxY.Size = new System.Drawing.Size(282, 45);
            this.tbMaxY.TabIndex = 4;
            this.tbMaxY.TickFrequency = 4;
            this.tbMaxY.Value = 255;
            this.tbMaxY.ValueChanged += new System.EventHandler(this.tbMaxY_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(121, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Vertical Slice";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(309, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(20, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "To";
            // 
            // StandardRendererConfig
            // 
            this.ClientSize = new System.Drawing.Size(347, 131);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.nudMaxY);
            this.Controls.Add(this.tbMaxY);
            this.Controls.Add(this.nudMinY);
            this.Controls.Add(this.tbMinY);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listRenderModes);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "StandardRendererConfig";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Advanced Config";
            ((System.ComponentModel.ISupportInitialize)(this.tbMinY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaxY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbMaxY)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        internal StandardRendererConfig()
        {
            InitializeComponent();
            listRenderModes.SelectedIndex = 0;
        }

        private void tbMinY_ValueChanged(object sender, EventArgs e)
        {
            nudMinY.Value = tbMinY.Value;
            tbMaxY.Value = Math.Max(tbMaxY.Value, tbMinY.Value);
        }

        private void tbMaxY_ValueChanged(object sender, EventArgs e)
        {
            nudMaxY.Value = tbMaxY.Value;
            tbMinY.Value = Math.Min(tbMaxY.Value, tbMinY.Value);
        }

        private void nudMinY_ValueChanged(object sender, EventArgs e)
        {
            tbMinY.Value = (int)nudMinY.Value;
        }

        private void nudMaxY_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
