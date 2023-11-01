using System.Drawing;
using System.Windows.Forms;

namespace UEExplorer.Plugin.Media.Image
{
    public partial class ImageViewPanel : UserControl
    {
        private Bitmap _SourceImage;

        public ImageViewPanel()
        {
            InitializeComponent();
        }

        public Bitmap SourceImage
        {
            get => _SourceImage;
            set
            {
                if (_SourceImage != value && _SourceImage != null)
                {
                    _SourceImage.Dispose();
                }
                _SourceImage = value;
                Invalidate();
            }
        }

        ~ImageViewPanel() => SourceImage?.Dispose();

        private void canvasControl_Paint(object sender, PaintEventArgs e)
        {
            if (_SourceImage == null)
            {
                return;
            }

            e.Graphics.DrawImage(_SourceImage, e.ClipRectangle);
        }
    }
}
