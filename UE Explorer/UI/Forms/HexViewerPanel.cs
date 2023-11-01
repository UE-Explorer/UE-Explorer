using System.Windows.Forms;

namespace UEExplorer.UI.Forms
{
    public class HexViewerPanel : Panel
    {
        public HexViewerPanel()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
        }
    }
}
