using System.Drawing;
using System.Globalization;

namespace UEExplorer.UI.Forms
{
    internal static class ColorCode
    {
        public const char ColorTag = (char)0x1B;

        public static string ToCode(Color c)
        {
            return
            (
                ColorTag
                + ((char)(c.R | 1)).ToString(CultureInfo.InvariantCulture)
                + ((char)(c.G | 1)).ToString(CultureInfo.InvariantCulture)
                + ((char)(c.B | 1)).ToString(CultureInfo.InvariantCulture)
            ).ToString(CultureInfo.InvariantCulture);
        }

        public static string ToXMLCode(Color c)
        {
            // e.g. "<Color:R=1.0,G=1.0,B=1.0,A=1.0>"
            return "<Color"
                   + ":R=" + c.R / 255F
                   + ",G=" + c.G / 255F
                   + ",B=" + c.B / 255F
                   + ",A=" + c.A / 255F
                   + "></Color>";
        }

        public static string ToHEX(Color c)
        {
            return ColorTranslator.ToHtml(c);
        }

        public static Color ToRGB(string hex)
        {
            return ColorTranslator.FromHtml(hex);
        }
    }
}