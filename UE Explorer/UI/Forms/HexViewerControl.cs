using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using Eliot.Utilities;
using UEExplorer.Properties;
using UEExplorer.UI.Dialogs;
using UELib;
using UELib.Core;

namespace UEExplorer.UI.Forms
{
    public partial class HexViewerControl : UserControl
    {
        public delegate void BufferModifiedEventHandler();

        public delegate void OffsetChangedEventHandler(int selectedOffset);

        private const int MAPVK_VK_TO_CHAR = 2;

        private static readonly string[] s_hexTable =
        {
            "00", "01", "02", "03", "04", "05", "06", "07", "08", "09", "0A", "0B", "0C", "0D", "0E", "0F", "10",
            "11", "12", "13", "14", "15", "16", "17", "18", "19", "1A", "1B", "1C", "1D", "1E", "1F", "20", "21",
            "22", "23", "24", "25", "26", "27", "28", "29", "2A", "2B", "2C", "2D", "2E", "2F", "30", "31", "32",
            "33", "34", "35", "36", "37", "38", "39", "3A", "3B", "3C", "3D", "3E", "3F", "40", "41", "42", "43",
            "44", "45", "46", "47", "48", "49", "4A", "4B", "4C", "4D", "4E", "4F", "50", "51", "52", "53", "54",
            "55", "56", "57", "58", "59", "5A", "5B", "5C", "5D", "5E", "5F", "60", "61", "62", "63", "64", "65",
            "66", "67", "68", "69", "6A", "6B", "6C", "6D", "6E", "6F", "70", "71", "72", "73", "74", "75", "76",
            "77", "78", "79", "7A", "7B", "7C", "7D", "7E", "7F", "80", "81", "82", "83", "84", "85", "86", "87",
            "88", "89", "8A", "8B", "8C", "8D", "8E", "8F", "90", "91", "92", "93", "94", "95", "96", "97", "98",
            "99", "9A", "9B", "9C", "9D", "9E", "9F", "A0", "A1", "A2", "A3", "A4", "A5", "A6", "A7", "A8", "A9",
            "AA", "AB", "AC", "AD", "AE", "AF", "B0", "B1", "B2", "B3", "B4", "B5", "B6", "B7", "B8", "B9", "BA",
            "BB", "BC", "BD", "BE", "BF", "C0", "C1", "C2", "C3", "C4", "C5", "C6", "C7", "C8", "C9", "CA", "CB",
            "CC", "CD", "CE", "CF", "D0", "D1", "D2", "D3", "D4", "D5", "D6", "D7", "D8", "D9", "DA", "DB", "DC",
            "DD", "DE", "DF", "E0", "E1", "E2", "E3", "E4", "E5", "E6", "E7", "E8", "E9", "EA", "EB", "EC", "ED",
            "EE", "EF", "F0", "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "FA", "FB", "FC", "FD", "FE",
            "FF"
        };

        private readonly SolidBrush _ActiveNibbleBrush;

        private readonly string _AddressSample;
        private readonly SolidBrush _BorderBrush = new SolidBrush(Color.FromArgb(237, 237, 237));
        private readonly Pen _BorderHoverPen = new Pen(Color.FromArgb(unchecked((int)0x88000000)));

        private readonly Pen _BorderPen;

        private readonly string _ConfigPath =
            Path.Combine(Application.StartupPath, "DataStructures", "{0}", "{1}") + ".xml";

        private readonly SolidBrush _EvenBrush = new SolidBrush(Color.FromArgb(80, 80, 80));
        private readonly SolidBrush _EvenLitBrush = new SolidBrush(Color.FromArgb(112, 32, 32));

        private readonly Brush _ForeBrush;
        private readonly SolidBrush _HoveredBrush = new SolidBrush(Color.FromArgb(unchecked((int)0x880088FF)));
        private readonly SolidBrush _HoveredFieldBrush = new SolidBrush(Color.FromArgb(unchecked((int)0x88000000)));
        private readonly Pen _HoverPen;
        private readonly Pen _LineHoverPen;
        private readonly Pen _LineSelectionPen;
        private readonly SolidBrush _MuteBrush;
        private readonly SolidBrush _OddBrush = new SolidBrush(Color.FromArgb(150, 150, 150));
        private readonly SolidBrush _OddLitBrush = new SolidBrush(Color.FromArgb(182, 102, 102));
        private readonly SolidBrush _OffsetBrush = new SolidBrush(Color.FromArgb(160, 160, 160));
        private readonly SolidBrush _SelectedBrush = new SolidBrush(Color.FromArgb(unchecked((int)0x880000FF)));
        private readonly Pen _SelectionPen;
        private readonly SolidBrush _UnderlineBrush = new SolidBrush(Color.FromArgb(0x55EDEDED));
        private readonly Pen _UnderlinePen;
        private readonly SolidBrush _WhiteForeBrush;

        private int _ActiveNibbleIndex;

        /// <summary>
        ///     Editing byte's buffer index.
        /// </summary>
        private int _ActiveOffset = -1;

        private SizeF _AddressSize;
        private DateTime _CarretStartTime;

        private SolidBrush _EvenCellBrush;

        private bool _LastKeyWasLeft, _LastKeyWasRight;

        private int _SelectedOffset = -1;

        private HexMetaInfo _Structure;

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

        public byte[] Buffer { get; set; }
        public IBuffered Target { get; private set; }

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

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, uint wMsg, IntPtr wParam, IntPtr lParam);

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
            using (var w = new XmlTextWriter(path, Encoding.ASCII))
            {
                var xser = new XmlSerializer(typeof(HexMetaInfo));
                xser.Serialize(w, _Structure);
            }

            _Structure.MetaInfoList.AddRange(backupInfo);
        }

        public void SetHexData(IBuffered target)
        {
            if (target == null)
            {
                return;
            }

            Target = target;
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

        private void UpdateScrollBar()
        {
            if (Buffer == null)
            {
                return;
            }

            double totalLines = Math.Ceiling(Buffer.Length / (float)CellCount) + 2;
            double visibleLines = Math.Ceiling(_HexLinePanel.Height / CellHeight);
            double trailingLines = totalLines % visibleLines;
            double scrollableLines = totalLines - trailingLines;

            _HexViewScrollBar.Minimum = 0;
            _HexViewScrollBar.Maximum = (int)Math.Max(totalLines, 0);
        }

        /// <summary>
        ///     Attempts to update its state to the current's target state.
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
            {
                return;
            }

            if (unStruct.ByteCodeManager?.DeserializedTokens == null ||
                unStruct.ByteCodeManager.DeserializedTokens.Count <= 0)
            {
                return;
            }

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
            int r = (int)(((argb >> 16) & byte.MaxValue) / 255f * max + min);
            int g = (int)(((argb >> 8) & byte.MaxValue) / 255f * max + min);
            int b = (int)((argb & byte.MaxValue) / 255f * max + min);
            return Color.FromArgb(60, r, g, b);
        }

        private string GetConfigPath()
        {
            string folderName = Path.GetFileNameWithoutExtension(Target.GetBufferId(true));
            return string.Format(_ConfigPath, folderName, Target.GetBufferId());
        }

        private void HexLinePanel_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(_BorderPen, 0f, 0f, _HexLinePanel.Width - 1, _HexLinePanel.Height - 1);
            if (Buffer == null)
            {
                return;
            }

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
                    string c = s_hexTable[i];
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
                    bool isOddGroup = i / 4.0F / 1.00 % 2.00 < 1.00;
                    var textBrush = SelectedOffset % CellCount == i ? _SelectedBrush
                        : HoveredOffset % CellCount == i ? _HoveredBrush
                        : isOddGroup ? _EvenBrush : _OffsetBrush;
                    string c = s_hexTable[i];
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

                int maxCells = Math.Min(Buffer.Length - offset, CellCount);
                bool lineIsSelected = offset <= SelectedOffset && offset + CellCount > SelectedOffset;
                bool lineIsHovered = offset <= HoveredOffset && offset + CellCount > HoveredOffset;

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

                string lineText = $"{offset:x8}".PadLeft(8, '0').ToUpper();
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
                        string cellText = s_hexTable[Buffer[byteIndex]];

                        float y1 = lineOffsetY;
                        float y2 = lineOffsetY + extraLineOffset;
                        float x1 = byteColumnOffset + cellIndex * CellWidth;
                        float x2 = byteColumnOffset + (cellIndex + 1) * CellWidth;

                        foreach (var meta in _Structure.MetaInfoList)
                        {
                            int drawSize = hoveredMetaItem == meta || selectedMetaItem == meta
                                ? meta.HoverSize > 0 ? meta.HoverSize : meta.Size
                                : meta.Size;
                            if (byteIndex < meta.Position || byteIndex >= meta.Position + drawSize)
                            {
                                continue;
                            }

                            float cellHeight = extraLineOffset;
                            float cellRectangleY = y1;
                            if (meta.Tag is UStruct.UByteCodeDecompiler.Token)
                            {
                                cellHeight *= 0.5F;
                                cellRectangleY = y1 + (y2 - y1) * 0.5F - cellHeight * 0.5F;
                            }

                            var rectBrush =
                                new SolidBrush(Color.FromArgb(60, meta.Color.R, meta.Color.G, meta.Color.B));
                            e.Graphics.FillRectangle(rectBrush, x1, cellRectangleY, CellWidth, cellHeight);
                            if (HoveredOffset >= meta.Position && HoveredOffset < meta.Position + drawSize)
                            {
                                e.Graphics.DrawLine(_BorderHoverPen, x1, y1, x2, y1); // Top	
                                e.Graphics.DrawLine(_BorderHoverPen, x1, y2, x2, y2); // Bottom

                                if (byteIndex == meta.Position)
                                {
                                    e.Graphics.DrawLine(_BorderHoverPen, x1, y1, x1, y2); // Left
                                }

                                if (byteIndex == meta.Position + drawSize - 1)
                                {
                                    e.Graphics.DrawLine(_BorderHoverPen, x2, y1, x2, y2); // Right
                                }
                            }
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
                            float nibbleWidth = (float)(CellWidth * 0.5);
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

        internal static char FilterByte(byte code)
        {
            if (code >= 0x20 && code <= 0x7E)
            {
                return (char)code;
            }

            return '.';
        }

        public event OffsetChangedEventHandler OffsetChangedEvent;

        private void OnOffsetChangedEvent()
        {
            OffsetChangedEvent?.Invoke(SelectedOffset);
        }

        private void OffsetChanged()
        {
            _HexLinePanel.Invalidate();
            if (SelectedOffset == -1)
            {
                return;
            }

            DissambledObject.Text = string.Empty;
            DissambledName.Text = string.Empty;

            byte[] bufferSelection = new byte[8];
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
            {
                return;
            }

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

                int maxCells = Math.Min(Buffer.Length - offset, CellCount);

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
                string type = e.ClickedItem.Text.Mid(e.ClickedItem.Text.LastIndexOf(' ') + 1);
                dialog.TextBoxName.Text = type;
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                if (dialog.TextBoxName.Text == string.Empty)
                {
                    // Show error box?
                    return;
                }

                InitStructure(type, out byte size, out var color);
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

                _HexLinePanel.Invalidate();
            }
        }

        private void HexLinePanel_MouseMove(object sender, MouseEventArgs e)
        {
            int lastHoveredOffset = HoveredOffset;
            HoveredOffset = GetHoveredByte(e);

            if (lastHoveredOffset == HoveredOffset)
            {
                return;
            }

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
                string message = dataStruct.Name;
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

        private void DataInfoPanel_Paint(object sender, PaintEventArgs e) =>
            e.Graphics.DrawRectangle(_BorderPen, 0f, 0f, DataInfoPanel.Width - 1, DataInfoPanel.Height - 1);

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

        public event BufferModifiedEventHandler BufferModifiedEvent;

        private void OnBufferModifiedEvent()
        {
            BufferModifiedEvent?.Invoke();
        }

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
            {
                return;
            }

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

                int hexKeyIndex = HexKeyCodeToIndex(e.KeyCode);
                if (hexKeyIndex == -1)
                {
                    return;
                }

                _CarretStartTime = DateTime.Now;
                byte newByte = Buffer[_ActiveOffset];
                switch (_ActiveNibbleIndex)
                {
                    case 0:
                        newByte = (byte)((newByte & 0x0F) | (hexKeyIndex << 4));
                        Buffer[_ActiveOffset] = newByte;
                        _ActiveNibbleIndex = 1;
                        break;

                    case 1:
                        newByte = (byte)((newByte & 0xF0) | hexKeyIndex);
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
        private static extern int MapVirtualKey(Keys uCode, int uMapType);

        private static int HexKeyCodeToIndex(Keys keyCode)
        {
            int c = MapVirtualKey(keyCode, MAPVK_VK_TO_CHAR) & ~(1 << 31);
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

        private void HexViewerControl_Scroll(object sender, ScrollEventArgs e) => HexScrollBar_Scroll(sender, e);

        private void HexScrollBar_KeyDown(object sender, KeyEventArgs e) => EditKeyDown(sender, e);

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            switch (m.Msg)
            {
                case 0x20a:
                    {
                        _HexViewScrollBar.Focus();
                        SendMessage(_HexViewScrollBar.Handle, 0x20a, m.WParam, m.LParam);
                        break;
                    }
            }
        }

        public class HexMetaInfo
        {
            public List<BytesMetaInfo> MetaInfoList;

            public class BytesMetaInfo
            {
                [XmlIgnore] public Color Color;

                [XmlIgnore] public int HoverSize;

                public string Name;
                public int Position;

                [XmlIgnore] public int Size;

                [XmlIgnore] public IUnrealDecompilable Tag;

                public string Type;
            }
        }

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
            get => _DrawASCII;
            set
            {
                _DrawASCII = value;
                _HexLinePanel.Invalidate();
            }
        }

        private bool _DrawByte = true;

        public bool DrawByte
        {
            get => _DrawByte;
            set
            {
                _DrawByte = value;
                _HexLinePanel.Invalidate();
            }
        }

        #endregion
    }
}
