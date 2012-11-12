using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using UEExplorer.Properties;
using UELib.Core;
using UELib.Tokens;
using UStruct = UELib.Core.UStruct;
using System.Xml;
using System.Xml.Serialization;

namespace UEExplorer.UI.Dialogs
{
	public partial class UserControl_HexView : UserControl
	{
		public class HexMetaInfo 
		{
			public class BytesMetaInfo
			{
				public int Position;
				[XmlIgnore]
				public int Size;
				public string Type;
				public string Name;
				[XmlIgnore]
				public Color Color;
			}

			public List<BytesMetaInfo> MetaInfoList;
		}

		private HexMetaInfo _Structure;

		public byte[] Buffer
		{
			get;
			set;
		}

		private const int ViewWidth = 16;
		private const float HexSpacing = 20;
		private const float ColumnSize = (ViewWidth * HexSpacing);
		private const float ColumnOffset = 12;
		private readonly float _LineSpacing;

		private bool _DrawASCII = true;
		private bool _DrawByte = true;

		public bool DrawASCII
		{
			get{ return _DrawASCII; }
			set
			{ 
				_DrawASCII = value;
				HexLinePanel.Invalidate();
			}
		}

		public bool DrawByte
		{
			get{ return _DrawByte; }
			set
			{
				_DrawByte = value;
				HexLinePanel.Invalidate();
			}
		}

		private UObject _Object;
		public UObject HexObject
		{
			get{ return _Object; }
		}

		public UserControl_HexView()
		{
			InitializeComponent();
			_LineSpacing = HexLinePanel.Font.Height + 4;
		}

		private long _ScriptOffset = -1;
		private long _ScriptSize = -1;

		private void LoadConfig( string path )
		{
			using( var r = new XmlTextReader( path ) )
			{
				var xser = new XmlSerializer( typeof(HexMetaInfo) );
				_Structure = (HexMetaInfo)xser.Deserialize( r );

				foreach( var s in _Structure.MetaInfoList )
				{
					byte size;
					Color color;

					InitStructure( s.Type, out size, out color );	
					s.Size = size;
					s.Color = color;
				}

				HexLinePanel.Invalidate();
			}
		}

		private void InitStructure( string type, out byte size, out Color color )
		{
			switch( type.ToLower() )
			{
				case "char":
				case "byte":
				case "code":
					size = 1;
					color = Color.Blue;
					break;

				case "short":
					size = 2;
					color = Color.Orange;
					break;

				case "int":
				case "float":
					size = 4;
					color = Color.Magenta;
					break;

				case "long":
					size = 8;
					color = Color.Pink;
					break;

				case "name":
				case "object":
				case "index":
					size = 4;
					color = Color.Green;
					break;

				default:
					size = 1;
					color = Color.Black;
					break;
			}
		}

		private void SaveConfig( string path )
		{
			if( !System.IO.Directory.Exists( System.IO.Path.GetDirectoryName( path ) ) )
			{
				System.IO.Directory.CreateDirectory( System.IO.Path.GetDirectoryName( path ) );	
			}

			using( var w = new XmlTextWriter( path, System.Text.Encoding.ASCII ) )
			{
				var xser = new XmlSerializer( typeof(HexMetaInfo) );
				xser.Serialize( w, _Structure );
			}
		}

		public void SetHexData( UObject uObject )
		{
			_Object = uObject;
			Buffer = _Object.GetBuffer();
			vScrollBar1.Value = 0;
			if( Buffer != null )
			{
				var lines = (int)Math.Ceiling( Buffer.Length / ((float)ViewWidth) );
				vScrollBar1.Maximum = lines - 1;
				vScrollBar1.Visible = lines > 0;
			}
			HexLinePanel.Invalidate();

			if( _Object is UStruct )
			{
				_ScriptOffset = (_Object as UStruct).ScriptOffset;
				_ScriptSize = (_Object as UStruct).ScriptSize; 
			}

			if( _Object != null )
			{
				var path = GetConfigPath();
				if( System.IO.File.Exists( path ) )
				{
					LoadConfig( path );
				}
				else
				{
					_Structure = new HexMetaInfo{MetaInfoList = new List<HexMetaInfo.BytesMetaInfo>()};
				}
			}
		}

		private string GetConfigPath()
		{
			string folderName = _Object.Package != null ? _Object.Package.PackageName : _Object.Name;
			var outers = _Object.Package != null ? _Object.GetOuterGroup() : folderName; 
			return System.IO.Path.Combine( 
				Application.StartupPath, 
				"DataStructures",
				folderName,
				outers
			) + ".xml";
		}

		private readonly char[] _Hexs = new[]{'0','1','2','3','4','5','6','7','8','9','A','B','C','D','E','F'};

		private void HexLinePanel_Paint( object sender, PaintEventArgs e )
		{
			if( Buffer == null ) 
				return;

			//e.Graphics.PageUnit = GraphicsUnit.Pixel;
			int offset = (ViewWidth * vScrollBar1.Value);
			int lineCount = Math.Min( (int)(HexLinePanel.ClientSize.Height / _LineSpacing), (Buffer.Length - offset) / ViewWidth + 
				(((Buffer.Length - offset) % ViewWidth) > 0 ? 1 : 0) );
		
			float lineyOffset = _LineSpacing;
			float byteColumoffset = _DrawByte ? 74 : 0;
			float asciiColumoffset = byteColumoffset == 0 ? 74 : byteColumoffset + ColumnSize + ColumnOffset;

			Brush brush = new SolidBrush( ForeColor );

			const string text = "Offset";
			var textLength = e.Graphics.MeasureString( text, HexLinePanel.Font, 
				new PointF(0,0), StringFormat.GenericTypographic 
			);
			e.Graphics.DrawString( text, HexLinePanel.Font, brush, 
				2, 
				textLength.Height*0.5f,
				StringFormat.GenericTypographic
			);
			e.Graphics.DrawLine( new Pen( new SolidBrush( Color.FromArgb( 0x55EDEDED ) ) ), 
				2, _LineSpacing + HexLinePanel.Font.Height*.5f, 
				2 + byteColumoffset - HexSpacing*.5f, _LineSpacing + HexLinePanel.Font.Height*.5f 
			);

			if( _DrawByte )
			{
				float x = byteColumoffset;
				float y = 0;

				//e.Graphics.FillRectangle( new SolidBrush( Color.FromArgb(44, 44, 44) ), x, y, ColumnSize, _LineSpacing );
				e.Graphics.DrawLine( new Pen( new SolidBrush( Color.FromArgb( 0x55EDEDED ) ) ), 
					x, y + _LineSpacing + HexLinePanel.Font.Height*.5f, 
					x + ColumnSize, y + _LineSpacing + HexLinePanel.Font.Height*.5f 
				);

				for( int i = 0; i < ViewWidth; ++ i )
				{
					var c = "0" + _Hexs[i].ToString( CultureInfo.InvariantCulture );
					var cs = e.Graphics.MeasureString( c, HexLinePanel.Font, new PointF(0,0), StringFormat.GenericTypographic );
					e.Graphics.DrawString( c, HexLinePanel.Font, brush, 
						x + i*HexSpacing + 2, 
						y + cs.Height*0.5f, 
						StringFormat.GenericTypographic 
					);
				}
			}

			const float asciiWidth = HexSpacing;
			if( _DrawASCII )
			{
				float x = asciiColumoffset;
				float y = 0;

				//e.Graphics.FillRectangle( new SolidBrush( Color.FromArgb(44, 44, 44) ), x, y, ColumnSize, _LineSpacing );
				e.Graphics.DrawLine( new Pen( new SolidBrush( Color.FromArgb( 0x55EDEDED ) ) ), 
					x, y + _LineSpacing + HexLinePanel.Font.Height*.5f, x + ViewWidth * asciiWidth, 
					y + _LineSpacing + HexLinePanel.Font.Height*.5f 
				);

				for( int i = 0; i < ViewWidth; ++ i )
				{
					var c = "0" + _Hexs[i].ToString( CultureInfo.InvariantCulture );
					var cs = e.Graphics.MeasureString( c, HexLinePanel.Font, new PointF(0,0), StringFormat.GenericTypographic );
					e.Graphics.DrawString( c, HexLinePanel.Font, brush, 
						x + i*asciiWidth + 2, 
						y + cs.Height*0.5f, 
						StringFormat.GenericTypographic 
					);
				}
			}

			lineyOffset += HexLinePanel.Font.Height;
			float extraLineOffset = _LineSpacing;
			for( int line = 0; line < lineCount; ++ line )
			{
				if( lineyOffset >= HexLinePanel.ClientSize.Height )
				{
					break;
				}

				string lineText = String.Format( "{0:x8}", offset ).PadLeft( 8, '0' ).ToUpper();
				e.Graphics.DrawString( lineText, HexLinePanel.Font, brush, 0, lineyOffset );
			
				if( _DrawByte )
				{
					for( int hexByte = 0; hexByte < ViewWidth; ++ hexByte )
					{
						int byteOffset = (offset + hexByte);
						if( byteOffset < Buffer.Length )
						{
							Brush drawbrush = brush;
							string drawntext = String.Format( "{0:x2}", Buffer[byteOffset] ).ToUpper();
							if( byteOffset == SelectedOffset )
							{
								// Draw the selection.
								var drawPen = new Pen( new SolidBrush( Color.Teal ) );
								e.Graphics.DrawRectangle( drawPen, new Rectangle(
									(int)(byteColumoffset + (hexByte * HexSpacing)),
									(int)(lineyOffset),
									(int)(HexSpacing),
									(int)(_LineSpacing)
								));  
							}
							else if( _ScriptSize > 0 )
							{
								if( byteOffset == _ScriptOffset )
								{
									// Draw the selection.
									var p = new Pen( new SolidBrush( Color.Red ) );
									e.Graphics.DrawLine( p, new Point( (int)(byteColumoffset + (hexByte * HexSpacing)), 
										(int)(lineyOffset + extraLineOffset) ), 
										new Point( (int)(byteColumoffset + ((hexByte + 1) * HexSpacing)), 
										(int)(lineyOffset + extraLineOffset) ) 
									);   
								}
								else if( byteOffset == _ScriptOffset + _ScriptSize - 1 )
								{
									// Draw the selection.
									var p = new Pen( new SolidBrush( Color.Orange ) );
									e.Graphics.DrawLine( p, new Point( (int)(byteColumoffset + (hexByte * HexSpacing)), 
										(int)(lineyOffset + extraLineOffset) ), 
										new Point( (int)(byteColumoffset + ((hexByte + 1) * HexSpacing)), 
										(int)(lineyOffset + extraLineOffset) ) 
									);   
								}
							}
							e.Graphics.DrawString( drawntext, HexLinePanel.Font, drawbrush, 
								byteColumoffset + (hexByte * HexSpacing), lineyOffset 
							);

							foreach( var s in _Structure.MetaInfoList )
							{
								if( byteOffset >= s.Position && byteOffset < s.Position + s.Size )
								{
									var p = new Pen( new SolidBrush( s.Color ) );
									e.Graphics.DrawLine( p, 
										new Point( 
											(int)(byteColumoffset + (hexByte * HexSpacing)), 
											(int)(lineyOffset + extraLineOffset) 
										), 
										new Point( 
											(int)(byteColumoffset + ((hexByte + 1) * HexSpacing)), 
											(int)(lineyOffset + extraLineOffset) 
										) 
									); 		
								}
							}
						}				
					}
				}

				if( _DrawASCII )
				{
					for( int hexByte = 0; hexByte < ViewWidth; ++ hexByte )
					{
						int byteOffset = (offset + hexByte);
						if( byteOffset < Buffer.Length )
						{
							Brush drawbrush = brush;
							string drawntext = "" + FilterByte( Buffer[byteOffset] );
							if( byteOffset == SelectedOffset )
							{
								// Draw the selection.
								var drawPen = new Pen( new SolidBrush( Color.Teal ) );
								e.Graphics.DrawRectangle( drawPen, new Rectangle(
									(int)(asciiColumoffset + (hexByte * asciiWidth)),
									(int)(lineyOffset),
									(int)(HexSpacing),
									(int)(_LineSpacing)
								));    
							}
							e.Graphics.DrawString( drawntext, HexLinePanel.Font, drawbrush, 
								asciiColumoffset + (hexByte * asciiWidth), lineyOffset 
							);
						}			
					}
				}
				offset += ViewWidth;
				lineyOffset += extraLineOffset;
			}
		}

		internal static char FilterByte( byte code )
		{
			if( code >= 0x20 && code <= 0x7E )
			{
				return (char)code;
			}
			return '.';
		}

		private int _SelectedOffset = -1;
		private int SelectedOffset
		{
			get{ return _SelectedOffset; }
			set{ _SelectedOffset = value; OffsetChanged(); }
		}

		private void OffsetChanged()
		{
			DissambledObject.Text = "";
			DissambledName.Text = "";

			var hexViewDialog = (HexViewDialog)ParentForm;
			if( hexViewDialog != null )
			{
				hexViewDialog.ToolStripStatusLabel_Position.Text = 
					String.Format( 
						"Position: {0} + {1} ; 0x{2:x8}", 
						HexObject.ExportTable != null ? HexObject.ExportTable.SerialOffset : 0, 
						SelectedOffset,
						HexObject.ExportTable != null ? HexObject.ExportTable.SerialOffset : 0 + SelectedOffset
					);
			}
			var cb = new byte[8];
			int i = 0;
 			for( ; SelectedOffset + i < Buffer.Length && i < 8; ++ i )
			{
				cb[i] = Buffer[SelectedOffset + i];
			}

			/*if( i != 8 )
			{
				for( ; i < 8; ++ i )
				{
					cb[i] = 0;
				}
			}*/

			DissambledChar.Text = string.Format( "{0}.{1}", (char)cb[0], BitConverter.ToChar( cb, 0 ) );
			DissambledByte.Text = "" + cb[0];

			DissambledShort.Text = "" + BitConverter.ToInt16( cb, 0 );
			DissambledUShort.Text = "" + BitConverter.ToUInt16( cb, 0 );

			DissambledInt.Text = "" + BitConverter.ToInt32( cb, 0 );
			DissambledUInt.Text = "" + BitConverter.ToUInt32( cb, 0 );
			DissambledFloat.Text = "" + BitConverter.ToSingle( cb, 0 );

			DissambledLong.Text = "" + BitConverter.ToInt64( cb, 0 );
			DissambledULong.Text = "" + BitConverter.ToUInt64( cb, 0 );

			try
			{
				var index = UELib.UnrealReader.ReadIndexFromBuffer( cb, _Object.Package );
				DissambledIndex.Text = index.ToString( CultureInfo.InvariantCulture );
			}
			catch
			{
				DissambledIndex.Text = Resources.NOT_AVAILABLE;
			}

			try
			{
				UObject obj = _Object.Package.GetIndexObject( UELib.UnrealReader.ReadIndexFromBuffer( cb, _Object.Package ) );
				DissambledObject.Text = obj == null ? Resources.NOT_AVAILABLE : obj.GetOuterGroup();
			}
			catch
			{
				DissambledObject.Text = Resources.NOT_AVAILABLE;
			}

			try
			{
				DissambledName.Text = String.Empty + _Object.Package.GetIndexName( UELib.UnrealReader.ReadIndexFromBuffer( cb, _Object.Package ) );
			}
			catch
			{
				DissambledName.Text = Resources.NOT_AVAILABLE;
			}

			try
			{
				byte byteCode = cb[0];
				if( _Object.Package.Version >= 184 && 
					((byteCode >= (byte)ExprToken.Unknown && byteCode < (byte)ExprToken.ReturnNothing) 
					|| (byteCode > (byte)ExprToken.NoDelegate && byteCode < (byte)ExprToken.ExtendedNative)) )
				{
		  			++ byteCode;
				}
				DissambledByteCode.Text = string.Format( "{0}:{1}", (ExprToken)byteCode, (CastToken)byteCode );
			}
			catch
			{
				DissambledByteCode.Text = Resources.NOT_AVAILABLE;
			}

			DissambledStruct.Text = String.Empty;
			foreach( var s in _Structure.MetaInfoList )
			{
				if( SelectedOffset >= s.Position && SelectedOffset < s.Position + s.Size )
				{
					DissambledStruct.Text = s.Name;
				}
			}
		}

		private void VScrollBar1_Scroll( object sender, ScrollEventArgs e )
		{
			HexLinePanel.Invalidate();
		}

		private void HexLinePanel_MouseClick( object sender, MouseEventArgs e )
		{
			if( Buffer == null || e.Button != MouseButtons.Left )
			{
				return;
			}

			float x = e.X - HexLinePanel.Location.X;
			float y = e.Y - HexLinePanel.Location.Y;

			int offset = (ViewWidth * vScrollBar1.Value);
			int lineCount = Math.Min( (int)(HexLinePanel.ClientSize.Height / _LineSpacing), 
				(Buffer.Length - offset) / ViewWidth + 
				(((Buffer.Length - offset) % ViewWidth) > 0 ? 1 : 0) 
			);
		
			float lineyoffset = HexLinePanel.Font.Height;
			float byteoffset = _DrawByte ? 74 : 0;
			float charoffset = byteoffset == 0 ? 74 : byteoffset + ColumnSize + ColumnOffset;

			float extraLineOffset = _LineSpacing;
			for( int line = 0; line < lineCount; ++ line )
			{
				if( lineyoffset >= HexLinePanel.ClientSize.Height )
				{
					break;
				}

				// The user definitely didn't click on this line?, so skip!.
				if( !(y >= (lineyoffset + extraLineOffset) && y <= (lineyoffset + (extraLineOffset * 2))) )
				{
					offset += ViewWidth;
					lineyoffset += extraLineOffset;
					continue;
				}

				// Check if the bytes field is selected.
				if( _DrawByte && x >= byteoffset && x < charoffset )
				{
					for( int hexByte = 0; hexByte < ViewWidth; ++ hexByte )
					{
						int byteOffset = (offset + hexByte);
						if( byteOffset < Buffer.Length )
						{							
							if
							( 
								x >= (byteoffset + (hexByte * HexSpacing)) && x <= (byteoffset + ((hexByte + 1) * HexSpacing)) 
							)
							{
								SelectedOffset = byteOffset;
								HexLinePanel.Invalidate();
								//MessageBox.Show( "Click Detected at " + Offset );
								return;
							}
						}				
					}
				}

				// Check if the ascii's field is selected.
				if( _DrawASCII && x >= charoffset )
				{
					const float asciiWidth = HexSpacing;
					for( int hexByte = 0; hexByte < ViewWidth; ++ hexByte )
					{
						int byteOffset = (offset + hexByte);
						if( byteOffset < Buffer.Length )
						{
							if
							(
								x >= (charoffset + (hexByte * asciiWidth)) && x <= (charoffset + ((hexByte + 1) * asciiWidth))
							)
							{
								SelectedOffset = byteOffset;
								HexLinePanel.Invalidate();
								//MessageBox.Show( "Click Detected at " + Offset );
								return;
							}
						}
					}
				}
				offset += ViewWidth;
				lineyoffset += extraLineOffset;
			}
			//_SelectedOffset = -1;
			//HexLinePanel.Invalidate();
			Focus();
		}

		private void UserControl_HexView_KeyDown( object sender, KeyEventArgs e )
		{
			switch( e.KeyCode )
			{
				case Keys.Up:
					SelectedOffset = Math.Max( SelectedOffset - 16, 0 );
					HexLinePanel.Invalidate();
					break;

				case Keys.Down:
					SelectedOffset = Math.Min( SelectedOffset + 16, Buffer.Length - 1 );
					HexLinePanel.Invalidate();
					break;

				case Keys.Left:
					SelectedOffset = Math.Max( SelectedOffset - 1, 0 );
					HexLinePanel.Invalidate();
					break;

				case Keys.Right:
					SelectedOffset = Math.Min( SelectedOffset + 1, Buffer.Length - 1 );
					HexLinePanel.Invalidate();
					break;
			}
		}

		private void Context_Structure_ItemClicked( object sender, ToolStripItemClickedEventArgs e )
		{
			var dialog = new StructureInputDialog();
			var type = e.ClickedItem.Text.Substring( 7 );
			dialog.TextBoxName.Text = type;
			if( dialog.ShowDialog() == DialogResult.OK )
			{
				byte size;
				Color color;
				InitStructure( type, out size, out color );	
				_Structure.MetaInfoList.Add( new HexMetaInfo.BytesMetaInfo
				{
				    Name = dialog.TextBoxName.Text, Position = SelectedOffset, Size = size, Type = type,
					Color = color
				} );

				var path = GetConfigPath();
				SaveConfig( path );

				HexLinePanel.Invalidate();
			}
		}
	}

	public class HexPanel : Panel
	{
		public HexPanel()
		{
			SetStyle( ControlStyles.UserPaint, true );
			SetStyle( ControlStyles.AllPaintingInWmPaint, true );
			SetStyle( ControlStyles.OptimizedDoubleBuffer, true );
			SetStyle( ControlStyles.ResizeRedraw, true );
		}
	}
}