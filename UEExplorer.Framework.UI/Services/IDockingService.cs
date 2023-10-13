using System.Windows.Forms;
using Krypton.Navigator;

namespace UEExplorer.Framework.UI.Services
{
    public interface IDockingService
    {
        bool HasPage(string uniqueName);
        bool HasPage(string uniqueName, out KryptonPage page);
        
        void RemovePage(string uniqueName);

        void AddWindow(string uniqueName, KryptonPage page);

        bool HasDocument(string uniqueName);

        bool HasDocument<T>(string uniqueName, out T page)
            where T : KryptonPage;

        void AddDocument(KryptonPage page);

        void AddDocument<T>(string uniqueName, string title)
            where T : Control, new();

        void AddDocumentUnique<T>(string uniqueName, string title)
            where T : Control, new();
    }
}
