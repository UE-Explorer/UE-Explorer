using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using Eliot.Utilities;

namespace UEExplorer.UI.Dialogs
{
    using Properties;
    using UELib;
    using UStruct = UELib.Core.UStruct;

    // TODO: REFACTOR, and rewrite all of it to be more concise, less duplicational.
    public partial class UserControl_HexView : UserControl
    {
        public byte[] Buffer{ get; set; }
        public IBuffered Target{ get; private set; }

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

                [XmlIgnore]
                public IUnrealDecompilable Tag;
            }

            public List<BytesMetaInfo> MetaInfoList;
        }
        private HexMetaInfo _Structure;

        #region View Properties
        private readonly float			_LineSpacing;
        private readonly char[]			_Hexadecimal	= {'0','1','2','3','4','5','6','7','8','9','A','B','C','D','E','F'};
        private const int				ByteCount		= 16;
        private const float				ByteWidth		= 20;
        private const float				ColumnSize		= ByteCount * ByteWidth;
        private const float				ColumnMargin	= 12;
        
        private bool					_DrawASCII		= true;
        public bool						DrawASCII
        {
            get{ return _DrawASCII; }
            set
            { 
                _DrawASCII = value;
                HexLinePanel.Invalidate();
            }
        }

        private bool					_DrawByte		= true;
        public bool						DrawByte
        {
            get{ return _DrawByte; }
            set
            {
                _DrawByte = value;
                HexLinePanel.Invalidate();
            }
        }
        #endregion

        public UserControl_HexView()
        {
            InitializeComponent();
            _LineSpacing = HexLinePanel.Font.Height + 4;
        }

        private void LoadConfig( string path )
        {
            using( var r = new XmlTextReader( path ) )
            {
                var xser = new XmlSerializer( typeof(HexMetaInfo) );
                _Structure = (HexMetaInfo)xser.Deserialize( r );

                foreach( var s in _Structure.MetaInfoList.Where( s => s.Type != "Generated" ) )
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
                    size = 1;
                    color = Color.Peru;
                    break;

                case "byte":
                    size = 1;
                    color = Color.DarkBlue;
                    break;

                case "code":
                    size = 1;
                    color = Color.Blue;
                    break;

                case "short":
                    size = 2;
                    color = Color.MediumBlue;
                    break;

                case "int":
                    size = 4;
                    color = Color.DodgerBlue;
                    break;

                case "float":
                    size = 4;
                    color = Color.SlateBlue;
                    break;

                case "long":
                    size = 8;
                    color = Color.Purple;
                    break;

                case "name":
                    size = 4;
                    color = Color.Green;
                    break;

                case "object":
                    size = 4;
                    color = Color.DarkTurquoise;
                    break;

                case "index":
                    size = 4;
                    color = Color.MediumOrchid;
                    break;

                default:
                    size = 1;
                    color = Color.Black;
                    break;
            }
        }

        private void SaveConfig( string path )
        {
            if( !Directory.Exists( Path.GetDirectoryName( path ) ) )
            {
                Directory.CreateDirectory( Path.GetDirectoryName( path ) );	
            }

            var backupInfo = _Structure.MetaInfoList.ToArray();
            _Structure.MetaInfoList.RemoveAll( i => i.Type == "Generated" );
            using( var w = new XmlTextWriter( path, System.Text.Encoding.ASCII ) )
            {
                var xser = new XmlSerializer( typeof(HexMetaInfo) );
                xser.Serialize( w, _Structure );
            }
            _Structure.MetaInfoList.AddRange( backupInfo );
        }

        public void SetHexData( IBuffered target )
        {
            if( target == null ) 
                return;

            Target = target;
            Buffer = Target.CopyBuffer();
            
            if( Buffer != null )
            {
                var lines = (int)Math.Ceiling( Buffer.Length / ((float)ByteCount) );
                HexScrollBar.Value = 0;
                HexScrollBar.Maximum = lines - 1;
                HexScrollBar.Visible = lines > 0;
            }

            var path = GetConfigPath();
            if( File.Exists( path ) )
            {
                LoadConfig( path );
            }
            else
            {
                _Structure = new HexMetaInfo{MetaInfoList = new List<HexMetaInfo.BytesMetaInfo>()};
            }

            InitializeMetaInfoFields();
        }

        private void InitializeMetaInfoFields()
        {
#if DEBUG || BINARYMETADATA
            var binaryTarget = Target as IBinaryData;
            if( binaryTarget != null && binaryTarget.BinaryMetaData != null )
            {
                var randomizer1 = new Random( binaryTarget.BinaryMetaData.Fields.Count );
                foreach( var binaryField in binaryTarget.BinaryMetaData.Fields )
                {
                    var red = randomizer1.Next( Byte.MaxValue );
                    var green = randomizer1.Next( Byte.MaxValue );
                    var blue = randomizer1.Next( Byte.MaxValue );

                    _Structure.MetaInfoList.Add
                    (
                        new HexMetaInfo.BytesMetaInfo
                        {
                            Position = (int)(binaryField.Position),
                            Size = (int)binaryField.Size,
                            Type = "Generated",
                            Color = Color.FromArgb( red, green, blue ),
                            Name = binaryField.Name,
                            Tag = binaryField
                        }
                    );
                }
            }
#endif
            if( !(Target is UStruct) )
                return;

            var unStruct = Target as UStruct;
            if( unStruct.ByteCodeManager == null || unStruct.ByteCodeManager.DeserializedTokens.Count <= 0 )
                return;

            var randomizer2 = new Random( unStruct.ByteCodeManager.DeserializedTokens.Count );
            foreach( var token in unStruct.ByteCodeManager.DeserializedTokens )
            {
                var red = randomizer2.Next( Byte.MaxValue );
                var green = randomizer2.Next( Byte.MaxValue );
                var blue = randomizer2.Next( Byte.MaxValue );

                _Structure.MetaInfoList.Add
                (
                    new HexMetaInfo.BytesMetaInfo
                    {
                        Position = (int)(token.StoragePosition - 1) + (int)unStruct.ScriptOffset,
                        Size = 1,
                        Type = "Generated",
                        Color = Color.FromArgb( red, green, blue ),
                        Name = token.GetType().Name,
                        Tag = token
                    }
                );
            }
        }

        private readonly string _ConfigPath = Path.Combine( Application.StartupPath, "DataStructures", "{0}", "{1}" ) + ".xml";
        private string GetConfigPath()
        {
            var folderName = Path.GetFileNameWithoutExtension( Target.GetBufferId( true ) );
            return String.Format( _ConfigPath, folderName, Target.GetBufferId() );
        }

        private void HexLinePanel_Paint( object sender, PaintEventArgs e )
        {
            if( Buffer == null ) 
                return;

            //e.Graphics.PageUnit = GraphicsUnit.Pixel;
            int offset = (ByteCount * HexScrollBar.Value);
            int lineCount = Math.Min( 
                (int)(HexLinePanel.ClientSize.Height / _LineSpacing), 
                (Buffer.Length - offset) / ByteCount + (((Buffer.Length - offset) % ByteCount) > 0 ? 1 : 0) 
            );
        
            float lineyOffset = _LineSpacing;
            float byteColumnOffset = _DrawByte ? 74 : 0;
            float asciiColumnOffset = byteColumnOffset == 0 ? 74 : byteColumnOffset + ColumnSize + ColumnMargin;

            Brush brush = new SolidBrush( ForeColor );

            string text = Resources.HexView_Offset;
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
                2 + byteColumnOffset - ByteWidth*.5f, _LineSpacing + HexLinePanel.Font.Height*.5f 
            );

            if( _DrawByte )
            {
                float x = byteColumnOffset;
                float y = 0;

                //e.Graphics.FillRectangle( new SolidBrush( Color.FromArgb(44, 44, 44) ), x, y, ColumnSize, _LineSpacing );
                e.Graphics.DrawLine( new Pen( new SolidBrush( Color.FromArgb( 0x55EDEDED ) ) ), 
                    x, y + _LineSpacing + HexLinePanel.Font.Height*.5f, 
                    x + ColumnSize, y + _LineSpacing + HexLinePanel.Font.Height*.5f 
                );

                for( int i = 0; i < ByteCount; ++ i )
                {
                    var c = "0" + _Hexadecimal[i].ToString( CultureInfo.InvariantCulture );
                    var cs = e.Graphics.MeasureString( c, HexLinePanel.Font, new PointF(0,0), StringFormat.GenericTypographic );
                    e.Graphics.DrawString( c, HexLinePanel.Font, brush, 
                        x + i*ByteWidth + 2, 
                        y + cs.Height*0.5f, 
                        StringFormat.GenericTypographic 
                    );
                }
            }

            const float asciiWidth = ByteWidth;
            if( _DrawASCII )
            {
                float x = asciiColumnOffset;
                float y = 0;

                //e.Graphics.FillRectangle( new SolidBrush( Color.FromArgb(44, 44, 44) ), x, y, ColumnSize, _LineSpacing );
                e.Graphics.DrawLine( new Pen( new SolidBrush( Color.FromArgb( 0x55EDEDED ) ) ), 
                    x, y + _LineSpacing + HexLinePanel.Font.Height*.5f, x + ByteCount * asciiWidth, 
                    y + _LineSpacing + HexLinePanel.Font.Height*.5f 
                );

                for( int i = 0; i < ByteCount; ++ i )
                {
                    var c = "0" + _Hexadecimal[i].ToString( CultureInfo.InvariantCulture );
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
                    for( int hexByte = 0; hexByte < ByteCount; ++ hexByte )
                    {
                        int byteOffset = (offset + hexByte);
                        if( byteOffset < Buffer.Length )
                        {
                            Brush drawbrush = brush;
                            string drawntext = String.Format( "{0:x2}", Buffer[byteOffset] ).ToUpper();

                            foreach( var s in _Structure.MetaInfoList )
                            {
                                if( byteOffset < s.Position || byteOffset >= s.Position + s.Size )
                                    continue;

                                var y1 = (int)lineyOffset; 
                                var y2 = (int)(lineyOffset + extraLineOffset);
                                var x1 = (int)(byteColumnOffset + hexByte * ByteWidth);
                                var x2 = (int)(byteColumnOffset + (hexByte + 1) * ByteWidth);
                                var p = new Pen( new SolidBrush( s.Color ) );

                                if( s.Tag is UStruct.UByteCodeDecompiler.Token )
                                {
                                    e.Graphics.DrawLine( p, x1, y2 - 4, x2, y2 - 4 );
                                }
                                else
                                {
                                    var rectBrush = new SolidBrush( Color.FromArgb( 128, s.Color.R, s.Color.G, s.Color.B ) );
                                    e.Graphics.FillRectangle( rectBrush, x1, y1, ByteWidth, extraLineOffset );
                                    if( HoveredOffset >= s.Position && HoveredOffset < s.Position + s.Size )
                                    {
                                        var borderPen = new Pen( new SolidBrush( Color.FromArgb( 60, 20, 20, 20 ) ) );
                                        e.Graphics.DrawLine( borderPen, x1, y1, x2, y1 );		// Top	
                                        e.Graphics.DrawLine( borderPen, x1, y2, x2, y2 );		// Bottom

                                        if( byteOffset == s.Position )
                                            e.Graphics.DrawLine( borderPen, x1, y1, x1, y2 );	// Left

                                        if( byteOffset == s.Position + s.Size - 1 )
                                            e.Graphics.DrawLine( borderPen, x2, y1, x2, y2 );	// Right
                                    }
                                }
                            }

                            e.Graphics.DrawString( drawntext, HexLinePanel.Font, drawbrush, 
                                byteColumnOffset + (hexByte * ByteWidth), lineyOffset 
                            );

                            if( byteOffset == SelectedOffset )
                            {
                                // Draw the selection.
                                var drawPen = new Pen( new SolidBrush( Color.Teal ) );
                                e.Graphics.DrawRectangle( drawPen, new Rectangle(
                                    (int)(byteColumnOffset + (hexByte * ByteWidth)),
                                    (int)(lineyOffset),
                                    (int)(ByteWidth),
                                    (int)(_LineSpacing)
                                ));  
                            }

                            if( byteOffset == HoveredOffset )
                            {
                                var drawPen = new Pen( new SolidBrush( Color.FromArgb( 128, 0x22, 0x22, 0x22 ) ) );
                                e.Graphics.DrawRectangle( drawPen, new Rectangle(
                                    (int)(byteColumnOffset + (hexByte * ByteWidth)),
                                    (int)(lineyOffset),
                                    (int)(ByteWidth),
                                    (int)(_LineSpacing)
                                ));	
                            }
                        }				
                    }
                }

                if( _DrawASCII )
                {
                    for( int hexByte = 0; hexByte < ByteCount; ++ hexByte )
                    {
                        int byteOffset = offset + hexByte;
                        if( byteOffset < Buffer.Length )
                        {

                            if( byteOffset == SelectedOffset )
                            {
                                // Draw the selection.
                                var drawPen = new Pen( new SolidBrush( Color.Teal ) );
                                e.Graphics.DrawRectangle( drawPen, new Rectangle(
                                    (int)(asciiColumnOffset + (hexByte * asciiWidth)),
                                    (int)(lineyOffset),
                                    (int)(ByteWidth),
                                    (int)(_LineSpacing)
                                ));    
                            }
                            
                            if( byteOffset == HoveredOffset )
                            {
                                var drawPen = new Pen( new SolidBrush( Color.FromArgb( 128, 0x22, 0x22, 0x22 ) ) );
                                e.Graphics.DrawRectangle( drawPen, new Rectangle(
                                    (int)(asciiColumnOffset + (hexByte * asciiWidth)),
                                    (int)(lineyOffset),
                                    (int)(ByteWidth),
                                    (int)(_LineSpacing)
                                ));	
                            }

                            string drawnChar;
                            switch( Buffer[byteOffset] )
                            {
                                case 0x09:
                                    drawnChar = "\\t";
                                    break;

                                case 0x0A:
                                    drawnChar = "\\n";
                                    break;

                                case 0x0D:
                                    drawnChar = "\\r";
                                    break;

                                default:
                                    drawnChar = FilterByte( Buffer[byteOffset] ).ToString( CultureInfo.InvariantCulture );
                                    break;
                            }

                            e.Graphics.DrawString( 
                                drawnChar, HexLinePanel.Font, brush, 
                                asciiColumnOffset + hexByte*asciiWidth, 
                                lineyOffset 
                            );
                        }			
                    }
                }
                offset += ByteCount;
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

        [DefaultValue( -1 )]
        private int HoveredOffset{ get; set; }

        private void OffsetChanged()
        {
            HexLinePanel.Invalidate();
            if( SelectedOffset == -1 )
                return;

            DissambledObject.Text = String.Empty;
            DissambledName.Text = String.Empty;

            var hexViewDialog = (HexViewDialog)ParentForm;
            if( hexViewDialog != null )
            {
                hexViewDialog.ToolStripStatusLabel_Position.Text = String.Format
                ( 
                    Resources.HexView_Position, 
                    Target.GetBufferPosition(), 
                    SelectedOffset,
                    Target.GetBufferPosition()
                );
            }
            var bufferSelection = new byte[8];
            for( int i = 0; SelectedOffset + i < Buffer.Length && i < 8; ++ i )
            {
                bufferSelection[i] = Buffer[SelectedOffset + i];
            }

            DissambledChar.Text		=	((char)(bufferSelection[0])).ToString( CultureInfo.InvariantCulture );
            DissambledByte.Text		=	bufferSelection[0].ToString( CultureInfo.InvariantCulture );
            DissambledShort.Text	=	BitConverter.ToInt16( bufferSelection, 0 ).ToString( CultureInfo.InvariantCulture );
            DissambledUShort.Text	=	BitConverter.ToUInt16( bufferSelection, 0 ).ToString( CultureInfo.InvariantCulture );
            DissambledInt.Text		=	BitConverter.ToInt32( bufferSelection, 0 ).ToString( CultureInfo.InvariantCulture );
            DissambledUInt.Text		=	BitConverter.ToUInt32( bufferSelection, 0 ).ToString( CultureInfo.InvariantCulture );
            DissambledFloat.Text	=	BitConverter.ToSingle( bufferSelection, 0 ).ToString( CultureInfo.InvariantCulture );
            DissambledLong.Text		=	BitConverter.ToInt64( bufferSelection, 0 ).ToString( CultureInfo.InvariantCulture );
            DissambledULong.Text	=	BitConverter.ToUInt64( bufferSelection, 0 ).ToString( CultureInfo.InvariantCulture );

            try
            {
                var index = UnrealReader.ReadIndexFromBuffer( bufferSelection, Target.GetBuffer() );
                DissambledIndex.Text = index.ToString( CultureInfo.InvariantCulture );
            }
            catch
            {
                DissambledIndex.Text = Resources.NOT_AVAILABLE;
            }

            try
            {
                var obj = Target.GetBuffer().ParseObject( UnrealReader.ReadIndexFromBuffer( bufferSelection, Target.GetBuffer() ) );
                DissambledObject.Text = obj == null ? Resources.NOT_AVAILABLE : obj.GetOuterGroup();
            }
            catch
            {
                DissambledObject.Text = Resources.NOT_AVAILABLE;
            }

            try
            {
                DissambledName.Text = Target.GetBuffer().ParseName( UnrealReader.ReadIndexFromBuffer( bufferSelection, Target.GetBuffer() ) );
            }
            catch
            {
                DissambledName.Text = Resources.NOT_AVAILABLE;
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

        private void HexScrollBar_Scroll( object sender, ScrollEventArgs e )
        {
            switch( e.Type )
            {
                case ScrollEventType.SmallDecrement:
                    SelectedOffset = e.ScrollOrientation == ScrollOrientation.VerticalScroll
                        ? Math.Max( SelectedOffset - ByteCount, 0 )
                        : Math.Max( SelectedOffset - 1, 0 );
                    break;

                case ScrollEventType.SmallIncrement:
                    SelectedOffset = e.ScrollOrientation == ScrollOrientation.VerticalScroll
                        ? Math.Min( SelectedOffset + ByteCount, Buffer.Length - 1 )
                        : Math.Min( SelectedOffset + 1, Buffer.Length - 1 );
                    break;
            }
            HexLinePanel.Invalidate();
        }

        private void HexLinePanel_MouseClick( object sender, MouseEventArgs e )
        {
            if( Buffer == null )
            {
                return;
            }
            SelectedOffset = GetHoveredByte( e );
            HexScrollBar.Focus();
        }

        private int GetHoveredByte( MouseEventArgs e )
        {
            float x = e.X - HexLinePanel.Location.X;
            float y = e.Y - HexLinePanel.Location.Y;

            int offset = (ByteCount * HexScrollBar.Value);
            int lineCount = Math.Min( (int)(HexLinePanel.ClientSize.Height / _LineSpacing), 
                (Buffer.Length - offset) / ByteCount + 
                (((Buffer.Length - offset) % ByteCount) > 0 ? 1 : 0) 
            );
        
            float lineyoffset = HexLinePanel.Font.Height;
            float byteoffset = _DrawByte ? 74 : 0;
            float charoffset = byteoffset == 0 ? 74 : byteoffset + ColumnSize + ColumnMargin;

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
                    offset += ByteCount;
                    lineyoffset += extraLineOffset;
                    continue;
                }

                // Check if the bytes field is selected.
                if( _DrawByte && x >= byteoffset && x < charoffset )
                {
                    for( int hexByte = 0; hexByte < ByteCount; ++ hexByte )
                    {
                        int byteOffset = (offset + hexByte);
                        if( byteOffset < Buffer.Length )
                        {							
                            if
                            ( 
                                x >= (byteoffset + (hexByte * ByteWidth)) && x <= (byteoffset + ((hexByte + 1) * ByteWidth)) 
                            )
                            {
                                return byteOffset;
                            }
                        }				
                    }
                }

                // Check if the ascii's field is selected.
                if( _DrawASCII && x >= charoffset )
                {
                    const float asciiWidth = ByteWidth;
                    for( int hexByte = 0; hexByte < ByteCount; ++ hexByte )
                    {
                        int byteOffset = (offset + hexByte);
                        if( byteOffset < Buffer.Length )
                        {
                            if
                            (
                                x >= (charoffset + (hexByte * asciiWidth)) && x <= (charoffset + ((hexByte + 1) * asciiWidth))
                            )
                            {
                                return byteOffset;
                            }
                        }
                    }
                }
                offset += ByteCount;
                lineyoffset += extraLineOffset;
            }
            return -1;
        }

        private void Context_Structure_ItemClicked( object sender, ToolStripItemClickedEventArgs e )
        {
            var dialog = new StructureInputDialog();
            var type = e.ClickedItem.Text.Mid( e.ClickedItem.Text.LastIndexOf( ' ' ) + 1 );
            dialog.TextBoxName.Text = type;
            if( dialog.ShowDialog() == DialogResult.OK )
            {
                if( dialog.TextBoxName.Text == String.Empty )
                {
                    // Show error box?
                    return;
                }

                byte size;
                Color color;
                InitStructure( type, out size, out color );	
                _Structure.MetaInfoList.Add
                ( 
                    new HexMetaInfo.BytesMetaInfo
                    {
                        Name = dialog.TextBoxName.Text, 
                        Position = SelectedOffset, 
                        Size = size, 
                        Type = type,
                        Color = color
                    } 
                );

                var path = GetConfigPath();
                SaveConfig( path );

                HexLinePanel.Invalidate();
            }
        }

        private void HexLinePanel_MouseMove( object sender, MouseEventArgs e )
        {
            var lastHoveredOffset = HoveredOffset;
            HoveredOffset = GetHoveredByte( e );

            if( lastHoveredOffset != HoveredOffset )
            {
                HexLinePanel.Invalidate();
                if( HoveredOffset != -1 )
                {
                    var dataStruct = _Structure.MetaInfoList.Find( 
                        i => HoveredOffset >= i.Position && HoveredOffset < i.Position + i.Size  
                    );

                    if( dataStruct == null )
                    {
                        HexToolTip.Hide( this );
                        return;
                    }

                    var toolTipPoint = PointToClient( MousePosition );
                    var message = dataStruct.Name;
                    if( dataStruct.Tag != null )
                    {
                        try
                        {
                            // Restart token index.
                            var token = dataStruct.Tag as UStruct.UByteCodeDecompiler.Token;
                            if( token != null )
                            {
                                token.Decompiler.JumpTo( (ushort)token.Position );
                            }
                            message += "\r\n\r\n" + dataStruct.Tag.Decompile();
                        }
                        catch
                        {
                            message += "\r\n\r\n" + Resources.HexView_COULDNT_ACQUIRE_VALUE;
                        }
                    }
                    HexToolTip.Show( message, this,
                        toolTipPoint.X + (int)(Cursor.Size.Width*0.5f),
                        toolTipPoint.Y + (int)(Cursor.Size.Height*0.5f),
                        4000
                    );
                }
                else
                {
                    HexToolTip.Hide( this );
                }
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