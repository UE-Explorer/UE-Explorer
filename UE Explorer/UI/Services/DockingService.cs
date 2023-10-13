using System.Drawing;
using System.Windows.Forms;
using Krypton.Docking;
using Krypton.Navigator;
using UEExplorer.Framework.UI.Services;

namespace UEExplorer.UI.Services
{
    public class DockingService : IDockingService
    {
        private const string WindowsPath = "Floating";
        private const string DocumentsPath = "Documents";

        public KryptonDockingManager DockingManager { get; set; }

        public bool HasPage(string uniqueName) => DockingManager.ContainsPage(uniqueName);

        public bool HasPage(string uniqueName, out KryptonPage page)
        {
            page = DockingManager.PageForUniqueName(uniqueName);
            return page != null;
        }

        public void RemovePage(string uniqueName)
        {
            DockingManager.RemovePage(uniqueName, true);
        }
        
        public void AddWindow(string uniqueName, KryptonPage page)
        {
            // FIXME center window
            //new Point(
            //    Bounds.Location.X + Bounds.Right / 2 - page.ClientSize.Width / 2,
            //    Bounds.Location.Y + Bounds.Bottom / 2 - page.ClientSize.Height / 2),
                
            DockingManager.AddFloatingWindow(WindowsPath, new[] { page },
                Point.Empty,
                page.Size);
        }

        public bool HasDocument(string uniqueName) =>
            DockingManager.ResolvePath(DocumentsPath)?.FindPageElement(uniqueName) != null;

        public bool HasDocument<T>(string uniqueName, out T page) where T : KryptonPage
        {
            page = (T)DockingManager.ResolvePath(DocumentsPath)
                ?.PropogatePageState(DockingPropogatePageState.PageForUniqueName, uniqueName);
            return page != null;
        }

        public void AddDocument(KryptonPage page)
        {
            var documentsNav = DockingManager.AddToNavigator(DocumentsPath, new[] { page });
            documentsNav.SelectPage(page.UniqueName);
        }

        public void AddDocumentUnique<T>(string uniqueName, string title)
            where T : Control, new()
        {
            if (HasDocument(uniqueName))
            {
                return;
            }

            AddDocument<T>(uniqueName, title);
        }

        public void AddDocument<T>(string uniqueName, string title)
            where T : Control, new()
        {
            var page = new KryptonPage(title, uniqueName);

            var control = new T { Dock = DockStyle.Fill };
            page.Controls.Add(control);

            var documentsNav = DockingManager.AddToNavigator(DocumentsPath, new[] { page });
            documentsNav.SelectPage(page.UniqueName);
        }
    }
}
