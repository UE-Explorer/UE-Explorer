using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.Win32;
using Storm.TabControl;
using UEExplorer.Properties;
using UEExplorer.UI.Forms;
using AutoUpdaterDotNET;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;

namespace UEExplorer.UI
{
    using Development;
    using Dialogs;
    using Tabs;
    using UELib;

    public partial class ProgramForm : Form
    {
        private const string AppKey = "EliotVU";
        public TabsCollection Tabs;

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
            InitializeConfig();
            InitializeUI();

            Tabs = new TabsCollection(TabComponentsStrip);
            Tabs.InsertTab(typeof(UC_Default), Resources.Homepage);

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
        }

        private void InitializeUI()
        {
            ProgressStatus.Status = progressStatusLabel;
            ProgressStatus.Loading = loadingProgressBar;

            nativeTableDropDownButton.Text = Program.Options.NTLPath;
            platformMenuItem.Text = Program.Options.Platform;
#if DEBUG
            cacheExtractorMenuItem.Enabled = true;
#endif
        }

        private void PushRecentOpenedFile(string filePath)
        {
            var filePaths = UserHistory.Default.RecentFiles;
            // If we have one already, we'd like to move it to the most recent.
            filePaths.Remove(filePath);
            filePaths.Add(filePath);
            if (filePaths.Count > 15)
            {
                filePaths.RemoveAt(0);
            }

            mostRecentMenuItem.Enabled = true;
            UserHistory.Default.Save();
        }

        private void InitializeConfig() => Program.LoadConfig();

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
                    var item = menuItem13.DropDownItems.Add(extensionName);
                    item.Click += ext.OnActivate;
                    menuItem13.Enabled = true;
                }
            }
        }

        private void NativeTableDropDownButton_DropDownOpening(object sender, EventArgs e)
        {
            // Rebuild it, to reflect the changes made to the current directory of NTL files.
            nativeTableDropDownButton.DropDown.Items.Clear();
            foreach (string filePath in UC_Options.GetNativeTables())
            {
                nativeTableDropDownButton.DropDown.Items.Add(Path.GetFileNameWithoutExtension(filePath));
            }

            // In case it got changed!
            nativeTableDropDownButton.Text = Program.Options.NTLPath;
        }

        private void NativeTableDropDownButton_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            nativeTableDropDownButton.Text = e.ClickedItem.Text;

            Program.Options.NTLPath = nativeTableDropDownButton.Text;
            Program.SaveConfig();
        }

        private void ToolsMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            toggleFilesAssociationMenuItem.Checked = Program.AreFileTypesRegistered();

            if (menuItem13.Enabled)
            {
                return;
            }

            InitializeExtensions();
        }

        public void LoadFromFile(string filePath)
        {
            if (Tabs.HasTab(filePath))
            {
                return;
            }

            PushRecentOpenedFile(filePath);
            switch (Path.GetExtension(filePath))
            {
                case ".uc":
                case ".uci":
                    var classFileTab = new UC_UClassFile();
                    Tabs.AddTab(classFileTab, Path.GetFileName(filePath), filePath);

                    classFileTab.FileName = filePath;
                    classFileTab.PostInitialize();

                    break;

                default:
                    var packageExplorer = new UC_PackageExplorer();
                    Tabs.AddTab(packageExplorer, Path.GetFileName(filePath), filePath);

                    packageExplorer.FileName = filePath;
                    BeginInvoke((MethodInvoker)(() => packageExplorer.InitializeFromFile(filePath)));

                    break;
            }
        }

        private void DonateMenuItem_Click(object sender, EventArgs e) => Process.Start(Program.Donate_URL);

        private void CheckForUpdates()
        {
            Console.WriteLine(Resources.CHECKING_FOR_UPDATES_LOG, Version);
            AutoUpdater.Start(string.Format(Program.UPDATE_URL, Version));
        }

        private void CheckForUpdatesMenuItem_Click(object sender, EventArgs e)
        {
            var softKey = Registry.CurrentUser.OpenSubKey("Software", true);
            var appKey = softKey?.OpenSubKey(AppKey);
            if (appKey != null)
            {
                softKey.DeleteSubKeyTree(AppKey);
            }

            CheckForUpdates();
        }

        private void OptionsMenuItem_Click(object sender, EventArgs e) =>
            Tabs.InsertTab(typeof(UC_Options), Resources.Options);

        private void ForumMenuItem_Click(object sender, EventArgs e) => Process.Start(Program.Forum_URL);

        private void WebMenuItem_Click(object sender, EventArgs e) => Process.Start(Program.WEBSITE_URL);

        private void platformMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            platformMenuItem.Text = e.ClickedItem.Text;
            Program.Options.Platform = platformMenuItem.Text;
            Program.SaveConfig();
        }

        private void ContactMenuItem_Click(object sender, EventArgs e) => Process.Start(Program.Contact_URL);

        private void OpenHomeButton_Click(object sender, EventArgs e) =>
            Tabs.InsertTab(typeof(UC_Default), Resources.Homepage);

        private void SocialMenuItem_Click(object sender, EventArgs e) =>
            Process.Start("https://www.facebook.com/UE.Explorer");

        private void OnClosed(object sender, FormClosedEventArgs e) => Program.LogManager.EndLogStream();

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

                if (Tabs != null)
                {
                    Tabs.Dispose();
                    Tabs = null;
                }
            }

            base.Dispose(disposing);
        }

        private void ReportAnIssue(object sender, EventArgs e) =>
            Process.Start("https://github.com/UE-Explorer/UE-Explorer/issues");

        private void AboutMenuItem_Click(object sender, EventArgs e)
        {
            using (var about = new AboutDialog())
            {
                about.ShowDialog();
            }
        }

        private void OpenFileMenuItem_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.DefaultExt = "u";
                ofd.Filter = UnrealExtensions.FormatUnrealExtensionsAsFilter();
                ofd.FilterIndex = 1;
                ofd.Title = Resources.Open_File;
                ofd.Multiselect = true;

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

        private void ColorGeneratorMenuItem_Click(object sender, EventArgs e)
        {
            // open a tool dialog!
            var cgf = new ColorGeneratorForm();
            cgf.Show();
        }

        private void CacheExtractorMenuItem_Click(object sender, EventArgs e) =>
            Tabs.InsertTab(typeof(UC_CacheExtractor), Resources.ProgramForm_Cache_Extractor);

        private void TabComponentsStrip_TabStripItemClosing(TabStripItemClosingEventArgs e) => e.Item.Dispose();

        private void TabComponentsStrip_TabStripItemClosed(object sender, EventArgs e)
        {
            //TabComponentsStrip.Visible = TabComponentsStrip.Items.Count > 0;
            //openHomeButton.Visible = !TabComponentsStrip.Visible;
        }

        private void ExitMenuItem_Click(object sender, EventArgs e) => Application.Exit();

        private void ProgramForm_Shown(object sender, EventArgs e) => CheckForUpdates();

        private void ToggleFilesAssociationMenuItem_Click(object sender, EventArgs e)
        {
            if (!toggleFilesAssociationMenuItem.Checked)
            {
                MessageBox.Show(
                    Resources.RegistryWarning,
                    Resources.Warning,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }

            Program.ToggleRegisterFileTypes(toggleFilesAssociationMenuItem.Checked);
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
            string allowedExtensions = UnrealExtensions.FormatUnrealExtensionsAsFilter().Replace(
                "*.u;",
                "*.u;*.uc;*.uci;"
            );

            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                return;
            }

            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string filePath in files)
            {
                if (allowedExtensions.Contains(Path.GetExtension(filePath)))
                {
                    BeginInvoke((MethodInvoker)(() => LoadFromFile(filePath)));
                }
            }
        }

        private void SaveFileToolMenu(object sender, EventArgs e) => Tabs.SelectedComponent?.TabSave();

        private void FindMenuItem_Click(object sender, EventArgs e) => Tabs.SelectedComponent?.TabFind();

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
                if (filePaths[i] == string.Empty)
                {
                    filePaths.Remove(filePaths[i]);
                    --i;
                    continue;
                }

                var itemText =
                    $"{filePaths.Count - i} {Path.GetFileName(filePaths[i])} ({Path.GetDirectoryName(filePaths[i])})";
                var item = mostRecentMenuItem.DropDownItems.Add(itemText);
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

            // Unshift to the top
            BeginInvoke((MethodInvoker)(() => LoadFromFile(filePath)));
        }

        // Temporary solution for 1.4.2
        private void TabComponentsStrip_TabStripItemMouseEnter(TabStripItemMouseEventArgs e)
        {
            if (tabsToolTip.Active)
            {
                return;
            }

            tabsToolTip.Active = true;
            var r = e.Item.RectangleToClient(e.Item.ClientRectangle);
            tabsToolTip.Show(e.Item.Name, e.Item, r.X, r.Y);
        }

        // Temporary solution for 1.4.2
        private void TabComponentsStrip_TabStripItemMouseLeave(TabStripItemMouseEventArgs e)
        {
            tabsToolTip.Active = false;
            tabsToolTip.Hide(e.Item);
        }

        // Temporary solution for 1.4.2
        private void openInFileExplorerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var tabItem = TabComponentsStrip.SelectedItem;
            var displayControl = tabItem?.Controls[0];
            if (displayControl is UC_PackageExplorer packageExplorer)
            {
                string filePath = packageExplorer.FileName;

                BeginInvoke((MethodInvoker)(() =>
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = "explorer", Arguments = $"/select, \"{filePath}\""
                    });
                }));
            }
        }

        // Temporary solution for 1.4.2
        private void tabsContextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            openInFileExplorerToolStripMenuItem.Enabled = false;

            var tabItem = TabComponentsStrip.SelectedItem;
            var displayControl = tabItem?.Controls[0];
            if (displayControl is UC_PackageExplorer)
            {
                openInFileExplorerToolStripMenuItem.Enabled = true;
            }
        }
    }

    public static class ProgressStatus
    {
        public static ToolStripProgressBar Loading;
        public static ToolStripStatusLabel Status;

        private static string _SavedStatus;

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
            // Disposed?
            if (Status == null)
            {
                return;
            }

            Status.Text = status;
            Status.Owner.Refresh();
        }

        public static void Reset()
        {
            ResetStatus();
            ResetValue();
        }

        public static void ResetStatus() => SetStatus(_SavedStatus);

        public static void SaveStatus() => _SavedStatus = Status.Text;

        public static void IncrementValue() => ++Loading.Value;

        public static void ResetValue()
        {
            Loading.Visible = false;
            Loading.Value = 0;
        }

        public static int GetProgress() => Loading.Value;

        public static void SetMaxProgress(int max) => Loading.Maximum = max;
    }
}
