using System.Windows.Forms;
using Krypton.Navigator;

namespace UEExplorer.Framework.UI.Services
{
    public interface IDockingService
    {
        bool HasPage(string uniqueName);

        bool HasPage<TPage>(string uniqueName, out TPage page)
            where TPage : KryptonPage;

        void RemovePage(string uniqueName);

        void AddWindow(string uniqueName, KryptonPage page);

        bool HasDocument(string uniqueName);

        bool HasDocument<TPage>(string uniqueName, out TPage page)
            where TPage : KryptonPage;

        void AddDocument(KryptonPage page);

        TControl AddDocument<TControl>(string uniqueName, string title)
            where TControl : Control;

        TControl AddDocumentUnique<TControl>(string uniqueName, string title)
            where TControl : Control;
    }
}
