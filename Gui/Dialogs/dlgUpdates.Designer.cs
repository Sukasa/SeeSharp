namespace SeeSharp.Gui.Dialogs
{
    partial class DlgUpdates
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblPluginList = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.dgUpdates = new System.Windows.Forms.DataGridView();
            this.cName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cVersion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button1 = new System.Windows.Forms.Button();
            this.btnMoreInfo = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgUpdates)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(226, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Updates are available for the following plugins:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Update?";
            // 
            // lblPluginList
            // 
            this.lblPluginList.AutoSize = true;
            this.lblPluginList.Location = new System.Drawing.Point(15, 29);
            this.lblPluginList.Name = "lblPluginList";
            this.lblPluginList.Size = new System.Drawing.Size(89, 13);
            this.lblPluginList.TabIndex = 2;
            this.lblPluginList.Text = "PluginsToUpdate";
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.No;
            this.button2.Location = new System.Drawing.Point(298, 62);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 4;
            this.button2.Text = "No";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // dgUpdates
            // 
            this.dgUpdates.AllowUserToAddRows = false;
            this.dgUpdates.AllowUserToDeleteRows = false;
            this.dgUpdates.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.dgUpdates.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgUpdates.ColumnHeadersVisible = false;
            this.dgUpdates.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cName,
            this.cVersion});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgUpdates.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgUpdates.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgUpdates.Location = new System.Drawing.Point(12, 99);
            this.dgUpdates.MultiSelect = false;
            this.dgUpdates.Name = "dgUpdates";
            this.dgUpdates.ReadOnly = true;
            this.dgUpdates.RowHeadersVisible = false;
            this.dgUpdates.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dgUpdates.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgUpdates.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgUpdates.ShowEditingIcon = false;
            this.dgUpdates.Size = new System.Drawing.Size(361, 159);
            this.dgUpdates.TabIndex = 7;
            this.dgUpdates.Tag = "";
            // 
            // cName
            // 
            this.cName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.cName.HeaderText = "Name";
            this.cName.Name = "cName";
            this.cName.ReadOnly = true;
            this.cName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // cVersion
            // 
            this.cVersion.FillWeight = 1F;
            this.cVersion.HeaderText = "";
            this.cVersion.MinimumWidth = 60;
            this.cVersion.Name = "cVersion";
            this.cVersion.ReadOnly = true;
            this.cVersion.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cVersion.Width = 60;
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.button1.Location = new System.Drawing.Point(217, 62);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Yes";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // btnMoreInfo
            // 
            this.btnMoreInfo.Location = new System.Drawing.Point(136, 62);
            this.btnMoreInfo.Name = "btnMoreInfo";
            this.btnMoreInfo.Size = new System.Drawing.Size(75, 22);
            this.btnMoreInfo.TabIndex = 5;
            this.btnMoreInfo.Tag = "expand";
            this.btnMoreInfo.Text = "More Info...";
            this.btnMoreInfo.UseVisualStyleBackColor = true;
            this.btnMoreInfo.Click += new System.EventHandler(this.ResizeMoreInfo);
            // 
            // DlgUpdates
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(385, 97);
            this.Controls.Add(this.dgUpdates);
            this.Controls.Add(this.btnMoreInfo);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lblPluginList);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DlgUpdates";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Updates are available";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.dgUpdates)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblPluginList;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.DataGridView dgUpdates;
        private System.Windows.Forms.DataGridViewTextBoxColumn cName;
        private System.Windows.Forms.DataGridViewTextBoxColumn cVersion;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnMoreInfo;
    }
}