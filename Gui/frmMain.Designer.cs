namespace SeeSharp.Gui
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pbPreview = new System.Windows.Forms.PictureBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.pbRenderProgress = new System.Windows.Forms.ToolStripProgressBar();
            this.dgPalettes = new System.Windows.Forms.DataGridView();
            this.cSelected = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.cName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cVersion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblSelectPalettes = new System.Windows.Forms.Label();
            this.fdSave = new System.Windows.Forms.SaveFileDialog();
            this.tbLightLevel = new System.Windows.Forms.TrackBar();
            this.cbDimension = new System.Windows.Forms.ComboBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnAbortRender = new System.Windows.Forms.Button();
            this.gbWorldDimensions = new System.Windows.Forms.GroupBox();
            this.lblWorldMin = new System.Windows.Forms.Label();
            this.lblWorldMax = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.cbCropMap = new System.Windows.Forms.CheckBox();
            this.gbSubregion = new System.Windows.Forms.GroupBox();
            this.nudZMax = new System.Windows.Forms.NumericUpDown();
            this.nudXMax = new System.Windows.Forms.NumericUpDown();
            this.nudZMin = new System.Windows.Forms.NumericUpDown();
            this.nudXMin = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnRendererOptions = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.cbRenderer = new System.Windows.Forms.ComboBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            this.lblDimension = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.btnAbortScan = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.dgMetrics = new System.Windows.Forms.DataGridView();
            this.dataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tpSettings = new System.Windows.Forms.TabPage();
            this.cbMultithread = new System.Windows.Forms.CheckBox();
            this.nudThreads = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.cbShowCLIButton = new System.Windows.Forms.CheckBox();
            this.chkTrackChanges = new System.Windows.Forms.CheckBox();
            this.pnlUpdate = new System.Windows.Forms.Panel();
            this.rbAlwaysUpdate = new System.Windows.Forms.RadioButton();
            this.rbPromptUpdates = new System.Windows.Forms.RadioButton();
            this.cbAutoUpdate = new System.Windows.Forms.CheckBox();
            this.ilTabs = new System.Windows.Forms.ImageList(this.components);
            this.fbOpenFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnOpenWorld = new System.Windows.Forms.ToolStripButton();
            this.btnCopyCLI = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.tmrPreview = new System.Windows.Forms.Timer(this.components);
            this.Tooltips = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbPreview)).BeginInit();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgPalettes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbLightLevel)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.gbWorldDimensions.SuspendLayout();
            this.gbSubregion.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudZMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudXMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudZMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudXMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgMetrics)).BeginInit();
            this.tpSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudThreads)).BeginInit();
            this.pnlUpdate.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.pbPreview);
            this.groupBox1.Location = new System.Drawing.Point(374, 45);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(203, 220);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Preview";
            // 
            // pbPreview
            // 
            this.pbPreview.BackColor = System.Drawing.Color.Black;
            this.pbPreview.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pbPreview.Location = new System.Drawing.Point(6, 19);
            this.pbPreview.Name = "pbPreview";
            this.pbPreview.Size = new System.Drawing.Size(192, 192);
            this.pbPreview.TabIndex = 0;
            this.pbPreview.TabStop = false;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus,
            this.pbRenderProgress});
            this.statusStrip1.Location = new System.Drawing.Point(0, 358);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.statusStrip1.Size = new System.Drawing.Size(593, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Tag = "ALWAYS";
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = false;
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(140, 17);
            this.lblStatus.Tag = "ALWAYS";
            this.lblStatus.Text = "See Sharp v1.3.x.x";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pbRenderProgress
            // 
            this.pbRenderProgress.Maximum = 10000;
            this.pbRenderProgress.Name = "pbRenderProgress";
            this.pbRenderProgress.Size = new System.Drawing.Size(446, 16);
            this.pbRenderProgress.Tag = "ALWAYS";
            // 
            // dgPalettes
            // 
            this.dgPalettes.AllowUserToAddRows = false;
            this.dgPalettes.AllowUserToDeleteRows = false;
            this.dgPalettes.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.dgPalettes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgPalettes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cSelected,
            this.cName,
            this.cVersion});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgPalettes.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgPalettes.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgPalettes.Location = new System.Drawing.Point(11, 35);
            this.dgPalettes.MultiSelect = false;
            this.dgPalettes.Name = "dgPalettes";
            this.dgPalettes.RowHeadersVisible = false;
            this.dgPalettes.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dgPalettes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgPalettes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgPalettes.ShowEditingIcon = false;
            this.dgPalettes.Size = new System.Drawing.Size(305, 146);
            this.dgPalettes.TabIndex = 5;
            this.dgPalettes.Tag = "ALWAYS";
            this.dgPalettes.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgPalettes_CurrentCellDirtyStateChanged);
            // 
            // cSelected
            // 
            this.cSelected.FalseValue = false;
            this.cSelected.FillWeight = 1F;
            this.cSelected.HeaderText = " ";
            this.cSelected.IndeterminateValue = false;
            this.cSelected.MinimumWidth = 25;
            this.cSelected.Name = "cSelected";
            this.cSelected.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cSelected.TrueValue = true;
            this.cSelected.Width = 25;
            // 
            // cName
            // 
            this.cName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.cName.HeaderText = "Name";
            this.cName.Name = "cName";
            this.cName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // cVersion
            // 
            this.cVersion.FillWeight = 1F;
            this.cVersion.HeaderText = "Version";
            this.cVersion.MinimumWidth = 60;
            this.cVersion.Name = "cVersion";
            this.cVersion.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cVersion.Width = 60;
            // 
            // lblSelectPalettes
            // 
            this.lblSelectPalettes.AutoSize = true;
            this.lblSelectPalettes.Location = new System.Drawing.Point(10, 19);
            this.lblSelectPalettes.Name = "lblSelectPalettes";
            this.lblSelectPalettes.Size = new System.Drawing.Size(78, 13);
            this.lblSelectPalettes.TabIndex = 7;
            this.lblSelectPalettes.Text = "Select Palettes";
            // 
            // fdSave
            // 
            this.fdSave.DefaultExt = "png";
            this.fdSave.Filter = "All Files|*.*|PNG Files (*.png)|*.png";
            this.fdSave.FilterIndex = 2;
            this.fdSave.SupportMultiDottedExtensions = true;
            this.fdSave.Title = "Save Map...";
            // 
            // tbLightLevel
            // 
            this.tbLightLevel.BackColor = System.Drawing.SystemColors.Window;
            this.tbLightLevel.Location = new System.Drawing.Point(329, 85);
            this.tbLightLevel.Maximum = 15;
            this.tbLightLevel.Name = "tbLightLevel";
            this.tbLightLevel.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbLightLevel.Size = new System.Drawing.Size(45, 151);
            this.tbLightLevel.TabIndex = 8;
            this.tbLightLevel.Value = 15;
            this.tbLightLevel.Scroll += new System.EventHandler(this.tbLightLevel_Scroll);
            // 
            // cbDimension
            // 
            this.cbDimension.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDimension.FormattingEnabled = true;
            this.cbDimension.Location = new System.Drawing.Point(420, 16);
            this.cbDimension.Name = "cbDimension";
            this.cbDimension.Size = new System.Drawing.Size(152, 21);
            this.cbDimension.TabIndex = 9;
            this.cbDimension.SelectedIndexChanged += new System.EventHandler(this.cbDimension_SelectedIndexChanged);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tpSettings);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.HotTrack = true;
            this.tabControl1.ImageList = this.ilTabs;
            this.tabControl1.Location = new System.Drawing.Point(0, 25);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(593, 333);
            this.tabControl1.TabIndex = 10;
            this.tabControl1.TabStop = false;
            this.tabControl1.Tag = "ALWAYS";
            this.tabControl1.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tabControl1_Selecting);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnAbortRender);
            this.tabPage1.Controls.Add(this.gbWorldDimensions);
            this.tabPage1.Controls.Add(this.cbCropMap);
            this.tabPage1.Controls.Add(this.gbSubregion);
            this.tabPage1.Controls.Add(this.btnRendererOptions);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.cbRenderer);
            this.tabPage1.Controls.Add(this.pictureBox3);
            this.tabPage1.Controls.Add(this.pictureBox2);
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Controls.Add(this.lblDimension);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.cbDimension);
            this.tabPage1.Controls.Add(this.dgPalettes);
            this.tabPage1.Controls.Add(this.tbLightLevel);
            this.tabPage1.Controls.Add(this.lblSelectPalettes);
            this.tabPage1.ImageIndex = 0;
            this.tabPage1.Location = new System.Drawing.Point(4, 23);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(585, 306);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Tag = "IGNORE";
            this.tabPage1.Text = "Map Export";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnAbortRender
            // 
            this.btnAbortRender.Enabled = false;
            this.btnAbortRender.Image = global::SeeSharp.Properties.Resources.Cross;
            this.btnAbortRender.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAbortRender.Location = new System.Drawing.Point(519, 269);
            this.btnAbortRender.Name = "btnAbortRender";
            this.btnAbortRender.Size = new System.Drawing.Size(58, 27);
            this.btnAbortRender.TabIndex = 31;
            this.btnAbortRender.Tag = "!NOLOCK PREVIEW";
            this.btnAbortRender.Text = "Abort";
            this.btnAbortRender.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnAbortRender.UseVisualStyleBackColor = true;
            this.btnAbortRender.Click += new System.EventHandler(this.btnAbortRender_Click);
            // 
            // gbWorldDimensions
            // 
            this.gbWorldDimensions.Controls.Add(this.lblWorldMin);
            this.gbWorldDimensions.Controls.Add(this.lblWorldMax);
            this.gbWorldDimensions.Controls.Add(this.label7);
            this.gbWorldDimensions.Enabled = false;
            this.gbWorldDimensions.Location = new System.Drawing.Point(150, 218);
            this.gbWorldDimensions.Name = "gbWorldDimensions";
            this.gbWorldDimensions.Size = new System.Drawing.Size(133, 82);
            this.gbWorldDimensions.TabIndex = 30;
            this.gbWorldDimensions.TabStop = false;
            this.gbWorldDimensions.Text = "Chunk Dimensions";
            // 
            // lblWorldMin
            // 
            this.lblWorldMin.Location = new System.Drawing.Point(24, 20);
            this.lblWorldMin.Name = "lblWorldMin";
            this.lblWorldMin.Size = new System.Drawing.Size(84, 18);
            this.lblWorldMin.TabIndex = 28;
            this.lblWorldMin.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblWorldMax
            // 
            this.lblWorldMax.Location = new System.Drawing.Point(25, 52);
            this.lblWorldMax.Name = "lblWorldMax";
            this.lblWorldMax.Size = new System.Drawing.Size(84, 18);
            this.lblWorldMax.TabIndex = 27;
            this.lblWorldMax.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(57, 36);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(20, 13);
            this.label7.TabIndex = 24;
            this.label7.Text = "To";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cbCropMap
            // 
            this.cbCropMap.AutoSize = true;
            this.cbCropMap.Location = new System.Drawing.Point(22, 216);
            this.cbCropMap.Name = "cbCropMap";
            this.cbCropMap.Size = new System.Drawing.Size(74, 17);
            this.cbCropMap.TabIndex = 17;
            this.cbCropMap.Text = "Subregion";
            this.cbCropMap.UseVisualStyleBackColor = true;
            this.cbCropMap.CheckedChanged += new System.EventHandler(this.cbCropMap_CheckedChanged);
            // 
            // gbSubregion
            // 
            this.gbSubregion.Controls.Add(this.nudZMax);
            this.gbSubregion.Controls.Add(this.nudXMax);
            this.gbSubregion.Controls.Add(this.nudZMin);
            this.gbSubregion.Controls.Add(this.nudXMin);
            this.gbSubregion.Controls.Add(this.label6);
            this.gbSubregion.Controls.Add(this.label5);
            this.gbSubregion.Controls.Add(this.label4);
            this.gbSubregion.Enabled = false;
            this.gbSubregion.Location = new System.Drawing.Point(11, 218);
            this.gbSubregion.Name = "gbSubregion";
            this.gbSubregion.Size = new System.Drawing.Size(133, 82);
            this.gbSubregion.TabIndex = 18;
            this.gbSubregion.TabStop = false;
            this.gbSubregion.Tag = "SUBREGION";
            this.gbSubregion.Text = "                       ";
            // 
            // nudZMax
            // 
            this.nudZMax.Location = new System.Drawing.Point(78, 56);
            this.nudZMax.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.nudZMax.Name = "nudZMax";
            this.nudZMax.Size = new System.Drawing.Size(46, 20);
            this.nudZMax.TabIndex = 29;
            this.nudZMax.Tag = "";
            this.nudZMax.Value = new decimal(new int[] {
            16,
            0,
            0,
            0});
            // 
            // nudXMax
            // 
            this.nudXMax.Location = new System.Drawing.Point(10, 56);
            this.nudXMax.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.nudXMax.Name = "nudXMax";
            this.nudXMax.Size = new System.Drawing.Size(46, 20);
            this.nudXMax.TabIndex = 28;
            this.nudXMax.Tag = "";
            this.nudXMax.Value = new decimal(new int[] {
            16,
            0,
            0,
            0});
            // 
            // nudZMin
            // 
            this.nudZMin.Location = new System.Drawing.Point(78, 22);
            this.nudZMin.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.nudZMin.Name = "nudZMin";
            this.nudZMin.Size = new System.Drawing.Size(46, 20);
            this.nudZMin.TabIndex = 27;
            this.nudZMin.Tag = "";
            this.nudZMin.Value = new decimal(new int[] {
            16,
            0,
            0,
            -2147483648});
            // 
            // nudXMin
            // 
            this.nudXMin.Location = new System.Drawing.Point(10, 22);
            this.nudXMin.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.nudXMin.Name = "nudXMin";
            this.nudXMin.Size = new System.Drawing.Size(46, 20);
            this.nudXMin.TabIndex = 26;
            this.nudXMin.Tag = "";
            this.nudXMin.Value = new decimal(new int[] {
            16,
            0,
            0,
            -2147483648});
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(57, 38);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(20, 13);
            this.label6.TabIndex = 24;
            this.label6.Text = "To";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(62, 58);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(10, 13);
            this.label5.TabIndex = 23;
            this.label5.Text = ",";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(62, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(10, 13);
            this.label4.TabIndex = 22;
            this.label4.Text = ",";
            // 
            // btnRendererOptions
            // 
            this.btnRendererOptions.Location = new System.Drawing.Point(196, 190);
            this.btnRendererOptions.Name = "btnRendererOptions";
            this.btnRendererOptions.Size = new System.Drawing.Size(75, 23);
            this.btnRendererOptions.TabIndex = 16;
            this.btnRendererOptions.Text = "Options...";
            this.btnRendererOptions.UseVisualStyleBackColor = true;
            this.btnRendererOptions.Click += new System.EventHandler(this.btnRendererOptions_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 194);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 13);
            this.label3.TabIndex = 15;
            this.label3.Text = "Renderer:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbRenderer
            // 
            this.cbRenderer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRenderer.FormattingEnabled = true;
            this.cbRenderer.Items.AddRange(new object[] {
            "Standard"});
            this.cbRenderer.Location = new System.Drawing.Point(69, 191);
            this.cbRenderer.Name = "cbRenderer";
            this.cbRenderer.Size = new System.Drawing.Size(121, 21);
            this.cbRenderer.TabIndex = 14;
            this.cbRenderer.SelectedIndexChanged += new System.EventHandler(this.cbRenderer_SelectedIndexChanged);
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = global::SeeSharp.Properties.Resources.Dark;
            this.pictureBox3.Location = new System.Drawing.Point(345, 229);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(16, 16);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox3.TabIndex = 13;
            this.pictureBox3.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::SeeSharp.Properties.Resources.Light;
            this.pictureBox2.Location = new System.Drawing.Point(345, 70);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(16, 16);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox2.TabIndex = 12;
            this.pictureBox2.TabStop = false;
            // 
            // button1
            // 
            this.button1.Image = global::SeeSharp.Properties.Resources.RenderMap;
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.Location = new System.Drawing.Point(419, 269);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(94, 27);
            this.button1.TabIndex = 11;
            this.button1.Tag = "";
            this.button1.Text = "Render Map";
            this.button1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // lblDimension
            // 
            this.lblDimension.AutoSize = true;
            this.lblDimension.Location = new System.Drawing.Point(360, 21);
            this.lblDimension.Name = "lblDimension";
            this.lblDimension.Size = new System.Drawing.Size(56, 13);
            this.lblDimension.TabIndex = 10;
            this.lblDimension.Text = "Dimension";
            // 
            // tabPage2
            // 
            this.tabPage2.ImageIndex = 1;
            this.tabPage2.Location = new System.Drawing.Point(4, 23);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(585, 306);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Tag = "IGNORE";
            this.tabPage2.Text = "Sign Processing";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.btnAbortScan);
            this.tabPage3.Controls.Add(this.button2);
            this.tabPage3.Controls.Add(this.label1);
            this.tabPage3.Controls.Add(this.dgMetrics);
            this.tabPage3.ImageIndex = 2;
            this.tabPage3.Location = new System.Drawing.Point(4, 23);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(585, 306);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Tag = "IGNORE";
            this.tabPage3.Text = "World Metrics";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // btnAbortScan
            // 
            this.btnAbortScan.Enabled = false;
            this.btnAbortScan.Image = global::SeeSharp.Properties.Resources.Cross;
            this.btnAbortScan.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAbortScan.Location = new System.Drawing.Point(519, 269);
            this.btnAbortScan.Name = "btnAbortScan";
            this.btnAbortScan.Size = new System.Drawing.Size(58, 27);
            this.btnAbortScan.TabIndex = 39;
            this.btnAbortScan.Tag = "!NOLOCK PREVIEW";
            this.btnAbortScan.Text = "Abort";
            this.btnAbortScan.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnAbortScan.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Image = global::SeeSharp.Properties.Resources.ExamineMap;
            this.button2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button2.Location = new System.Drawing.Point(419, 269);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(94, 27);
            this.button2.TabIndex = 38;
            this.button2.Text = "Scan Metrics";
            this.button2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button2.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(330, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 13);
            this.label1.TabIndex = 37;
            this.label1.Text = "Available Metrics";
            // 
            // dgMetrics
            // 
            this.dgMetrics.AllowUserToAddRows = false;
            this.dgMetrics.AllowUserToDeleteRows = false;
            this.dgMetrics.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.dgMetrics.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgMetrics.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewCheckBoxColumn1,
            this.dataGridViewTextBoxColumn1});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgMetrics.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgMetrics.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgMetrics.Location = new System.Drawing.Point(333, 64);
            this.dgMetrics.MultiSelect = false;
            this.dgMetrics.Name = "dgMetrics";
            this.dgMetrics.RowHeadersVisible = false;
            this.dgMetrics.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dgMetrics.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgMetrics.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgMetrics.ShowEditingIcon = false;
            this.dgMetrics.Size = new System.Drawing.Size(244, 199);
            this.dgMetrics.TabIndex = 36;
            this.dgMetrics.Tag = "ALWAYS";
            // 
            // dataGridViewCheckBoxColumn1
            // 
            this.dataGridViewCheckBoxColumn1.FalseValue = false;
            this.dataGridViewCheckBoxColumn1.FillWeight = 1F;
            this.dataGridViewCheckBoxColumn1.HeaderText = " ";
            this.dataGridViewCheckBoxColumn1.IndeterminateValue = false;
            this.dataGridViewCheckBoxColumn1.MinimumWidth = 25;
            this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
            this.dataGridViewCheckBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewCheckBoxColumn1.TrueValue = true;
            this.dataGridViewCheckBoxColumn1.Width = 25;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn1.HeaderText = "Metric";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // tabPage4
            // 
            this.tabPage4.ImageIndex = 4;
            this.tabPage4.Location = new System.Drawing.Point(4, 23);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(585, 306);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Tag = "IGNORE";
            this.tabPage4.Text = "Palette Editor";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // tpSettings
            // 
            this.tpSettings.Controls.Add(this.cbMultithread);
            this.tpSettings.Controls.Add(this.nudThreads);
            this.tpSettings.Controls.Add(this.label8);
            this.tpSettings.Controls.Add(this.cbShowCLIButton);
            this.tpSettings.Controls.Add(this.chkTrackChanges);
            this.tpSettings.Controls.Add(this.pnlUpdate);
            this.tpSettings.Controls.Add(this.cbAutoUpdate);
            this.tpSettings.ImageIndex = 3;
            this.tpSettings.Location = new System.Drawing.Point(4, 23);
            this.tpSettings.Name = "tpSettings";
            this.tpSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tpSettings.Size = new System.Drawing.Size(585, 306);
            this.tpSettings.TabIndex = 4;
            this.tpSettings.Tag = "IGNORE";
            this.tpSettings.Text = "Settings";
            this.tpSettings.UseVisualStyleBackColor = true;
            // 
            // chkMultithread
            // 
            this.cbMultithread.AutoSize = true;
            this.cbMultithread.Location = new System.Drawing.Point(10, 127);
            this.cbMultithread.Name = "chkMultithread";
            this.cbMultithread.Size = new System.Drawing.Size(190, 17);
            this.cbMultithread.TabIndex = 36;
            this.cbMultithread.Tag = "IGNORE";
            this.cbMultithread.Text = "Use multithreading where available";
            this.Tooltips.SetToolTip(this.cbMultithread, "If checked, will multi-thread operations where possible.  Takes effect immediatel" +
                    "y.");
            this.cbMultithread.UseVisualStyleBackColor = true;
            this.cbMultithread.CheckedChanged += new System.EventHandler(this.cbMultithread_CheckedChanged);
            // 
            // nudThreads
            // 
            this.nudThreads.Location = new System.Drawing.Point(78, 145);
            this.nudThreads.Maximum = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.nudThreads.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.nudThreads.Name = "nudThreads";
            this.nudThreads.Size = new System.Drawing.Size(33, 20);
            this.nudThreads.TabIndex = 34;
            this.nudThreads.Tag = "MULTITHREAD ALWAYS";
            this.nudThreads.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.nudThreads.ValueChanged += new System.EventHandler(this.nudThreads_ValueChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(23, 147);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(49, 13);
            this.label8.TabIndex = 35;
            this.label8.Text = "Threads:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chkShowCLIButton
            // 
            this.cbShowCLIButton.AutoSize = true;
            this.cbShowCLIButton.Location = new System.Drawing.Point(10, 104);
            this.cbShowCLIButton.Name = "chkShowCLIButton";
            this.cbShowCLIButton.Size = new System.Drawing.Size(193, 17);
            this.cbShowCLIButton.TabIndex = 4;
            this.cbShowCLIButton.Tag = "IGNORE";
            this.cbShowCLIButton.Text = "Show \'CLI Params\' button in toolbar";
            this.Tooltips.SetToolTip(this.cbShowCLIButton, "If checked, show the \'CLI Params\' button in the menu bar.  Not useful unless you " +
                    "are planning on writing a batch script");
            this.cbShowCLIButton.UseVisualStyleBackColor = true;
            this.cbShowCLIButton.CheckedChanged += new System.EventHandler(this.cbShowCLIButton_CheckedChanged);
            // 
            // chkTrackChanges
            // 
            this.chkTrackChanges.AutoSize = true;
            this.chkTrackChanges.Checked = true;
            this.chkTrackChanges.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTrackChanges.Location = new System.Drawing.Point(10, 81);
            this.chkTrackChanges.Name = "chkTrackChanges";
            this.chkTrackChanges.Size = new System.Drawing.Size(205, 17);
            this.chkTrackChanges.TabIndex = 3;
            this.chkTrackChanges.Tag = "IGNORE";
            this.chkTrackChanges.Text = "Match palette list to directory changes";
            this.Tooltips.SetToolTip(this.chkTrackChanges, resources.GetString("chkTrackChanges.ToolTip"));
            this.chkTrackChanges.UseVisualStyleBackColor = true;
            this.chkTrackChanges.CheckedChanged += new System.EventHandler(this.cbTrackChanges_CheckedChanged);
            // 
            // pnlUpdate
            // 
            this.pnlUpdate.Controls.Add(this.rbAlwaysUpdate);
            this.pnlUpdate.Controls.Add(this.rbPromptUpdates);
            this.pnlUpdate.Location = new System.Drawing.Point(23, 31);
            this.pnlUpdate.Name = "pnlUpdate";
            this.pnlUpdate.Size = new System.Drawing.Size(177, 42);
            this.pnlUpdate.TabIndex = 2;
            this.pnlUpdate.Tag = "AUTOUPDATE ALWAYS";
            // 
            // rbAlwaysUpdate
            // 
            this.rbAlwaysUpdate.AutoSize = true;
            this.rbAlwaysUpdate.Location = new System.Drawing.Point(3, 21);
            this.rbAlwaysUpdate.Name = "rbAlwaysUpdate";
            this.rbAlwaysUpdate.Size = new System.Drawing.Size(125, 17);
            this.rbAlwaysUpdate.TabIndex = 2;
            this.rbAlwaysUpdate.Tag = "IGNORE";
            this.rbAlwaysUpdate.Text = "Update Automatically";
            this.rbAlwaysUpdate.UseVisualStyleBackColor = true;
            this.rbAlwaysUpdate.CheckedChanged += new System.EventHandler(this.rbAlwaysUpdate_CheckedChanged);
            // 
            // rbPromptUpdates
            // 
            this.rbPromptUpdates.AutoSize = true;
            this.rbPromptUpdates.Checked = true;
            this.rbPromptUpdates.Location = new System.Drawing.Point(3, 3);
            this.rbPromptUpdates.Name = "rbPromptUpdates";
            this.rbPromptUpdates.Size = new System.Drawing.Size(159, 17);
            this.rbPromptUpdates.TabIndex = 1;
            this.rbPromptUpdates.TabStop = true;
            this.rbPromptUpdates.Tag = "IGNORE";
            this.rbPromptUpdates.Text = "Prompt on updates available";
            this.rbPromptUpdates.UseVisualStyleBackColor = true;
            // 
            // chkAutoUpdate
            // 
            this.cbAutoUpdate.AutoSize = true;
            this.cbAutoUpdate.Checked = true;
            this.cbAutoUpdate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbAutoUpdate.Location = new System.Drawing.Point(10, 8);
            this.cbAutoUpdate.Name = "chkAutoUpdate";
            this.cbAutoUpdate.Size = new System.Drawing.Size(191, 17);
            this.cbAutoUpdate.TabIndex = 0;
            this.cbAutoUpdate.Tag = "IGNORE";
            this.cbAutoUpdate.Text = "Check for plugin updates at launch";
            this.cbAutoUpdate.UseVisualStyleBackColor = true;
            this.cbAutoUpdate.CheckedChanged += new System.EventHandler(this.cbAutoUpdate_CheckedChanged);
            // 
            // ilTabs
            // 
            this.ilTabs.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilTabs.ImageStream")));
            this.ilTabs.TransparentColor = System.Drawing.Color.Transparent;
            this.ilTabs.Images.SetKeyName(0, "map.png");
            this.ilTabs.Images.SetKeyName(1, "application_view_columns.png");
            this.ilTabs.Images.SetKeyName(2, "report.png");
            this.ilTabs.Images.SetKeyName(3, "wrench.png");
            this.ilTabs.Images.SetKeyName(4, "palette.png");
            // 
            // fbOpenFolder
            // 
            this.fbOpenFolder.Description = "Select World Folder...";
            this.fbOpenFolder.RootFolder = System.Environment.SpecialFolder.MyComputer;
            this.fbOpenFolder.ShowNewFolderButton = false;
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnOpenWorld,
            this.toolStripSeparator1,
            this.btnCopyCLI,
            this.toolStripButton2});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(593, 25);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Tag = "ALWAYS";
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnOpenWorld
            // 
            this.btnOpenWorld.Image = global::SeeSharp.Properties.Resources.OpenWorld;
            this.btnOpenWorld.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnOpenWorld.Name = "btnOpenWorld";
            this.btnOpenWorld.Size = new System.Drawing.Size(91, 22);
            this.btnOpenWorld.Tag = "NOWORLD";
            this.btnOpenWorld.Text = "Open World";
            this.btnOpenWorld.Click += new System.EventHandler(this.ExecuteOpenWorld);
            // 
            // btnCopyCLI
            // 
            this.btnCopyCLI.Image = global::SeeSharp.Properties.Resources.Terminal;
            this.btnCopyCLI.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCopyCLI.Name = "btnCopyCLI";
            this.btnCopyCLI.Size = new System.Drawing.Size(86, 22);
            this.btnCopyCLI.Text = "CLI Params";
            this.btnCopyCLI.ToolTipText = "Copy the current operation settings in the GUI to clipboard as command-line argum" +
                "ents";
            this.btnCopyCLI.Click += new System.EventHandler(this.btnCopyCLI_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = global::SeeSharp.Properties.Resources.Help;
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton2.Tag = "ALWAYS";
            this.toolStripButton2.Text = "Open help file";
            // 
            // tmrPreview
            // 
            this.tmrPreview.Interval = 750;
            this.tmrPreview.Tag = "IGNORE";
            this.tmrPreview.Tick += new System.EventHandler(this.PreviewTimeout);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(593, 380);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.statusStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMain";
            this.Text = "See Sharp";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbPreview)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgPalettes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbLightLevel)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.gbWorldDimensions.ResumeLayout(false);
            this.gbWorldDimensions.PerformLayout();
            this.gbSubregion.ResumeLayout(false);
            this.gbSubregion.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudZMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudXMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudZMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudXMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgMetrics)).EndInit();
            this.tpSettings.ResumeLayout(false);
            this.tpSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudThreads)).EndInit();
            this.pnlUpdate.ResumeLayout(false);
            this.pnlUpdate.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox pbPreview;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.DataGridView dgPalettes;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.ToolStripProgressBar pbRenderProgress;
        private System.Windows.Forms.Label lblSelectPalettes;
        private System.Windows.Forms.SaveFileDialog fdSave;
        private System.Windows.Forms.TrackBar tbLightLevel;
        private System.Windows.Forms.ComboBox cbDimension;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label lblDimension;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.DataGridViewCheckBoxColumn cSelected;
        private System.Windows.Forms.DataGridViewTextBoxColumn cName;
        private System.Windows.Forms.DataGridViewTextBoxColumn cVersion;
        private System.Windows.Forms.FolderBrowserDialog fbOpenFolder;
        private System.Windows.Forms.ImageList ilTabs;
        private System.Windows.Forms.Button btnRendererOptions;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbRenderer;
        private System.Windows.Forms.GroupBox gbSubregion;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox cbCropMap;
        private System.Windows.Forms.NumericUpDown nudZMax;
        private System.Windows.Forms.NumericUpDown nudXMax;
        private System.Windows.Forms.NumericUpDown nudZMin;
        private System.Windows.Forms.NumericUpDown nudXMin;
        private System.Windows.Forms.GroupBox gbWorldDimensions;
        private System.Windows.Forms.Label lblWorldMax;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblWorldMin;
        private System.Windows.Forms.Button btnAbortRender;
        private System.Windows.Forms.ToolStripButton btnOpenWorld;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnCopyCLI;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.Timer tmrPreview;
        private System.Windows.Forms.DataGridView dgMetrics;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btnAbortScan;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TabPage tpSettings;
        private System.Windows.Forms.CheckBox cbShowCLIButton;
        private System.Windows.Forms.CheckBox chkTrackChanges;
        private System.Windows.Forms.ToolTip Tooltips;
        private System.Windows.Forms.Panel pnlUpdate;
        private System.Windows.Forms.RadioButton rbAlwaysUpdate;
        private System.Windows.Forms.RadioButton rbPromptUpdates;
        private System.Windows.Forms.CheckBox cbAutoUpdate;
        private System.Windows.Forms.CheckBox cbMultithread;
        private System.Windows.Forms.NumericUpDown nudThreads;
        private System.Windows.Forms.Label label8;
    }
}