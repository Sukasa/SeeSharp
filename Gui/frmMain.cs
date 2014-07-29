using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using SeeSharp.Palette;
using SeeSharp.Plugins;
using SeeSharp.Rendering;
using Substrate;

namespace SeeSharp.Gui
{
    internal partial class frmMain : Form
    {
        private List<Type> RendererTypes = new List<Type>();
        private List<String> RendererNames = new List<string>();
        private List<int> SkipErrors = new List<int>();

        private AnvilWorld World;

        private RenderConfiguration RenderingConfig = new RenderConfiguration();
        private RendererListEntry SelectedRenderer;
        private RendererConfigForm CachedConfigForm;

        private delegate void AbortRenderDelegate();

        private AbortRenderDelegate AbortRender;
        private bool Abort = false;

        private FileSystemWatcher PaletteWatcher;

        internal frmMain()
        {
            InitializeComponent();

            // *** Why is this here?
            this.nudZMin.ValueChanged += new System.EventHandler(this.SetupSubregion);
            this.nudXMin.ValueChanged += new System.EventHandler(this.SetupSubregion);
            this.nudZMax.ValueChanged += new System.EventHandler(this.SetupSubregion);
            this.nudXMax.ValueChanged += new System.EventHandler(this.SetupSubregion);

            InvokableControlToggler = this.ToggleControls;
            InvokablePaletteSelector = this.UpdatePaletteSelection;
            PaletteManager.Instance();

        }

        #region "Meta Configuration"

        private void nudThreads_ValueChanged(object sender, EventArgs e)
        {
            int Value = (int)nudThreads.Value;
            RenderingConfig.EnableMultithreading = Value > 1;
            RenderingConfig.MaxThreads = Value;
            Properties.Settings.Default.MTThreadCount = Value;
            Properties.Settings.Default.Save();
        }

        #endregion

        #region "Render Configuration"

        private void cbCropMap_CheckedChanged(object sender, EventArgs e)
        {
            gbSubregion.Enabled = cbCropMap.Checked;
        }
        private void SetupSubregion(object sender, EventArgs e)
        {
            SetupSubregion();
        }
        private void SetupSubregion()
        {
            nudXMin.Maximum = nudXMax.Value - 1;
            nudZMin.Maximum = nudZMax.Value - 1;
            nudXMax.Minimum = nudXMin.Value + 1;
            nudZMax.Minimum = nudZMin.Value + 1;

            RenderingConfig.RenderSubregion = cbCropMap.Checked;
        }

        #endregion

        #region "Setup"

        private void frmMain_Load(object sender, EventArgs e)
        {
            int ThreadingCount = Properties.Settings.Default.MTThreadCount;
            if (ThreadingCount == 0)
                ThreadingCount = Environment.ProcessorCount;

            cbAutoUpdate.Checked = Properties.Settings.Default.AutoCheck;
            rbAlwaysUpdate.Checked = Properties.Settings.Default.AutoUpdate;
            cbShowCLIButton.Checked = Properties.Settings.Default.ShowCLIButton;
            chkTrackChanges.Checked = Properties.Settings.Default.WatchFolder;
            cbMultithread.Checked = ThreadingCount != 1;

            btnCopyCLI.Visible = cbShowCLIButton.Checked;

            // *** Check for plugin updates here

            SetupPalettes();

            lblStatus.Text = string.Format("See Sharp v{0}", Assembly.GetExecutingAssembly().GetName().Version.ToString());
            LoadRenderers();

            nudThreads.Maximum = Environment.ProcessorCount;


            nudThreads.Value = ThreadingCount > 1 ? Math.Min(ThreadingCount, nudThreads.Maximum) : nudThreads.Maximum;

            toolStrip1.Renderer = new ToolStripProfessionalRendererNoSideLine();

            tabPage2.Enabled = false;

            ToggleControls(false, false, false);
        }

        void PaletteFilesUpdated(object sender, FileSystemEventArgs e)
        {
            SetupPalettes();
        }

        private void SetupPalettes()
        {
            PaletteManager PM = PaletteManager.Instance();
            PM.Reload();
            PM.AutoConfig(World != null ? World.Path : "", SelectedRenderer.RendererName);
            dgPalettes.Rows.Clear();
            foreach (PaletteFile File in PM.AllPalettes)
            {
                DataGridViewRow Row = dgPalettes.Rows[dgPalettes.Rows.Add(new object[] { File.Selected, File.Name, File.Version })];
                Row.Tag = File;
                if (File.Version != String.Empty)
                    Row.Cells[1].ToolTipText = string.Format("{0} v{1}\r\n{2}", File.Name, File.Version, File.Description);
                else
                    Row.Cells[1].ToolTipText = string.Format("{0}\r\n{1}", File.Name, File.Description);

            }

            if (chkTrackChanges.Checked && PaletteWatcher == null)
            {
                PaletteWatcher = new FileSystemWatcher(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
                PaletteWatcher.EnableRaisingEvents = true;
                PaletteWatcher.IncludeSubdirectories = true;
                PaletteWatcher.Filter = "*.pal";
                PaletteWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.DirectoryName;
                PaletteWatcher.Changed += PaletteFilesUpdated;
            }


        }
        private void LoadRenderers()
        {
            cbRenderer.Items.Clear();
            foreach (String RendererName in RendererManager.Instance().AvailableRendererCodes)
                cbRenderer.Items.Add(new RendererListEntry(RendererName));
            cbRenderer.SelectedIndex = 0;
        }

        #endregion

        #region "Rendering Functions"

        private void ToggleControls(bool IsBusy, bool WorldLoaded, bool IsPreviewing, Control ControlRef = null)
        {
            if (ControlRef == null)
                ControlRef = this;

            if (ControlRef.InvokeRequired)
            {
                ControlRef.Invoke(InvokableControlToggler, IsBusy, WorldLoaded, IsPreviewing, ControlRef);
                return;
            }

            btnCopyCLI.Enabled = WorldLoaded;

            foreach (Control Control in ControlRef.GetSubControls())
            {
                String Tag = (String)Control.Tag;
                if (Tag == null)
                    Tag = String.Empty;

                if (Tag.Contains("IGNORE"))
                    continue;

                Control.Enabled = true;

                if (typeof(ScrollBar).IsAssignableFrom(Control.GetType()) || typeof(NumericUpDown).IsAssignableFrom(Control.Parent.GetType()))
                {
                    continue;
                }

                if (Tag.Contains("SUBREGION") && !cbCropMap.Checked)
                    Control.Enabled = false;

                if (Tag.Contains("AUTOUPDATE") && !cbAutoUpdate.Checked)
                    Control.Enabled = false;

                if (Tag.Contains("MULTITHREAD") && !cbMultithread.Checked)
                    Control.Enabled = false;

                if (Tag.Contains("ALWAYS")) // *** Always enabled
                    continue;

                if (!WorldLoaded && !Tag.Contains("NOWORLD"))
                {
                    Control.Enabled = false;
                }
                else
                {
                    if (IsBusy ^ Tag.Contains("!NOLOCK"))
                        Control.Enabled = false;

                    if (IsPreviewing && Tag.Contains("PREVIEW"))
                        Control.Enabled = true;
                }
            }
        }
        private delegate void ControlToggler(bool a, bool b, bool c, Control d = null);
        private ControlToggler InvokableControlToggler;
        private void Render(bool IsPreview)
        {
            Abort = false;

            ToggleControls(!IsPreview, true, IsPreview);
            SetStatus("Initializing...");
            IRenderer Renderer = null;

            // *** Set up renderer config
            RenderingConfig.IsPreview = IsPreview;
            RenderingConfig.Chunks = World.GetChunkManager(RenderingConfig.Dimension);
            RenderingConfig.Palette = new BlockPalette();
            RenderingConfig.RenderSubregion = cbCropMap.Checked;
            RenderingConfig.MaxThreads = cbMultithread.Checked ? (int)nudThreads.Value : 1;
            SkipErrors.Clear();

            if (IsPreview)
            {
                RenderingConfig.RenderSubregion = true;
                RenderingConfig.SubregionChunks = new Rectangle(-15, -15, 31, 31);
            }
            else if (RenderingConfig.RenderSubregion)
                RenderingConfig.SubregionChunks = new Rectangle((int)nudXMin.Value, (int)nudZMin.Value, (int)nudXMax.Value - (int)nudXMin.Value + 1, (int)nudZMax.Value - (int)nudZMin.Value + 1);
            else
                RenderingConfig.SubregionChunks = new Rectangle(RenderingConfig.Metrics.MinX, RenderingConfig.Metrics.MinZ, RenderingConfig.Metrics.MaxX - RenderingConfig.Metrics.MinX, RenderingConfig.Metrics.MaxZ - RenderingConfig.Metrics.MinZ);

            if (Abort)
                goto Cleanup;

            foreach (DataGridViewRow Row in dgPalettes.Rows)
                ((PaletteFile)Row.Tag).Selected = (bool)Row.Cells[0].Value;


            // *** Set up palettes
            foreach (String Palette in from PaletteFile File in PaletteManager.Instance().AllPalettes where File.Selected select File.PalettePath)
            {
                try
                {
                    RenderingConfig.Palette.LoadPalette(Palette);
                }
                catch (BlockPalette.PaletteExecutionException ex)
                {
                    MessageBox.Show(string.Format("The palette file {0} is invalid and will be skipped:\r\n{1}", Path.GetFileName(Palette), ex.Message), "Palette Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    PaletteManager.Instance().AllPalettes.First((x) => x.PalettePath == Palette).Selected = false;
                }
                catch
                {

                }
            }

            if (Abort)
                goto Cleanup;

            UpdatePaletteSelection();

            RenderingConfig.Palette.AssembleLookupTables();

            // *** Initialize renderer
            Renderer = RendererManager.Instance().InstantiateRenderer((SelectedRenderer).RendererName);

            Renderer.ProgressUpdate += DoUpdate;
            Renderer.RenderError += HandleError;

            Renderer.Configure(RenderingConfig);
            Renderer.Initialize();

            AbortRender = Renderer.Abort;

            // *** Render
            if (IsPreview)
                pbPreview.Image = Renderer.Preview();
            else
                Renderer.Render();


        Cleanup:

            // *** Clean up
            if (Renderer != null)
            {
                Renderer.ProgressUpdate -= DoUpdate;
                Renderer.RenderError -= HandleError;
            }

            ToggleControls(false, World != null, false);
            SetStatus(Abort ? "Aborted" : "Finished");
            SetProgress(0);
            AbortRender = null;
            Abort = false;
        }
        private void DoUpdate(object sender, ProgressUpdateEventArgs e)
        {
            SetStatus(e.ProgressShortDescription);
            SetProgress(e.Progress);
        }
        private void HandleError(object sender, RenderingErrorEventArgs e)
        {
            IRenderer Renderer = (IRenderer)sender;
            if (e.IsFatal)
                Renderer.Abort();
            if (!RenderingConfig.IsPreview)
            {
                lock (SkipErrors)
                {
                    // *** Alert user
                    if (SkipErrors.Contains(e.ErrorCode))
                        return;

                    String ErrorMessage = String.Format("An error occurred while rendering: \r\n{0}", e.UserErrorMessage);
                    if (e.ShowInnerException)
                        ErrorMessage += e.ErrorException.Message + "\r\n";
                    if (!e.IsFatal)
                        ErrorMessage += "Ignore this error if it occurs again?";

                    DialogResult R = MessageBox.Show(ErrorMessage, e.IsFatal ? "Rendering Failed" : "Rendering Error", e.IsFatal ? MessageBoxButtons.OK : MessageBoxButtons.YesNoCancel, e.IsFatal ? MessageBoxIcon.Stop : MessageBoxIcon.Error);

                    if (R == System.Windows.Forms.DialogResult.Yes)
                        SkipErrors.Add(e.ErrorCode);
                }
            }


        }
        private void PreviewTimeout(object sender, EventArgs e)
        {
            tmrPreview.Stop();
            Thread RenderThread = new Thread(() => Render(true));
            RenderThread.Start();
        }

        #endregion

        #region "UI update functions"

        /// <summary>
        ///     Thread-safe way to set the progress bar
        /// </summary>
        /// <param name="Progress">
        ///     Scalar value between 0.0 and 1.0 representing the progress bar file amount 
        /// </param>
        void SetProgress(float Progress)
        {
            int Value = (int)Math.Round(Progress * pbRenderProgress.Maximum);
            Value = Math.Max(Math.Min(Value, pbRenderProgress.Maximum), pbRenderProgress.Minimum);
            statusStrip1.UIThread(() => pbRenderProgress.Value = Value);
        }

        /// <summary>
        ///     Thread-safe way to set the status bar
        /// </summary>
        /// <param name="Status">
        ///     Text to set the status bar to
        /// </param>
        void SetStatus(String Status)
        {
            statusStrip1.UIThread(() => lblStatus.Text = Status);
        }

        #endregion

        private delegate void SelectionUpdater();
        private SelectionUpdater InvokablePaletteSelector;
        private void UpdatePaletteSelection()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(InvokablePaletteSelector);
                return;
            }

            foreach (DataGridViewRow Row in dgPalettes.Rows)
            {
                Row.Cells[0].Value = ((PaletteFile)Row.Tag).Selected;

            }

        }

        private void ExecuteOpenWorld(object sender, EventArgs e)
        {
            DialogResult Result = fbOpenFolder.ShowDialog();

            // *** Open world, or at least try
            if (Result != System.Windows.Forms.DialogResult.OK)
                return;

            ToggleControls(false, false, false);

            World = null;

            try
            {
                World = Substrate.AnvilWorld.Open(fbOpenFolder.SelectedPath);

                // *** Load Dimension list
                String FilePath = World.Path;

                Regex ValidDimNames = new Regex(@"(?<Path>(?:DIM[^\-\d]*)?(?<Name>-?\d{1,}|region))$");

                cbDimension.Items.Clear();
                cbDimension.BeginUpdate();

                int UseIndex = -1;

                foreach (String S in Directory.GetDirectories(World.Path))
                {
                    Match Match = ValidDimNames.Match(S);

                    if (!Match.Success)
                        continue;

                    DimensionListEntry Entry = new DimensionListEntry("Dimension " + Match.Groups["Name"].Value, Match.Groups["Path"].Value);

                    if (Match.Groups["Name"].Value == "region")
                    {
                        Entry.Name = "Overworld";
                        Entry.Value = "";
                        UseIndex = cbDimension.Items.Count;
                    }
                    else if (Match.Groups["Name"].Value == "-1")
                    {
                        Entry.Name = "The Nether";
                    }
                    else if (Match.Groups["Name"].Value == "1")
                    {
                        Entry.Name = "The End";
                    }
                    else if (Match.Groups["Path"].Value.Contains("_MYST"))
                    {
                        Entry.Name = "Mystcraft " + Entry.Name;
                    }
                    cbDimension.Items.Add(Entry);

                }
                cbDimension.SelectedIndex = UseIndex;
                cbDimension.EndUpdate();
                SetStatus(String.Format("Loaded {0}", World.Level.LevelName));

                ToggleControls(false, true, false);

                return;
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show(string.Format("The folder {0} does not appear to contain a valid minecraft world", fbOpenFolder.SelectedPath), "Error Loading World", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error - Could not load world:\r\n{1}\r\n({0})", ex.Message, ex.GetType().Name), "Error Loading World", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            lblStatus.Text = "World load failed";

        }

        private void QueuePreview()
        {
            if (World != null)
            {
                tmrPreview.Stop();
                tmrPreview.Start();
            }
        }

        #region "Controls"

        private void tbLightLevel_Scroll(object sender, EventArgs e)
        {
            RenderingConfig.MinLightLevel = tbLightLevel.Value;
            QueuePreview();
        }
        private void cbDimension_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Measure World
            WorldMetrics Metrics = new WorldMetrics(World, ((DimensionListEntry)cbDimension.SelectedItem).Value);

            RenderingConfig.Dimension = Metrics.Dimension;
            RenderingConfig.Metrics = Metrics;

            lblWorldMax.Text = String.Format("{0}, {1}", Metrics.MaxX, Metrics.MaxZ);
            lblWorldMin.Text = String.Format("{0}, {1}", Metrics.MinX, Metrics.MinZ);

            QueuePreview();
        }
        private void cbRenderer_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedRenderer = (RendererListEntry)cbRenderer.SelectedItem;
            CachedConfigForm = null;
        }
        private void btnAbortRender_Click(object sender, EventArgs e)
        {
            if (AbortRender != null)
                AbortRender();
            Abort = true;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            fdSave.Filter = "All Files|*.*|PNG Files (*.png)|*.png";
            fdSave.ShowDialog();
            RenderingConfig.SaveFilename = fdSave.FileName;
            Thread RenderThread = new Thread(() => Render(false));
            RenderThread.Start();
        }
        private void dgPalettes_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            QueuePreview();
            dgPalettes.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }
        private void btnRendererOptions_Click(object sender, EventArgs e)
        {
            if (CachedConfigForm == null)
            {
                IRenderer Renderer = RendererManager.Instance().InstantiateRenderer((SelectedRenderer).RendererName);
                CachedConfigForm = Renderer.ConfigurationForm;
            }
            if (CachedConfigForm == null)
            {
                MessageBox.Show(". does not have an advanced configuration.", "No Advanced Configuration", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                CachedConfigForm.ConfigStrings = RenderingConfig.AdvancedRenderOptions;
                CachedConfigForm.ShowDialog(this);
                RenderingConfig.AdvancedRenderOptions = CachedConfigForm.ConfigStrings;
                QueuePreview();
            }
        }

        #endregion

        private void btnCopyCLI_Click(object sender, EventArgs e)
        {
            StringBuilder SB = new StringBuilder();

            fbOpenFolder.Description = "Select output folder";
            fbOpenFolder.ShowDialog(this);

            SB.Append("SeeSharp");
            SB.Append(" --world \"" + World.Path + "\"");
            SB.Append(" --dimension \"" + ((DimensionListEntry)cbDimension.SelectedItem).Value + "\"");

            SB.Append(" --output \"" + fbOpenFolder.SelectedPath + Path.DirectorySeparatorChar + "map.png\"");
            SB.Append(" --light-level " + tbLightLevel.Value.ToString());
            SB.Append(" --render-core " + ((RendererListEntry)cbRenderer.SelectedItem).RendererName);

            if (CachedConfigForm != null)
            {
                List<KeyValuePair<String, String>> L = CachedConfigForm.ConfigStrings;
                foreach (KeyValuePair<String, String> ConfigPair in L)
                    SB.Append(" --render-option " + ConfigPair.Key + " \"" + ConfigPair.Value + "\"");
            }

            if (cbCropMap.Checked)
            {
                SB.Append(" --subregion");
                SB.Append(" --subregion-min-x " + nudXMin.Value.ToString());
                SB.Append(" --subregion-min-z " + nudZMin.Value.ToString());
                SB.Append(" --subregion-max-x " + nudXMax.Value.ToString());
                SB.Append(" --subregion-max-z " + nudZMax.Value.ToString());
            }


            if (nudThreads.Value != 1)
                SB.Append(" --multi-thread --max-threads " + nudThreads.Value.ToString());

            foreach (PaletteFile Palette in PaletteManager.Instance().AllPalettes.Where((x) => x.Selected))
                SB.Append(" --palette " + Palette.PalettePath.ConvertToRelativePath());


            SB.Append("");
            Clipboard.SetText(SB.ToString());

            SetStatus("Copied to Clipboard");
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            btnAbortRender_Click(null, null);
            tmrPreview.Stop();
        }

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            e.Cancel = !e.TabPage.Enabled;
            if (!e.Cancel && e.TabPageIndex < 3)
            {
                cbCropMap.Parent = e.TabPage;
                gbSubregion.Parent = e.TabPage;
                gbWorldDimensions.Parent = e.TabPage;
                cbDimension.Parent = e.TabPage;
                lblDimension.Parent = e.TabPage;
                dgPalettes.Parent = e.TabPage;
                lblSelectPalettes.Parent = e.TabPage;
            }
        }

        private void cbShowCLIButton_CheckedChanged(object sender, EventArgs e)
        {
            btnCopyCLI.Visible = cbShowCLIButton.Checked;
            Properties.Settings.Default.ShowCLIButton = cbShowCLIButton.Checked;
            Properties.Settings.Default.Save();
        }

        private void cbAutoUpdate_CheckedChanged(object sender, EventArgs e)
        {
            pnlUpdate.Enabled = cbAutoUpdate.Checked;
            Properties.Settings.Default.AutoCheck = cbAutoUpdate.Checked;
            Properties.Settings.Default.Save();
            ToggleControls(false, World != null, false, tpSettings);
        }

        private void rbAlwaysUpdate_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.AutoUpdate = rbAlwaysUpdate.Checked;
            Properties.Settings.Default.Save();
        }

        private void cbTrackChanges_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.ShowCLIButton = cbShowCLIButton.Checked;
            Properties.Settings.Default.Save();
        }

        private void cbMultithread_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.MTThreadCount = cbMultithread.Checked ? (int)nudThreads.Value : 1;
            Properties.Settings.Default.Save();
            ToggleControls(false, World != null, false, tpSettings);
        }
    }
}