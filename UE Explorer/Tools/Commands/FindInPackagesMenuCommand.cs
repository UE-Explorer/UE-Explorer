using System;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UEExplorer.Framework;
using UEExplorer.Framework.Commands;
using UEExplorer.Framework.Services;
using UEExplorer.Framework.Tasks;
using UEExplorer.Framework.UI.Commands;
using UEExplorer.Framework.UI.Pages;
using UEExplorer.Framework.UI.Services;
using UEExplorer.PackageTasks;
using UEExplorer.Properties;
using UEExplorer.UI.Panels;
using UELib.Core;

namespace UEExplorer.Tools.Commands
{
    // ReSharper disable once ClassNeverInstantiated.Global
    [CommandCategory(CommandCategories.Edit)]
    internal sealed class FindInPackagesMenuCommand : MenuCommand, ICommand<object>
    {
        private const string FindInPackagesUniqueName = "FindInPackages";
        private const string FindResultsUniqueName = "FindResults";
        public override Image Icon => Resources.FindInFile;
        public override Keys ShortcutKeys => Keys.Control | Keys.Shift | Keys.F;
        public override string Text => Resources.FindInObjects;

        public bool CanExecute(object subject)
        {
            var dockingService = ServiceHost.GetRequired<IDockingService>();
            if (dockingService.HasPage(FindInPackagesUniqueName))
            {
                return false;
            }

            var packageManager = ServiceHost.GetRequired<PackageManager>();
            return packageManager.Packages.Any();
        }

        public Task Execute(object subject)
        {
            var dockingService = ServiceHost.GetRequired<IDockingService>();

            var content = new FindInPanel();
            var page = PageFactory.CreatePage(Resources.FindInObjects, FindInPackagesUniqueName, content);
            content.Find += (findSender, findInEvent) =>
            {
                dockingService.RemovePage(FindInPackagesUniqueName);
                OpenFindResultsPanel<UClass>(findInEvent.SearchText, findInEvent.PackageReference);
            };

            dockingService.AddWindow(FindInPackagesUniqueName, page);

            return Task.CompletedTask;
        }

        private static void OpenFindResultsPanel<T>(string searchText, PackageReference inPackageReference = null)
            where T : UObject
        {
            var content = new FindResultsPanel();
            var findPage = PageFactory.CreatePage(
                string.Format(Resources.FIND_RESULTS_TITLE, searchText),
                FindResultsUniqueName + DateTime.Now,
                content);

            findPage.TextTitle = string.Format(Resources.FindResultsPage_TextTitle___0__, searchText);
            ServiceHost
                .GetRequired<IDockingService>()
                .AddWindow(FindResultsUniqueName, findPage);

            var cs = new CancellationTokenSource();
            findPage.HandleDestroyed += (sender, args) =>
            {
                cs.Cancel();
            };

            var task = new SearchInPackagesTask<T>(inPackageReference, searchText);
            task.Completed += (sender, args) => content.UpdateResults(task.Results);

            var tasksManager = ServiceHost.GetRequired<TasksManager>();
            tasksManager.Enqueue(task, cs.Token);
        }
    }
}
