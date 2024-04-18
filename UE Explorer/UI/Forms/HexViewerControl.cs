using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
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
using UELib.Annotations;
using UELib.Branch;
using UELib.Core;

namespace UEExplorer.UI.Forms
{
    using Properties;
    using UELib;
    using UStruct = UStruct;

    public partial class HexViewerControl : UserControl
    {
        public byte[] Buffer { get; set; }
        public IBuffered Target { get; private set; }

        public class HexMetaInfo
        {
            public class BytesMetaInfo
            {
                [XmlElement("Position")]
                public int Offset;

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

        private class CellState
        {
            public bool IsModified;
        }

        private readonly Dictionary<int, CellState> _CellStates = new Dictionary<int, CellState>();

        #region View Properties

        private const int CellCount = 16;
        private const float CellPadding = 6;
        private float CellWidth => HexViewPanel.Font.Height + CellPadding;
        private float CellHeight => HexViewPanel.Font.Height;
        private float ColumnWidth => CellCount * CellWidth;
        private const float ColumnMargin = 8;

        private bool _DrawASCII = true;

        public bool DrawASCII
        {
            get => _DrawASCII;
            set
            {
                _DrawASCII = value;
                HexViewPanel.Invalidate();
            }
        }

        private bool _DrawByte = true;

        public bool DrawByte
        {
            get => _DrawByte;
            set
            {
                _DrawByte = value;
                HexViewPanel.Invalidate();
            }
        }

        #endregion

        [DllImport("user32.dll")]
        static extern int SendMessage(IntPtr hWnd, uint wMsg, IntPtr wParam, IntPtr lParam);

        public HexViewerControl()
        {
            InitializeComponent();

            CellModifiedFont = new Font(HexViewPanel.Font, FontStyle.Italic | FontStyle.Bold);

            _UnderlinePen = new Pen(_UnderlineBrush);

            _SelectionPen = new Pen(_SelectedBrush);
            _HoverPen = new Pen(_HoveredBrush);

            _LineSelectionPen = _SelectionPen;
            _LineHoverPen = _HoverPen;
            _ForeBrush = new SolidBrush(Color.Black);
            _WhiteForeBrush = new SolidBrush(Color.White);
            _ActiveNibbleBrush = new SolidBrush(Color.FromArgb(unchecked((int)0xEE000000)));

            _AddressSample = $"{99999999:x8}".PadLeft(8, '0').ToUpper();
            _MuteBrush = _EvenBrush;
            _BorderPen = new Pen(_BorderBrush);
        }

        private void HexViewerControl_Load(object sender, EventArgs e)
        {

        }

        private void LoadConfig(string path)
        {
            using (var r = new XmlTextReader(path))
            {
                var xser = new XmlSerializer(typeof(HexMetaInfo));
                _Structure = (HexMetaInfo)xser.Deserialize(r);

                foreach (var s in _Structure.MetaInfoList.Where(s => s.Type != "Generated"))
                {
                    InitStructure(s.Type, out byte size, out var color);
                    s.Size = size;
                    s.Color = color;
                }

                HexViewPanel.Invalidate();
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
                    // FIXME: Dynamic size
                    size = 4;
                    color = Color.DarkTurquoise;
                    break;

                case "index":
                    // FIXME: Dynamic size
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
            Target = target ?? throw new Exception("target cannot be null");
            
            Buffer = Target.CopyBuffer();
            UpdateScrollBar();

            string path = GetConfigPath();
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

        private void SetCellValue(int cellIndex, byte cellValue)
        {
            Buffer[cellIndex] = cellValue;
            if (_CellStates.TryGetValue(cellIndex, out var cellState))
            {
                cellState.IsModified = true;
            }
            else
            {
                _CellStates.Add(cellIndex, new CellState
                {
                    IsModified = true
                });
            }
        }
        
        private byte GetCellValue(int cellIndex)
        {
            return Buffer[cellIndex];
        }

        [CanBeNull]
        private HexMetaInfo.BytesMetaInfo GetCellStruct(int cellIndex)
        {
            if (cellIndex == -1)
            {
                return null;
            }

            var cellStruct = _Structure.MetaInfoList.Find(metaInfo => cellIndex >= metaInfo.Offset && cellIndex <= metaInfo.Offset + metaInfo.Size);

            return cellStruct;
        }

        private void SetCellStructValue(int cellIndex, byte[] cellValue)
        {
            for (int i = 0; i < cellValue.Length; i++)
            {
                SetCellValue(cellIndex + i, cellValue[i]);
            }
        }

        private void UpdateScrollBar()
        {
            var totalLines = (int)Math.Ceiling(Buffer.Length / (float)CellCount) + 2;
            var visibleLines = (int)Math.Ceiling(HexViewPanel.Height / CellHeight);
            var trailingLines = totalLines % visibleLines;
            var scrollableLines = totalLines - trailingLines;

            HexViewScrollBar.Minimum = 0;
            HexViewScrollBar.Maximum = Math.Max(totalLines, 0);
            HexViewScrollBar.Invalidate();
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
                    int colorHash = binaryField.Name.GetHashCode()
                                    ^ (int)binaryField.Size
                                    ^ (binaryField.Tag?.GetType().GetHashCode() ?? 1);
                    _Structure.MetaInfoList.Add
                    (
                        new HexMetaInfo.BytesMetaInfo
                        {
                            Offset = (int)binaryField.Position,
                            Size = (int)binaryField.Size,
                            Type = "Generated",
                            Color = NormalizeArgbToColor(colorHash),
                            Name = binaryField.Name,
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
                        Offset = (int)(token.StoragePosition + (int)unStruct.ScriptOffset),
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
            return Color.FromArgb(60, r, g, b);
        }

        private readonly string _ConfigPath =
            Path.Combine(Program.s_appDataDir, "DataStructures", "{0}", "{1}") + ".xml";

        private string GetConfigPath()
        {
            var folderName = Path.GetFileNameWithoutExtension(Target.GetBufferId(true));
            return string.Format(_ConfigPath, folderName, Target.GetBufferId());
        }

        [EditorBrowsable] public Font CellModifiedFont { get; set; }

        private readonly Brush _ForeBrush;
        private readonly SolidBrush _BorderBrush = new SolidBrush(Color.FromArgb(237, 237, 237));
        private readonly SolidBrush _UnderlineBrush = new SolidBrush(Color.FromArgb(0x55EDEDED));
        private readonly SolidBrush _EvenBrush = new SolidBrush(Color.FromArgb(80, 80, 80));
        private readonly SolidBrush _EvenLitBrush = new SolidBrush(Color.FromArgb(112, 32, 32));
        private readonly SolidBrush _OddBrush = new SolidBrush(Color.FromArgb(150, 150, 150));
        private readonly SolidBrush _OddLitBrush = new SolidBrush(Color.FromArgb(182, 102, 102));
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
            _AddressSize = e.Graphics.MeasureString(_AddressSample, HexViewPanel.Font,
                new PointF(0, 0), StringFormat.GenericTypographic
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

            e.Graphics.DrawString(text, HexViewPanel.Font, _ForeBrush,
                addressColumnOffset,
                ColumnMargin,
                StringFormat.GenericDefault
            );
            e.Graphics.DrawLine(_UnderlinePen,
                addressColumnOffset, ColumnMargin + CellHeight,
                addressColumnOffset + byteColumnOffset - ColumnMargin, ColumnMargin + CellHeight
            );
            
            if (Buffer == null)
                return;
            
            int offset = CellCount * HexViewScrollBar.Value;
            int lineCount = Math.Min(
                (int)(HexViewPanel.ClientSize.Height / CellHeight),
                (Buffer.Length - offset) / CellCount + ((Buffer.Length - offset) % CellCount > 0 ? 1 : 0)
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
                    e.Graphics.DrawString(c, HexViewPanel.Font, textBrush,
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

                e.Graphics.DrawString("ASCII", HexViewPanel.Font, _OffsetBrush,
                    x,
                    y,
                    StringFormat.GenericDefault
                );
            }

            float lineOffsetY = (float)(ColumnMargin + CellHeight + CellHeight * .5);
            float extraLineOffset = CellHeight;
            for (int line = 0; line < lineCount; ++line)
            {
                if (lineOffsetY >= HexViewPanel.ClientSize.Height)
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

                string lineText = $"{offset:X8}".PadLeft(8, '0');
                var textBrush = line % 2 == 0 ? _EvenBrush : _OddBrush;
                var lineBrush = lineIsSelected ? _SelectedBrush : lineIsHovered ? _HoveredBrush : textBrush;
                e.Graphics.DrawString(lineText, HexViewPanel.Font, lineBrush, addressColumnOffset, lineOffsetY);

                _EvenCellBrush = new SolidBrush(Color.FromArgb(textBrush.Color.ToArgb() - 0x303030 + 0x500000));
                if (_DrawByte)
                {
                    var hoveredMetaItem = _Structure.MetaInfoList.Find(t => t.Tag is UStruct.UByteCodeDecompiler.Token
                                                                            && t.Offset == HoveredOffset
                    );

                    var selectedMetaItem = _Structure.MetaInfoList.Find(t => t.Tag is UStruct.UByteCodeDecompiler.Token
                                                                             && t.Offset == SelectedOffset
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
                            if (byteIndex < s.Offset || byteIndex >= s.Offset + drawSize)
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
                            if (HoveredOffset >= s.Offset && HoveredOffset < s.Offset + drawSize)
                            {
                                var borderPen = new Pen(_HoveredFieldBrush);
                                e.Graphics.DrawLine(borderPen, x1, y1, x2, y1); // Top	
                                e.Graphics.DrawLine(borderPen, x1, y2, x2, y2); // Bottom

                                if (byteIndex == s.Offset)
                                    e.Graphics.DrawLine(borderPen, x1, y1, x1, y2); // Left

                                if (byteIndex == s.Offset + drawSize - 1)
                                    e.Graphics.DrawLine(borderPen, x2, y1, x2, y2); // Right
                            }

                            cellTextBrush = new SolidBrush(cellTextBrush.Color.Darken(30F));
                        }

                        var cellFont = HexViewPanel.Font;

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
                                        x1 + nibbleWidth * 0.5F, y1, x1 + nibbleWidth * 0.5F, y2
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
                        else
                        {
                            if (_CellStates.TryGetValue(byteIndex, out var cellState) && cellState.IsModified)
                            {
                                cellFont = CellModifiedFont;
                            }
                        }

                        e.Graphics.DrawString(cellText, cellFont, cellTextBrush,
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
                    float cellWidth = CellWidth;
                    float cellHeight = CellHeight;
                    for (int cellIndex = 0; cellIndex < maxCells; ++cellIndex)
                    {
                        int byteIndex = offset + cellIndex;

                        if (byteIndex == SelectedOffset)
                        {
                            // Draw the selection.
                            var drawPen = _SelectionPen;
                            e.Graphics.DrawRectangle(drawPen,
                                (int)(asciiColumnOffset + cellIndex * cellWidth),
                                (int)lineOffsetY,
                                (int)cellWidth,
                                (int)cellHeight
                            );
                        }

                        if (byteIndex == HoveredOffset)
                        {
                            var drawPen = _HoverPen;
                            e.Graphics.DrawRectangle(drawPen,
                                (int)(asciiColumnOffset + cellIndex * cellWidth),
                                (int)lineOffsetY,
                                (int)cellWidth,
                                (int)cellHeight
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
                            drawnChar, HexViewPanel.Font, drawBrush,
                            asciiColumnOffset + cellIndex * cellWidth,
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

        private int _ContextOffset = -1;

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
            HexViewPanel.Invalidate();
            if (SelectedOffset == -1)
                return;

            DissambledObject.Text = string.Empty;
            DissambledName.Text = string.Empty;

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

            DissambledStruct.Text = string.Empty;
            foreach (var s in _Structure.MetaInfoList)
            {
                if (SelectedOffset >= s.Offset && SelectedOffset < s.Offset + s.Size)
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

            HexViewPanel.Invalidate();
        }

        private void HexLinePanel_MouseClick(object sender, MouseEventArgs e)
        {
            if (Buffer == null)
            {
                return;
            }

            var cellIndex = GetHoveredByte(e);
            if (cellIndex == SelectedOffset)
            {
                return;
            }

            SelectedOffset = cellIndex;
            SetActiveCell(-1);
        }

        private void HexLinePanel_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (Buffer == null)
            {
                return;
            }

            var cellIndex = GetHoveredByte(e);
            if (cellIndex == _ActiveOffset)
            {
                return;
            }

            SetActiveCell(cellIndex);
        }

        private void SetActiveCell(int index)
        {
            _ActiveOffset = index;
            _ActiveNibbleIndex = 0;

            _CarretStartTime = DateTime.Now;
            HexViewPanel.Invalidate();
        }

        private int GetHoveredByte(MouseEventArgs e)
        {
            float x = e.X - HexViewPanel.Location.X;
            float y = e.Y - HexViewPanel.Location.Y;

            int offset = CellCount * HexViewScrollBar.Value;
            int lineCount = Math.Min((int)(HexViewPanel.ClientSize.Height / CellHeight),
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
                if (lineYOffset >= HexViewPanel.ClientSize.Height)
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
                    float cellWidth = CellWidth;
                    for (int cellIndex = 0; cellIndex < maxCells; ++cellIndex)
                    {
                        int byteIndex = offset + cellIndex;
                        if
                        (
                            x >= byteColumnOffset + cellIndex * cellWidth &&
                            x <= byteColumnOffset + (cellIndex + 1) * cellWidth
                        )
                        {
                            return byteIndex;
                        }
                    }
                }

                // Check if the ascii's field is selected.
                if (_DrawASCII && x >= asciiColumnOffset)
                {
                    float cellWidth = CellWidth;
                    for (int cellIndex = 0; cellIndex < maxCells; ++cellIndex)
                    {
                        int byteIndex = offset + cellIndex;
                        if
                        (
                            x >= asciiColumnOffset + cellIndex * cellWidth &&
                            x <= asciiColumnOffset + (cellIndex + 1) * cellWidth
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

        private void HexLinePanel_MouseMove(object sender, MouseEventArgs e)
        {
            var lastHoveredOffset = HoveredOffset;
            HoveredOffset = GetHoveredByte(e);

            if (lastHoveredOffset != HoveredOffset)
            {
                HexViewPanel.Invalidate();
                if (HoveredOffset != -1)
                {
                    var dataStruct = _Structure.MetaInfoList.Find(
                        i => HoveredOffset >= i.Offset && HoveredOffset < i.Offset + i.Size
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
                            if (dataStruct.Tag is UStruct.UByteCodeDecompiler.Token token)
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

        private void HexViewPanel_KeyDown(object sender, KeyEventArgs e)
        {
            int currentCellIndex = _ActiveOffset;
            
            // Can't change selection if we are editing.
            if (currentCellIndex != -1 && (
                    e.KeyCode == Keys.Left ||
                    e.KeyCode == Keys.Right ||
                    e.KeyCode == Keys.Up ||
                    e.KeyCode == Keys.Down))
            {
                if (e.KeyCode == Keys.Left)
                {
                    _ActiveNibbleIndex = 0;
                    HexViewPanel.Invalidate();
                }
                else if (e.KeyCode == Keys.Right)
                {
                    _ActiveNibbleIndex = 1;
                    HexViewPanel.Invalidate();
                }

                e.Handled = true;
                e.SuppressKeyPress = true;
                return;
            }

            // HACK: To determine the cause of increment and decrement events when scrolling.
            _LastKeyWasLeft = e.KeyCode == Keys.Left;
            _LastKeyWasRight = e.KeyCode == Keys.Right;
            if (_LastKeyWasLeft || _LastKeyWasRight)
            {
                return;
            }

            if (e.KeyCode == Keys.Return)
            {
                if (currentCellIndex == -1 && SelectedOffset != -1)
                {
                    SetActiveCell(_SelectedOffset);
                }
                else if (currentCellIndex != -1)
                {
                    SelectedOffset = currentCellIndex;
                    SetActiveCell(-1);
                }

                e.Handled = true;
                e.SuppressKeyPress = true;
            }
            else
            {
                if (currentCellIndex == -1 || e.KeyCode == Keys.Shift)
                {
                    return;
                }

                var hexKeyIndex = HexKeyCodeToIndex(e.KeyCode);
                if (hexKeyIndex == -1)
                {
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    return;
                }

                _CarretStartTime = DateTime.Now;
                byte newByte = Buffer[currentCellIndex];
                switch (_ActiveNibbleIndex)
                {
                    case 0:
                        newByte = (byte)((byte)(newByte & 0x0F) | (hexKeyIndex << 4));
                        SetCellValue(currentCellIndex, newByte);
                        _ActiveNibbleIndex = 1;
                        break;

                    case 1:
                        newByte = (byte)((byte)(newByte & 0xF0) | hexKeyIndex);
                        Buffer[currentCellIndex] = newByte;
                        SetCellValue(currentCellIndex, newByte);
                        _ActiveNibbleIndex = 0;
                        // Move the active cell index to the next cell
                        currentCellIndex = Math.Min(currentCellIndex + 1, Buffer.Length - 1);
                        SelectedOffset = currentCellIndex;
                        break;
                }

                HexViewPanel.Invalidate();
                OnBufferModifiedEvent();

                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }
        
        private void HexViewPanel_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
        
        private void HexViewPanel_KeyUp(object sender, KeyEventArgs e)
        {
            int currentCellIndex = SelectedOffset;
            if (currentCellIndex == -1)
            {
                return;
            }

            // TODO: Migrate to MainMenu->Edit dropdown
            if (e.Control && e.KeyCode == Keys.C)
            {
                if (e.Shift)
                {
                    int value = currentCellIndex;
                    string text = "0x" + value.ToString("X8", CultureInfo.InvariantCulture);
                    Clipboard.SetText(text);

                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }
                else
                {
                    byte value = GetCellValue(currentCellIndex);
                    string text = "0x" + value.ToString("X2", CultureInfo.InvariantCulture);
                    Clipboard.SetText(text);
                    
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }
            }
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

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            switch (m.Msg)
            {
                case 0x20a:
                    {
                        HexViewScrollBar.Focus();
                        SendMessage(HexViewScrollBar.Handle, 0x20a, m.WParam, m.LParam);
                        //int value = _HexViewScrollBar.Value;
                        //_HexViewScrollBar.Value = Math.Min(value + 1, _HexViewScrollBar.Maximum);
                        //HexScrollBar_Scroll(this,
                        //    new ScrollEventArgs(ScrollEventType.SmallIncrement, value,
                        //        _HexViewScrollBar.Value));
                        break;
                    }
            }
        }

        private void Context_Structure_Opening(object sender, CancelEventArgs e)
        {
            _ContextOffset = HoveredOffset != -1
                ? HoveredOffset
                : SelectedOffset != -1
                    ? SelectedOffset
                    : -1;
            
            e.Cancel = _ContextOffset == -1;
            if (e.Cancel)
            {
                return;
            }

            if (HoveredOffset != -1)
            {
                SelectedOffset = HoveredOffset;
            }

            foreach (ToolStripItem item in Context_Structure.Items)
            {
                bool isVisible;
                switch (item.Tag)
                {
                    case "Cell":
                        isVisible = _ContextOffset != -1;
                        break;

                    case "Struct":
                        isVisible = _ContextOffset != -1 && GetCellStruct(_ContextOffset) != null;
                        break;

                    default:
                        isVisible = false;
                        break;
                }

                item.Visible = isVisible;
            }

            // FIXME: temp
            editStructValueToolStripMenuItem.Visible = GetCellStruct(_ContextOffset)?.Tag is BinaryMetaData.BinaryField binaryField
                                                       && (binaryField.Value is UObject || binaryField.Value is UName);

            defineStructToolStripMenuItem.Visible = GetCellStruct(_ContextOffset) == null;
        }

        private void editCellToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Contract.Assert(_ContextOffset != -1);
            SetActiveCell(_ContextOffset);
        }

        private void editStructValueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var cellStruct = GetCellStruct(_ContextOffset);
            Contract.Assert(cellStruct != null);

            var linker = Target is UObject o ? o.Package : null;
            Contract.Assert(linker != null);

            switch (cellStruct.Tag)
            {
                case BinaryMetaData.BinaryField binaryField:
                    switch (binaryField.Value)
                    {
                        case UName uName:
                        {
                            var inputDialog = new NameReferenceInputDialog
                            {
                                Linker = linker, DefaultNameReference = uName
                            };
                            if (inputDialog.ShowDialog(this) == DialogResult.OK)
                            {
                                int numberValue = inputDialog.InputNameNumber;
                                var newValue = new UName(inputDialog.InputNameItem, numberValue - 1);
                                int index = (int)newValue;

                                var archive = linker.GetBuffer();
                                Contract.Assert(archive != null);

                                // HACK: workaround IUnrealStream limitations for now.
                                // This approach will likely fail for some games that have a different FName structure.
                                if (archive.Version >= (uint)PackageObjectLegacyVersion.NumberAddedToName)
                                {
                                    const int indexMaxSize = 8;
                                    // Let's use the UnrealWriter so that we can conform to the varying formats.
                                    using (var uStream = new UnrealWriter(archive, new MemoryStream(indexMaxSize)))
                                    {
                                        uStream.WriteIndex(index);
                                        uStream.Write(numberValue);
                                        byte[] buffer = new byte[indexMaxSize];
                                        uStream.Seek(0, SeekOrigin.Begin);
                                        int read = uStream.BaseStream.Read(buffer, 0, buffer.Length);
                                        Contract.Assert(read == buffer.Length);

                                        Contract.Assert(buffer.Length == cellStruct.Size,
                                            "Struct size must remain the same.");
                                        SetCellStructValue(cellStruct.Offset, buffer);
                                    }
                                }
                                else
                                {
                                    const int indexMaxSize = 5;
                                    // Let's use the UnrealWriter so that we can conform to the varying formats.
                                    using (var uStream = new UnrealWriter(archive, new MemoryStream(indexMaxSize)))
                                    {
                                        uStream.WriteIndex(index);
                                        // dynamically sized to however many bytes were written for the index.
                                        byte[] buffer = new byte[uStream.BaseStream.Position];
                                        uStream.Seek(0, SeekOrigin.Begin);
                                        int read = uStream.BaseStream.Read(buffer, 0, buffer.Length);
                                        Contract.Assert(read == buffer.Length);

                                        Contract.Assert(buffer.Length == cellStruct.Size,
                                            "Struct size must remain the same.");
                                        SetCellStructValue(cellStruct.Offset, buffer);
                                    }
                                }

                                cellStruct.Tag = new BinaryMetaData.BinaryField
                                {
                                    Value = newValue,
                                    Field = binaryField.Field,
                                    Offset = binaryField.Offset,
                                    Size = binaryField.Size
                                };
                            }

                            break;
                        }

                        case UObject uObject:
                        {
                            var inputDialog = new ObjectReferenceInputDialog
                            {
                                Linker = linker, DefaultObjectReference = uObject
                            };
                            if (inputDialog.ShowDialog(this) == DialogResult.OK)
                            {
                                var newValue = inputDialog.InputObjectReference;
                                int objectIndex = inputDialog.InputObjectReference is UObject input
                                    ? (int)input
                                    : 0;

                                var archive = uObject.Package.GetBuffer();
                                Contract.Assert(archive != null);

                                const int indexMaxSize = 5;
                                // Let's use the UnrealWriter so that we can conform to the varying formats.
                                using (var uStream = new UnrealWriter(archive, new MemoryStream(indexMaxSize)))
                                {
                                    // Only valid for UE3's default format
                                    //byte[] buffer = BitConverter.GetBytes(objectIndex);

                                    uStream.WriteIndex(objectIndex);
                                    // dynamically sized to however many bytes were written for the index.
                                    byte[] buffer = new byte[uStream.BaseStream.Position];
                                    uStream.Seek(0, SeekOrigin.Begin);
                                    int read = uStream.BaseStream.Read(buffer, 0, buffer.Length);
                                    Contract.Assert(read == buffer.Length);

                                    Contract.Assert(buffer.Length == cellStruct.Size,
                                        "Struct size must remain the same.");
                                    SetCellStructValue(cellStruct.Offset, buffer);
                                }

                                cellStruct.Tag = new BinaryMetaData.BinaryField
                                {
                                    Value = newValue,
                                    Field = binaryField.Field,
                                    Offset = binaryField.Offset,
                                    Size = binaryField.Size
                                };
                            }

                            break;
                        }
                        
                        default:
                            throw new NotSupportedException("Field tag is not supported.");
                    }
                    
                    break;

                default:
                    throw new NotSupportedException("Struct tag is not supported.");
            }
        }

        private void hexValueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Contract.Assert(_ContextOffset != -1);
            
            byte value = GetCellValue(_ContextOffset);
            string text = "0x" + value.ToString("X2", CultureInfo.InvariantCulture);
            Clipboard.SetText(text);
        }

        private void hexOffsetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Contract.Assert(_ContextOffset != -1);

            int value = _ContextOffset;
            string text = "0x" + value.ToString("X8", CultureInfo.InvariantCulture);
            Clipboard.SetText(text);
        }

        private void decimalValueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Contract.Assert(_ContextOffset != -1);

            byte value = GetCellValue(_ContextOffset);
            string text = value.ToString("D", CultureInfo.InvariantCulture);
            Clipboard.SetText(text);
        }

        private void decimalOffsetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Contract.Assert(_ContextOffset != -1);
            int value = _ContextOffset;
            string text = value.ToString("D", CultureInfo.InvariantCulture);
            Clipboard.SetText(text);
        }

        private void structNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var cellStruct = GetCellStruct(_ContextOffset);
            Contract.Assert(cellStruct != null);
            
            string text = cellStruct.Name;
            Clipboard.SetText(text);
        }

        private void structValueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var cellStruct = GetCellStruct(_ContextOffset);
            Contract.Assert(cellStruct != null);

            if (cellStruct.Tag == null)
            {
                return;
            }

            string text;
            switch (cellStruct.Tag)
            {
                case UStruct.UByteCodeDecompiler.Token token:
                    token.Decompiler.JumpTo((ushort)token.Position);
                    text = cellStruct.Tag.Decompile();
                    break;
                
                case BinaryMetaData.BinaryField binaryField:
                    // Copy the raw tag
                    text = binaryField.Value.ToString();
                    break;
                
                default:
                    text = cellStruct.Tag.Decompile();
                    break;
            }
            Clipboard.SetText(text);
        }

        private void structHexSizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var cellStruct = GetCellStruct(_ContextOffset);
            Contract.Assert(cellStruct != null);

            int size = cellStruct.HoverSize > 0 ? cellStruct.HoverSize : cellStruct.Size;
            string text = "0x" + size.ToString("X4", CultureInfo.InvariantCulture);
            Clipboard.SetText(text);
        }

        private void structDecimalSizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var cellStruct = GetCellStruct(_ContextOffset);
            Contract.Assert(cellStruct != null);

            int size = cellStruct.HoverSize > 0 ? cellStruct.HoverSize : cellStruct.Size;
            string text = size.ToString("D", CultureInfo.InvariantCulture);
            Clipboard.SetText(text);
        }

        private void defineStructToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var dialog = new StructureInputDialog())
            {
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                
                string type = dialog.InputStructType;
                if (type == string.Empty)
                {
                    MessageBox.Show(this, "Missing type");
                    return;
                }
                
                string name = dialog.InputStructName;
                if (name == string.Empty)
                {
                    MessageBox.Show(this, "Missing name");
                    return;
                }
                
                InitStructure(type, out byte size, out var color);
                _Structure.MetaInfoList.Add
                (
                    new HexMetaInfo.BytesMetaInfo
                    {
                        Name = name,
                        Offset = SelectedOffset,
                        Size = size,
                        Type = type,
                        Color = color
                    }
                );

                string path = GetConfigPath();
                SaveConfig(path);

                HexViewPanel.Invalidate();
            }
        }

        private void removeStructToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var cellStruct = GetCellStruct(_ContextOffset);
            Contract.Assert(cellStruct != null);
            
            _Structure.MetaInfoList.Remove(cellStruct);

            string path = GetConfigPath();
            SaveConfig(path);

            HexViewPanel.Invalidate();
        }
    }
}
