using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using AutoUpdaterDotNET;
using Eliot.Utilities;
using Microsoft.Win32;
using Storm.TabControl;
using UEExplorer.Development;
using UEExplorer.Properties;
using UEExplorer.UI.Dialogs;
using UEExplorer.UI.Forms;
using UEExplorer.UI.Tabs;
using UELib;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;

namespace UEExplorer.UI.Main
{
    public partial class ProgramForm : Form
    {
        private const string APP_KEY = "EliotVU";
        private MRUManager _MRUManager;
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
            Program.LogManager.StartLogStream();

            Text = $"{Application.ProductName} {Version}";

            InitializeConfig();
            InitializeUI();

            Tabs = new TabsCollection(TabComponentsStrip);
            Tabs.InsertTab(typeof(UC_Default), Resources.Homepage);
            string[] args = Environment.GetCommandLineArgs();
            for (var i = 1; i < args.Length; ++i)
            {
                if (File.Exists(args[i]))
                {
                    LoadFile(args[i]);
                }
            }
        }

        private void InitializeUI()
        {
            ProgressStatus.Status = ProgressLabel;
            ProgressStatus.Loading = LoadingProgress;

            SelectedNativeTable.Text = Program.Options.NTLPath;
            Platform.Text = Program.Options.Platform;
#if DEBUG
            _CacheExtractorItem.Enabled = true;
#endif
            MRUManager.SetStoragePath(Application.StartupPath);
            _MRUManager = MRUManager.Load();
            _MRUManager.RefreshEvent += RefreshMRUEvent;
            RefreshMRUEvent();
        }

        private void RefreshMRUEvent()
        {
            _ROF.MenuItems.Clear();
            for (int i = _MRUManager.Files.Count - 1; i >= 0; --i)
            {
                if (!File.Exists(_MRUManager.Files[i]))
                {
                    _MRUManager.Files.RemoveAt(i);
                    --i;
                    continue;
                }

                var item = _ROF.MenuItems.Add
                (
                    $"{_MRUManager.Files.Count - i} {Path.GetFileName(_MRUManager.Files[i])} -> {Path.GetDirectoryName(_MRUManager.Files[i])}"
                );
                item.Tag = _MRUManager.Files[i];
                item.Click += (sender, e) =>
                {
                    var item1 = (MenuItem)sender;
                    LoadFile(item1.Tag as string);
                    RefreshMRUEvent();
                };
            }

            _ROF.Enabled = _ROF.MenuItems.Count > 0;
            _MRUManager.Save();
        }

        private void InitializeConfig()
        {
            Program.LoadConfig();
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
                    var item = menuItem13.MenuItems.Add(extensionName);
                    item.Click += ext.OnActivate;
                    menuItem13.Enabled = true;
                }
            }
        }

        private void SelectedNativeTable_DropDownOpening(object sender, EventArgs e)
        {
            // Rebuild it, to reflect the changes made to the current directory of NTL files.
            SelectedNativeTable.DropDown.Items.Clear();
            foreach (string filePath in UC_Options.GetNativeTables())
            {
                SelectedNativeTable.DropDown.Items.Add(Path.GetFileNameWithoutExtension(filePath));
            }

            // In case it got changed!
            SelectedNativeTable.Text = Program.Options.NTLPath;
        }

        private void SelectedNativeTable_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            SelectedNativeTable.Text = e.ClickedItem.Text;

            Program.Options.NTLPath = SelectedNativeTable.Text;
            Program.SaveConfig();
        }

        private void ToolsToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            menuItem20.Checked = Program.AreFileTypesRegistered();

            if (menuItem13.Enabled)
            {
                return;
            }

            InitializeExtensions();
        }

        public void LoadFile(string filePath)
        {
            ProgressStatus.SaveStatus();
            ProgressStatus.SetStatus(string.Format(
                    Resources.ProgramForm_LoadFile_Loading_file,
                    Path.GetFileName(filePath)
                )
            );

            try
            {
                _MRUManager.AddFile(filePath);
                switch (Path.GetExtension(filePath))
                {
                    default:
                        var packageExplorer = new UC_PackageExplorer(filePath);
                        Tabs.AddTab(packageExplorer, Path.GetFileName(filePath));
                        break;
                }
            }
            catch (Exception exception)
            {
                ExceptionDialog.Show(string.Format(Resources.ProgramForm_LoadFile_Failed_loading_package,
                        filePath), exception
                );
            }
            finally
            {
                ProgressStatus.Reset();
            }
        }

        private void DonateToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Process.Start(Program.Donate_URL);
        }

        private void CheckForUpdates()
        {
            Console.WriteLine(Resources.CHECKING_FOR_UPDATES_LOG, Version);
            AutoUpdater.Start(string.Format(Program.UPDATE_URL, Version));
        }

        private void OnCheckForUpdates(object sender, EventArgs e)
        {
            var softKey = Registry.CurrentUser.OpenSubKey("Software", true);
            var appKey = softKey?.OpenSubKey(APP_KEY);
            if (appKey != null)
            {
                softKey.DeleteSubKeyTree(APP_KEY);
            }

            CheckForUpdates();
        }

        private void MenuItem7_Click(object sender, EventArgs e)
        {
            Tabs.InsertTab(typeof(UC_Options), Resources.Options);
        }

        private void MenuItem24_Click(object sender, EventArgs e)
        {
            Process.Start(Program.Forum_URL);
        }

        private void MenuItem26_Click(object sender, EventArgs e)
        {
            Process.Start(Program.WEBSITE_URL);
        }

        private void Platform_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            Platform.Text = e.ClickedItem.Text;
            Program.Options.Platform = Platform.Text;
            Program.SaveConfig();
        }

        private void MenuItem4_Click(object sender, EventArgs e)
        {
            Process.Start(Program.Contact_URL);
        }

        private void OpenHome_Click(object sender, EventArgs e)
        {
            Tabs.InsertTab(typeof(UC_Default), Resources.Homepage);
        }

        private void SocialMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.facebook.com/UE.Explorer");
        }

        private void OnClosed(object sender, FormClosedEventArgs e)
        {
            Program.LogManager.EndLogStream();
        }

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
                if (_MRUManager != null)
                {
                    _MRUManager.RefreshEvent -= RefreshMRUEvent;
                    _MRUManager = null;
                }

                components?.Dispose();

                if (Tabs != null)
                {
                    Tabs.Dispose();
                    Tabs = null;
                }
            }

            base.Dispose(disposing);
        }

        private void ReportAnIssue(object sender, EventArgs e)
        {
            Process.Start("https://github.com/UE-Explorer/UE-Explorer/issues");
        }

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
                       DefaultExt = "u",
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

                foreach (string fileName in ofd.FileNames)
                {
                    LoadFile(fileName);
                }
            }
        }

        private void UnrealColorGeneratorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // open a tool dialog!
            var cgf = new ColorGeneratorForm();
            cgf.Show();
        }

        private void UnrealCacheExtractorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Tabs.InsertTab(typeof(UC_CacheExtractor), Resources.ProgramForm_Cache_Extractor);
        }

        private void TabComponentsStrip_TabStripItemClosing(TabStripItemClosingEventArgs e)
        {
            e.Item.Dispose();
        }

        private void TabComponentsStrip_TabStripItemClosed(object sender, EventArgs e)
        {
            TabComponentsStrip.Visible = TabComponentsStrip.Items.Count > 0;
            HomepageButton.Visible = !TabComponentsStrip.Visible;
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void ProgramForm_Shown(object sender, EventArgs e)
        {
            CheckForUpdates();
        }

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

        private void ProgramForm_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop)
                ? DragDropEffects.Link
                : DragDropEffects.None;
        }

        private void ProgramForm_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop)
                ? DragDropEffects.Link
                : DragDropEffects.None;
        }

        private void ProgramForm_DragDrop(object sender, DragEventArgs e)
        {
            string allowedExtensions = UnrealExtensions.FormatUnrealExtensionsAsFilter();

            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                return;
            }

            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string filePath in files)
            {
                if (allowedExtensions.Contains(Path.GetExtension(filePath)))
                {
                    LoadFile(filePath);
                }
            }
        }

        private void SaveFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Tabs.SelectedComponent?.TabSave();
        }

        private void FindToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Tabs.SelectedComponent?.TabFind();
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
            Status.Text = status;
            Status.Owner.Refresh();
        }

        public static void Reset()
        {
            ResetStatus();
            ResetValue();
        }

        public static void ResetStatus()
        {
            SetStatus(_SavedStatus);
        }

        public static void SaveStatus()
        {
            _SavedStatus = Status.Text;
        }

        public static void IncrementValue()
        {
            ++Loading.Value;
        }

        public static void ResetValue()
        {
            Loading.Visible = false;
            Loading.Value = 0;
        }

        public static int GetProgress()
        {
            return Loading.Value;
        }

        public static void SetMaxProgress(int max)
        {
            Loading.Maximum = max;
        }
    }
}