using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Krypton.Docking;
using Krypton.Navigator;
using UEExplorer.Framework;
using UEExplorer.Framework.Commands;
using UEExplorer.Framework.Services;
using UEExplorer.Framework.UI;
using UEExplorer.Framework.UI.Pages;
using UEExplorer.Framework.UI.Services;
using UEExplorer.PackageTasks;
using UEExplorer.Properties;
using UEExplorer.Tools.Commands;
using UEExplorer.UI.ActionPanels;
using UEExplorer.UI.Pages;
using UEExplorer.UI.Panels;
using UEExplorer.UI.Services;
using UELib;

namespace UEExplorer.UI.Tabs
{
    // TODO: Rename to PackageWorkspace
    public partial class UC_PackageExplorer : UserControl
    {
        private const string ExplorerPath = "Explorer";
        private const string DocumentsPath = "Documents";
        private const string FooterPath = "Footer";

        private readonly List<ContextChangeInfo> _ContextChangeHistory = new List<ContextChangeInfo>(15);

        private readonly ContextService _ContextService;
        private readonly DockingService _DockingService;
        private readonly PackageManager _PackageManager;
        private readonly TasksManager _TasksManager;

        private int _CurrentHistoryIndex = -1;

        public UC_PackageExplorer()
        {
            InitializeComponent();

            if (LicenseManager.UsageMode == LicenseUsageMode.Runtime)
            {
                _ContextService = ServiceHost.GetRequired<ContextService>();
                _ContextService.ContextChanged += ContextServiceOnContextChanged;

                _PackageManager = ServiceHost.GetRequired<PackageManager>();
                _PackageManager.PackageRegistered += PackageManagerOnPackageRegistered;
                _PackageManager.PackageLoaded += PackageManagerOnPackageLoaded;
                _PackageManager.PackageInitialized += PackageManagerOnPackageInitialized;
                _PackageManager.PackageUnloaded += PackageManagerOnPackageUnloaded;
                _PackageManager.PackageRemoved += PackageManagerOnPackageRemoved;
                _PackageManager.PackageError += PackageManagerOnPackageError;

                _DockingService = (DockingService)ServiceHost.GetRequired<IDockingService>();
                _DockingService.DockingManager = dockingManager;

                _TasksManager = ServiceHost.GetRequired<TasksManager>();
            }
        }

        public string FilePath { get; set; }

        /// <summary>
        ///     Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            Debug.WriteLine("Disposing UC_PackageExplorer " + disposing);

            dockingManager.SaveConfigToFile(Program.DockingConfigPath);
            dockingManager.Pages.ToList().ForEach(p => p.Dispose());

            Program.SaveConfig();

            base.Dispose(disposing);
        }

        private void UC_PackageExplorer_Load(object sender, EventArgs e)
        {
            SuspendLayout();

            dockingManager.ManageFloating("Floating", ParentForm);

            dockingManager.ManageControl(ExplorerPath, dockingPanel);
            dockingManager.ManageNavigator(DocumentsPath, documentsNavigator);
            dockingManager.ManageControl(FooterPath, dockingPanel);

            if (LicenseManager.UsageMode == LicenseUsageMode.Runtime)
            {
                if (File.Exists(Program.DockingConfigPath))
                {
                    dockingManager.LoadConfigFromFile(Program.DockingConfigPath);
                }

                AddPackageExplorer();
                //AddPropertiesManager();

                var webControl = _DockingService.AddDocumentUnique<WebViewPanel>("Start", Resources.Homepage);
                webControl?.NavigateTo($"{Program.StartUrl}?version={Application.ProductVersion}");

                // Restore all open packages
                if (UserHistory.Default.OpenFiles != null && !Environment.GetCommandLineArgs().Contains("-noautoload"))
                {
                    foreach (var defaultOpenFile in UserHistory.Default.OpenFiles)
                    {
                        BeginInvoke((MethodInvoker)(() => _PackageManager.RegisterPackage(defaultOpenFile)));
                    }
                }
            }

            ResumeLayout();
        }

        private void kryptonDockingManagerMain_PageFloatingRequest(object sender, CancelUniqueNameEventArgs e)
        {
            var page = dockingManager.PageForUniqueName(e.UniqueName);
            if (page is ITrackingContext trackingPage && trackingPage.IsTracking)
            {
                trackingPage.IsTracking = false;
            }
        }

        private void kryptonDockingManagerMain_PageCloseRequest(object sender, CloseRequestEventArgs e)
        {
            var page = dockingManager.PageForUniqueName(e.UniqueName);
            Debug.Assert(page != null);
            page.Dispose();
        }

        private void ContextServiceOnContextChanged(object sender, ContextChangedEventArgs e)
        {
            if (sender == this)
            {
                return;
            }

            if (e.Context.ActionKind == ContextActionKind.Location)
            {
                TrackContextChange(e.Context);
                return;
            }

            TrackContextChange(e.Context);
        }

        private void PackageManagerOnPackageRegistered(object sender, PackageEventArgs e)
        {
            var packageReference = e.Package;
            Debug.Assert(packageReference.Linker == null, "packageReference.Linker == null");

            UnrealConfig.SuppressSignature = false;
            bool isUnrealFileExtension = UnrealLoader.IsUnrealFileSignature(packageReference.FilePath);
            if (!isUnrealFileExtension)
            {
                if (MessageBox.Show(Resources.PACKAGE_UNKNOWN_SIGNATURE,
                        Resources.Warning,
                        MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    UnrealConfig.SuppressSignature = true;
                }
                else
                {
                    return;
                }
            }

            if (UserHistory.Default.OpenFiles == null)
            {
                UserHistory.Default.OpenFiles = new PackageCollection();
            }

            UserHistory.Default.OpenFiles.Add(e.Package);
            UserHistory.Default.Save();

            if (Program.Options.bForceLicenseeMode)
            {
                UnrealPackage.OverrideLicenseeVersion = Program.Options.LicenseeMode;
            }

            if (Program.Options.bForceVersion)
            {
                UnrealPackage.OverrideVersion = Program.Options.Version;
            }

            if (packageReference.Flags.HasFlag(PackageReferenceFlags.NoAutoLoad))
            {
                return;
            }

            BeginInvoke((MethodInvoker)(() => _PackageManager.LoadPackage(packageReference)));
        }

        private void PackageManagerOnPackageLoaded(object sender, PackageEventArgs e)
        {
            var packageReference = e.Package;
            Debug.Assert(packageReference.Linker != null, "packageReference.Linker != null");

            if (HasCompressedChunks(packageReference.Linker))
            {
                if (MessageBox.Show(Resources.PACKAGE_IS_COMPRESSED,
                        Resources.NOTICE_TITLE,
                        MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Question) == DialogResult.OK)
                {
                    Process.Start(Resources.GildorDownloadsWebUrl);
                    MessageBox.Show(Resources.COMPRESSED_HOWTO,
                        Resources.NOTICE_TITLE,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }

                // Don't initialize this package.
                return;
            }

            string ntlFilePath = Path.Combine(Application.StartupPath, "Native Tables", Program.Options.NTLPath);
            if (File.Exists(ntlFilePath + NativesTablePackage.Extension))
            {
                packageReference.Linker.NTLPackage = new NativesTablePackage();
                packageReference.Linker.NTLPackage.LoadPackage(ntlFilePath);
            }

            BeginInvoke((MethodInvoker)(() => _PackageManager.InitializePackage(packageReference)));
        }

        private void PackageManagerOnPackageInitialized(object sender, PackageEventArgs e)
        {
            var packageReference = e.Package;
            Debug.Assert(packageReference.Linker != null, "packageReference.Linker != null");

            // Restore auto-load for the next startup.
            packageReference.Flags &= ~PackageReferenceFlags.NoAutoLoad;
            UserHistory.Default.Save();

            var task = new InitializePackageTask(packageReference.Linker);
            task.Error += (o, ex) =>
            {
                packageReference.Flags |= PackageReferenceFlags.NoAutoLoad;
                UserHistory.Default.Save();
                throw new UnrealException($"Couldn't initialize objects of package \"{packageReference.FilePath}\"",
                    ex);
            };
            _TasksManager.Enqueue(task, CancellationToken.None);
        }

        private void PackageManagerOnPackageUnloaded(object sender, PackageEventArgs e)
        {
            var packageReference = e.Package;
            // Load this cold on next startup :)
            packageReference.Flags |= PackageReferenceFlags.NoAutoLoad;
            UserHistory.Default.Save();
        }

        private void PackageManagerOnPackageRemoved(object sender, PackageEventArgs e)
        {
            UserHistory.Default.OpenFiles.Remove(e.Package);
            UserHistory.Default.Save();
        }

        private void PackageManagerOnPackageError(object sender, PackageEventArgs e)
        {
            var packageReference = e.Package;
            packageReference.Flags |= PackageReferenceFlags.NoAutoLoad;
            UserHistory.Default.Save();

            //throw new UnrealException($"Package error \"{e.Package.FilePath}\"", e.Package.Error);
        }

        private static bool HasCompressedChunks(UnrealPackage linker) =>
            linker.Summary.CompressedChunks != null &&
            linker.Summary.CompressedChunks.Any();

        private void TrackContextChange(ContextInfo context)
        {
            var actionKind = context.ActionKind;
            if (actionKind > ContextActionKind.Command)
            {
                return;
            }

            object target = context.Target;
            if (_ContextChangeHistory.Count > 0)
            {
                // No need to buffer the same content
                var last = _ContextChangeHistory.Last();
                if (last.Context.Target == target && last.Context.ActionKind == actionKind &&
                    actionKind != ContextActionKind.Location)
                {
                    return;
                }

                // Clean all above buffers when a new node was user-selected
                if (_ContextChangeHistory.Count - 1 - _CurrentHistoryIndex > 0)
                {
                    _ContextChangeHistory.RemoveRange(_CurrentHistoryIndex,
                        _ContextChangeHistory.Count - _CurrentHistoryIndex);
                    _CurrentHistoryIndex = _ContextChangeHistory.Count - 1;
                    nextButton.Enabled = false;
                }
            }

            if (_ContextChangeHistory.Count + 1 > _ContextChangeHistory.Capacity)
            {
                _ContextChangeHistory.RemoveRange(0, 1);
            }

            var data = new ContextChangeInfo(context);
            _ContextChangeHistory.Add(data);
            _CurrentHistoryIndex = _ContextChangeHistory.Count - 1;

            if (_CurrentHistoryIndex > 0)
            {
                prevButton.Enabled = true;
            }
        }

        private void RestoreContentHistoryData(int historyIndex)
        {
            var data = _ContextChangeHistory[historyIndex];
            _ContextService.OnContextChanged(this, new ContextChangedEventArgs(data.Context));
        }

        internal void ToolStripButton_Backward_Click(object sender, EventArgs e)
        {
            Debug.Assert(_CurrentHistoryIndex - 1 >= 0);

            RestoreContentHistoryData(--_CurrentHistoryIndex);

            if (_CurrentHistoryIndex == 0)
            {
                prevButton.Enabled = false;
            }

            nextButton.Enabled = true;
        }

        internal void ToolStripButton_Forward_Click(object sender, EventArgs e)
        {
            Debug.Assert(_CurrentHistoryIndex + 1 < _ContextChangeHistory.Count);

            RestoreContentHistoryData(++_CurrentHistoryIndex);

            if (_CurrentHistoryIndex == _ContextChangeHistory.Count - 1)
            {
                nextButton.Enabled = false;
            }

            prevButton.Enabled = true;
        }

        private void findInStripMenuItem_Click(object sender, EventArgs e) =>
            ServiceHost
                .GetRequired<CommandService>()
                .Execute<FindInPackagesMenuCommand>(sender);

        private void openFileToolStripMenuItem_Click(object sender, EventArgs e) =>
            ServiceHost
                .GetRequired<CommandService>()
                .Execute<OpenPackageFilePickerCommand>(sender);

        private void AddPageToSide(KryptonPage page) =>
            dockingManager.AddDockspace(ExplorerPath, DockingEdge.Left, new[] { page });

        public void AddPackageExplorer()
        {
            var docked = dockingManager.FindPageElement("PackageExplorer");
            if (docked != null)
            {
                return;
            }

            var content = new PackageExplorerPanel();
            var page = PageFactory.CreatePage(
                Resources.PackageExplorerPage_PackageExplorerPage_Package_Explorer_Title,
                "PackageExplorer",
                content);
            page.TextTitle = page.Text;
            page.ClearFlags(KryptonPageFlags.DockingAllowFloating |
                            KryptonPageFlags.DockingAllowWorkspace);
            AddPageToSide(page);
        }

        public void AddPropertiesManager()
        {
            var docked = dockingManager.FindPageElement("PropertiesManager");
            if (docked != null)
            {
                return;
            }

            var content = new PropertiesManagerPanel();
            var page = PageFactory.CreatePage(
                "Properties Manager",
                "PropertiesManager",
                content);
            page.TextTitle = page.Text;
            page.ClearFlags(KryptonPageFlags.DockingAllowFloating |
                            KryptonPageFlags.DockingAllowWorkspace);
            AddPageToSide(page);
        }
    }
}
