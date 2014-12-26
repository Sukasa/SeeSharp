using System;
using System.Collections.Generic;
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
    internal partial class FrmMain : Form
    {
        //private List<Type> RendererTypes = new List<Type>();
        //private List<String> RendererNames = new List<string>();
        private readonly List<int> _SkipErrors = new List<int>();

        private AnvilWorld _World;

        private readonly RenderConfiguration _RenderingConfig = new RenderConfiguration();
        private RendererListEntry _SelectedRenderer;
        private RendererConfigForm _CachedConfigForm;

        private delegate void AbortRenderDelegate();

        private AbortRenderDelegate _AbortRender;
        private bool _Abort;

        private FileSystemWatcher _PaletteWatcher;

        internal FrmMain()
        {
            InitializeComponent();

            // *** Why is this here?
            nudZMin.ValueChanged += SetupSubregion;
            nudXMin.ValueChanged += SetupSubregion;
            nudZMax.ValueChanged += SetupSubregion;
            nudXMax.ValueChanged += SetupSubregion;

            _InvokableControlToggler = ToggleControls;
            _InvokablePaletteSelector = UpdatePaletteSelection;
            PaletteManager.Instance();

        }

        #region "Meta Configuration"

        private void nudThreads_ValueChanged(object Sender, EventArgs E)
        {
            int Value = (int)nudThreads.Value;
            _RenderingConfig.EnableMultithreading = Value > 1;
            _RenderingConfig.MaxThreads = Value;
            Properties.Settings.Default.MTThreadCount = Value;
            Properties.Settings.Default.Save();
        }

        #endregion

        #region "Render Configuration"

        private void cbCropMap_CheckedChanged(object Sender, EventArgs E)
        {
            gbSubregion.Enabled = cbCropMap.Checked;
        }
        private void SetupSubregion(object Sender, EventArgs E)
        {
            SetupSubregion();
        }
        private void SetupSubregion()
        {
            nudXMin.Maximum = nudXMax.Value - 1;
            nudZMin.Maximum = nudZMax.Value - 1;
            nudXMax.Minimum = nudXMin.Value + 1;
            nudZMax.Minimum = nudZMin.Value + 1;

            _RenderingConfig.RenderSubregion = cbCropMap.Checked;
        }

        #endregion

        #region "Setup"

        private void frmMain_Load(object Sender, EventArgs E)
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

            lblStatus.Text = string.Format("See Sharp v{0}", Assembly.GetExecutingAssembly().GetName().Version);
            LoadRenderers();

            nudThreads.Maximum = Environment.ProcessorCount;


            nudThreads.Value = ThreadingCount > 1 ? Math.Min(ThreadingCount, nudThreads.Maximum) : nudThreads.Maximum;

            toolStrip1.Renderer = new ToolStripProfessionalRendererNoSideLine();

            tabPage2.Enabled = false;

            ToggleControls(false, false, false);
        }

        void PaletteFilesUpdated(object Sender, FileSystemEventArgs E)
        {
            SetupPalettes();
        }

        private void SetupPalettes()
        {
            PaletteManager Manager = PaletteManager.Instance();
            Manager.Reload();
            Manager.AutoConfig(_World != null ? _World.Path : "", _SelectedRenderer.RendererName);
            dgPalettes.Rows.Clear();
            foreach (PaletteFile File in Manager.AllPalettes)
            {
                DataGridViewRow Row = dgPalettes.Rows[dgPalettes.Rows.Add(File.Selected, File.Name, File.Version)];
                Row.Tag = File;
                if (File.Version != String.Empty)
                    Row.Cells[1].ToolTipText = string.Format("{0} v{1}\r\n{2}", File.Name, File.Version, File.Description);
                else
                    Row.Cells[1].ToolTipText = string.Format("{0}\r\n{1}", File.Name, File.Description);

            }

            if (chkTrackChanges.Checked && _PaletteWatcher == null)
            {
                // ReSharper disable once AssignNullToNotNullAttribute
                // *** Safe to assume the EXE file isn't going to have a NULL path.
                _PaletteWatcher = new FileSystemWatcher(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
                {
                    EnableRaisingEvents = true,
                    IncludeSubdirectories = true,
                    Filter = "*.pal",
                    NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.DirectoryName
                };
                _PaletteWatcher.Changed += PaletteFilesUpdated;
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
                ControlRef.Invoke(_InvokableControlToggler, IsBusy, WorldLoaded, IsPreviewing, ControlRef);
                return;
            }

            btnCopyCLI.Enabled = WorldLoaded;

            foreach (Control Control in ControlRef.GetSubControls())
            {
                String ControlTag = (String)Control.Tag;
                if (ControlTag == null)
                    ControlTag = String.Empty;

                if (ControlTag.Contains("IGNORE"))
                    continue;

                Control.Enabled = true;

                if ((Control is ScrollBar) || (Control is NumericUpDown)) //typeof(NumericUpDown).IsAssignableFrom(Control.Parent.GetType()))
                {
                    continue;
                }

                if (ControlTag.Contains("SUBREGION") && !cbCropMap.Checked)
                    Control.Enabled = false;

                if (ControlTag.Contains("AUTOUPDATE") && !cbAutoUpdate.Checked)
                    Control.Enabled = false;

                if (ControlTag.Contains("MULTITHREAD") && !cbMultithread.Checked)
                    Control.Enabled = false;

                if (ControlTag.Contains("ALWAYS")) // *** Always enabled
                    continue;

                if (!WorldLoaded && !ControlTag.Contains("NOWORLD"))
                {
                    Control.Enabled = false;
                }
                else
                {
                    if (IsBusy ^ ControlTag.Contains("!NOLOCK"))
                        Control.Enabled = false;

                    if (IsPreviewing && ControlTag.Contains("PREVIEW"))
                        Control.Enabled = true;
                }
            }
        }
        private delegate void ControlToggler(bool A, bool B, bool C, Control D = null);

        private readonly ControlToggler _InvokableControlToggler;
        private void Render(bool IsPreview)
        {
            _Abort = false;

            ToggleControls(!IsPreview, true, IsPreview);
            SetStatus("Initializing...");
            IRenderer Renderer = null;

            // *** Set up renderer config
            _RenderingConfig.IsPreview = IsPreview;
            _RenderingConfig.Chunks = _World.GetChunkManager(_RenderingConfig.Dimension);
            _RenderingConfig.Palette = new BlockPalette();
            _RenderingConfig.RenderSubregion = cbCropMap.Checked;
            _RenderingConfig.MaxThreads = cbMultithread.Checked ? (int)nudThreads.Value : 1;

            // ReSharper disable once InconsistentlySynchronizedField
            // *** This section of code is run before the parallel processing begins, so there is no danger of sync issues.
            _SkipErrors.Clear();

            if (IsPreview)
            {
                _RenderingConfig.RenderSubregion = true;
                _RenderingConfig.SubregionChunks = new Rectangle(-15, -15, 31, 31);
            }
            else if (_RenderingConfig.RenderSubregion)
                _RenderingConfig.SubregionChunks = new Rectangle((int)nudXMin.Value, (int)nudZMin.Value, (int)nudXMax.Value - (int)nudXMin.Value + 1, (int)nudZMax.Value - (int)nudZMin.Value + 1);
            else
                _RenderingConfig.SubregionChunks = new Rectangle(_RenderingConfig.Metrics.MinX, _RenderingConfig.Metrics.MinZ, _RenderingConfig.Metrics.MaxX - _RenderingConfig.Metrics.MinX, _RenderingConfig.Metrics.MaxZ - _RenderingConfig.Metrics.MinZ);

            if (_Abort)
                goto Cleanup;

            foreach (DataGridViewRow Row in dgPalettes.Rows)
                ((PaletteFile)Row.Tag).Selected = (bool)Row.Cells[0].Value;


            // *** Set up palettes
            foreach (String Palette in from PaletteFile File in PaletteManager.Instance().AllPalettes where File.Selected select File.PalettePath)
            {
                try
                {
                    _RenderingConfig.Palette.LoadPalette(Palette);
                }
                catch (BlockPalette.PaletteExecutionException Ex)
                {
                    MessageBox.Show(string.Format("The palette file {0} is invalid and will be skipped:\r\n{1}", Path.GetFileName(Palette), Ex.Message), "Palette Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    PaletteManager.Instance().AllPalettes.First(x => x.PalettePath == Palette).Selected = false;
                }
            }

            if (_Abort)
                goto Cleanup;

            UpdatePaletteSelection();

            _RenderingConfig.Palette.AssembleLookupTables();

            // *** Initialize renderer
            Renderer = RendererManager.Instance().InstantiateRenderer((_SelectedRenderer).RendererName);

            Renderer.ProgressUpdate += DoUpdate;
            Renderer.RenderError += HandleError;

            Renderer.Configure(_RenderingConfig);
            Renderer.Initialize();

            _AbortRender = Renderer.Abort;

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

            ToggleControls(false, _World != null, false);
            SetStatus(_Abort ? "Aborted" : "Finished");
            SetProgress(0);
            _AbortRender = null;
            _Abort = false;
        }
        private void DoUpdate(object Sender, ProgressUpdateEventArgs E)
        {
            SetStatus(E.ProgressShortDescription);
            SetProgress(E.Progress);
        }
        private void HandleError(object Sender, RenderingErrorEventArgs E)
        {
            IRenderer Renderer = (IRenderer)Sender;
            if (E.IsFatal)
                Renderer.Abort();
            if (!_RenderingConfig.IsPreview)
            {
                lock (_SkipErrors)
                {
                    // *** Alert user
                    if (_SkipErrors.Contains(E.ErrorCode))
                        return;

                    String ErrorMessage = String.Format("An error occurred while rendering: \r\n{0}", E.UserErrorMessage);
                    if (E.ShowInnerException)
                        ErrorMessage += E.ErrorException.Message + "\r\n";
                    if (!E.IsFatal)
                        ErrorMessage += "Ignore this error if it occurs again?";

                    DialogResult R = MessageBox.Show(ErrorMessage, E.IsFatal ? "Rendering Failed" : "Rendering Error", E.IsFatal ? MessageBoxButtons.OK : MessageBoxButtons.YesNoCancel, E.IsFatal ? MessageBoxIcon.Stop : MessageBoxIcon.Error);

                    if (R == DialogResult.Yes)
                        _SkipErrors.Add(E.ErrorCode);
                }
            }


        }
        private void PreviewTimeout(object Sender, EventArgs E)
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
        private readonly SelectionUpdater _InvokablePaletteSelector;
        private void UpdatePaletteSelection()
        {
            if (InvokeRequired)
            {
                Invoke(_InvokablePaletteSelector);
                return;
            }

            foreach (DataGridViewRow Row in dgPalettes.Rows)
            {
                Row.Cells[0].Value = ((PaletteFile)Row.Tag).Selected;

            }

        }

        private void ExecuteOpenWorld(object Sender, EventArgs E)
        {
            DialogResult Result = fbOpenFolder.ShowDialog();

            // *** Open world, or at least try
            if (Result != DialogResult.OK)
                return;

            ToggleControls(false, false, false);

            _World = null;

            try
            {
                _World = AnvilWorld.Open(fbOpenFolder.SelectedPath);

                // *** Load Dimension list

                Regex ValidDimNames = new Regex(@"(?<Path>(?:DIM[^\-\d]*)?(?<Name>-?\d{1,}|region))$");

                cbDimension.Items.Clear();
                cbDimension.BeginUpdate();

                int UseIndex = -1;

                foreach (String S in Directory.GetDirectories(_World.Path))
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
                SetStatus(String.Format("Loaded {0}", _World.Level.LevelName));

                ToggleControls(false, true, false);

                return;
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show(string.Format("The folder {0} does not appear to contain a valid minecraft world", fbOpenFolder.SelectedPath), "Error Loading World", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            catch (Exception Ex)
            {
                MessageBox.Show(string.Format("Error - Could not load world:\r\n{1}\r\n({0})", Ex.Message, Ex.GetType().Name), "Error Loading World", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            lblStatus.Text = "World load failed";

        }

        private void QueuePreview()
        {
            if (_World != null)
            {
                tmrPreview.Stop();
                tmrPreview.Start();
            }
        }

        #region "Controls"

        private void tbLightLevel_Scroll(object Sender, EventArgs E)
        {
            _RenderingConfig.MinLightLevel = tbLightLevel.Value;
            QueuePreview();
        }
        private void cbDimension_SelectedIndexChanged(object Sender, EventArgs E)
        {
            // Measure World
            WorldMetrics Metrics = new WorldMetrics(_World, ((DimensionListEntry)cbDimension.SelectedItem).Value);

            _RenderingConfig.Dimension = Metrics.Dimension;
            _RenderingConfig.Metrics = Metrics;

            lblWorldMax.Text = String.Format("{0}, {1}", Metrics.MaxX, Metrics.MaxZ);
            lblWorldMin.Text = String.Format("{0}, {1}", Metrics.MinX, Metrics.MinZ);

            QueuePreview();
        }
        private void cbRenderer_SelectedIndexChanged(object Sender, EventArgs E)
        {
            _SelectedRenderer = (RendererListEntry)cbRenderer.SelectedItem;
            _CachedConfigForm = null;
        }
        private void btnAbortRender_Click(object Sender, EventArgs E)
        {
            if (_AbortRender != null)
                _AbortRender();
            _Abort = true;
        }
        private void button1_Click(object Sender, EventArgs E)
        {
            fdSave.Filter = "All Files|*.*|PNG Files (*.png)|*.png";
            fdSave.ShowDialog();
            _RenderingConfig.SaveFilename = fdSave.FileName;
            Thread RenderThread = new Thread(() => Render(false));
            RenderThread.Start();
        }
        private void dgPalettes_CurrentCellDirtyStateChanged(object Sender, EventArgs E)
        {
            QueuePreview();
            dgPalettes.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }
        private void btnRendererOptions_Click(object Sender, EventArgs E)
        {
            if (_CachedConfigForm == null)
            {
                IRenderer Renderer = RendererManager.Instance().InstantiateRenderer((_SelectedRenderer).RendererName);
                _CachedConfigForm = Renderer.ConfigurationForm;
            }
            if (_CachedConfigForm == null)
            {
                MessageBox.Show(". does not have an advanced configuration.", "No Advanced Configuration", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                _CachedConfigForm.ConfigStrings = _RenderingConfig.AdvancedRenderOptions;
                _CachedConfigForm.ShowDialog(this);
                _RenderingConfig.AdvancedRenderOptions = _CachedConfigForm.ConfigStrings;
                QueuePreview();
            }
        }

        #endregion

        private void btnCopyCLI_Click(object Sender, EventArgs E)
        {
            StringBuilder SB = new StringBuilder();

            fbOpenFolder.Description = "Select output folder";
            fbOpenFolder.ShowDialog(this);

            SB.Append("SeeSharp");
            SB.Append(" --world \"" + _World.Path + "\"");
            SB.Append(" --dimension \"" + ((DimensionListEntry)cbDimension.SelectedItem).Value + "\"");

            SB.Append(" --output \"" + fbOpenFolder.SelectedPath + Path.DirectorySeparatorChar + "map.png\"");
            SB.Append(" --light-level " + tbLightLevel.Value.ToString());
            SB.Append(" --render-core " + ((RendererListEntry)cbRenderer.SelectedItem).RendererName);

            if (_CachedConfigForm != null)
            {
                List<KeyValuePair<String, String>> L = _CachedConfigForm.ConfigStrings;
                foreach (KeyValuePair<String, String> ConfigPair in L)
                    SB.Append(" --render-option " + ConfigPair.Key + " \"" + ConfigPair.Value + "\"");
            }

            if (cbCropMap.Checked)
            {
                SB.Append(" --subregion");
                SB.Append(" --subregion-min-x " + nudXMin.Value.ToString(Thread.CurrentThread.CurrentCulture));
                SB.Append(" --subregion-min-z " + nudZMin.Value.ToString(Thread.CurrentThread.CurrentCulture));
                SB.Append(" --subregion-max-x " + nudXMax.Value.ToString(Thread.CurrentThread.CurrentCulture));
                SB.Append(" --subregion-max-z " + nudZMax.Value.ToString(Thread.CurrentThread.CurrentCulture));
            }


            if (nudThreads.Value != 1)
                SB.Append(" --multi-thread --max-threads " + nudThreads.Value.ToString(Thread.CurrentThread.CurrentCulture));

            foreach (PaletteFile Palette in PaletteManager.Instance().AllPalettes.Where(x => x.Selected))
                SB.Append(" --palette " + Palette.PalettePath.ConvertToRelativePath());


            SB.Append("");
            Clipboard.SetText(SB.ToString());

            SetStatus("Copied to Clipboard");
        }

        private void frmMain_FormClosing(object Sender, FormClosingEventArgs E)
        {
            btnAbortRender_Click(null, null);
            tmrPreview.Stop();
        }

        private void tabControl1_Selecting(object Sender, TabControlCancelEventArgs E)
        {
            E.Cancel = !E.TabPage.Enabled;
            if (!E.Cancel && E.TabPageIndex < 3)
            {
                cbCropMap.Parent = E.TabPage;
                gbSubregion.Parent = E.TabPage;
                gbWorldDimensions.Parent = E.TabPage;
                cbDimension.Parent = E.TabPage;
                lblDimension.Parent = E.TabPage;
                dgPalettes.Parent = E.TabPage;
                lblSelectPalettes.Parent = E.TabPage;
            }
        }

        private void cbShowCLIButton_CheckedChanged(object Sender, EventArgs E)
        {
            btnCopyCLI.Visible = cbShowCLIButton.Checked;
            Properties.Settings.Default.ShowCLIButton = cbShowCLIButton.Checked;
            Properties.Settings.Default.Save();
        }

        private void cbAutoUpdate_CheckedChanged(object Sender, EventArgs E)
        {
            pnlUpdate.Enabled = cbAutoUpdate.Checked;
            Properties.Settings.Default.AutoCheck = cbAutoUpdate.Checked;
            Properties.Settings.Default.Save();
            ToggleControls(false, _World != null, false, tpSettings);
        }

        private void rbAlwaysUpdate_CheckedChanged(object Sender, EventArgs E)
        {
            Properties.Settings.Default.AutoUpdate = rbAlwaysUpdate.Checked;
            Properties.Settings.Default.Save();
        }

        private void cbTrackChanges_CheckedChanged(object Sender, EventArgs E)
        {
            Properties.Settings.Default.ShowCLIButton = cbShowCLIButton.Checked;
            Properties.Settings.Default.Save();
        }

        private void cbMultithread_CheckedChanged(object Sender, EventArgs E)
        {
            Properties.Settings.Default.MTThreadCount = cbMultithread.Checked ? (int)nudThreads.Value : 1;
            Properties.Settings.Default.Save();
            ToggleControls(false, _World != null, false, tpSettings);
        }
    }
}