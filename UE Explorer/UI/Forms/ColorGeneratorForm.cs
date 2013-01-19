using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace UEExplorer.UI.Forms
{
    public partial class ColorGeneratorForm : Form
    {
        public ColorGeneratorForm()
        {
            InitializeComponent();
        }

        private void PickColorCodeButton_Click( object sender, EventArgs e )
        {
            var result = ColorDialog.ShowDialog();
            if( result != DialogResult.OK )
            {
                return;
            }
            ColoredTextInput.Text = ColoredTextInput.Text.Insert( 
                ColoredTextInput.SelectionStart, 
                XMLFormatCheckBox.Checked 
                    ? ColorCode.ToXMLCode( ColorDialog.Color ) 
                    : ColorCode.ToCode( ColorDialog.Color ) 
            );
        }

        private void PickHTMLColorButton_Click( object sender, EventArgs e )
        {
            var result = ColorDialog.ShowDialog();
            if( result != DialogResult.OK )
            {
                return;
            }									   
            HTMLColorText.Text = ColorCode.ToHEX( ColorDialog.Color );
        }

        private struct ColoredText
        {
            public string Text;
            public Color Color;
        }

        private void ColoredTextPreview_Paint( object sender, PaintEventArgs e )
        {
            if( ColoredTextInput.Text.Length == 0 )
            {
                e.Graphics.DrawString( 
                    "Colorized Preview", 
                    ColoredTextPreview.Font, 
                    new SolidBrush( Color.Black ), 
                    0, 0 
                );
            }

            // Grab all color codes and construct a structure 
            // with the color code converted to a color and with the text next to it until the next color.
            var colors = new List<ColoredText>();
            string s = ColoredTextInput.Text;
            for( var i = 0; i < s.Length; ++ i )
            {
                if( s[i] != ColorCode.ColorTag || i + 3 >= s.Length ) 
                    continue;

                var textColor = new ColoredText
                {
                    Color = Color.FromArgb( (byte)s[i + 1], (byte)s[i + 2], (byte)s[i + 3] ), 
                    Text = s.Substring( i + 4 )
                };

                for( int j = 0; j < textColor.Text.Length; ++ j )
                {
                    if( textColor.Text[j] != ColorCode.ColorTag ) 
                        continue;

                    textColor.Text = textColor.Text.Remove( j );
                    break;
                }
                colors.Add( textColor );
                i += 3 + textColor.Text.Length;
            }

            // Grab all text with no color code before it.
            string remainingText = null;
            for( int i = 0; i < ColoredTextInput.Text.Length; ++ i )
            {
                if( ColoredTextInput.Text[i] == ColorCode.ColorTag )
                {
                    break;
                }

                remainingText = ColoredTextInput.Text.Substring( 0, i + 1 );
            }

            if( !String.IsNullOrEmpty( remainingText ) )
            { 
                var textColor = new ColoredText
                {
                    Text = remainingText,
                    Color = Color.Black
                };
                colors.Insert( 0, textColor );
            }
            
            // Draw all grabbed colored text.
            float x = 0.0f;
            float y = 0.0f;
            for( int i = 0; i < colors.Count; ++ i )
            {	
                var xl = e.Graphics.MeasureString( colors[i].Text, ColoredTextPreview.Font ).Width;
                if( x + xl >= ColoredTextPreview.ClientSize.Width )
                {			
                    x = 0.0f;
                    y += ColoredTextPreview.Font.Height;
                }
                e.Graphics.DrawString( 
                    colors[i].Text, 
                    Font, 
                    new SolidBrush( colors[i].Color ), 
                    x, y 
                );	
                x += xl;
            }
        }

        private void ColoredTextPreview_TextChanged( object sender, EventArgs e )
        {
            ColoredTextPreview.Refresh();
        }

        private void ColoredTextPreview_SizeChanged( object sender, EventArgs e )
        {
            ColoredTextPreview.Refresh();
        }
    }
}
