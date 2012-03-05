using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UEExplorer.UI.Dialogs
{
	public partial class ColorGeneratorForm : Form
	{
		public ColorGeneratorForm()
		{
			InitializeComponent();
		}

		private void button2_Click( object sender, EventArgs e )
		{
			var dr = colorDialog1.ShowDialog();
			if( dr != DialogResult.OK )
			{
				// Don't add a color because the user didn't want it!
				return;
			}
			textBox1.Text = textBox1.Text.Insert( textBox1.SelectionStart, checkBox1.Checked 
				? ColorCodeGenerator.ConvertToUE3ColorCode( colorDialog1.Color ) 
				: ColorCodeGenerator.ConvertToColorCode( colorDialog1.Color ) );
		}

		private void button1_Click( object sender, EventArgs e )
		{
			var dr = colorDialog1.ShowDialog();
			if( dr != DialogResult.OK )
			{
				// Don't add a color cuz the user didn't want it!
				return;
			}
													   
			textBox3.Text = ColorCodeGenerator.RGBToHEX( colorDialog1.Color );
		}

		private struct ColoredText
		{
			public string Text;
			public Color Color;
		}

		private void panel1_Paint( object sender, PaintEventArgs e )
		{
			if( textBox1.Text.Length == 0 )
			{
				e.Graphics.DrawString( "Colorized Preview", Font, new SolidBrush( Color.Black ), 0, 0 );
			}

			// Grab all color codes and construct a structure 
			// with the color code converted to a color and with the text next to it until the next color.
			var colors = new List<ColoredText>();
			string S = textBox1.Text;
			for( var i = 0; i < S.Length; ++ i )
			{
				if( S[i].ToString() == ColorCodeGenerator.Chr( ColorCodeGenerator.ColorTag ) )
				{
					if( i + 3 < S.Length )
					{
						ColoredText T;
						byte[] ColorText = Encoding.GetEncoding( 1252 ).GetBytes( new char[] { S[i + 1], S[i + 2], S[i + 3] } );
						T.Color = Color.FromArgb( (int)ColorText[0], (int)ColorText[1], (int)ColorText[2] );
						T.Text = S.Substring( i + 4 );
						for( int j = 0; j < T.Text.Length; ++ j )
						{
							if( T.Text[j].ToString() == ColorCodeGenerator.Chr( ColorCodeGenerator.ColorTag ) )
							{
								T.Text = T.Text.Remove( j );
								break;
							}
						}
						colors.Add( T );
						i += 3 + T.Text.Length;
					}
				}
			}

			// Grab all text with no color code before it.
			string xS = "";
			for( int j = 0; j < textBox1.Text.Length; ++ j )
			{
				if( textBox1.Text[j].ToString() == ColorCodeGenerator.Chr( ColorCodeGenerator.ColorTag ) )
				{
					break;
				}
				xS = textBox1.Text.Substring( 0, j + 1 );
			}

			if( xS != "" )
			{
				ColoredText T;
				T.Text = xS;
				T.Color = Color.Black;
				colors.Insert( 0, T );
			}
			
			// Draw all grabbed colored text.
			float xoff = 0;
			float yoff = 0;
			for( int i = 0; i < colors.Count; xoff += e.Graphics.MeasureString( colors[i].Text, Font ).Width, ++ i )
			{	
				// kinda buggy xd.
				if( xoff + e.Graphics.MeasureString( colors[i].Text, Font ).Width >= panel1.ClientSize.Width )
				{			
					xoff = 0;
					yoff += Font.Height;
				}
				e.Graphics.DrawString( colors[i].Text, Font, new SolidBrush( colors[i].Color ), xoff, yoff );	
			}
		}

		private void textBox1_TextChanged( object sender, EventArgs e )
		{
			panel1.Refresh();
		}

		private void panel1_SizeChanged( object sender, EventArgs e )
		{
			panel1.Refresh();
		}

		private void ColorGeneratorForm_FontChanged( object sender, EventArgs e )
		{
			panel1.Refresh();
		}
	}

	public static class ColorCodeGenerator
	{
		public const byte ColorTag = 0x1B;

		public static string Chr( byte num )
		{
			return Encoding.GetEncoding( 1252 ).GetString( new byte[]{ num } );
		}

		public static string ConvertToColorCode( Color colorToConvert )
		{
			string colorCode = Chr( Math.Max( colorToConvert.R, (byte)1 ) ) +
				Chr( Math.Max( colorToConvert.G, (byte)1 ) ) +
				Chr( Math.Max( colorToConvert.B, (byte)1 ) );

			return Chr( ColorTag ) + colorCode;
		}

		public static string ConvertToUE3ColorCode( Color colorToConvert )
		{
			// e.g. "<Color:R=1.0,G=1.0,B=1.0,A=1.0>"
			return "<Color:R=" +
				(float)colorToConvert.R / 255 + ",G=" + 
				(float)colorToConvert.G / 255 + ",B=" +
				(float)colorToConvert.B / 255 + ",A=" +
				(float)colorToConvert.A / 255 + ">";
		}

		public static string RGBToHEX( Color colorToConvert )
		{
			return ColorTranslator.ToHtml( colorToConvert ); 
			//return "#" + (ColorToConvert.ToArgb() & 0x00FFFFFF).ToString( "X6" );
		}

		public static Color HEXToRGB( string HEXToConvert )
		{
			return ColorTranslator.FromHtml( HEXToConvert );
		}
	}
}
