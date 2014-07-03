namespace SeeSharp.Gui.Dialogs
{
    partial class dlgCheckingForUpdates
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
            this.pbProgress = new System.Windows.Forms.ProgressBar();
            this.lblCurrentlyChecking = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // progressBar1
            // 
            this.pbProgress.Location = new System.Drawing.Point(12, 12);
            this.pbProgress.Name = "progressBar1";
            this.pbProgress.Size = new System.Drawing.Size(260, 23);
            this.pbProgress.TabIndex = 0;
            // 
            // lblCurrentlyChecking
            // 
            this.lblCurrentlyChecking.AutoSize = true;
            this.lblCurrentlyChecking.Location = new System.Drawing.Point(18, 46);
            this.lblCurrentlyChecking.Name = "lblCurrentlyChecking";
            this.lblCurrentlyChecking.Size = new System.Drawing.Size(35, 13);
            this.lblCurrentlyChecking.TabIndex = 1;
            this.lblCurrentlyChecking.Text = "label1";
            // 
            // dlgCheckingForUpdates
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 68);
            this.ControlBox = false;
            this.Controls.Add(this.lblCurrentlyChecking);
            this.Controls.Add(this.pbProgress);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "dlgCheckingForUpdates";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Checking for updates...";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar pbProgress;
        private System.Windows.Forms.Label lblCurrentlyChecking;
    }
}