using System;
using System.Drawing.Imaging;
using UELib.Engine;

namespace UEExplorer.Render.Extensions.Drawing
{
    // TODO: Move System extensions into a new project "UELib.Extensions.Drawing"
    public static class ImagingExtensions
    {
        public static PixelFormat ToPixelFormat(this UBitmapMaterial.TextureFormat unrealFormat)
        {
            switch (unrealFormat)
            {
                case UBitmapMaterial.TextureFormat.P8:
                case UBitmapMaterial.TextureFormat.L8:
                case UBitmapMaterial.TextureFormat.DXT3:
                case UBitmapMaterial.TextureFormat.DXT5:
                    return PixelFormat.Format8bppIndexed;
                case UBitmapMaterial.TextureFormat.DXT1:
                    return PixelFormat.Format4bppIndexed;
                case UBitmapMaterial.TextureFormat.RGB8:
                    return PixelFormat.Format24bppRgb;
                case UBitmapMaterial.TextureFormat.RGBA7:
                case UBitmapMaterial.TextureFormat.RGBA8:
                    return PixelFormat.Format32bppArgb;
                case UBitmapMaterial.TextureFormat.RGB16:
                case UBitmapMaterial.TextureFormat.G16:
                    return PixelFormat.Format16bppGrayScale;
                case UBitmapMaterial.TextureFormat.NODATA:
                    return PixelFormat.DontCare;
                // Not implemented
                case UBitmapMaterial.TextureFormat.RRRGGGBBB:
                default:
                    throw new NotImplementedException($"TextureFormat '{unrealFormat}' is not implemented yet.");
            }
        }
    }
}