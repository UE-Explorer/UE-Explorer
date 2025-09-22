using System.Windows.Forms;

namespace UEExplorer.UI.Forms
{
    public class HexViewerPanel : Panel
    {
        public HexViewerPanel()
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw | 
                ControlStyles.Selectable,
                true);
        }
    }
}
