using System.Drawing;
using Krypton.Navigator;

namespace UEExplorer.Framework.UI.Pages
{
    public class Page : KryptonPage
    {
        public Page()
        {
        }

        public Page(string text, string uniqueName, Size size) : base(text, null, uniqueName, size)
        {
        }
    }
}
