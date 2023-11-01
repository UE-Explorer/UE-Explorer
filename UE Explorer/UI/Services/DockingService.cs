using System;
using System.Drawing;
using System.Windows.Forms;
using Krypton.Docking;
using Krypton.Navigator;
using Microsoft.Extensions.DependencyInjection;
using UEExplorer.Framework.UI.Services;
using UELib.Annotations;

namespace UEExplorer.UI.Services
{
    public class DockingService : IDockingService
    {
        private const string WindowsPath = "Floating";
        private const string DocumentsPath = "Documents";
        private const string ControlName = "Control";
        private readonly IServiceProvider _ServiceProvider;

        public DockingService(IServiceProvider serviceProvider) => _ServiceProvider = serviceProvider;

        public KryptonDockingManager DockingManager { get; set; }

        public bool HasPage(string uniqueName) => DockingManager.ContainsPage(uniqueName);

        public bool HasPage<TPage>(string uniqueName, out TPage page)
            where TPage : KryptonPage
        {
            page = (TPage)DockingManager.PageForUniqueName(uniqueName);
            return page != null;
        }

        public void RemovePage(string uniqueName) => DockingManager.RemovePage(uniqueName, true);

        public void AddWindow(string uniqueName, KryptonPage page) =>
            // FIXME center window
            //new Point(
            //    Bounds.Location.X + Bounds.Right / 2 - page.ClientSize.Width / 2,
            //    Bounds.Location.Y + Bounds.Bottom / 2 - page.ClientSize.Height / 2),
            DockingManager.AddFloatingWindow(WindowsPath, new[] { page },
                Point.Empty,
                page.Size);

        public bool HasDocument(string uniqueName) =>
            DockingManager.ResolvePath(DocumentsPath)?.FindPageElement(uniqueName) != null;

        public bool HasDocument<TPage>(string uniqueName, out TPage page)
            where TPage : KryptonPage
        {
            page = (TPage)DockingManager.ResolvePath(DocumentsPath)
                ?.PropogatePageState(DockingPropogatePageState.PageForUniqueName, uniqueName);
            return page != null;
        }

        public void AddDocument(KryptonPage page)
        {
            var documentsNav = DockingManager.AddToNavigator(DocumentsPath, new[] { page });
            documentsNav.SelectPage(page.UniqueName);
        }

        [CanBeNull]
        public TControl AddDocumentUnique<TControl>(string uniqueName, string title)
            where TControl : Control
        {
            return HasDocument(uniqueName, out KryptonPage page)
                ? (TControl)page.Controls[ControlName] 
                : AddDocument<TControl>(uniqueName, title);
        }

        public TControl AddDocument<TControl>(string uniqueName, string title)
            where TControl : Control
        {
            var page = new KryptonPage(title, uniqueName);

            var control = ActivatorUtilities.CreateInstance<TControl>(_ServiceProvider);
            control.Name = ControlName;
            control.Dock = DockStyle.Fill;
            page.Controls.Add(control);

            var documentsNav = DockingManager.AddToNavigator(DocumentsPath, new[] { page });
            documentsNav.SelectPage(page.UniqueName);

            return control;
        }
    }
}
