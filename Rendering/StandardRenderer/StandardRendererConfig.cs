using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SeeSharp.Rendering
{
    class StandardRendererConfig : RendererConfigForm
    {
        private System.Windows.Forms.ComboBox _ListRenderModes;
        private System.Windows.Forms.TrackBar _TBMinY;
        private System.Windows.Forms.NumericUpDown _NUDMinY;
        private System.Windows.Forms.NumericUpDown _NUDMaxY;
        private System.Windows.Forms.TrackBar _TBMaxY;
        private System.Windows.Forms.Label _Label2;
        private System.Windows.Forms.Label _Label3;
        private System.Windows.Forms.Label _Label1;

        public override List<KeyValuePair<string, string>> ConfigStrings
        {
            get {
                List<KeyValuePair<String, String>> L = new List<KeyValuePair<string, string>>();
                String N = "";

                switch (_ListRenderModes.SelectedIndex)
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
                L.Add(new KeyValuePair<string, string>("MinY", _NUDMinY.Value.ToString(Thread.CurrentThread.CurrentCulture)));
                L.Add(new KeyValuePair<string, string>("MaxY", _NUDMaxY.Value.ToString(Thread.CurrentThread.CurrentCulture)));

                return L;
            }
            set {
                if (value == null || !value.Exists(x => true))
                    return;

                String Mode = (from KeyValuePair<String, String> T in value where T.Key == "Mode" select T.Value).First();

                switch (Mode)
                {
                    case "c":
                        _ListRenderModes.SelectedIndex = 1;
                        break;
                    case "C":
                        _ListRenderModes.SelectedIndex = 2;
                        break;
                    default:
                        _ListRenderModes.SelectedIndex = 0;
                        break;
                }

                String ValueMax = (from KeyValuePair<String, String> T in value where T.Key == "MaxY" select T.Value).First();
                String ValueMin = (from KeyValuePair<String, String> T in value where T.Key == "MinY" select T.Value).First();

                if (ValueMax != null)
                    _NUDMaxY.Value = int.Parse(ValueMax);
                if (ValueMin != null)
                    _NUDMinY.Value = int.Parse(ValueMin);
            }
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager Resources = new System.ComponentModel.ComponentResourceManager(typeof(StandardRendererConfig));
            _ListRenderModes = new System.Windows.Forms.ComboBox();
            _Label1 = new System.Windows.Forms.Label();
            _TBMinY = new System.Windows.Forms.TrackBar();
            _NUDMinY = new System.Windows.Forms.NumericUpDown();
            _NUDMaxY = new System.Windows.Forms.NumericUpDown();
            _TBMaxY = new System.Windows.Forms.TrackBar();
            _Label2 = new System.Windows.Forms.Label();
            _Label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(_TBMinY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(_NUDMinY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(_NUDMaxY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(_TBMaxY)).BeginInit();
            SuspendLayout();
            // 
            // listRenderModes
            // 
            _ListRenderModes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            _ListRenderModes.FormattingEnabled = true;
            _ListRenderModes.Items.AddRange(new object[] {
            "Regular",
            "Glass Ceilings",
            "Caves Only"});
            _ListRenderModes.Location = new System.Drawing.Point(195, 6);
            _ListRenderModes.Name = "_ListRenderModes";
            _ListRenderModes.Size = new System.Drawing.Size(149, 21);
            _ListRenderModes.TabIndex = 0;
            // 
            // _Label1
            // 
            _Label1.AutoSize = true;
            _Label1.Location = new System.Drawing.Point(120, 9);
            _Label1.Name = "_Label1";
            _Label1.Size = new System.Drawing.Size(69, 13);
            _Label1.TabIndex = 1;
            _Label1.Text = "Render Type";
            // 
            // _TBMinY
            // 
            _TBMinY.Location = new System.Drawing.Point(12, 33);
            _TBMinY.Maximum = 255;
            _TBMinY.Name = "_TBMinY";
            _TBMinY.Size = new System.Drawing.Size(282, 45);
            _TBMinY.TabIndex = 2;
            _TBMinY.TickFrequency = 4;
            _TBMinY.ValueChanged += tbMinY_ValueChanged;
            // 
            // _NUDMinY
            // 
            _NUDMinY.Location = new System.Drawing.Point(300, 33);
            _NUDMinY.Name = "_NUDMinY";
            _NUDMinY.Size = new System.Drawing.Size(44, 20);
            _NUDMinY.TabIndex = 3;
            _NUDMinY.ValueChanged += nudMinY_ValueChanged;
            // 
            // _NUDMaxY
            // 
            _NUDMaxY.Location = new System.Drawing.Point(300, 84);
            _NUDMaxY.Maximum = new decimal(new [] {
            255,
            0,
            0,
            0});
            _NUDMaxY.Name = "_NUDMaxY";
            _NUDMaxY.Size = new System.Drawing.Size(44, 20);
            _NUDMaxY.TabIndex = 5;
            _NUDMaxY.Value = new decimal(new [] {
            255,
            0,
            0,
            0});
            // 
            // _TBMaxY
            // 
            _TBMaxY.Location = new System.Drawing.Point(12, 84);
            _TBMaxY.Maximum = 255;
            _TBMaxY.Name = "_TBMaxY";
            _TBMaxY.Size = new System.Drawing.Size(282, 45);
            _TBMaxY.TabIndex = 4;
            _TBMaxY.TickFrequency = 4;
            _TBMaxY.Value = 255;
            _TBMaxY.ValueChanged += tbMaxY_ValueChanged;
            // 
            // _Label2
            // 
            _Label2.AutoSize = true;
            _Label2.Location = new System.Drawing.Point(121, 68);
            _Label2.Name = "_Label2";
            _Label2.Size = new System.Drawing.Size(68, 13);
            _Label2.TabIndex = 6;
            _Label2.Text = "Vertical Slice";
            // 
            // _Label3
            // 
            _Label3.AutoSize = true;
            _Label3.Location = new System.Drawing.Point(309, 61);
            _Label3.Name = "_Label3";
            _Label3.Size = new System.Drawing.Size(20, 13);
            _Label3.TabIndex = 7;
            _Label3.Text = "To";
            // 
            // StandardRendererConfig
            // 
            ClientSize = new System.Drawing.Size(347, 131);
            Controls.Add(_Label3);
            Controls.Add(_Label2);
            Controls.Add(_NUDMaxY);
            Controls.Add(_TBMaxY);
            Controls.Add(_NUDMinY);
            Controls.Add(_TBMinY);
            Controls.Add(_Label1);
            Controls.Add(_ListRenderModes);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            Icon = ((System.Drawing.Icon)(Resources.GetObject("$this.Icon")));
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "StandardRendererConfig";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Advanced Config";
            ((System.ComponentModel.ISupportInitialize)(_TBMinY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(_NUDMinY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(_NUDMaxY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(_TBMaxY)).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        internal StandardRendererConfig()
        {
            InitializeComponent();
            _ListRenderModes.SelectedIndex = 0;
        }

        private void tbMinY_ValueChanged(object Sender, EventArgs E)
        {
            _NUDMinY.Value = _TBMinY.Value;
            _TBMaxY.Value = Math.Max(_TBMaxY.Value, _TBMinY.Value);
        }

        private void tbMaxY_ValueChanged(object Sender, EventArgs E)
        {
            _NUDMaxY.Value = _TBMaxY.Value;
            _TBMinY.Value = Math.Min(_TBMaxY.Value, _TBMinY.Value);
        }

        private void nudMinY_ValueChanged(object Sender, EventArgs E)
        {
            _TBMinY.Value = (int)_NUDMinY.Value;
        }

    }
}
