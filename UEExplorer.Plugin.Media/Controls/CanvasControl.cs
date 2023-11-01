using System.ComponentModel;
using System.Windows.Forms;

namespace UEExplorer.Plugin.Media.Controls
{
    [DesignerCategory("code")]
    public class CanvasControl : Control
    {
        public CanvasControl()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.DoubleBuffer |
                     ControlStyles.ResizeRedraw, true);
        }
    }
}
