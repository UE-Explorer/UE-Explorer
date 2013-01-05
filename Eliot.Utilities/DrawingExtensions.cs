using System.Drawing;

namespace Eliot.Utilities
{
    public static class DrawingExtensions
    {
        public static Color Darken( this Color c, float pct )
        {
            pct = 1.0F - pct/100F;
            var r = (byte)(c.R*pct);
            var g = (byte)(c.G*pct);
            var b = (byte)(c.B*pct);
            return Color.FromArgb( c.A, r, g, b );
        }

        public static Color Lighten( this Color c, float pct )
        {
            pct = 1.0F + pct/100F;
            var r = (byte)(c.R*pct);
            var g = (byte)(c.G*pct);
            var b = (byte)(c.B*pct);
            return Color.FromArgb( c.A, r, g, b );
        }
    }
}
