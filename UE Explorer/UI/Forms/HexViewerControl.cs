using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using Eliot.Utilities;
using UEExplorer.UI.Dialogs;

namespace UEExplorer.UI.Forms
{
    using Properties;
    using UELib;
    using UStruct = UELib.Core.UStruct;

    public partial class HexViewerControl : UserControl
    {
        public byte[] Buffer { get; set; }
        public IBuffered Target { get; private set; }

        public class HexMetaInfo
        {
            public class BytesMetaInfo
            {
                public int Position;

                [XmlIgnore] public int Size;

                [XmlIgnore] public int HoverSize;

                public string Type;

                public string Name;

                [XmlIgnore] public Color Color;

                [XmlIgnore] public IUnrealDecompilable Tag;
            }

            public List<BytesMetaInfo> MetaInfoList;
        }

        private HexMetaInfo _Structure;

        #region View Properties

        private readonly char[] _Hexadecimal =
            { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };

        private const int CellCount = 16;
        private const float CellWidth = 18;
        private float CellHeight => HexLinePanel.Font.Height;
        private const float ColumnWidth = CellCount * CellWidth;
        private const float ColumnMargin = 12;

        private bool _DrawASCII = true;

        public bool DrawASCII
        {
            get => _DrawASCII;
            set
            {
                _DrawASCII = value;
                HexLinePanel.Invalidate();
            }
        }

        private bool _DrawByte = true;

        public bool DrawByte
        {
            get => _DrawByte;
            set
            {
                _DrawByte = value;
                HexLinePanel.Invalidate();
            }
        }

        #endregion

        public HexViewerControl()
        {
            InitializeComponent();
        }

        private void LoadConfig(string path)
        {
            using (var r = new XmlTextReader(path))
            {
                var xser = new XmlSerializer(typeof(HexMetaInfo));
                _Structure = (HexMetaInfo)xser.Deserialize(r);

                foreach (var s in _Structure.MetaInfoList.Where(s => s.Type != "Generated"))
                {
                    byte size;
                    Color color;

                    InitStructure(s.Type, out size, out color);
                    s.Size = size;
                    s.Color = color;
                }

                HexLinePanel.Invalidate();
            }
        }

        private void InitStructure(string type, out byte size, out Color color)
        {
            switch (type.ToLower())
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

        private void SaveConfig(string path)
        {
            if (!Directory.Exists(Path.GetDirectoryName(path))) Directory.CreateDirectory(Path.GetDirectoryName(path));

            var backupInfo = _Structure.MetaInfoList.ToArray();
            _Structure.MetaInfoList.RemoveAll(i => i.Type == "Generated");
            using (var w = new XmlTextWriter(path, System.Text.Encoding.ASCII))
            {
                var xser = new XmlSerializer(typeof(HexMetaInfo));
                xser.Serialize(w, _Structure);
            }

            _Structure.MetaInfoList.AddRange(backupInfo);
        }

        public void SetHexData(IBuffered target)
        {
            if (target == null)
                return;

            Target = target;
            Buffer = Target.CopyBuffer();
            UpdateScrollBar();

            string path = GetConfigPath();
            if (File.Exists(path))
                LoadConfig(path);
            else
                _Structure = new HexMetaInfo { MetaInfoList = new List<HexMetaInfo.BytesMetaInfo>() };

            InitializeMetaInfoFields();
        }

        private void UpdateScrollBar()
        {
            if (Buffer == null)
                return;

            double totalLines = Math.Ceiling(Buffer.Length / (float)CellCount) + 2;
            double visibleLines = Math.Ceiling(HexLinePanel.Height / CellHeight);
            double trailingLines = totalLines % visibleLines;
            double scrollableLines = totalLines - trailingLines;

            HexScrollBar.Minimum = 0;
            HexScrollBar.Maximum = (int)Math.Max(totalLines, 0);
            HexScrollBar.Visible = totalLines > 0 && totalLines > visibleLines;
        }

        /// <summary>
        /// Attempts to update its state to the current's target state.
        /// </summary>
        public void Reload()
        {
            _Structure.MetaInfoList.Clear();
            InitializeMetaInfoFields();
        }

        private void InitializeMetaInfoFields()
        {
            if (Target is IBinaryData binaryTarget && binaryTarget.BinaryMetaData != null)
            {
                foreach (var binaryField in binaryTarget.BinaryMetaData.Fields)
                {
                    int colorHash = binaryField.Field.GetHashCode()
                                     ^ (int)binaryField.Size
                                     ^ (binaryField.Value?.GetType().GetHashCode() ?? 1);
                    _Structure.MetaInfoList.Add
                    (
                        new HexMetaInfo.BytesMetaInfo
                        {
                            Position = (int)binaryField.Offset,
                            Size = (int)binaryField.Size,
                            Type = "Generated",
                            Color = NormalizeArgbToColor(colorHash),
                            Name = binaryField.Field,
                            Tag = binaryField
                        }
                    );
                }
            }

            if (!(Target is UStruct unStruct))
                return;

            if (unStruct.ByteCodeManager?.DeserializedTokens == null ||
                unStruct.ByteCodeManager.DeserializedTokens.Count <= 0)
                return;

            foreach (var token in unStruct.ByteCodeManager.DeserializedTokens)
            {
                _Structure.MetaInfoList.Add
                (
                    new HexMetaInfo.BytesMetaInfo
                    {
                        Position = token.StoragePosition + (int)unStruct.ScriptOffset,
                        Size = 1,
                        HoverSize = token.StorageSize,
                        Type = "Generated",
                        Color = NormalizeArgbToColor(token.GetHashCode()),
                        Name = token.GetType().Name,
                        Tag = token
                    }
                );
            }
        }

        private Color NormalizeArgbToColor(int argb)
        {
            const float min = 68f;
            const float max = 255 - min;
            var r = (int)(((argb >> 16) & byte.MaxValue) / 255f * max + min);
            var g = (int)(((argb >> 8) & byte.MaxValue) / 255f * max + min);
            var b = (int)((argb & byte.MaxValue) / 255f * max + min);
            return Color.FromArgb(60, r , g, b);
        }

        private readonly string _ConfigPath =
            Path.Combine(Application.StartupPath, "DataStructures", "{0}", "{1}") + ".xml";

        private string GetConfigPath()
        {
            string folderName = Path.GetFileNameWithoutExtension(Target.GetBufferId(true));
            return string.Format(_ConfigPath, folderName, Target.GetBufferId());
        }

        private readonly SolidBrush _BorderBrush = new SolidBrush(Color.FromArgb(237, 237, 237));
        private readonly SolidBrush _UnderlineBrush = new SolidBrush(Color.FromArgb(0x55EDEDED));
        private readonly SolidBrush _EvenBrush = new SolidBrush(Color.FromArgb(80, 80, 80));
        private readonly SolidBrush _EvenLitBrush = new SolidBrush(Color.FromArgb(112, 32, 32));
        private readonly SolidBrush _OddBrush = new SolidBrush(Color.FromArgb(150, 150, 150));
        private readonly SolidBrush _OddLitBrush = new SolidBrush(Color.FromArgb(182, 102, 102));
        private readonly SolidBrush _OffsetBrush = new SolidBrush(Color.FromArgb(160, 160, 160));
        private readonly SolidBrush _SelectedBrush = new SolidBrush(Color.FromArgb(unchecked((int)0x880000FF)));
        private readonly SolidBrush _HoveredBrush = new SolidBrush(Color.FromArgb(unchecked((int)0x880088FF)));
        private readonly Pen _BorderHoverPen = new Pen(Color.FromArgb(unchecked((int)0x88000000)));
        
        private void HexLinePanel_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(_BorderBrush), 0f, 0f, HexLinePanel.Width - 1, HexLinePanel.Height - 1);
            if (Buffer == null)
                return;

            //e.Graphics.PageUnit = GraphicsUnit.Pixel;
            int offset = CellCount * HexScrollBar.Value;
            int lineCount = Math.Min(
                (int)(HexLinePanel.ClientSize.Height / CellHeight),
                (Buffer.Length - offset) / CellCount + ((Buffer.Length - offset) % CellCount > 0 ? 1 : 0)
            );

            float lineOffsetY = CellHeight;
            float byteColumnOffset = _DrawByte ? 74 : 0;
            float asciiColumnOffset = byteColumnOffset == 0 ? 74 : byteColumnOffset + ColumnWidth + ColumnMargin;

            Brush foreTextBrush = new SolidBrush(ForeColor);

            string text = Resources.HexView_Offset;
            var textLength = e.Graphics.MeasureString(text, HexLinePanel.Font,
                new PointF(0, 0), StringFormat.GenericTypographic
            );
            e.Graphics.DrawString(text, HexLinePanel.Font, foreTextBrush,
                2,
                textLength.Height * 0.5f,
                StringFormat.GenericTypographic
            );
            e.Graphics.DrawLine(new Pen(_UnderlineBrush),
                2, CellHeight + HexLinePanel.Font.Height * .5f,
                2 + byteColumnOffset - CellWidth * .5f, CellHeight + HexLinePanel.Font.Height * .5f
            );

            if (_DrawByte)
            {
                float x = byteColumnOffset;
                float y = 0;

                e.Graphics.DrawLine(new Pen(_UnderlineBrush),
                    x, y + CellHeight + HexLinePanel.Font.Height * .5f,
                    x + ColumnWidth, y + CellHeight + HexLinePanel.Font.Height * .5f
                );

                for (var i = 0; i < CellCount; ++i)
                {
                    var textBrush = SelectedOffset % CellCount == i ? _SelectedBrush
                        : HoveredOffset % CellCount == i ? _HoveredBrush
                        : i / 4.0F / 1.00 % 2.00 < 1.00 ? _EvenBrush : _OffsetBrush;
                    string c = "0" + _Hexadecimal[i].ToString(CultureInfo.InvariantCulture);
                    var cs = e.Graphics.MeasureString(c, HexLinePanel.Font, PointF.Empty,
                        StringFormat.GenericTypographic);
                    e.Graphics.DrawString(c, HexLinePanel.Font, textBrush,
                        x + i * CellWidth + 2,
                        y + cs.Height * 0.5f,
                        StringFormat.GenericTypographic
                    );
                }
            }

            const float asciiWidth = CellWidth;
            if (_DrawASCII)
            {
                float x = asciiColumnOffset;
                float y = 0;

                e.Graphics.DrawLine(new Pen(_UnderlineBrush),
                    x, y + CellHeight + HexLinePanel.Font.Height * .5f, x + CellCount * asciiWidth,
                    y + CellHeight + HexLinePanel.Font.Height * .5f
                );

                for (var i = 0; i < CellCount; ++i)
                {
                    bool isOddGroup = i / 4.0F / 1.00 % 2.00 < 1.00;
                    var textBrush = SelectedOffset % CellCount == i ? _SelectedBrush
                        : HoveredOffset % CellCount == i ? _HoveredBrush
                        : isOddGroup ? _EvenBrush : _OffsetBrush;
                    string c = "0" + _Hexadecimal[i].ToString(CultureInfo.InvariantCulture);
                    var cs = e.Graphics.MeasureString(c, HexLinePanel.Font, PointF.Empty,
                        StringFormat.GenericTypographic);
                    e.Graphics.DrawString(c, HexLinePanel.Font, textBrush,
                        x + i * asciiWidth + 2,
                        y + cs.Height * 0.5f,
                        StringFormat.GenericTypographic
                    );
                }
            }

            lineOffsetY += HexLinePanel.Font.Height;
            float extraLineOffset = CellHeight;
            for (var line = 0; line < lineCount; ++line)
            {
                if (lineOffsetY >= HexLinePanel.ClientSize.Height) break;

                var textBrush = line % 2 == 0 ? _EvenBrush : _OddBrush;
                var litTextBrush = line % 2 == 0 ? _EvenLitBrush : _OddLitBrush;

                bool lineIsSelected = offset <= SelectedOffset && offset + CellCount > SelectedOffset;
                bool lineIsHovered = offset <= HoveredOffset && offset + CellCount > HoveredOffset;
                var lineBrush = lineIsSelected ? _SelectedBrush : lineIsHovered ? _HoveredBrush : textBrush;

                if (lineIsSelected)
                {
                    e.Graphics.DrawLine
                    (
                        new Pen(lineBrush),
                        0, lineOffsetY + extraLineOffset,
                        (SelectedOffset - offset) * CellWidth + byteColumnOffset, lineOffsetY + extraLineOffset
                    );
                }

                if (lineIsHovered)
                {
                    e.Graphics.DrawLine
                    (
                        new Pen(lineBrush),
                        0, lineOffsetY + extraLineOffset,
                        (HoveredOffset - offset) * CellWidth + byteColumnOffset, lineOffsetY + extraLineOffset
                    );
                }

                string lineText = $"{offset:x8}".PadLeft(8, '0').ToUpper();
                e.Graphics.DrawString(lineText, HexLinePanel.Font, lineBrush, 0, lineOffsetY);

                if (_DrawByte)
                {
                    var metaRectBrush = new SolidBrush(Color.CadetBlue);
                    for (var hexByte = 0; hexByte < CellCount; ++hexByte)
                    {
                        int byteOffset = offset + hexByte;
                        if (byteOffset >= Buffer.Length) continue;
                        
                        var cellBrush = hexByte % 4 == 0
                            ? litTextBrush
                            : textBrush;
                        var hexText = $"{Buffer[byteOffset]:X2}";

                        var y1 = (int)lineOffsetY;
                        var y2 = (int)(lineOffsetY + extraLineOffset);
                        var x1 = (int)(byteColumnOffset + hexByte * CellWidth);
                        var x2 = (int)(byteColumnOffset + (hexByte + 1) * CellWidth);

                        foreach (var meta in _Structure.MetaInfoList)
                        {
                            if (byteOffset < meta.Position) continue;
                                
                            int drawSize = meta.HoverSize > 0 && (meta.Position == HoveredOffset || meta.Position == SelectedOffset)
                                ? meta.HoverSize 
                                : meta.Size;
                            if (byteOffset >= meta.Position + drawSize) continue;

                            float cellHeight = extraLineOffset;
                            var cellRectangleY = (float)y1;
                            if (meta.Tag is UStruct.UByteCodeDecompiler.Token)
                            {
                                cellHeight *= 0.5F;
                                cellRectangleY = y1 + (y2 - y1) * 0.5F - cellHeight * 0.5F;
                            }

                            metaRectBrush.Color = meta.Color;
                            e.Graphics.FillRectangle(metaRectBrush, x1, cellRectangleY, CellWidth, cellHeight);
                            if (HoveredOffset >= meta.Position && HoveredOffset < meta.Position + drawSize)
                            {
                                e.Graphics.DrawLine(_BorderHoverPen, x1, y1, x2, y1); // Top	
                                e.Graphics.DrawLine(_BorderHoverPen, x1, y2, x2, y2); // Bottom

                                if (byteOffset == meta.Position)
                                    e.Graphics.DrawLine(_BorderHoverPen, x1, y1, x1, y2); // Left

                                if (byteOffset == meta.Position + drawSize - 1)
                                    e.Graphics.DrawLine(_BorderHoverPen, x2, y1, x2, y2); // Right
                            }
                        }

                        // Render edit carret.
                        if (byteOffset == _ActiveOffset)
                        {
                            e.Graphics.FillRectangle(cellBrush, new Rectangle(
                                x1, y1,
                                (int)CellWidth, (int)CellHeight
                            ));
                            //if( (DateTime.Now - _CarretStartTime).TotalMilliseconds % 600 < 500 )
                            //{
                            float nibbleWidth = (x2 - x1) * 0.5F;
                            switch (_ActiveNibbleIndex)
                            {
                                case 0:
                                    e.Graphics.DrawLine(new Pen(
                                            new SolidBrush(Color.FromArgb(unchecked((int)0xEE000000))),
                                            nibbleWidth
                                        ),
                                        x1 + 1 + nibbleWidth * 0.5F, y1, x1 + 1 + nibbleWidth * 0.5F, y2
                                    );
                                    break;

                                case 1:
                                    e.Graphics.DrawLine(new Pen(
                                            new SolidBrush(Color.FromArgb(unchecked((int)0xEE000000))),
                                            nibbleWidth
                                        ),
                                        x1 + nibbleWidth + nibbleWidth * 0.5F, y1,
                                        x1 + nibbleWidth + nibbleWidth * 0.5F, y2
                                    );
                                    break;
                            }

                            cellBrush = new SolidBrush(Color.White);
                            //}
                        }

                        e.Graphics.DrawString(hexText, HexLinePanel.Font, cellBrush,
                            byteColumnOffset + hexByte * CellWidth, lineOffsetY
                        );

                        if (byteOffset == SelectedOffset)
                        {
                            // Draw the selection.
                            var drawPen = new Pen(_SelectedBrush);
                            e.Graphics.DrawRectangle(drawPen, new Rectangle(
                                (int)(byteColumnOffset + hexByte * CellWidth),
                                (int)lineOffsetY,
                                (int)CellWidth,
                                (int)CellHeight
                            ));
                        }

                        if (byteOffset == HoveredOffset)
                        {
                            var drawPen = new Pen(_HoveredBrush);
                            e.Graphics.DrawRectangle(drawPen, new Rectangle(
                                (int)(byteColumnOffset + hexByte * CellWidth),
                                (int)lineOffsetY,
                                (int)CellWidth,
                                (int)CellHeight
                            ));
                        }
                    }
                }

                if (_DrawASCII)
                {
                    for (var hexByte = 0; hexByte < CellCount; ++hexByte)
                    {
                        int byteOffset = offset + hexByte;
                        if (byteOffset >= Buffer.Length) continue;
                        
                        var cellBrush = hexByte % 4 == 0
                            ? litTextBrush
                            : textBrush;
                        if (byteOffset == SelectedOffset)
                        {
                            // Draw the selection.
                            var drawPen = new Pen(_SelectedBrush);
                            e.Graphics.DrawRectangle(drawPen, new Rectangle(
                                (int)(asciiColumnOffset + hexByte * asciiWidth),
                                (int)lineOffsetY,
                                (int)CellWidth,
                                (int)CellHeight
                            ));
                        }

                        if (byteOffset == HoveredOffset)
                        {
                            var drawPen = new Pen(_HoveredBrush);
                            e.Graphics.DrawRectangle(drawPen, new Rectangle(
                                (int)(asciiColumnOffset + hexByte * asciiWidth),
                                (int)lineOffsetY,
                                (int)CellWidth,
                                (int)CellHeight
                            ));
                        }

                        string drawnChar;
                        switch (Buffer[byteOffset])
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
                                drawnChar = FilterByte(Buffer[byteOffset]).ToString(CultureInfo.InvariantCulture);
                                break;
                        }

                        e.Graphics.DrawString(
                            drawnChar, HexLinePanel.Font, cellBrush,
                            asciiColumnOffset + hexByte * asciiWidth,
                            lineOffsetY
                        );
                    }
                }

                offset += CellCount;
                lineOffsetY += extraLineOffset;
            }
        }

        internal static char FilterByte(byte code)
        {
            if (code >= 0x20 && code <= 0x7E) return (char)code;
            return '.';
        }

        /// <summary>
        /// Editing byte's buffer index.
        /// </summary>
        private int _ActiveOffset = -1;

        private int _ActiveNibbleIndex;
        private DateTime _CarretStartTime;

        private int _SelectedOffset = -1;

        private int SelectedOffset
        {
            get => _SelectedOffset;
            set
            {
                _SelectedOffset = value;
                OffsetChanged();
            }
        }

        [DefaultValue(-1)] private int HoveredOffset { get; set; }

        public delegate void OffsetChangedEventHandler(int selectedOffset);

        public event OffsetChangedEventHandler OffsetChangedEvent = null;

        private void OnOffsetChangedEvent()
        {
            if (OffsetChangedEvent != null) OffsetChangedEvent.Invoke(SelectedOffset);
        }

        private void OffsetChanged()
        {
            HexLinePanel.Invalidate();
            if (SelectedOffset == -1)
                return;

            DissambledObject.Text = string.Empty;
            DissambledName.Text = string.Empty;

            var bufferSelection = new byte[8];
            for (var i = 0; SelectedOffset + i < Buffer.Length && i < 8; ++i)
                bufferSelection[i] = Buffer[SelectedOffset + i];

            DissambledChar.Text = ((char)bufferSelection[0]).ToString(CultureInfo.InvariantCulture);
            DissambledByte.Text = bufferSelection[0].ToString(CultureInfo.InvariantCulture);
            DissambledShort.Text = BitConverter.ToInt16(bufferSelection, 0).ToString(CultureInfo.InvariantCulture);
            DissambledUShort.Text = BitConverter.ToUInt16(bufferSelection, 0).ToString(CultureInfo.InvariantCulture);
            DissambledInt.Text = BitConverter.ToInt32(bufferSelection, 0).ToString(CultureInfo.InvariantCulture);
            DissambledUInt.Text = BitConverter.ToUInt32(bufferSelection, 0).ToString(CultureInfo.InvariantCulture);
            DissambledFloat.Text = BitConverter.ToSingle(bufferSelection, 0).ToString(CultureInfo.InvariantCulture);
            DissambledLong.Text = BitConverter.ToInt64(bufferSelection, 0).ToString(CultureInfo.InvariantCulture);
            DissambledULong.Text = BitConverter.ToUInt64(bufferSelection, 0).ToString(CultureInfo.InvariantCulture);

            try
            {
                int index = UnrealReader.ReadIndexFromBuffer(bufferSelection, Target.GetBuffer());
                DissambledIndex.Text = index.ToString(CultureInfo.InvariantCulture);
            }
            catch
            {
                DissambledIndex.Text = Resources.NOT_AVAILABLE;
            }

            try
            {
                var obj = Target.GetBuffer()
                    .ParseObject(UnrealReader.ReadIndexFromBuffer(bufferSelection, Target.GetBuffer()));
                DissambledObject.Text = obj == null ? Resources.NOT_AVAILABLE : obj.GetPath();
            }
            catch
            {
                DissambledObject.Text = Resources.NOT_AVAILABLE;
            }

            try
            {
                DissambledName.Text = Target.GetBuffer()
                    .ParseName(UnrealReader.ReadIndexFromBuffer(bufferSelection, Target.GetBuffer()));
            }
            catch
            {
                DissambledName.Text = Resources.NOT_AVAILABLE;
            }

            DissambledStruct.Text = string.Empty;
            foreach (var s in _Structure.MetaInfoList)
            {
                if (SelectedOffset >= s.Position && SelectedOffset < s.Position + s.Size)
                    DissambledStruct.Text = s.Name;
            }

            OnOffsetChangedEvent();
        }

        private void HexScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            if (_LastKeyWasLeft)
            {
                SelectedOffset = Math.Max(SelectedOffset - 1, 0);
                e.NewValue = e.OldValue;
                _LastKeyWasLeft = false;
            }
            else if (_LastKeyWasRight)
            {
                SelectedOffset = Math.Min(SelectedOffset + 1, Buffer.Length - 1);
                e.NewValue = e.OldValue;
                _LastKeyWasRight = false;
            }
            else
            {
                switch (e.Type)
                {
                    case ScrollEventType.SmallDecrement:
                        SelectedOffset = e.ScrollOrientation == ScrollOrientation.VerticalScroll
                            ? Math.Max(SelectedOffset - CellCount, 0)
                            : Math.Max(SelectedOffset - 1, 0);
                        break;

                    case ScrollEventType.SmallIncrement:
                        SelectedOffset = e.ScrollOrientation == ScrollOrientation.VerticalScroll
                            ? Math.Min(SelectedOffset + CellCount, Buffer.Length - 1)
                            : Math.Min(SelectedOffset + 1, Buffer.Length - 1);
                        break;
                }
            }

            HexLinePanel.Invalidate();
        }

        private void HexLinePanel_MouseClick(object sender, MouseEventArgs e)
        {
            if (Buffer == null) return;
            //ActiveOffset = -1;

            SelectedOffset = GetHoveredByte(e);
            HexScrollBar.Focus();
            HexLinePanel.Invalidate();
        }

        private void HexLinePanel_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (Buffer == null) return;

            ActivateCell(GetHoveredByte(e));
        }

        private void ActivateCell(int index)
        {
            if (index == -1)
                return;

            _ActiveOffset = index;
            _ActiveNibbleIndex = 0;
            HexScrollBar.Focus();

            _CarretStartTime = DateTime.Now;
            HexLinePanel.Invalidate();
        }

        private int GetHoveredByte(MouseEventArgs e)
        {
            float x = e.X - HexLinePanel.Location.X;
            float y = e.Y - HexLinePanel.Location.Y;

            int offset = CellCount * HexScrollBar.Value;
            int lineCount = Math.Min((int)(HexLinePanel.ClientSize.Height / CellHeight),
                (Buffer.Length - offset) / CellCount +
                ((Buffer.Length - offset) % CellCount > 0 ? 1 : 0)
            );

            float lineyoffset = HexLinePanel.Font.Height;
            float byteoffset = _DrawByte ? 74 : 0;
            float charoffset = byteoffset == 0 ? 74 : byteoffset + ColumnWidth + ColumnMargin;

            float extraLineOffset = CellHeight;
            for (var line = 0; line < lineCount; ++line)
            {
                if (lineyoffset >= HexLinePanel.ClientSize.Height) break;

                // The user definitely didn't click on this line?, so skip!.
                if (!(y >= lineyoffset + extraLineOffset && y <= lineyoffset + extraLineOffset * 2))
                {
                    offset += CellCount;
                    lineyoffset += extraLineOffset;
                    continue;
                }

                // Check if the bytes field is selected.
                if (_DrawByte && x >= byteoffset && x < charoffset)
                {
                    for (var hexByte = 0; hexByte < CellCount; ++hexByte)
                    {
                        int byteOffset = offset + hexByte;
                        if (byteOffset < Buffer.Length)
                        {
                            if
                            (
                                x >= byteoffset + hexByte * CellWidth && x <= byteoffset + (hexByte + 1) * CellWidth
                            )
                                return byteOffset;
                        }
                    }
                }

                // Check if the ascii's field is selected.
                if (_DrawASCII && x >= charoffset)
                {
                    const float asciiWidth = CellWidth;
                    for (var hexByte = 0; hexByte < CellCount; ++hexByte)
                    {
                        int byteOffset = offset + hexByte;
                        if (byteOffset < Buffer.Length)
                        {
                            if
                            (
                                x >= charoffset + hexByte * asciiWidth && x <= charoffset + (hexByte + 1) * asciiWidth
                            )
                                return byteOffset;
                        }
                    }
                }

                offset += CellCount;
                lineyoffset += extraLineOffset;
            }

            return -1;
        }

        private void Context_Structure_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem == EditMenuItem)
            {
                ActivateCell(HoveredOffset != -1 ? HoveredOffset : SelectedOffset);
                return;
            }

            using (var dialog = new StructureInputDialog())
            {
                string type = e.ClickedItem.Text.Mid(e.ClickedItem.Text.LastIndexOf(' ') + 1);
                dialog.TextBoxName.Text = type;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    if (dialog.TextBoxName.Text == string.Empty)
                    {
                        // Show error box?
                        return;
                    }

                    byte size;
                    Color color;
                    InitStructure(type, out size, out color);
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

                    string path = GetConfigPath();
                    SaveConfig(path);

                    HexLinePanel.Invalidate();
                }
            }
        }

        private void HexLinePanel_MouseMove(object sender, MouseEventArgs e)
        {
            int lastHoveredOffset = HoveredOffset;
            HoveredOffset = GetHoveredByte(e);

            if (lastHoveredOffset != HoveredOffset)
            {
                HexLinePanel.Invalidate();
                if (HoveredOffset != -1)
                {
                    var dataStruct = _Structure.MetaInfoList.Find(
                        i => HoveredOffset >= i.Position && HoveredOffset < i.Position + i.Size
                    );

                    if (dataStruct == null)
                    {
                        HexToolTip.Hide(this);
                        return;
                    }

                    var toolTipPoint = PointToClient(MousePosition);
                    string message = dataStruct.Name;
                    if (dataStruct.Tag != null)
                    {
                        try
                        {
                            // Restart token index.
                            var token = dataStruct.Tag as UStruct.UByteCodeDecompiler.Token;
                            if (token != null) token.Decompiler.JumpTo((ushort)token.Position);
                            message += "\r\n\r\n" + dataStruct.Tag.Decompile();
                        }
                        catch
                        {
                            message += "\r\n\r\n" + Resources.HexView_COULDNT_ACQUIRE_VALUE;
                        }
                    }

                    HexToolTip.Show(message, this,
                        toolTipPoint.X + (int)(Cursor.Size.Width * 0.5f),
                        toolTipPoint.Y + (int)(Cursor.Size.Height * 0.5f),
                        4000
                    );
                }
                else
                    HexToolTip.Hide(this);
            }
        }

        private void DataInfoPanel_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(_BorderBrush), 0f, 0f, DataInfoPanel.Width - 1, DataInfoPanel.Height - 1);
        }

        private void SplitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            DataInfoPanel.Invalidate();
            HexLinePanel.Invalidate();
        }

        private void UserControl_HexView_Resize(object sender, EventArgs e)
        {
            DataInfoPanel.Invalidate();
            HexLinePanel.Invalidate();
            UpdateScrollBar();
        }

        public delegate void BufferModifiedEventHandler();

        public event BufferModifiedEventHandler BufferModifiedEvent = null;

        private void OnBufferModifiedEvent()
        {
            BufferModifiedEvent?.Invoke();
        }

        private bool _LastKeyWasLeft, _LastKeyWasRight;

        private void EditKeyDown(object sender, KeyEventArgs e)
        {
            // HACK: To determine the cause of increment and decrement events when scrolling.
            _LastKeyWasLeft = e.KeyCode == Keys.Left;
            _LastKeyWasRight = e.KeyCode == Keys.Right;
            if (_LastKeyWasLeft || _LastKeyWasRight) return;

            if (_ActiveOffset == -1)
                return;

            if (e.KeyCode == Keys.Return)
                _ActiveOffset = -1;
            else
            {
                if (e.KeyCode == Keys.Shift) return;

                int hexKeyIndex = HexKeyCodeToIndex(e.KeyCode);
                if (hexKeyIndex == -1)
                    return;

                _CarretStartTime = DateTime.Now;
                byte newByte = Buffer[_ActiveOffset];
                switch (_ActiveNibbleIndex)
                {
                    case 0:
                        newByte = (byte)((byte)(newByte & 0x0F) | (hexKeyIndex << 4));
                        Buffer[_ActiveOffset] = newByte;
                        _ActiveNibbleIndex = 1;
                        break;

                    case 1:
                        newByte = (byte)((byte)(newByte & 0xF0) | hexKeyIndex);
                        Buffer[_ActiveOffset] = newByte;
                        _ActiveOffset = Math.Min(_ActiveOffset + 1, Buffer.Length - 1);
                        _ActiveNibbleIndex = 0;
                        break;
                }

                OnBufferModifiedEvent();
            }

            HexLinePanel.Invalidate();
            e.SuppressKeyPress = true;
        }

        [DllImport("user32")]
        private static extern int MapVirtualKey(Keys uCode, int uMapType);

        private const int MAPVK_VK_TO_CHAR = 2;

        private static int HexKeyCodeToIndex(Keys keyCode)
        {
            int c = MapVirtualKey(keyCode, MAPVK_VK_TO_CHAR) & ~(1 << 31);
            if (c >= '0' && c <= '9') return c - '0';

            if (c >= 'A' && c <= 'F') return c - 'A' + 10;

            if (c >= 'a' && c <= 'f') return c - 'a' + 10;

            switch (c)
            {
                case 38: return 1;
                case 233: return 2;
                case 34: return 3;
                case 39: return 4;
                case 40: return 5;
                case 167: return 6;
                case 232: return 7;
                case 33: return 8;
                case 231: return 9;
                case 224: return 0;
            }

            return -1;
        }

        private void HexScrollBar_KeyDown(object sender, KeyEventArgs e)
        {
            EditKeyDown(sender, e);
        }
    }
}