using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace UEExplorer.UI.Controls
{
    internal class TreeViewExt : TreeView
    {
        public TreeViewExt() => SetWindowTheme(Handle, "Explorer", null);

        [DllImport("UxTheme.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
        private static extern int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string pszSubIdList);
    }
}
