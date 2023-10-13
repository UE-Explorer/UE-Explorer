using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Krypton.Toolkit;
using Microsoft.Extensions.DependencyInjection;
using UEExplorer.Framework;
using UEExplorer.Framework.Commands;
using UEExplorer.Framework.Plugin;
using UEExplorer.Framework.Services;
using UEExplorer.Framework.Tasks;
using UEExplorer.Framework.UI.Commands;
using UEExplorer.Framework.UI.Services;
using UEExplorer.Properties;
using UEExplorer.Tools.Commands;
using UEExplorer.UI.Dialogs;
using UEExplorer.UI.Forms;
using UEExplorer.UI.Panels;
using UELib;
using UELib.Annotations;

namespace UEExplorer.UI.Main
{
    public partial class ProgramForm : KryptonForm
    {
        private readonly IServiceProvider _ServiceProvider;
        private readonly TasksManager _TasksManager;

        public ProgramForm()
        {
            InitializeComponent();
        }
        
        [UsedImplicitly]
        public ProgramForm(IServiceProvider serviceProvider, TasksManager tasksManager) : this()
        {
            _ServiceProvider = serviceProvider;
            _TasksManager = tasksManager;

            WindowState = Settings.Default.WindowState;
            Size = Settings.Default.WindowSize;
            Location = Settings.Default.WindowLocation;

            tasksManager.TaskStart += ActionTasksManagerOnTaskStart;
            tasksManager.TaskStop += ActionTasksManagerOnTaskStop;
            tasksManager.ProgressChanged += ActionTasksManagerOnProgressChanged;
        }

        private void ProgramForm_Load(object sender, EventArgs e)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                return;
            }

            foreach (var pluginModule in _ServiceProvider.GetRequiredService<PluginService>().GetLoadedModules())
            {
                pluginModule.Activate();
            }

            var commandService = _ServiceProvider.GetRequiredService<CommandService>();

            var viewCommands = _ServiceProvider.GetServices<IMenuCommand>();
            var viewToolMenuItems = viewCommands
                .Where(cmd =>
                    cmd.GetType().GetCustomAttribute<CommandCategoryAttribute>()?.Category ==
                    CommandCategories.View)
                .Select(cmd =>
                {
                    string name = cmd.GetType().Name;
                    string text = cmd.Text;

                    var item = new ToolStripMenuItem
                    {
                        Name = name, Image = cmd.Icon, Text = text, ShortcutKeys = cmd.ShortcutKeys
                    };
                    item.Click += (clickSender, args) => commandService.Execute(cmd as ICommand<object>, sender);
                    return item;
                })
                .ToArray<ToolStripItem>();

            SuspendLayout();
            if (viewToolMenuItems.Any())
            {
                viewToolStripMenuItem.DropDownItems.Add(new ToolStripSeparator());
            }

            viewToolStripMenuItem.DropDownItems.AddRange(viewToolMenuItems);
            ResumeLayout();

            InitializeConfig();
            BeginInvoke((MethodInvoker)InitializeState);
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
        }

        private void ActionTasksManagerOnTaskStart(object sender, ActionTask e)
        {
            loadingProgressBar.Visible = true;
            e.ProgressChanged += OnTaskProgressChanged;
        }

        private void OnTaskProgressChanged(object sender, TaskProgressEventArgs e)
        {
            loadingProgressBar.Maximum = e.Max;
            loadingProgressBar.Value = e.Count;
        }

        private void ActionTasksManagerOnTaskStop(object sender, ActionTask e)
        {
            loadingProgressBar.Visible = false;
            e.ProgressChanged -= OnTaskProgressChanged;
        }

        private void ActionTasksManagerOnProgressChanged(object sender, TaskProgressEventArgs e)
        {
            var task = _TasksManager.CurrentTask;
            if (task == null)
            {
                progressStatusLabel.Text = Resources.StatusReady;
                return;
            }

            progressStatusLabel.Text = string.Format(Resources.StatusTask, e.Count, e.Max, task.Status());
        }

        private void InitializeConfig()
        {
            Program.LoadConfig();

            nativeTableDropDownButton.Text = Program.Options.NTLPath;
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
            if (toolsMenuItem.DropDownItems["tools"] != null)
            {
                return;
            }

            toolsMenuItem.DropDownItems.Add(new ToolStripSeparator { Name = "tools" });

            var items = _ServiceProvider
                .GetRequiredService<PluginService>()
                .GetLoadedModules()
                .Select(module =>
                {
                    string text = module.ToString();
                    var item = new ToolStripMenuItem(text);
                    return item;
                });

            toolsMenuItem.DropDownItems.AddRange(items.ToArray<ToolStripItem>());
        }

        public void LoadFromFile(string filePath)
        {
            Program.PushRecentOpenedFile(filePath);
            switch (Path.GetExtension(filePath))
            {
                default:
                    ServiceHost.GetRequired<PackageManager>().RegisterPackage(filePath);
                    break;
            }
        }

        private void CheckForUpdatesMenuItem_Click(object sender, EventArgs e) => Program.CheckForUpdates();

        private void SocialMenuItem_Click(object sender, EventArgs e) =>
            Process.Start(Resources.UEExplorerFacebookUrl);

        private void OnClosing(object sender, FormClosingEventArgs e)
        {
            UserHistory.Default.Save();

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

        private void OpenFileToolStripMenuItem_Click(object sender, EventArgs e) =>
            ServiceHost
                .GetRequired<CommandService>()
                .Execute<OpenPackageFilePickerCommand>(sender);

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
                if (filePaths[i] == string.Empty)
                {
                    continue;
                }

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
            string filePath = (string)e.ClickedItem.Tag;
            if (!File.Exists(filePath))
            {
                var filePaths = UserHistory.Default.RecentFiles;
                filePaths.Remove(filePath);
                return;
            }

            BeginInvoke((MethodInvoker)(() => LoadFromFile(filePath)));
        }

        private void packageExplorerToolStripMenuItem_Click(object sender, EventArgs e) =>
            dockSpace.AddPackageExplorer();

        // TODO: Propagate commands a different way.
        private void navigateBackwardToolStripMenuItem_Click(object sender, EventArgs e) =>
            dockSpace.ToolStripButton_Backward_Click(sender, e);

        // TODO: Propagate commands a different way.
        private void navigateForwardToolStripMenuItem_Click(object sender, EventArgs e) =>
            dockSpace.ToolStripButton_Forward_Click(sender, e);

        private void forumToolStripMenuItem_Click(object sender, EventArgs e) => Process.Start(Program.ForumUrl);

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dockingService = _ServiceProvider.GetRequiredService<IDockingService>();
            BeginInvoke((MethodInvoker)(() =>
                dockingService.AddDocumentUnique<SettingsPanel>("Settings", Resources.SettingsTitle)));
        }

        private void taskProcessor_Tick(object sender, EventArgs e) => _TasksManager.Process();
    }
}
