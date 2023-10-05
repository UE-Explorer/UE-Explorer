using System.Windows.Forms;

namespace UEExplorer.UI.Forms
{
    public partial class HexViewerPanel : UserControl
    {
        public HexViewerPanel()
        {
            InitializeComponent();
            
            SetStyle( ControlStyles.UserPaint, true );
            SetStyle( ControlStyles.AllPaintingInWmPaint, true );
            SetStyle( ControlStyles.OptimizedDoubleBuffer, true );
            SetStyle( ControlStyles.ResizeRedraw, true );
        }
    }
}