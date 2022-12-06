using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Krypton.Toolkit;
using UEExplorer.Development;
using UEExplorer.Properties;
using UEExplorer.UI.Dialogs;
using UEExplorer.UI.Forms;
using UEExplorer.UI.Pages;
using UEExplorer.UI.Panels;
using UELib;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;

namespace UEExplorer.UI.Main
{
    public partial class ProgramForm : KryptonForm
    {
        internal ProgramForm()
        {
            InitializeComponent();

            WindowState = Settings.Default.WindowState;
            Size = Settings.Default.WindowSize;
            Location = Settings.Default.WindowLocation;
        }

        public static string Version => Assembly.GetExecutingAssembly().GetName().Version.ToString();

        private void ProgramForm_Load(object sender, EventArgs e)
        {
            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                InitializeConfig();
                InitializeUI();
                InitializeControls();
                BeginInvoke((MethodInvoker)InitializeState);
            }
        }

        private void InitializeControls()
        {
            var startPage = new StartPage { UniqueName = "Start" };
            dockSpace.AddDocument(startPage);
            dockSpace.AddPackageExplorer();
        }

        private void InitializeState()
        {
            // Load all files from the commandline arguments.
            string[] args = Environment.GetCommandLineArgs();
            for (int i = 1; i < args.Length; ++i)
            {
                if (!File.Exists(args[i]))
                {
                    continue;
                }

                string filePath = args[i];
                BeginInvoke((MethodInvoker)(() => LoadFromFile(filePath)));
            }

            // FIXME:
            //var state = Program.Options.GetState(_UnrealPackage.FullPackageName);
            //if (state.SearchObjectValue != null)
            //{
            //    PerformActionByObjectPath(state.SearchObjectValue);
            //}

            // Restore all open packages
            if (UserHistory.Default.OpenFiles != null) foreach (var defaultOpenFile in UserHistory.Default.OpenFiles)
            {
                dockSpace.AddPackage(defaultOpenFile);
            }
        }

        private void InitializeUI()
        {
            Text = string.Format(Resources.ProgramTitle, Application.ProductName, Version);

            ProgressStatus.Status = progressStatusLabel;
            ProgressStatus.Loading = loadingProgressBar;
        }

        private void InitializeConfig()
        {
            Program.LoadConfig();

            nativeTableDropDownButton.Text = Program.Options.NTLPath;
            Platform.Text = Program.Options.Platform;
        }

        private void InitializeExtensions()
        {
            string extensionsPath = Path.Combine(Application.StartupPath, "Extensions");
            if (!Directory.Exists(extensionsPath))
            {
                return;
            }

            string[] files = Directory.GetFiles(extensionsPath);
            foreach (string file in files)
            {
                if (Path.GetExtension(file) != ".dll")
                {
                    continue;
                }

                var assembly = Assembly.LoadFile(file);
                var types = assembly.GetExportedTypes();
                foreach (var t in types)
                {
                    var i = t.GetInterface("IExtension");
                    if (i == null)
                    {
                        continue;
                    }

                    var ext = (IExtension)Activator.CreateInstance(t);
                    ext.Initialize(this);

                    var extensionTitleAttribute = t
                        .GetCustomAttribute<ExtensionTitleAttribute>(false);
                    if (extensionTitleAttribute == null)
                    {
                        throw new NotSupportedException("Missing ExtensionTitleAttribute on extension class");
                    }

                    string extensionName = extensionTitleAttribute.Title;
                    var item = extentionsMenuitem.DropDownItems.Add(extensionName);
                    item.Click += ext.OnActivate;
                    extentionsMenuitem.Enabled = true;
                }
            }
        }

        private void SelectedNativeTable_DropDownOpening(object sender, EventArgs e)
        {
            // Rebuild it, to reflect the changes made to the current directory of NTL files.
            nativeTableDropDownButton.DropDown.Items.Clear();
            foreach (string filePath in SettingsPanel.GetNativeTables())
            {
                nativeTableDropDownButton.DropDown.Items.Add(Path.GetFileNameWithoutExtension(filePath));
            }

            // In case it got changed!
            nativeTableDropDownButton.Text = Program.Options.NTLPath;
        }

        private void SelectedNativeTable_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            nativeTableDropDownButton.Text = e.ClickedItem.Text;

            Program.Options.NTLPath = nativeTableDropDownButton.Text;
            Program.SaveConfig();
        }

        private void ToolsToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            menuItem20.Checked = Program.AreFileTypesRegistered();

            if (extentionsMenuitem.Enabled)
            {
                return;
            }

            InitializeExtensions();
        }

        public void LoadFromFile(string filePath)
        {
            Program.PushRecentOpenedFile(filePath);
            switch (Path.GetExtension(filePath))
            {
                default:
                    dockSpace.AddPackage(filePath);
                    break;
            }
        }

        private void CheckForUpdatesMenuItem_Click(object sender, EventArgs e)
        {
            Program.CheckForUpdates();
        }

        private void Platform_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            Platform.Text = e.ClickedItem.Text;
            Program.Options.Platform = Platform.Text;
            Program.SaveConfig();
        }

        private void SocialMenuItem_Click(object sender, EventArgs e) =>
            Process.Start(Resources.UEExplorerFacebookUrl);

        private void OnClosing(object sender, FormClosingEventArgs e)
        {
            if (WindowState != FormWindowState.Normal)
            {
                Settings.Default.WindowLocation = RestoreBounds.Location;
                Settings.Default.WindowSize = RestoreBounds.Size;
            }
            else
            {
                Settings.Default.WindowLocation = Location;
                Settings.Default.WindowSize = ClientSize;
            }

            Settings.Default.WindowState =
                WindowState == FormWindowState.Minimized ? FormWindowState.Normal : WindowState;
            Settings.Default.Save();
        }

        /// <summary>
        ///     Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ProgressStatus.Dispose();
                components?.Dispose();
            }

            base.Dispose(disposing);
        }

        private void ReportAnIssue(object sender, EventArgs e) =>
            Process.Start(Resources.UEExplorerIssuesUrl);

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var about = new AboutDialog())
            {
                about.ShowDialog();
            }
        }

        private void OpenFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog
                   {
                       Filter = UnrealExtensions.FormatUnrealExtensionsAsFilter(),
                       FilterIndex = 1,
                       Title = Resources.Open_File,
                       Multiselect = true
                   })
            {
                if (ofd.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }

                foreach (string filePath in ofd.FileNames)
                {
                    BeginInvoke((MethodInvoker)(() => LoadFromFile(filePath)));
                }
            }
        }

        private void UnrealColorGeneratorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // open a tool dialog!
            var cgf = new ColorGeneratorForm();
            cgf.Show();
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e) => Application.Exit();

        private void ProgramForm_Shown(object sender, EventArgs e) => Program.CheckForUpdates();

        private void ToggleUEExplorerFileIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!menuItem20.Checked)
            {
                MessageBox.Show(
                    Resources.RegistryWarning,
                    Resources.Warning,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }

            Program.ToggleRegisterFileTypes(menuItem20.Checked);
        }

        private void ProgramForm_DragOver(object sender, DragEventArgs e) =>
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop)
                ? DragDropEffects.Link
                : DragDropEffects.None;

        private void ProgramForm_DragEnter(object sender, DragEventArgs e) =>
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop)
                ? DragDropEffects.Link
                : DragDropEffects.None;

        private void ProgramForm_DragDrop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                return;
            }

            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string filePath in files)
            {
                if (UnrealLoader.IsUnrealFileSignature(filePath))
                {
                    BeginInvoke((MethodInvoker)(() => LoadFromFile(filePath)));
                }
            }
        }

        private void fileMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            var filePaths = UserHistory.Default.RecentFiles;
            mostRecentMenuItem.Enabled = filePaths.Count > 0;
            if (mostRecentMenuItem.Enabled)
            {
                mostRecentMenuItem.DropDownItems.Add("PLACEHOLDER");
            }
        }

        private void mostRecentMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            var filePaths = UserHistory.Default.RecentFiles;

            // Rebuild all items
            mostRecentMenuItem.DropDownItems.Clear();
            for (int i = filePaths.Count - 1; i >= 0; --i)
            {
                if (filePaths[i] == string.Empty) continue;

                string fileName = Path.GetFileName(filePaths[i]);
                string directoryName = Path.GetDirectoryName(filePaths[i]);
                var item = mostRecentMenuItem.DropDownItems.Add
                (
                    $"{filePaths.Count - i} {fileName} ({directoryName})"
                );
                item.Tag = filePaths[i];
            }
        }

        private void mostRecentMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string filePath = e.ClickedItem.Tag as string;
            if (!File.Exists(filePath))
            {
                var filePaths = UserHistory.Default.RecentFiles;
                filePaths.Remove(filePath);
                return;
            }

            BeginInvoke((MethodInvoker)(() => LoadFromFile(filePath)));
        }

        private void packageExplorerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dockSpace.HasPage("PackageExplorer"))
            {
                return;
            }

            dockSpace.AddPackageExplorer();
        }

        // TODO: Propagate commands a different way.
        private void navigateBackwardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dockSpace.ToolStripButton_Backward_Click(sender, e);
        }

        // TODO: Propagate commands a different way.
        private void navigateForwardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dockSpace.ToolStripButton_Forward_Click(sender, e);
        }

        private void forumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(Program.ForumUrl);
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dockSpace.HasPage("Settings"))
            {
                return;
            }

            dockSpace.AddDocument<SettingsPanel>(Resources.SettingsTitle, "Settings");
        }
    }

    public static class ProgressStatus
    {
        public static ToolStripProgressBar Loading;
        public static ToolStripStatusLabel Status;

        private static string s_savedStatus;

        public static void Dispose()
        {
            if (Loading != null)
            {
                Loading.Dispose();
                Loading = null;
            }

            if (Status != null)
            {
                Status.Dispose();
                Status = null;
            }
        }

        public static void SetStatus(string status)
        {
            if (Status != null)
            {
                Status.Text = status;
                Status.Invalidate();
            }
        }

        public static void Reset()
        {
            ResetStatus();
            ResetValue();
        }

        public static void ResetStatus() => SetStatus(s_savedStatus);

        public static void SaveStatus() => s_savedStatus = Status.Text;

        public static void IncrementValue() => ++Loading.Value;

        public static void ResetValue()
        {
            Loading.Visible = false;
            Loading.Value = 0;
            Loading.Invalidate();
        }

        public static void SetMaxProgress(int max)
        {
            Loading.Visible = true;
            Loading.Maximum = max;
            Loading.Invalidate();
        }
    }
}
