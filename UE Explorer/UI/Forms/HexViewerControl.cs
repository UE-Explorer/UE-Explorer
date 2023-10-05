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

    // TODO: REFACTOR, and rewrite all of it to be more concise, less duplicational.
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

        private const int CellCount = 16;
        private const float CellPadding = 6;
        private float CellWidth => _HexLinePanel.Font.Height + CellPadding;
        private float CellHeight => _HexLinePanel.Font.Height;
        private float ColumnWidth => CellCount * CellWidth;
        private const float ColumnMargin = 8;

        private bool _DrawASCII = true;

        public bool DrawASCII
        {
            get { return _DrawASCII; }
            set
            {
                _DrawASCII = value;
                _HexLinePanel.Invalidate();
            }
        }

        private bool _DrawByte = true;

        public bool DrawByte
        {
            get { return _DrawByte; }
            set
            {
                _DrawByte = value;
                _HexLinePanel.Invalidate();
            }
        }

        #endregion

        [DllImport("user32.dll")]
        static extern int SendMessage(IntPtr hWnd, uint wMsg, IntPtr wParam, IntPtr lParam);

        public HexViewerControl()
        {
            InitializeComponent();

            _UnderlinePen = new Pen(_UnderlineBrush);

            _SelectionPen = new Pen(_SelectedBrush);
            _HoverPen = new Pen(_HoveredBrush);

            _LineSelectionPen = _SelectionPen;
            _LineHoverPen = _HoverPen;
            _ForeBrush = new SolidBrush(ForeColor);
            _WhiteForeBrush = new SolidBrush(Color.White);
            _ActiveNibbleBrush = new SolidBrush(Color.FromArgb(unchecked((int)0xEE000000)));

            _AddressSample = $"{99999999:x8}".PadLeft(8, '0').ToUpper();
            _MuteBrush = _EvenBrush;
            _BorderPen = new Pen(_BorderBrush);
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

                _HexLinePanel.Invalidate();
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
            if (!Directory.Exists(Path.GetDirectoryName(path)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }

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

            var path = GetConfigPath();
            if (File.Exists(path))
            {
                LoadConfig(path);
            }
            else
            {
                _Structure = new HexMetaInfo { MetaInfoList = new List<HexMetaInfo.BytesMetaInfo>() };
            }

            InitializeMetaInfoFields();
        }

        private void UpdateScrollBar()
        {
            if (Buffer == null)
                return;

            var totalLines = Math.Ceiling(Buffer.Length / (float)CellCount) + 2;
            var visibleLines = Math.Ceiling(_HexLinePanel.Height / CellHeight);
            var trailingLines = totalLines % visibleLines;
            var scrollableLines = totalLines - trailingLines;

            _HexViewScrollBar.Minimum = 0;
            _HexViewScrollBar.Maximum = (int)Math.Max(totalLines, 0);
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
#if DEBUG || BINARYMETADATA
            try
            {
                var binaryTarget = Target as IBinaryData;
                if (binaryTarget != null && binaryTarget.BinaryMetaData != null)
                {
                    var randomizer1 = new Random(binaryTarget.BinaryMetaData.Fields.Count);
                    foreach (var binaryField in binaryTarget.BinaryMetaData.Fields)
                    {
                        var red = randomizer1.Next(0x8F) | 70;
                        var green = randomizer1.Next(0x8F) | 70;
                        var blue = randomizer1.Next(0x8F) | 70;

                        _Structure.MetaInfoList.Add
                        (
                            new HexMetaInfo.BytesMetaInfo
                            {
                                Position = (int)binaryField.Position,
                                Size = (int)binaryField.Size,
                                Type = "Generated",
                                Color = Color.FromArgb(0x88, red, green, blue),
                                Name = binaryField.Name,
                                Tag = binaryField
                            }
                        );
                    }
                }
            }
            catch (Exception e)
            {
                ExceptionDialog.Show("Initializing binary fields", e);
            }
#endif
            if (!(Target is UStruct))
                return;

            var unStruct = Target as UStruct;
            if (unStruct.ByteCodeManager == null || unStruct.ByteCodeManager.DeserializedTokens == null ||
                unStruct.ByteCodeManager.DeserializedTokens.Count <= 0)
                return;

            var randomizer2 = new Random(unStruct.ByteCodeManager.DeserializedTokens.Count);
            foreach (var token in unStruct.ByteCodeManager.DeserializedTokens)
            {
                var red = randomizer2.Next(Byte.MaxValue);
                var green = randomizer2.Next(Byte.MaxValue);
                var blue = randomizer2.Next(Byte.MaxValue);

                _Structure.MetaInfoList.Add
                (
                    new HexMetaInfo.BytesMetaInfo
                    {
                        Position = (int)token.StoragePosition + (int)unStruct.ScriptOffset,
                        Size = 1,
                        HoverSize = token.StorageSize,
                        Type = "Generated",
                        Color = Color.FromArgb(Byte.MaxValue, red, green, blue),
                        Name = token.GetType().Name,
                        Tag = token
                    }
                );
            }
        }

        private readonly string _ConfigPath =
            Path.Combine(Application.StartupPath, "DataStructures", "{0}", "{1}") + ".xml";

        private string GetConfigPath()
        {
            var folderName = Path.GetFileNameWithoutExtension(Target.GetBufferId(true));
            return String.Format(_ConfigPath, folderName, Target.GetBufferId());
        }

        private readonly Brush _ForeBrush;
        private readonly SolidBrush _BorderBrush = new SolidBrush(Color.FromArgb(237, 237, 237));
        private readonly SolidBrush _UnderlineBrush = new SolidBrush(Color.FromArgb(0x55EDEDED));
        private readonly SolidBrush _EvenBrush = new SolidBrush(Color.FromArgb(80, 80, 80));
        private readonly SolidBrush _OddBrush = new SolidBrush(Color.FromArgb(150, 150, 150));
        private readonly SolidBrush _OffsetBrush = new SolidBrush(Color.FromArgb(160, 160, 160));
        private readonly SolidBrush _SelectedBrush = new SolidBrush(Color.FromArgb(unchecked((int)0x880000FF)));
        private readonly SolidBrush _HoveredBrush = new SolidBrush(Color.FromArgb(unchecked((int)0x880088FF)));
        private readonly SolidBrush _HoveredFieldBrush = new SolidBrush(Color.FromArgb(unchecked((int)0x88000000)));

        private SolidBrush _EvenCellBrush;

        private readonly Pen _BorderPen;
        private readonly Pen _UnderlinePen;
        private readonly Pen _SelectionPen;
        private readonly Pen _HoverPen;
        private readonly Pen _LineSelectionPen;
        private readonly Pen _LineHoverPen;
        private readonly SolidBrush _WhiteForeBrush;
        private readonly SolidBrush _ActiveNibbleBrush;
        private readonly SolidBrush _MuteBrush;

        private readonly string _AddressSample;

        private void HexLinePanel_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(_BorderPen, 0f, 0f, _HexLinePanel.Width - 1, _HexLinePanel.Height - 1);
            if (Buffer == null)
                return;

            _AddressSize = e.Graphics.MeasureString(_AddressSample, _HexLinePanel.Font,
                new PointF(0, 0), StringFormat.GenericTypographic
            );

            int offset = CellCount * _HexViewScrollBar.Value;
            int lineCount = Math.Min(
                (int)(_HexLinePanel.ClientSize.Height / CellHeight),
                (Buffer.Length - offset) / CellCount + ((Buffer.Length - offset) % CellCount > 0 ? 1 : 0)
            );

            float addressColumnOffset = ColumnMargin;
            float addressColumnWidth = _AddressSize.Width;
            float byteColumnOffset = _DrawByte
                ? addressColumnOffset + addressColumnWidth + ColumnMargin
                : 0;
            float asciiColumnOffset = _DrawByte
                ? byteColumnOffset + ColumnWidth + ColumnMargin
                : byteColumnOffset;

            string text = Resources.HexView_Offset;

            e.Graphics.DrawString(text, _HexLinePanel.Font, _ForeBrush,
                addressColumnOffset,
                ColumnMargin,
                StringFormat.GenericDefault
            );
            e.Graphics.DrawLine(_UnderlinePen,
                addressColumnOffset, ColumnMargin + CellHeight,
                addressColumnOffset + byteColumnOffset - ColumnMargin, ColumnMargin + CellHeight
            );

            if (_DrawByte)
            {
                float x = byteColumnOffset;
                float y = ColumnMargin;

                //e.Graphics.FillRectangle( new SolidBrush( Color.FromArgb(44, 44, 44) ), x, y, ColumnSize, _LineSpacing );
                e.Graphics.DrawLine(_UnderlinePen,
                    x, y + CellHeight,
                    x + ColumnWidth, y + CellHeight
                );

                for (int i = 0; i < CellCount; ++i)
                {
                    var textBrush = SelectedOffset % CellCount == i ? _SelectedBrush
                        : HoveredOffset % CellCount == i ? _HoveredBrush
                        : i / 4.0F / 1.00 % 2.00 < 1.00 ? _EvenBrush : _OffsetBrush;
                    var c = HexTable[i];
                    e.Graphics.DrawString(c, _HexLinePanel.Font, textBrush,
                        x + i * CellWidth,
                        y,
                        StringFormat.GenericDefault
                    );
                }
            }

            if (_DrawASCII)
            {
                float x = asciiColumnOffset;
                float y = ColumnMargin;

                //e.Graphics.FillRectangle( new SolidBrush( Color.FromArgb(44, 44, 44) ), x, y, ColumnSize, _LineSpacing );
                e.Graphics.DrawLine(_UnderlinePen,
                    x, y + CellHeight,
                    x + ColumnWidth, y + CellHeight
                );

                for (int i = 0; i < CellCount; ++i)
                {
                    var isOddGroup = i / 4.0F / 1.00 % 2.00 < 1.00;
                    var textBrush = SelectedOffset % CellCount == i ? _SelectedBrush
                        : HoveredOffset % CellCount == i ? _HoveredBrush
                        : isOddGroup ? _EvenBrush : _OffsetBrush;
                    var c = HexTable[i];
                    e.Graphics.DrawString(c, _HexLinePanel.Font, textBrush,
                        x + i * CellWidth,
                        y,
                        StringFormat.GenericDefault
                    );
                }
            }

            float lineOffsetY = (float)(ColumnMargin + CellHeight + CellHeight * .5);
            float extraLineOffset = CellHeight;
            for (int line = 0; line < lineCount; ++line)
            {
                if (lineOffsetY >= _HexLinePanel.ClientSize.Height)
                {
                    break;
                }

                var maxCells = Math.Min(Buffer.Length - offset, CellCount);
                var lineIsSelected = offset <= SelectedOffset && offset + CellCount > SelectedOffset;
                var lineIsHovered = offset <= HoveredOffset && offset + CellCount > HoveredOffset;

                if (lineIsSelected)
                {
                    e.Graphics.DrawLine
                    (
                        _LineSelectionPen,
                        0, lineOffsetY + extraLineOffset,
                        (SelectedOffset - offset) * CellWidth + byteColumnOffset, lineOffsetY + extraLineOffset
                    );
                }

                if (lineIsHovered)
                {
                    e.Graphics.DrawLine
                    (
                        _LineHoverPen,
                        0, lineOffsetY + extraLineOffset,
                        (HoveredOffset - offset) * CellWidth + byteColumnOffset, lineOffsetY + extraLineOffset
                    );
                }

                string lineText = String.Format("{0:x8}", offset).PadLeft(8, '0').ToUpper();
                var textBrush = line % 2 == 0 ? _EvenBrush : _OddBrush;
                var lineBrush = lineIsSelected ? _SelectedBrush : lineIsHovered ? _HoveredBrush : textBrush;
                e.Graphics.DrawString(lineText, _HexLinePanel.Font, lineBrush, addressColumnOffset, lineOffsetY);

                _EvenCellBrush = new SolidBrush(Color.FromArgb(textBrush.Color.ToArgb() - 0x303030 + 0x500000));
                if (_DrawByte)
                {
                    var hoveredMetaItem = _Structure.MetaInfoList.Find(t => t.Tag is UStruct.UByteCodeDecompiler.Token
                                                                            && t.Position == HoveredOffset
                    );

                    var selectedMetaItem = _Structure.MetaInfoList.Find(t => t.Tag is UStruct.UByteCodeDecompiler.Token
                                                                             && t.Position == SelectedOffset
                    );

                    for (int cellIndex = 0; cellIndex < maxCells; ++cellIndex)
                    {
                        int byteIndex = offset + cellIndex;
                        var cellTextBrush = cellIndex % 4 == 0
                            ? _EvenCellBrush
                            : textBrush;
                        string cellText = HexTable[Buffer[byteIndex]];

                        var y1 = lineOffsetY;
                        var y2 = lineOffsetY + extraLineOffset;
                        var x1 = byteColumnOffset + cellIndex * CellWidth;
                        var x2 = byteColumnOffset + (cellIndex + 1) * CellWidth;

                        foreach (var s in _Structure.MetaInfoList)
                        {
                            var drawSize = hoveredMetaItem == s || selectedMetaItem == s
                                ? s.HoverSize > 0 ? s.HoverSize : s.Size
                                : s.Size;
                            if (byteIndex < s.Position || byteIndex >= s.Position + drawSize)
                                continue;

                            var cellHeight = extraLineOffset;
                            var cellRectangleY = y1;
                            if (s.Tag is UStruct.UByteCodeDecompiler.Token)
                            {
                                cellHeight *= 0.5F;
                                cellRectangleY = y1 + (y2 - y1) * 0.5F - cellHeight * 0.5F;
                            }

                            var rectBrush = new SolidBrush(Color.FromArgb(60, s.Color.R, s.Color.G, s.Color.B));
                            e.Graphics.FillRectangle(rectBrush, x1, cellRectangleY, CellWidth, cellHeight);
                            if (HoveredOffset >= s.Position && HoveredOffset < s.Position + drawSize)
                            {
                                var borderPen = new Pen(_HoveredFieldBrush);
                                e.Graphics.DrawLine(borderPen, x1, y1, x2, y1); // Top	
                                e.Graphics.DrawLine(borderPen, x1, y2, x2, y2); // Bottom

                                if (byteIndex == s.Position)
                                    e.Graphics.DrawLine(borderPen, x1, y1, x1, y2); // Left

                                if (byteIndex == s.Position + drawSize - 1)
                                    e.Graphics.DrawLine(borderPen, x2, y1, x2, y2); // Right
                            }

                            cellTextBrush = new SolidBrush(cellTextBrush.Color.Darken(30F));
                        }

                        // Render edit carret.
                        if (byteIndex == _ActiveOffset)
                        {
                            e.Graphics.FillRectangle(cellTextBrush,
                                (int)x1, (int)y1,
                                (int)CellWidth, (int)CellHeight
                            );
                            //if( (DateTime.Now - _CarretStartTime).TotalMilliseconds % 600 < 500 )
                            //{
                            var nibbleWidth = (float)(CellWidth * 0.5);
                            switch (_ActiveNibbleIndex)
                            {
                                case 0:
                                    e.Graphics.DrawLine(new Pen(
                                            _ActiveNibbleBrush,
                                            nibbleWidth
                                        ),
                                        x1 + 1 + nibbleWidth * 0.5F, y1, x1 + 1 + nibbleWidth * 0.5F, y2
                                    );
                                    break;

                                case 1:
                                    e.Graphics.DrawLine(new Pen(
                                            _ActiveNibbleBrush,
                                            nibbleWidth
                                        ),
                                        x1 + nibbleWidth + nibbleWidth * 0.5F, y1,
                                        x1 + nibbleWidth + nibbleWidth * 0.5F, y2
                                    );
                                    break;
                            }

                            cellTextBrush = _WhiteForeBrush;
                            //}
                        }

                        e.Graphics.DrawString(cellText, _HexLinePanel.Font, cellTextBrush,
                            byteColumnOffset + cellIndex * CellWidth, lineOffsetY
                        );

                        if (byteIndex == SelectedOffset)
                        {
                            // Draw the selection.
                            var drawPen = _SelectionPen;
                            e.Graphics.DrawRectangle(drawPen,
                                (int)(byteColumnOffset + cellIndex * CellWidth),
                                (int)lineOffsetY,
                                (int)CellWidth,
                                (int)CellHeight
                            );
                        }

                        if (byteIndex == HoveredOffset)
                        {
                            var drawPen = _HoverPen;
                            e.Graphics.DrawRectangle(drawPen,
                                (int)(byteColumnOffset + cellIndex * CellWidth),
                                (int)lineOffsetY,
                                (int)CellWidth,
                                (int)CellHeight
                            );
                        }
                    }
                }

                if (_DrawASCII)
                {
                    for (int cellIndex = 0; cellIndex < maxCells; ++cellIndex)
                    {
                        int byteIndex = offset + cellIndex;

                        if (byteIndex == SelectedOffset)
                        {
                            // Draw the selection.
                            var drawPen = _SelectionPen;
                            e.Graphics.DrawRectangle(drawPen,
                                (int)(asciiColumnOffset + cellIndex * CellWidth),
                                (int)lineOffsetY,
                                (int)CellWidth,
                                (int)CellHeight
                            );
                        }

                        if (byteIndex == HoveredOffset)
                        {
                            var drawPen = _HoverPen;
                            e.Graphics.DrawRectangle(drawPen,
                                (int)(asciiColumnOffset + cellIndex * CellWidth),
                                (int)lineOffsetY,
                                (int)CellWidth,
                                (int)CellHeight
                            );
                        }

                        string drawnChar;
                        Brush drawBrush;
                        switch (Buffer[byteIndex])
                        {
                            case 0x09:
                                drawnChar = "\\t";
                                drawBrush = _EvenCellBrush;
                                break;

                            case 0x0A:
                                drawnChar = "\\n";
                                drawBrush = _EvenCellBrush;
                                break;

                            case 0x0D:
                                drawnChar = "\\r";
                                drawBrush = _EvenCellBrush;
                                break;

                            default:
                                drawnChar = FilterByte(Buffer[byteIndex]).ToString(CultureInfo.InvariantCulture);
                                drawBrush = drawnChar == "." ? _MuteBrush : textBrush;
                                break;
                        }

                        e.Graphics.DrawString(
                            drawnChar, _HexLinePanel.Font, drawBrush,
                            asciiColumnOffset + cellIndex * CellWidth,
                            lineOffsetY
                        );
                    }
                }

                offset += maxCells;
                lineOffsetY += extraLineOffset;
            }
        }

        private static readonly string[] HexTable =
        {
            "00", "01", "02", "03", "04", "05", "06", "07", "08", "09", "0A", "0B", "0C", "0D", "0E", "0F",
            "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "1A", "1B", "1C", "1D", "1E", "1F",
            "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "2A", "2B", "2C", "2D", "2E", "2F",
            "30", "31", "32", "33", "34", "35", "36", "37", "38", "39", "3A", "3B", "3C", "3D", "3E", "3F",
            "40", "41", "42", "43", "44", "45", "46", "47", "48", "49", "4A", "4B", "4C", "4D", "4E", "4F",
            "50", "51", "52", "53", "54", "55", "56", "57", "58", "59", "5A", "5B", "5C", "5D", "5E", "5F",
            "60", "61", "62", "63", "64", "65", "66", "67", "68", "69", "6A", "6B", "6C", "6D", "6E", "6F",
            "70", "71", "72", "73", "74", "75", "76", "77", "78", "79", "7A", "7B", "7C", "7D", "7E", "7F",
            "80", "81", "82", "83", "84", "85", "86", "87", "88", "89", "8A", "8B", "8C", "8D", "8E", "8F",
            "90", "91", "92", "93", "94", "95", "96", "97", "98", "99", "9A", "9B", "9C", "9D", "9E", "9F",
            "A0", "A1", "A2", "A3", "A4", "A5", "A6", "A7", "A8", "A9", "AA", "AB", "AC", "AD", "AE", "AF",
            "B0", "B1", "B2", "B3", "B4", "B5", "B6", "B7", "B8", "B9", "BA", "BB", "BC", "BD", "BE", "BF",
            "C0", "C1", "C2", "C3", "C4", "C5", "C6", "C7", "C8", "C9", "CA", "CB", "CC", "CD", "CE", "CF",
            "D0", "D1", "D2", "D3", "D4", "D5", "D6", "D7", "D8", "D9", "DA", "DB", "DC", "DD", "DE", "DF",
            "E0", "E1", "E2", "E3", "E4", "E5", "E6", "E7", "E8", "E9", "EA", "EB", "EC", "ED", "EE", "EF",
            "F0", "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "FA", "FB", "FC", "FD", "FE", "FF"
        };

        internal static char FilterByte(byte code)
        {
            if (code >= 0x20 && code <= 0x7E)
            {
                return (char)code;
            }

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
            get { return _SelectedOffset; }
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
            if (OffsetChangedEvent != null)
            {
                OffsetChangedEvent.Invoke(SelectedOffset);
            }
        }

        private void OffsetChanged()
        {
            _HexLinePanel.Invalidate();
            if (SelectedOffset == -1)
                return;

            DissambledObject.Text = String.Empty;
            DissambledName.Text = String.Empty;

            var bufferSelection = new byte[8];
            for (int i = 0; SelectedOffset + i < Buffer.Length && i < 8; ++i)
            {
                bufferSelection[i] = Buffer[SelectedOffset + i];
            }

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
                var index = UnrealReader.ReadIndexFromBuffer(bufferSelection, Target.GetBuffer());
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
                DissambledObject.Text = obj == null ? Resources.NOT_AVAILABLE : obj.GetOuterGroup();
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

            DissambledStruct.Text = String.Empty;
            foreach (var s in _Structure.MetaInfoList)
            {
                if (SelectedOffset >= s.Position && SelectedOffset < s.Position + s.Size)
                {
                    DissambledStruct.Text = s.Name;
                }
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

            _HexLinePanel.Invalidate();
        }

        private void HexLinePanel_MouseClick(object sender, MouseEventArgs e)
        {
            if (Buffer == null)
            {
                return;
            }
            //ActiveOffset = -1;

            SelectedOffset = GetHoveredByte(e);
            _HexLinePanel.Invalidate();
        }

        private void HexLinePanel_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (Buffer == null)
            {
                return;
            }

            ActivateCell(GetHoveredByte(e));
        }

        private void ActivateCell(int index)
        {
            if (index == -1)
                return;

            _ActiveOffset = index;
            _ActiveNibbleIndex = 0;

            _CarretStartTime = DateTime.Now;
            _HexLinePanel.Invalidate();
        }

        private int GetHoveredByte(MouseEventArgs e)
        {
            float x = e.X - _HexLinePanel.Location.X;
            float y = e.Y - _HexLinePanel.Location.Y;

            int offset = CellCount * _HexViewScrollBar.Value;
            int lineCount = Math.Min((int)(_HexLinePanel.ClientSize.Height / CellHeight),
                (Buffer.Length - offset) / CellCount +
                ((Buffer.Length - offset) % CellCount > 0 ? 1 : 0)
            );

            float addressColumnOffset = ColumnMargin;
            float addressColumnWidth = _AddressSize.Width;
            float byteColumnOffset = _DrawByte
                ? addressColumnOffset + addressColumnWidth + ColumnMargin
                : 0;
            float asciiColumnOffset = _DrawByte
                ? byteColumnOffset + ColumnWidth + ColumnMargin
                : byteColumnOffset;

            float lineYOffset = (float)(ColumnMargin + CellHeight + CellHeight * .5);
            for (int line = 0; line < lineCount; ++line)
            {
                if (lineYOffset >= _HexLinePanel.ClientSize.Height)
                {
                    break;
                }

                // The user definitely didn't click on this line?, so skip!.
                if (!(y >= lineYOffset && y <= lineYOffset + CellHeight))
                {
                    offset += CellCount;
                    lineYOffset += CellHeight;
                    continue;
                }

                var maxCells = Math.Min(Buffer.Length - offset, CellCount);

                // Check if the bytes field is selected.
                if (_DrawByte && x >= byteColumnOffset && x < asciiColumnOffset)
                {
                    for (int cellIndex = 0; cellIndex < maxCells; ++cellIndex)
                    {
                        int byteIndex = offset + cellIndex;
                        if
                        (
                            x >= byteColumnOffset + cellIndex * CellWidth &&
                            x <= byteColumnOffset + (cellIndex + 1) * CellWidth
                        )
                        {
                            return byteIndex;
                        }
                    }
                }

                // Check if the ascii's field is selected.
                if (_DrawASCII && x >= asciiColumnOffset)
                {
                    float asciiWidth = CellWidth;
                    for (int cellIndex = 0; cellIndex < maxCells; ++cellIndex)
                    {
                        int byteIndex = offset + cellIndex;
                        if
                        (
                            x >= asciiColumnOffset + cellIndex * asciiWidth &&
                            x <= asciiColumnOffset + (cellIndex + 1) * asciiWidth
                        )
                        {
                            return byteIndex;
                        }
                    }
                }

                offset += maxCells;
                lineYOffset += CellHeight;
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
                var type = e.ClickedItem.Text.Mid(e.ClickedItem.Text.LastIndexOf(' ') + 1);
                dialog.TextBoxName.Text = type;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    if (dialog.TextBoxName.Text == String.Empty)
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

                    var path = GetConfigPath();
                    SaveConfig(path);

                    _HexLinePanel.Invalidate();
                }
            }
        }

        private void HexLinePanel_MouseMove(object sender, MouseEventArgs e)
        {
            var lastHoveredOffset = HoveredOffset;
            HoveredOffset = GetHoveredByte(e);

            if (lastHoveredOffset != HoveredOffset)
            {
                _HexLinePanel.Invalidate();
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
                    var message = dataStruct.Name;
                    if (dataStruct.Tag != null)
                    {
                        try
                        {
                            // Restart token index.
                            var token = dataStruct.Tag as UStruct.UByteCodeDecompiler.Token;
                            if (token != null)
                            {
                                token.Decompiler.JumpTo((ushort)token.Position);
                            }

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
                {
                    HexToolTip.Hide(this);
                }
            }
        }

        private void DataInfoPanel_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(_BorderPen, 0f, 0f, DataInfoPanel.Width - 1, DataInfoPanel.Height - 1);
        }

        private void SplitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            DataInfoPanel.Invalidate();
            _HexLinePanel.Invalidate();
        }

        private void UserControl_HexView_Resize(object sender, EventArgs e)
        {
            DataInfoPanel.Invalidate();
            _HexLinePanel.Invalidate();
            UpdateScrollBar();
        }

        public delegate void BufferModifiedEventHandler();

        public event BufferModifiedEventHandler BufferModifiedEvent = null;

        private void OnBufferModifiedEvent()
        {
            if (BufferModifiedEvent != null)
            {
                BufferModifiedEvent.Invoke();
            }
        }

        private bool _LastKeyWasLeft, _LastKeyWasRight;
        private SizeF _AddressSize;

        private void EditKeyDown(object sender, KeyEventArgs e)
        {
            // HACK: To determine the cause of increment and decrement events when scrolling.
            _LastKeyWasLeft = e.KeyCode == Keys.Left;
            _LastKeyWasRight = e.KeyCode == Keys.Right;
            if (_LastKeyWasLeft || _LastKeyWasRight)
            {
                return;
            }

            if (_ActiveOffset == -1)
                return;

            if (e.KeyCode == Keys.Return)
            {
                _ActiveOffset = -1;
            }
            else
            {
                if (e.KeyCode == Keys.Shift)
                {
                    return;
                }

                var hexKeyIndex = HexKeyCodeToIndex(e.KeyCode);
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

            _HexLinePanel.Invalidate();
            e.SuppressKeyPress = true;
        }

        [DllImport("user32")]
        static extern int MapVirtualKey(Keys uCode, int uMapType);

        const int MAPVK_VK_TO_CHAR = 2;

        private static int HexKeyCodeToIndex(Keys keyCode)
        {
            var c = MapVirtualKey(keyCode, MAPVK_VK_TO_CHAR) & ~(1 << 31);
            if (c >= '0' && c <= '9')
            {
                return c - '0';
            }

            if (c >= 'A' && c <= 'F')
            {
                return c - 'A' + 10;
            }

            if (c >= 'a' && c <= 'f')
            {
                return c - 'a' + 10;
            }

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

        private void HexViewerControl_Scroll(object sender, ScrollEventArgs e)
        {
            HexScrollBar_Scroll(sender, e);
        }

        private void HexScrollBar_KeyDown(object sender, KeyEventArgs e)
        {
            EditKeyDown(sender, e);
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            switch (m.Msg)
            {
                case 0x20a:
                {
                    _HexViewScrollBar.Focus();
                    SendMessage(_HexViewScrollBar.Handle, 0x20a, m.WParam, m.LParam);
                    //int value = _HexViewScrollBar.Value;
                    //_HexViewScrollBar.Value = Math.Min(value + 1, _HexViewScrollBar.Maximum);
                    //HexScrollBar_Scroll(this,
                    //    new ScrollEventArgs(ScrollEventType.SmallIncrement, value,
                    //        _HexViewScrollBar.Value));
                    break;
                }
            }
        }
    }
}