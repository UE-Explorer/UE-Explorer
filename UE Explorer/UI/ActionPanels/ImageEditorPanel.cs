using System;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using UEExplorer.Render.Extensions.Drawing;
using UEExplorer.UI.Tabs;
using UELib.Core;
using UELib.Engine;

namespace UEExplorer.UI.ActionPanels
{
    public partial class ImageEditorPanel : ActionPanel, IActionPanel<object>
    {
        private Bitmap _Image;

        public ImageEditorPanel()
        {
            InitializeComponent();
        }

        public void RestoreState(ref ActionState state)
        {
        }

        public void StoreState(ref ActionState state)
        {
        }

        ~ImageEditorPanel()
        {
            _Image?.Dispose();
        }

        protected override void UpdateOutput(object target)
        {
            _Image?.Dispose();
            _Image = null;

            // Bad and slow bitmap, but it servers our purpose for now!
            switch (target)
            {
                case null:
                    break;

                case UPalette uPalette:
                {
                    _Image = new Bitmap(128, 128, PixelFormat.Format32bppArgb);
                    Rectangle rect = new Rectangle(0, 0, _Image.Width, _Image.Height);
                    BitmapData bmpData = _Image.LockBits(rect, ImageLockMode.ReadWrite, _Image.PixelFormat);

                    const int size = 256 * 4;
                    byte[] data = new byte[size];
                    IntPtr ptr = bmpData.Scan0;
                    for (int i = 0, offset = 0; offset < size; ++offset, ++i)
                    {
                        data[offset] = uPalette.Colors[i].A;
                        data[++offset] = uPalette.Colors[i].R;
                        data[++offset] = uPalette.Colors[i].G;
                        data[++offset] = uPalette.Colors[i].B;
                    }

                    Marshal.Copy(data, 0, ptr, size);
                    _Image.UnlockBits(bmpData);
                    break;
                }

                case UTexture uTexture:
                {
                    if (uTexture.Mips == null || !uTexture.Mips.Any())
                    {
                        if (uTexture.Palette != null)
                        {
                            UpdateOutput(uTexture.Palette);
                        }

                        // Nothing to render
                        break;
                    }

                    UTexture.MipMap mip = uTexture.Mips[0];
                    PixelFormat pixelFormat = uTexture.Format.ToPixelFormat();
                    if (uTexture.Format == UBitmapMaterial.TextureFormat.P8)
                    {
                    }

                    GCHandle pinnedArray = GCHandle.Alloc(mip.Data, GCHandleType.Pinned);
                    try
                    {
                        IntPtr pointer = pinnedArray.AddrOfPinnedObject();
                        _Image = new Bitmap(mip.USize, mip.VSize, 4, pixelFormat, pointer);
                    }
                    finally
                    {
                        pinnedArray.Free();
                    }

                    break;
                }

                case UPolys uPolys:
                    viewportPanel.SetRenderTarget((dynamic)Object);
                    break;
            }
        }

        private void ImageEditorPanel_Load(object sender, EventArgs e)
        {
        }
    }
}