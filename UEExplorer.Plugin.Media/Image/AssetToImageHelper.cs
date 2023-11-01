using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using UEExplorer.Plugin.Media.Extensions.Drawing;
using UELib.Engine;

namespace UEExplorer.Plugin.Media.Image
{
    public static class AssetToImageHelper
    {
        public static Bitmap From(UPalette palette)
        {
            var image = new Bitmap(128, 128, PixelFormat.Format32bppArgb);
            var rect = new Rectangle(0, 0, image.Width, image.Height);
            var bmpData = image.LockBits(rect, ImageLockMode.ReadWrite, image.PixelFormat);

            const int size = 256 * 4;
            byte[] data = new byte[size];
            var ptr = bmpData.Scan0;
            for (int i = 0, offset = 0; offset < size; ++offset, ++i)
            {
                data[offset] = palette.Colors[i].A;
                data[++offset] = palette.Colors[i].R;
                data[++offset] = palette.Colors[i].G;
                data[++offset] = palette.Colors[i].B;
            }

            Marshal.Copy(data, 0, ptr, size);
            image.UnlockBits(bmpData);

            return image;
        }

        public static Bitmap From(UTexture texture)
        {
            // FIXME: Remove
            texture.BeginDeserializing();

            if (texture.Mips == null || !texture.Mips.Any())
            {
                // Nothing to render
                return null;
            }

            Bitmap image;
            var mip = texture.Mips[0];
            var pixelFormat = texture.Format.ToPixelFormat();
            if (texture.Format == UBitmapMaterial.TextureFormat.P8)
            {
            }

            mip.Data.LoadData(texture.GetBuffer());
            var pinnedArray = GCHandle.Alloc(mip.Data.ElementData, GCHandleType.Pinned);
            try
            {
                var pointer = pinnedArray.AddrOfPinnedObject();
                image = new Bitmap(mip.USize, mip.VSize, 4, pixelFormat, pointer);
            }
            finally
            {
                pinnedArray.Free();
            }

            return image;
        }
    }
}
