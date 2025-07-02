using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using UEExplorer.UI.Dialogs;
using UELib.Annotations;
using UELib.Core;
using UELib.IO;
using UELib.UnrealScript;

namespace UEExplorer.UI.Forms
{
    using Properties;
    using UELib;
    using UStruct = UStruct;

    public partial class HexViewerControl : UserControl
    {
        public byte[] Buffer { get; set; } = [];
        public IBuffered Target { get; private set; }

        public sealed class HexMetaInfo
        {
            public sealed class BytesMetaInfo
            {
                [XmlElement("Position")] public int Offset;

                [XmlIgnore] public int Size;

                [XmlIgnore] public int HoverSize;

                public string Type;

                public string Name;

                [XmlIgnore]
                public Color Color
                {
                    get => _Color;
                    set
                    {
                        Debug.Assert(Brush == null);
                        Brush = new SolidBrush(Color.FromArgb(60, value.R, value.G, value.B));
                        _Color = value;
                    }
                }

                [XmlIgnore] public Brush Brush;

                [XmlIgnore] public object? Tag;

                private Color _Color;
            }

            public List<BytesMetaInfo> MetaInfoList = [];
        }

        private HexMetaInfo _Patterns = new();

        public List<HexMetaInfo.BytesMetaInfo> PatternRegions => _Patterns.MetaInfoList;

        private sealed class CellState
        {
            public bool IsModified;
            public byte OriginalValue;
        }

        private readonly Dictionary<int, CellState> _CellStates = [];

        #region View Properties

        private const int CellCount = 16;
        private const float CellPadding = 6;
        private float CellWidth { get; set; }
        private float CellHeight { get; set; }
        private Font CellFont { get; set; }
        private float ColumnWidth { get; set; }
        private const float ColumnMargin = 8;

        private float NibbleWidth { get; set; }

        private bool _DrawASCII = true;

        public bool DrawASCII
        {
            get => _DrawASCII;
            set
            {
                _DrawASCII = value;
                UpdateView();
            }
        }

        private bool _DrawByte = true;

        public bool DrawByte
        {
            get => _DrawByte;
            set
            {
                _DrawByte = value;
                UpdateView();
            }
        }

        #endregion

        public bool CanEdit { get; set; }

        [DllImport("user32.dll")]
        static extern int SendMessage(IntPtr hWnd, uint wMsg, IntPtr wParam, IntPtr lParam);

        public HexViewerControl()
        {
            InitializeComponent();

            _UnderlinePen = new Pen(_UnderlineBrush);

            _SelectionPen = new Pen(_SelectedBrush);
            _HoverPen = new Pen(_HoveredBrush);
            _HoveredBorderPen = new Pen(_HoveredFieldBrush);

            _LineSelectionPen = _SelectionPen;
            _LineHoverPen = _HoverPen;
            _ForeBrush = new SolidBrush(Color.Black);
            _WhiteForeBrush = new SolidBrush(Color.White);
            _ActiveNibbleBrush = new SolidBrush(Color.FromArgb(unchecked((int)0xFF228282)));

            _AddressSample = $"{99999999:x8}".PadLeft(8, '0').ToUpper();
            _MuteBrush = _EvenBrush;

            HexViewPanel.FontChanged += (_, _) =>
            {
                UpdateScaling();
            };

            UpdateScaling();

            SelectionDataGridView.Rows.AddRange(
                new DataGridViewRow
                {
                    Cells =
                    {
                        new DataGridViewTextBoxCell { Value = "Char" },
                        new DataGridViewTextBoxCell { Value = null, Tag = new UnrealPrimitiveParser<char>('0') }
                    }
                },
                new DataGridViewRow
                {
                    Cells =
                    {
                        new DataGridViewTextBoxCell { Value = "Byte" },
                        new DataGridViewTextBoxCell { Value = null, Tag = new UnrealPrimitiveParser<byte>(0) }
                    }
                },
                new DataGridViewRow
                {
                    Cells =
                    {
                        new DataGridViewTextBoxCell { Value = "Short" },
                        new DataGridViewTextBoxCell { Value = null, Tag = new UnrealPrimitiveParser<short>(0) }
                    }
                },
                new DataGridViewRow
                {
                    Cells =
                    {
                        new DataGridViewTextBoxCell { Value = "UShort" },
                        new DataGridViewTextBoxCell { Value = null, Tag = new UnrealPrimitiveParser<ushort>(0) }
                    }
                },
                new DataGridViewRow
                {
                    Cells =
                    {
                        new DataGridViewTextBoxCell { Value = "Int" },
                        new DataGridViewTextBoxCell { Value = null, Tag = new UnrealPrimitiveParser<int>(0) }
                    }
                },
                new DataGridViewRow
                {
                    Cells =
                    {
                        new DataGridViewTextBoxCell { Value = "UInt" },
                        new DataGridViewTextBoxCell { Value = null, Tag = new UnrealPrimitiveParser<uint>(0) }
                    }
                },
                new DataGridViewRow
                {
                    Cells =
                    {
                        new DataGridViewTextBoxCell { Value = "Long" },
                        new DataGridViewTextBoxCell { Value = null, Tag = new UnrealPrimitiveParser<long>(0) }
                    }
                },
                new DataGridViewRow
                {
                    Cells =
                    {
                        new DataGridViewTextBoxCell { Value = "ULong" },
                        new DataGridViewTextBoxCell { Value = null, Tag = new UnrealPrimitiveParser<ulong>(0) }
                    }
                },
                new DataGridViewRow
                {
                    Cells =
                    {
                        new DataGridViewTextBoxCell { Value = "Float" },
                        new DataGridViewTextBoxCell { Value = null, Tag = new UnrealPrimitiveParser<float>(0) }
                    }
                },
                new DataGridViewRow
                {
                    Cells =
                    {
                        new DataGridViewTextBoxCell { Value = "Object" },
                        new DataGridViewTextBoxCell { Value = null, Tag = new UnrealObjectParser() }
                    }
                },
                new DataGridViewRow
                {
                    Cells =
                    {
                        new DataGridViewTextBoxCell { Value = "Name" },
                        new DataGridViewTextBoxCell { Value = null, Tag = new UnrealNameParser() }
                    }
                },
                new DataGridViewRow
                {
                    Cells =
                    {
                        new DataGridViewTextBoxCell { Value = "Index" },
                        new DataGridViewTextBoxCell { Value = null, Tag = new UnrealIndexParser() }
                    }
                },
                new DataGridViewRow
                {
                    Cells =
                    {
                        new DataGridViewTextBoxCell { Value = "Struct" },
                        new DataGridViewTextBoxCell { Value = null, Tag = new HexPatternParser(this) }
                    }
                }
            );
        }

        private void UpdateScaling()
        {
            CellFont = HexViewPanel.Font;

            CellModifiedFont?.Dispose();
            CellModifiedFont = new Font(CellFont, FontStyle.Italic | FontStyle.Bold);

            var fontSize = TextRenderer.MeasureText("DD", CellFont,
                new Size((int)CellPadding, (int)CellPadding),
                TextFormatFlags.NoClipping
            );
            int cellSize = Math.Max(fontSize.Width, fontSize.Height);
            CellWidth = cellSize;
            CellHeight = fontSize.Height;
            ColumnWidth = CellCount * CellWidth;

            _AddressSize = TextRenderer.MeasureText(_AddressSample, CellFont);

            NibbleWidth = CellWidth * 0.5f;

            _ActiveNibblePen?.Dispose();
            _ActiveNibblePen = new Pen(
                _ActiveNibbleBrush,
                NibbleWidth
            );
        }

        private IDisposable _MouseInputSubscription;

        public sealed class HexMessageFilter(HexViewerControl hexViewer) : IMessageFilter
        {
            public bool PreFilterMessage(ref Message m)
            {
                if (m.Msg != 0x0100)
                {
                    return false;
                }

                if (m.WParam.ToInt32() != (int)Keys.Left &&
                    m.WParam.ToInt32() != (int)Keys.Right &&
                    m.WParam.ToInt32() != (int)Keys.Up &&
                    m.WParam.ToInt32() != (int)Keys.Down)
                {
                    return false;
                }

                if (hexViewer.IsSelecting())
                {
                    hexViewer.HexViewPanel_KeyDown(hexViewer, new KeyEventArgs((Keys)m.WParam.ToInt32()));
                }

                return true;
            }
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            FindForm()!.Deactivate -= HexViewerForm_Deactivate;

            if (_HexMessageFilter != null)
            {
                Application.RemoveMessageFilter(_HexMessageFilter);
            }

            _MouseInputSubscription.Dispose();

            _ForeBrush.Dispose();
            _BorderBrush.Dispose();
            _UnderlineBrush.Dispose();
            _EvenBrush.Dispose();
            _EvenLitBrush.Dispose();
            _OddBrush.Dispose();
            _OddLitBrush.Dispose();
            _OffsetBrush.Dispose();
            _SelectedBrush.Dispose();
            _HoveredBrush.Dispose();
            _HoveredFieldBrush.Dispose();
            _HoveredBorderPen.Dispose();
            _WhiteForeBrush.Dispose();
            _ActiveNibbleBrush.Dispose();
            _MuteBrush.Dispose();

            base.OnHandleDestroyed(e);
        }

        private void HexViewerControl_Load(object sender, EventArgs e)
        {
            var focusPanel = HexViewPanel;

            var mouseMoveObservable = Observable
                .FromEvent<MouseEventHandler, MouseEventArgs>(
                    handler => (_, e) => { handler(e); },
                    handler => focusPanel.MouseMove += handler,
                    handler => focusPanel.MouseMove -= handler
                );

            var mouseDownObservable = Observable
                .FromEvent<MouseEventHandler, MouseEventArgs>(
                    handler => (_, e) => { handler(e); },
                    handler => focusPanel.MouseDown += handler,
                    handler => focusPanel.MouseDown -= handler
                );

            var mouseUpObservable = Observable
                .FromEvent<MouseEventHandler, MouseEventArgs>(
                    handler => (_, e) => { handler(e); },
                    handler => focusPanel.MouseUp += handler,
                    handler => focusPanel.MouseUp -= handler
                );

            _MouseInputSubscription = mouseDownObservable
                .Where(mouseEvent =>
                    mouseEvent.Button == MouseButtons.Left && GetCellIndex(mouseEvent.X, mouseEvent.Y) != -1)
                .Select(mouseStartEvent =>
                {
                    Focus();

                    // In order to hook key inputs again (focus is not reliable)
                    HexViewerControl_Enter(this, EventArgs.Empty);

                    focusPanel.Capture = true;
                    Cursor.Clip = focusPanel.RectangleToScreen(focusPanel.ClientRectangle);

                    int startIndex, stopIndex, clickIndex;
                    if ((ModifierKeys & Keys.Shift) != 0 && Selection.HasValue)
                    {
                        stopIndex = GetCellIndex(mouseStartEvent.X, mouseStartEvent.Y);
                        startIndex = stopIndex < Selection.Value.Start.Value
                            ? Selection.Value.End.Value
                            : Selection.Value.Start.Value;

                        clickIndex = startIndex;

                        // Swap if we are selecting backwards.
                        if (stopIndex < startIndex)
                        {
                            (startIndex, stopIndex) = (stopIndex, startIndex);
                        }
                    }
                    else
                    {
                        startIndex = GetCellIndex(mouseStartEvent.X, mouseStartEvent.Y);
                        stopIndex = startIndex;
                        clickIndex = startIndex;
                    }

                    _DirectionalOffset = clickIndex;
                    Selection = new Range(startIndex, stopIndex);

                    // End editing.
                    SetActiveCell(-1);

                    return clickIndex;
                })
                .Select(clickIndex => mouseMoveObservable
                    .Do(mouseEvent =>
                    {
                        // Scroll according to mouse position.
                        var screenMouse = focusPanel.PointToScreen(new Point(mouseEvent.X, mouseEvent.Y));
                        if (screenMouse.Y + CellHeight >= Cursor.Clip.Bottom)
                        {
                            HexViewScrollBar.Value = Math.Min(
                                HexViewScrollBar.Maximum,
                                HexViewScrollBar.Value + 1
                            );

                            UpdateView();
                        }
                        else if (screenMouse.Y - CellHeight * 2.5 <= Cursor.Clip.Top)
                        {
                            HexViewScrollBar.Value = Math.Max(
                                HexViewScrollBar.Minimum,
                                HexViewScrollBar.Value - 1
                            );

                            UpdateView();
                        }
                    })
                    .Do(mouseEvent =>
                    {
                        int startIndex = clickIndex;

                        int stopIndex = GetCellIndex(mouseEvent.X, mouseEvent.Y);
                        if (stopIndex == -1)
                        {
                            return;
                        }

                        // Swap if we are selecting backwards.
                        if (stopIndex < startIndex)
                        {
                            (startIndex, stopIndex) = (stopIndex, startIndex);
                        }

                        Selection = new Range(startIndex, stopIndex);

                        UpdateView();
                    })
                    .TakeUntil(mouseUpObservable
                        .Where(_ => (MouseButtons & MouseButtons.Left) == 0)
                        .Do(mouseEvent =>
                        {
                            // So we can figure out the direction to shrink or expand, when using the shift key and arrows.
                            int stopIndex = GetCellIndex(mouseEvent.X, mouseEvent.Y);
                            _DirectionalOffset = stopIndex;
                        }))
                    .Finally(() =>
                    {
                        Cursor.Clip = Rectangle.Empty;

                        ActiveControl = focusPanel;

                        focusPanel.Capture = false;
                    }))
                .Switch()
                .Subscribe();

            // Unhook key input when leaving the dialog, somehow the 'Leave' event is not triggered when leaving the dialog.
            FindForm()!.Deactivate += HexViewerForm_Deactivate;
        }

        private void UpdateView()
        {
            HexViewPanel.Invalidate();
        }

        private void LoadConfig(string path)
        {
            using var reader = new XmlTextReader(path);
            var serializer = new XmlSerializer(typeof(HexMetaInfo));
            _Patterns = (HexMetaInfo)serializer.Deserialize(reader)!;

            foreach (var pattern in _Patterns.MetaInfoList.Where(pattern => pattern.Type != "Generated"))
            {
                InitStructure(pattern.Type, out byte size, out var color);
                pattern.Size = size;
                pattern.Color = color;
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

            var backupInfo = _Patterns.MetaInfoList.ToArray();
            _Patterns.MetaInfoList.RemoveAll(i => i.Type == "Generated");
            using var writer = new XmlTextWriter(path, System.Text.Encoding.ASCII);
            var serializer = new XmlSerializer(typeof(HexMetaInfo));
            serializer.Serialize(writer, _Patterns);

            _Patterns.MetaInfoList.AddRange(backupInfo);
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

            InitializeMetaInfoFields();

            TargetChangedEvent?.Invoke(this, target);
            SelectedOffset = -1;

            UpdateView();
        }

        private void SetCellValue(int cellIndex, byte cellValue)
        {
            byte originalValue = GetCellValue(cellIndex);
            Buffer[cellIndex] = cellValue;
            if (_CellStates.TryGetValue(cellIndex, out var cellState))
            {
                cellState.IsModified = true;
            }
            else
            {
                _CellStates.Add(cellIndex, new CellState { IsModified = true, OriginalValue = originalValue });
            }
        }

        private void RestoreCellValue(int cellIndex)
        {
            if (!_CellStates.TryGetValue(cellIndex, out var cellState) || !cellState.IsModified)
            {
                return;
            }

            Buffer[cellIndex] = cellState.OriginalValue;
            _CellStates.Remove(cellIndex);
        }

        private byte GetCellValue(int cellIndex)
        {
            return Buffer[cellIndex];
        }

        private CellState? GetCellState(int cellIndex)
        {
            _CellStates.TryGetValue(cellIndex, out var cellState);
            return cellState;
        }

        private HexMetaInfo.BytesMetaInfo? GetCellStruct(int cellIndex)
        {
            if (cellIndex == -1)
            {
                return null;
            }

            var cellStruct = _Patterns.MetaInfoList.Find(pattern =>
                cellIndex >= pattern.Offset && cellIndex <= pattern.Offset + pattern.Size);

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
            _Patterns.MetaInfoList.Clear();
            InitializeMetaInfoFields();
        }

        private void InitializeMetaInfoFields()
        {
            // Re-load the contents with a recording stream!
            if (Target is UObject { ExportTable: not null } uObject)
            {
                uObject.Load<UObjectRecordStream>();
                if (uObject.ThrownException != null)
                {
                    _Patterns.MetaInfoList.Add
                    (
                        new HexMetaInfo.BytesMetaInfo
                        {
                            Offset = (int)uObject.ExceptionPosition,
                            Size = (int)(uObject.ExportTable.SerialSize - uObject.ExceptionPosition),
                            Type = uObject.ThrownException.GetType().Name,
                            Color = Color.Red,
                            Name = uObject.ThrownException.GetType().Name
                        }
                    );
                }
            }

            if (Target is IBinaryData { BinaryMetaData: not null } binaryTarget)
            {
                foreach (var binaryField in binaryTarget.BinaryMetaData.Fields)
                {
                    int colorHash = binaryField.Field.GetHashCode()
                                    ^ (int)binaryField.Size
                                    ^ (binaryField.Value?.GetType().GetHashCode() ?? 1);
                    _Patterns.MetaInfoList.Add
                    (
                        new HexMetaInfo.BytesMetaInfo
                        {
                            Offset = (int)binaryField.Offset,
                            Size = (int)binaryField.Size,
                            Type = "Generated",
                            Color = NormalizeArgbToColor(colorHash),
                            Name = binaryField.Field,
                            Tag = binaryField
                        }
                    );
                }
            }

            if (Target is not UStruct unStruct)
            {
                return;
            }

            if (unStruct.Script == null)
            {
                return;
            }

            // Have the tokens been deserialized yet? (Lazy loaded for UE3)
            if (unStruct.Script.Tokens.Count == 0 && unStruct.ScriptSize > 0)
            {
                using var stream = unStruct.LoadStream<UObjectStream>(unStruct.Package.Stream);
                stream.Seek(unStruct.ScriptOffset, SeekOrigin.Begin);
                unStruct.Script.Deserialize(stream);
            }

            foreach (var token in unStruct.Script.Tokens)
            {
                _Patterns.MetaInfoList.Add
                (
                    new HexMetaInfo.BytesMetaInfo
                    {
                        Offset = token.StoragePosition + (int)unStruct.ScriptOffset,
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
            Path.Combine(Application.UserAppDataPath, "DataStructures", "{0}", "{1}") + ".xml";

        private string GetConfigPath()
        {
            var folderName = Path.GetFileNameWithoutExtension(Target.GetBufferId());
            return string.Format(_ConfigPath, folderName, Target.GetBufferId());
        }

        [EditorBrowsable] public Font CellModifiedFont { get; set; }

        private readonly Brush _ForeBrush;
        private readonly SolidBrush _BorderBrush = new(Color.FromArgb(237, 237, 237));
        private readonly SolidBrush _UnderlineBrush = new(Color.FromArgb(0x55EDEDED));
        private readonly SolidBrush _EvenBrush = new(Color.FromArgb(80, 80, 80));
        private readonly SolidBrush _EvenLitBrush = new(Color.FromArgb(112, 32, 32));
        private readonly SolidBrush _OddBrush = new(Color.FromArgb(150, 150, 150));
        private readonly SolidBrush _OddLitBrush = new(Color.FromArgb(182, 102, 102));
        private readonly SolidBrush _OffsetBrush = new(Color.FromArgb(160, 160, 160));
        private readonly SolidBrush _SelectedBrush = new(Color.FromArgb(unchecked((int)0xFF0000FF)));
        private readonly SolidBrush _HoveredBrush = new(Color.FromArgb(unchecked((int)0x880088FF)));
        private readonly SolidBrush _HoveredFieldBrush = new(Color.FromArgb(unchecked((int)0x88000000)));

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
            var g = e.Graphics;
            if (HexViewPanel.Focused)
            {
                //g.DrawRectangle(_BorderPen, e.ClipRectangle);
            }

            float addressColumnOffset = ColumnMargin;
            float addressColumnWidth = _AddressSize.Width;
            float byteColumnOffset = _DrawByte
                ? addressColumnOffset + addressColumnWidth + ColumnMargin
                : 0;
            float asciiColumnOffset = _DrawByte
                ? byteColumnOffset + ColumnWidth + ColumnMargin
                : byteColumnOffset;

            string text = Resources.HexView_Offset;

            TextRenderer.DrawText(g, text, CellFont,
                new Point((int)addressColumnOffset, (int)ColumnMargin),
                SystemColors.ControlText,
                TextFormatFlags.NoClipping
            );
            g.DrawLine(_UnderlinePen,
                addressColumnOffset, ColumnMargin + CellHeight,
                addressColumnOffset + byteColumnOffset - ColumnMargin, ColumnMargin + CellHeight
            );

            int offset = CellCount * HexViewScrollBar.Value;
            int lineCount = Math.Min(
                (int)(HexViewPanel.ClientSize.Height / CellHeight),
                (Buffer.Length - offset) / CellCount + ((Buffer.Length - offset) % CellCount > 0 ? 1 : 0)
            );

            if (_DrawByte)
            {
                float x = byteColumnOffset;
                float y = ColumnMargin;

                //g.FillRectangle( new SolidBrush( Color.FromArgb(44, 44, 44) ), x, y, ColumnSize, _LineSpacing );
                g.DrawLine(_UnderlinePen,
                    x, y + CellHeight,
                    x + ColumnWidth, y + CellHeight
                );

                for (int i = 0; i < CellCount; ++i)
                {
                    var textBrush = SelectedOffset % CellCount == i ? _SelectedBrush
                        : HoveredOffset % CellCount == i ? _HoveredBrush
                        : i / 4.0F / 1.00 % 2.00 < 1.00 ? _EvenBrush : _OffsetBrush;
                    string c = HexTable[i];
                    TextRenderer.DrawText(g, c, CellFont,
                        new Point((int)(x + i * CellWidth), (int)y),
                        textBrush.Color,
                        TextFormatFlags.NoClipping
                    );
                }
            }

            if (_DrawASCII)
            {
                float x = asciiColumnOffset;
                float y = ColumnMargin;

                //g.FillRectangle( new SolidBrush( Color.FromArgb(44, 44, 44) ), x, y, ColumnSize, _LineSpacing );
                g.DrawLine(_UnderlinePen,
                    x, y + CellHeight,
                    x + ColumnWidth, y + CellHeight
                );

                // Replaced g.DrawString with TextRenderer.DrawText
                TextRenderer.DrawText(g, "ASCII", CellFont, new Point((int)x, (int)y), _OffsetBrush.Color, TextFormatFlags.NoClipping);
            }

            int lineOffsetY = (int)(ColumnMargin + CellHeight + CellHeight * .5);
            float extraLineOffset = CellHeight;
            for (int line = 0; line < lineCount; ++line)
            {
                if (lineOffsetY >= HexViewPanel.ClientSize.Height)
                {
                    break;
                }

                var maxCells = Math.Min(Buffer.Length - offset, CellCount);
                var lineIsSelected = _Selection.HasValue && _Selection.Value.Start.Value - offset < CellCount &&
                                     offset <= _Selection.Value.End.Value;
                var lineIsHovered = offset <= HoveredOffset && offset + CellCount > HoveredOffset;

                if (lineIsSelected)
                {
                    g.DrawLine
                    (
                        _LineSelectionPen,
                        0, lineOffsetY + extraLineOffset,
                        (SelectedOffset - offset) * CellWidth + byteColumnOffset, lineOffsetY + extraLineOffset
                    );
                }

                if (lineIsHovered)
                {
                    g.DrawLine
                    (
                        _LineHoverPen,
                        0, lineOffsetY + extraLineOffset,
                        (HoveredOffset - offset) * CellWidth + byteColumnOffset, lineOffsetY + extraLineOffset
                    );
                }

                string lineText = $"{offset:X8}".PadLeft(8, '0');
                var textBrush = line % 2 == 0 ? _EvenBrush : _OddBrush;
                var lineBrush = lineIsSelected ? _SelectedBrush : lineIsHovered ? _HoveredBrush : textBrush;

                TextRenderer.DrawText(g, lineText, CellFont,
                    new Point((int)addressColumnOffset, lineOffsetY),
                    lineBrush.Color,
                    TextFormatFlags.NoClipping
                );

                if (_DrawByte)
                {
                    // TODO: Spatial hashing?
                    var hoveredMetaItem = HoveredOffset != -1
                        ? _Patterns.MetaInfoList.Find(pattern => pattern.Offset == HoveredOffset && pattern.Tag is UStruct.UByteCodeDecompiler.Token)
                        : null;

                    var selectedMetaItem = SelectedOffset != -1
                        ? _Patterns.MetaInfoList.Find(pattern => pattern.Offset == SelectedOffset && pattern.Tag is UStruct.UByteCodeDecompiler.Token)
                        : null;

                    for (int cellIndex = 0; cellIndex < maxCells; ++cellIndex)
                    {
                        int byteIndex = offset + cellIndex;
                        var cellTextColor = cellIndex % 4 == 0
                            ? SystemColors.ControlText
                            : textBrush.Color;
                        string cellText = HexTable[Buffer[byteIndex]];

                        float y1 = lineOffsetY;
                        float y2 = lineOffsetY + extraLineOffset;
                        float x1 = byteColumnOffset + cellIndex * CellWidth;
                        float x2 = byteColumnOffset + (cellIndex + 1) * CellWidth;

                        foreach (var pattern in _Patterns.MetaInfoList)
                        {
                            var drawSize = hoveredMetaItem == pattern || selectedMetaItem == pattern
                                ? pattern.HoverSize > 0 ? pattern.HoverSize : pattern.Size
                                : pattern.Size;
                            if (byteIndex < pattern.Offset || byteIndex >= pattern.Offset + drawSize)
                                continue;

                            float cellHeight = extraLineOffset;
                            float cellRectangleY = y1;
                            if (pattern is { Size: 1, Tag: UStruct.UByteCodeDecompiler.Token })
                            {
                                cellHeight *= 0.5F;
                                cellRectangleY = y1 + (y2 - y1) * 0.5F - cellHeight * 0.5F;
                            }

                            var rectBrush = pattern.Brush;
                            g.FillRectangle(rectBrush, x1, cellRectangleY, CellWidth, cellHeight);

                            if (HoveredOffset == -1 || HoveredOffset < pattern.Offset || HoveredOffset >= pattern.Offset + drawSize)
                            {
                                continue;
                            }

                            g.DrawLine(_HoveredBorderPen, x1, y1, x2, y1); // Top	
                            g.DrawLine(_HoveredBorderPen, x1, y2, x2, y2); // Bottom

                            if (byteIndex == pattern.Offset)
                                g.DrawLine(_HoveredBorderPen, x1, y1, x1, y2); // Left

                            if (byteIndex == pattern.Offset + drawSize - 1)
                                g.DrawLine(_HoveredBorderPen, x2, y1, x2, y2); // Right

                        }

                        var cellFont = CellFont;
                        if (byteIndex != _ActiveOffset)
                        {
                            if (_CellStates.Count > 0 && _CellStates.TryGetValue(byteIndex, out var cellState) && cellState.IsModified)
                            {
                                cellFont = CellModifiedFont;
                            }

                            TextRenderer.DrawText(g, cellText, cellFont,
                                new Point(
                                    (int)(byteColumnOffset + cellIndex * CellWidth),
                                    lineOffsetY
                                ),
                                cellTextColor,
                                TextFormatFlags.NoClipping
                            );
                        }
                        // Render edit caret.
                        else
                        {
                            using var cellTextBrushAlternate = new SolidBrush(Color.FromArgb(
                                cellTextColor.A,
                                (int)(cellTextColor.R * 0.7f),
                                (int)(cellTextColor.G * 0.7f),
                                (int)(cellTextColor.B * 0.7f)
                            ));

                            g.FillRectangle(cellTextBrushAlternate,
                                x1, y1,
                                CellWidth, CellHeight
                            );

                            if (_CaretTick)
                            {
                                switch (_ActiveNibbleIndex)
                                {
                                    case 0:
                                        g.DrawLine(_ActiveNibblePen,
                                            x1 + NibbleWidth * 0.5F, y1, x1 + NibbleWidth * 0.5F, y2
                                        );
                                        break;

                                    case 1:
                                        g.DrawLine(_ActiveNibblePen,
                                            x1 + NibbleWidth + NibbleWidth * 0.5F, y1,
                                            x1 + NibbleWidth + NibbleWidth * 0.5F, y2
                                        );
                                        break;
                                }
                            }

                            TextRenderer.DrawText(g, cellText, cellFont,
                                new Point((int)(byteColumnOffset + cellIndex * CellWidth), lineOffsetY),
                                _WhiteForeBrush.Color,
                                TextFormatFlags.NoClipping
                            );
                        }

                        if (Selection.HasValue)
                        {
                            if (byteIndex >= Selection.Value.Start.Value &&
                                byteIndex <= Selection.Value.End.Value)
                            {
                                // Draw the selection.
                                var drawPen = _SelectionPen;
                                g.DrawRectangle(drawPen,
                                    byteColumnOffset + cellIndex * CellWidth,
                                    lineOffsetY,
                                    CellWidth,
                                    CellHeight
                                );
                            }
                        }

                        if (byteIndex == HoveredOffset)
                        {
                            var drawPen = _HoverPen;
                            g.DrawRectangle(drawPen,
                                byteColumnOffset + cellIndex * CellWidth,
                                lineOffsetY,
                                CellWidth,
                                CellHeight
                            );
                        }
                    }
                }

                if (_DrawASCII)
                {
                    int cellWidth = (int)CellWidth;
                    int cellHeight = (int)CellHeight;
                    for (int cellIndex = 0; cellIndex < maxCells; ++cellIndex)
                    {
                        int byteIndex = offset + cellIndex;

                        if (Selection.HasValue)
                        {
                            if (byteIndex >= Selection.Value.Start.Value &&
                                byteIndex <= Selection.Value.End.Value)
                            {
                                // Draw the selection.
                                var drawPen = _SelectionPen;
                                g.DrawRectangle(drawPen,
                                    asciiColumnOffset + cellIndex * cellWidth,
                                    lineOffsetY,
                                    cellWidth,
                                    cellHeight
                                );
                            }
                        }

                        if (byteIndex == HoveredOffset)
                        {
                            var drawPen = _HoverPen;
                            g.DrawRectangle(drawPen,
                                asciiColumnOffset + cellIndex * cellWidth,
                                lineOffsetY,
                                cellWidth,
                                cellHeight
                            );
                        }

                        string drawnChar;
                        Color charColor;
                        switch (Buffer[byteIndex])
                        {
                            case 0x09:
                                drawnChar = "\\t";
                                charColor = SystemColors.ControlLight;
                                break;

                            case 0x0A:
                                drawnChar = "\\n";
                                charColor = SystemColors.ControlLight;
                                break;

                            case 0x0D:
                                drawnChar = "\\r";
                                charColor = SystemColors.ControlLight;
                                break;

                            default:
                                drawnChar = FilterByte(Buffer[byteIndex]).ToString(CultureInfo.InvariantCulture);
                                charColor = drawnChar == "." ? _MuteBrush.Color : textBrush.Color;
                                break;
                        }

                        TextRenderer.DrawText(g, drawnChar, CellFont,
                            new Point((int)(asciiColumnOffset + cellIndex * cellWidth), lineOffsetY),
                            charColor,
                            TextFormatFlags.NoPadding
                        );
                    }
                }

                offset += maxCells;
                lineOffsetY += (int)extraLineOffset;
            }
        }

        private static readonly string[] HexTable =
        [
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
        ];

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
        private bool _CaretTick;

        private int SelectedOffset
        {
            get { return Selection.HasValue ? Selection.Value.Start.Value : -1; }
            set
            {
                _DirectionalOffset = value;

                Selection = value == -1
                    ? null
                    : new Range(value, value);
            }
        }

        /// <summary>
        /// Initial selection offset.
        /// </summary>
        private int _DirectionalOffset;

        private int SelectionStart => Selection.HasValue ? Selection.Value.Start.Value : -1;
        private int SelectionEnd => Selection.HasValue ? Selection.Value.End.Value : -1;

        private int SelectionLength
        {
            get => Selection.HasValue ? Math.Abs(Selection.Value.End.Value - Selection.Value.Start.Value + 1) : 0;
        }

        private Range? Selection
        {
            get => _Selection;
            set
            {
                if (value == null)
                {
                    _DirectionalOffset = -1;
                }

                _Selection = value;
                OnOffsetChangedEvent();
            }
        }

        private int SelectionLockStart = 0;
        private int SelectionLockEnd => Buffer.Length - 1;

        private int _ContextOffset = -1;
        private int HoveredOffset { get; set; } = -1;

        public delegate void OffsetChangedEventHandler(int selectionOffset, int selectionLength);

        public event OffsetChangedEventHandler? OffsetChangedEvent;

        private void OnOffsetChangedEvent()
        {
            OffsetChangedEvent?.Invoke(SelectedOffset, SelectionLength);
            OffsetChanged();
        }

        private int _LastUpdatedPosition;
        private int _LastUpdatedSize;

        private void OffsetChanged()
        {
            if (_LastUpdatedPosition == SelectedOffset && _LastUpdatedSize == SelectionLength)
            {
                return;
            }

            _LastUpdatedPosition = SelectedOffset;
            _LastUpdatedSize = SelectionLength;

            if (SelectedOffset == -1)
            {
                foreach (DataGridViewRow row in SelectionDataGridView.Rows)
                {
                    row.Cells["Value"].Value = null;
                }

                return;
            }

            var buffer = Target.GetBuffer();
            int position = Target.GetBufferPosition() + SelectedOffset;

            foreach (DataGridViewRow row in SelectionDataGridView.Rows)
            {
                var valueCell = row.Cells["Value"];
                if (valueCell.Tag is not IUnrealPatternParser patternMatcher)
                {
                    continue;
                }

                try
                {
                    buffer.Seek(position, SeekOrigin.Begin);
                    object? value = patternMatcher.Parse(buffer, SelectedOffset, SelectionLength);
                    valueCell.Value = value;
                }
                catch
                {
                    valueCell.Value = null;
                }
            }
        }

        private void HexScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            UpdateView();
        }

        private void HexLinePanel_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void HexLinePanel_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (!CanEdit)
            {
                return;
            }

            int cellIndex = GetCellIndex(e.X, e.Y);
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

            if (_ActiveOffset != -1)
            {
                caretTimer.Stop(); // reset blink

                _CaretTick = true;
                caretTimer.Enabled = true;
                caretTimer.Start();
            }
            else
            {
                _CaretTick = false;
                caretTimer.Enabled = false;
                caretTimer.Stop();
            }

            UpdateView();
        }

        private int GetCellIndex(float x, float y)
        {
            x -= HexViewPanel.Location.X;
            y -= HexViewPanel.Location.Y;

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

                int maxCells = Math.Min(Buffer.Length - offset, CellCount);

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
            int lastHoveredOffset = HoveredOffset;
            HoveredOffset = GetCellIndex(e.X, e.Y);

            if (lastHoveredOffset == HoveredOffset)
            {
                return;
            }

            UpdateView();

            var pattern = HoveredOffset != -1
                ? _Patterns.MetaInfoList.Find(i => HoveredOffset >= i.Offset && HoveredOffset < i.Offset + i.Size)
                : null;

            if (pattern == null)
            {
                HexToolTip.Hide(this);

                return;
            }

            string message = pattern.Name;
            if (pattern.Tag != null)
            {
                try
                {
                    // Restart token index.
                    if (pattern is { Size: 1, Tag: UStruct.UByteCodeDecompiler.Token token })
                    {
                        var decompiler = new UStruct.UByteCodeDecompiler((UStruct)Target);

                        message += "\r\n" +
                                   $"\r\nOffset: {PropertyDisplay.FormatOffset(token.Position)}:{PropertyDisplay.FormatOffset(token.StoragePosition)}" +
                                   $"\r\nSize: {PropertyDisplay.FormatOffset(token.Size)}:{PropertyDisplay.FormatOffset(token.StorageSize)}";
                        decompiler.JumpTo((ushort)token.Position);
                        message += "\r\n\r\n" + token.Decompile(decompiler);
                    }
                    else if (pattern.Tag is IUnrealDecompilable decompilable)
                    {
                        message += "\r\n\r\n" + decompilable.Decompile();
                    }

                }
                catch
                {
                    message += "\r\n\r\n" + Resources.HexView_COULDNT_ACQUIRE_VALUE;
                }
            }

            var toolTipPoint = PointToClient(MousePosition);
            HexToolTip.Show(message, this,
                toolTipPoint.X + (int)(Cursor.Size.Width * 0.5f),
                toolTipPoint.Y + (int)(Cursor.Size.Height * 0.5f),
                4000
            );
        }

        public delegate void BufferModifiedEventHandler();

        public event BufferModifiedEventHandler? BufferModifiedEvent;

        private void OnBufferModifiedEvent()
        {
            BufferModifiedEvent?.Invoke();
        }

        public event EventHandler<IBuffered>? TargetChangedEvent;

        private SizeF _AddressSize;
        private Pen _ActiveNibblePen;
        private Range? _Selection;
        private HexMessageFilter? _HexMessageFilter;
        private Pen _HoveredBorderPen;

        private void HexViewPanel_KeyDown(object sender, KeyEventArgs e)
        {
            if (IsEditing())
            {
                int currentCellIndex = _ActiveOffset;

                if (ModifierKeys == 0)
                {
                    int hexKeyIndex = HexKeyCodeToIndex(e.KeyCode);
                    if (hexKeyIndex != -1)
                    {
                        byte newByte = GetCellValue(currentCellIndex);
                        switch (_ActiveNibbleIndex)
                        {
                            case 0:
                                newByte = (byte)((newByte & 0x0F) | (hexKeyIndex << 4));
                                SetCellValue(currentCellIndex, newByte);
                                SetActiveCell(currentCellIndex); // to update the caret blink
                                _ActiveNibbleIndex = 1;
                                break;

                            case 1:
                                newByte = (byte)((newByte & 0xF0) | hexKeyIndex);
                                SetCellValue(currentCellIndex, newByte);
                                _ActiveNibbleIndex = 0;
                                // Move the active cell index to the next cell
                                int nextCellIndex = Math.Min(currentCellIndex + 1, Buffer.Length - 1);
                                SetActiveCell(nextCellIndex);
                                break;
                        }

                        UpdateView();
                        OnBufferModifiedEvent();
                    }
                    else
                    {
                        switch (e.KeyCode)
                        {
                            case Keys.Left:
                                _ActiveNibbleIndex = 0;
                                UpdateView();
                                break;

                            case Keys.Right:
                                _ActiveNibbleIndex = 1;
                                UpdateView();
                                break;
                        }
                    }
                }
                else if ((ModifierKeys & Keys.Control) != 0)
                {
                    if (e.KeyCode == Keys.X)
                    {
                        RestoreCellValue(currentCellIndex);
                    }
                }

                e.Handled = true;
                e.SuppressKeyPress = true;
            }
            else if (IsSelecting())
            {
                switch (e.KeyCode)
                {
                    case Keys.Up:
                        HexViewScrollBar.Value = Math.Max(HexViewScrollBar.Value - 1, HexViewScrollBar.Minimum);
                        if ((ModifierKeys & Keys.Shift) != 0)
                        {
                            ShiftSelection(-CellCount);
                        }
                        else
                        {
                            SelectedOffset = Math.Max(SelectedOffset - CellCount, 0);
                        }

                        break;

                    case Keys.Down:
                        HexViewScrollBar.Value = Math.Min(HexViewScrollBar.Value + 1, HexViewScrollBar.Maximum);
                        if ((ModifierKeys & Keys.Shift) != 0)
                        {
                            ShiftSelection(CellCount);
                        }
                        else
                        {
                            SelectedOffset = Math.Min(SelectedOffset + CellCount, SelectionLockEnd);
                        }

                        break;

                    case Keys.Left:
                        if ((ModifierKeys & Keys.Shift) != 0)
                        {
                            ShiftSelection(-1);
                        }
                        else
                        {
                            SelectedOffset = Math.Max(SelectedOffset - 1, 0);
                        }

                        break;

                    case Keys.Right:
                        if ((ModifierKeys & Keys.Shift) != 0)
                        {
                            ShiftSelection(1);
                        }
                        else
                        {
                            SelectedOffset = Math.Min(SelectedOffset + 1, SelectionLockEnd);
                        }

                        break;
                }

                e.SuppressKeyPress = true;
                e.Handled = true;

                UpdateView();
            }

            return;

            void ShiftSelection(int count)
            {
                int selectionLockStart = SelectionLockStart;
                int selectionLockEnd = SelectionLockEnd;

                if (_DirectionalOffset == SelectionStart) // re-size backward
                {
                    int selectionStart = Math.Max(Math.Min(SelectionStart + count, selectionLockEnd),
                        selectionLockStart);
                    int selectionEnd;
                    if (selectionStart <= SelectionEnd)
                    {
                        selectionEnd = SelectionEnd;
                        _DirectionalOffset = selectionStart;
                    }
                    else
                    {
                        // Swap
                        selectionEnd = selectionStart;
                        selectionStart = SelectionEnd;
                        _DirectionalOffset = selectionEnd;
                    }

                    Selection = new Range(selectionStart, selectionEnd);
                }
                else if (_DirectionalOffset == SelectionEnd) // re-size forward
                {
                    int selectionStart = SelectionStart;
                    int selectionEnd = Math.Max(Math.Min(SelectionEnd + count, selectionLockEnd), selectionLockStart);
                    if (selectionStart > selectionEnd) // front -16 re-size?
                    {
                        selectionStart = selectionEnd;
                        selectionEnd = SelectionEnd;
                        _DirectionalOffset = selectionStart;
                    }
                    else
                    {
                        _DirectionalOffset = selectionEnd;
                    }

                    Selection = new Range(selectionStart, selectionEnd);
                }
                else
                {
                    throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void HexViewPanel_KeyUp(object sender, KeyEventArgs e)
        {
            if (!IsSelecting())
            {
                return;
            }

            // TODO: Migrate to MainMenu->Edit dropdown
            if (e.Control)
            {
                if (e.KeyCode == Keys.C)
                {
                    int currentCellIndex = SelectedOffset;

                    if (e.Shift) // Copy position address
                    {
                        int value = currentCellIndex;
                        string text = "0x" + value.ToString("X8", CultureInfo.InvariantCulture);
                        Clipboard.SetText(text);
                    }
                    else // Copy bytes at selection
                    {
                        copyToolStripMenuItem_Click(this, EventArgs.Empty);
                    }

                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }
                else if (e.KeyCode == Keys.X && CanEdit) // Restore bytes at the selection
                {
                    clearToolStripMenuItem_Click(this, EventArgs.Empty);

                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }
                else if (e.KeyCode == Keys.V && CanEdit) // Paste bytes at selection
                {
                    if (!Clipboard.ContainsText())
                    {
                        return;
                    }

                    pasteToolStripMenuItem_Click(this, EventArgs.Empty);

                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }
            }
            else if (e.Alt)
            {
                if (e.KeyCode == Keys.C) // Copy ASCII at selection
                {
                    CopyASCIIAtSelection();

                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }
            }
            else if (e.KeyCode == Keys.Return && CanEdit) // Start editing or move the caret.
            {
                if (IsEditing())
                {
                    SelectedOffset = Math.Min(_ActiveOffset + 1, Buffer.Length - 1);
                    SetActiveCell(-1);
                }
                else
                {
                    SetActiveCell(SelectedOffset);
                }
            }
            else if (e.KeyCode == Keys.Escape) // Stop editing
            {
                if (IsEditing() && CanEdit)
                {
                    SetActiveCell(-1);
                }
                else if (IsSelecting()) // Clear selection.
                {
                    SelectedOffset = -1;
                    UpdateView();
                }
            }
            else if (IsEditing())
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private Span<byte> GetBytesAtSelection()
        {
            return Buffer.AsSpan()[_Selection!.Value.Start..(_Selection.Value.End.Value + 1)];
        }

        private void CopyASCIIAtSelection()
        {
            var selectedBytes = GetBytesAtSelection();
            string text = string
                .Join(" ", selectedBytes
                    .ToArray()
                    .Where(b => b is >= 0x20 and <= 0x7E)
                    .Select(b => FilterByte(b).ToString(CultureInfo.InvariantCulture)));
            Clipboard.SetText(text);
        }

        private void CopyBytesAtSelection()
        {
            var selectedBytes = GetBytesAtSelection();
            string text = string
                .Join(" ", selectedBytes
                    .ToArray()
                    .Select(b => b.ToString("X2", CultureInfo.InvariantCulture)));
            Clipboard.SetText(text);
        }

        private void PasteBytesAtSelection(byte[] bytes)
        {
            Debug.Assert(_Selection != null, nameof(_Selection) + " != null");

            int editIndex = SelectionStart;
            if (SelectionLength > 1)
            {
                for (int i = 0; i < bytes.Length && editIndex + i < SelectionEnd; ++i)
                {
                    SetCellValue(editIndex + i, bytes[i]);
                }
            }
            else
            {
                for (int i = 0; i < Math.Min(bytes.Length, SelectionLockEnd); ++i)
                {
                    SetCellValue(editIndex + i, bytes[i]);
                }
            }

            UpdateView();
            OnBufferModifiedEvent();
        }

        private void ClearBytesAtSelection()
        {
            Debug.Assert(_Selection != null, nameof(_Selection) + " != null");

            int editIndex = SelectionStart;
            int selectionLength = SelectionLength;
            for (int i = 0; i < selectionLength; ++i)
            {
                RestoreCellValue(editIndex + i);
            }

            UpdateView();
            OnBufferModifiedEvent();
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

                        break;
                    }
            }
        }

        private void Context_Structure_Opening(object sender, CancelEventArgs e)
        {
            if (HoveredOffset == -1 && SelectionLength >= 1)
            {
                _ContextOffset = SelectedOffset;
            }
            else if (HoveredOffset != -1 && SelectionLength >= 1)
            {
                if (HoveredOffset >= SelectionStart &&
                    HoveredOffset <= SelectionEnd)
                {
                    _ContextOffset = HoveredOffset;
                }
                else
                {
                    SelectedOffset = HoveredOffset;
                    UpdateView();

                    _ContextOffset = SelectedOffset;
                }
            }
            else
            {
                _ContextOffset = -1;
            }

            e.Cancel = _ContextOffset == -1;
            if (e.Cancel)
            {
                return;
            }

            foreach (ToolStripItem item in Context_Structure.Items)
            {
                bool isVisible;
                switch (item.Tag)
                {
                    case "Cell":
                        isVisible = _ContextOffset != -1 && SelectionLength == 1;
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
            clearToolStripMenuItem.Visible = CanEdit;
            copyToolStripMenuItem.Visible = true;
            pasteToolStripMenuItem.Visible = CanEdit;
            pasteToolStripMenuItem.Enabled = Clipboard.ContainsText();
            editCellToolStripMenuItem.Visible = CanEdit && SelectionLength == 1;
            editStructValueToolStripMenuItem.Visible = CanEdit &&
                                                       GetCellStruct(_ContextOffset)?.Tag is BinaryMetaData.BinaryField
                                                       {
                                                           Value: UObject or UName
                                                       };

            defineStructToolStripMenuItem.Visible = GetCellStruct(_ContextOffset) == null;
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CopyBytesAtSelection();
            ClearBytesAtSelection();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string text = Clipboard.GetText();
            // Starts with a hex value?
            if (text.Length < 2 || !int.TryParse(
                    text[..2],
                    NumberStyles.HexNumber,
                    CultureInfo.InvariantCulture, out int _))
            {
                return;
            }

            byte[] bytes = text
                .Split([' '], StringSplitOptions.RemoveEmptyEntries)
                .Select(b => Convert.ToByte(b, 16))
                .ToArray();

            PasteBytesAtSelection(bytes);
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CopyBytesAtSelection();
        }

        private void editCellToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Contract.Assert(_ContextOffset != -1);
            SetActiveCell(_ContextOffset);
        }

        private static byte[] WriteBuffer(UnrealPackageArchive archive, int offset, int size,
            Action<UnrealPackageWriter> serializer)
        {
            byte[] buffer = new byte[size];

            var memoryStream = new MemoryStream(buffer, true);
            using (var writer = new UnrealPackageWriter(archive, new BinaryWriter(memoryStream)))
            {
                serializer(writer);
            }

            Contract.Assert(memoryStream.Position == size, "Size cannot be changed");

            return buffer;
        }

        private void editStructValueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var cellStruct = GetCellStruct(_ContextOffset);
            Contract.Assert(cellStruct != null);

            var linker = Target is UObject o ? o.Package : null;
            Contract.Assert(linker != null);

            int offset = _ContextOffset;
            int size = cellStruct.Size;

            switch (cellStruct.Tag)
            {
                case BinaryMetaData.BinaryField binaryField:
                    switch (binaryField.Value)
                    {
                        case UName uName:
                            {
                                var inputDialog = new NameReferenceInputDialog
                                {
                                    Linker = linker,
                                    DefaultNameReference = uName
                                };
                                if (inputDialog.ShowDialog(this) == DialogResult.OK)
                                {
                                    int numberValue = inputDialog.InputNameNumber;
                                    var newValue = new UName(inputDialog.InputNameItem, numberValue);

                                    var archive = linker.Archive;
                                    byte[] buffer = WriteBuffer(archive, offset, size, writer => writer.WriteName(newValue));
                                    SetCellStructValue(cellStruct.Offset, buffer);

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
                                    Linker = linker,
                                    DefaultObjectReference = uObject
                                };
                                if (inputDialog.ShowDialog(this) == DialogResult.OK)
                                {
                                    var newValue = inputDialog.InputObjectReference;
                                    int objectIndex = inputDialog.InputObjectReference is UObject input
                                        ? (int)input
                                        : 0;

                                    var archive = uObject.Package.Archive;
                                    byte[] buffer = WriteBuffer(archive, offset, size, writer => writer.WriteIndex(objectIndex));
                                    SetCellStructValue(cellStruct.Offset, buffer);

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
                    var decompiler = new UStruct.UByteCodeDecompiler((UStruct)Target);
                    decompiler.JumpTo((ushort)token.Position);
                    text = token.Decompile(decompiler);
                    break;

                case BinaryMetaData.BinaryField binaryField:
                    // Copy the raw tag
                    text = binaryField.Value?.ToString() ?? string.Empty;
                    break;

                default:
                    text = "";
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
            using var dialog = new StructureInputDialog();

            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            string? type = dialog.InputStructType;
            if (string.IsNullOrEmpty(type))
            {
                MessageBox.Show(this, "Missing type");
                return;
            }

            string? name = dialog.InputStructName;
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show(this, "Missing name");
                return;
            }

            InitStructure(type, out byte size, out var color);
            _Patterns.MetaInfoList.Add
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

            UpdateView();
        }

        private void removeStructToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var cellStruct = GetCellStruct(_ContextOffset);
            Contract.Assert(cellStruct != null);

            _Patterns.MetaInfoList.Remove(cellStruct);

            string path = GetConfigPath();
            SaveConfig(path);

            UpdateView();
        }

        private bool IsEditing()
        {
            return _ActiveOffset != -1;
        }

        private bool IsSelecting()
        {
            return Selection.HasValue;
        }

        private void HexViewPanel_Click(object sender, EventArgs e)
        {
            HexViewPanel.Focus();
        }

        private void OnHexViewScrollBarOnPreviewKeyDown(object? s, PreviewKeyDownEventArgs e)
        {
            if (IsSelecting())
            {
                Focus();
            }
        }

        private void caretTimer_Tick(object sender, EventArgs e)
        {
            if (!IsEditing())
            {
                return;
            }

            _CaretTick = !_CaretTick;
            UpdateView();
        }

        private void SelectionDataGridView_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                return;
            }

            if (!IsSelecting())
            {
                return;
            }

            var stream = Target.GetBuffer();
            if (SelectionDataGridView.Rows[e.RowIndex].Cells["Value"].Tag is not IUnrealPatternParser patternMatcher)
            {
                Selection = new Range(SelectedOffset, SelectedOffset);
                UpdateView();

                return;
            }

            try
            {
                int index = Target.GetBufferPosition() + SelectedOffset;
                stream.Seek(index, SeekOrigin.Begin);
                object? value = patternMatcher.Parse(stream, index, 16);
                if (value == null)
                {
                    return;
                }

                int newSelectionEnd = (int)stream.Position - Target.GetBufferPosition() - 1;
                Selection = new Range(SelectedOffset, newSelectionEnd);
                UpdateView();
            }
            catch
            {
                // ignored
            }
        }

        private void HexViewerControl_Enter(object sender, EventArgs e)
        {
            if (_HexMessageFilter == null)
            {
                _HexMessageFilter = new HexMessageFilter(this);
                Application.AddMessageFilter(_HexMessageFilter);
            }
        }

        private void HexViewPanel_Leave(object sender, EventArgs e)
        {
            if (_HexMessageFilter != null)
            {
                Application.RemoveMessageFilter(_HexMessageFilter);
                _HexMessageFilter = null;
            }
        }

        private void HexViewerForm_Deactivate(object sender, EventArgs e)
        {
            HexViewPanel_Leave(sender, e);
        }
    }

    public interface IUnrealPatternParser
    {
        public object? Parse(IUnrealStream stream, int index, int length);
    }

    internal sealed class UnrealPrimitiveParser<T>(T value) : IUnrealPatternParser where T : unmanaged
    {
        public object? Parse(IUnrealStream stream, int index, int length)
        {
            if (length > 1 && length < Unsafe.SizeOf<T>())
            {
                return null;
            }

            return value switch
            {
                char => length == 1
                    ? ((char)stream.ReadByte()).ToString(CultureInfo.InvariantCulture)
                    : ((char)stream.ReadUInt16()).ToString(CultureInfo.InvariantCulture),
                byte => stream.ReadByte().ToString(CultureInfo.InvariantCulture),
                short => stream.ReadInt16().ToString(CultureInfo.InvariantCulture),
                ushort => stream.ReadUInt16().ToString(CultureInfo.InvariantCulture),
                int => stream.ReadInt32().ToString(CultureInfo.InvariantCulture),
                uint => stream.ReadUInt32().ToString(CultureInfo.InvariantCulture),
                long => stream.ReadInt64().ToString(CultureInfo.InvariantCulture),
                ulong => stream.ReadUInt64().ToString(CultureInfo.InvariantCulture),
                float => stream.ReadFloat().ToString(CultureInfo.InvariantCulture),
                _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
            };
        }
    }

    internal sealed class UnrealIndexParser : IUnrealPatternParser
    {
        public object Parse(IUnrealStream stream, int index, int length)
        {
            return stream.ReadIndex();
        }
    }

    internal sealed class UnrealNameParser : IUnrealPatternParser
    {
        public object Parse(IUnrealStream stream, int index, int length)
        {
            return stream.ReadName();
        }
    }

    internal sealed class UnrealObjectParser : IUnrealPatternParser
    {
        public object? Parse(IUnrealStream stream, int index, int length)
        {
            return stream.ReadObject();
        }
    }

    internal sealed class HexPatternParser(HexViewerControl hexViewControl) : IUnrealPatternParser
    {
        public object? Parse(IUnrealStream stream, int index, int length)
        {
            foreach (var patternRegion in hexViewControl
                         .PatternRegions
                         .Where(pattern => index >= pattern.Offset && index < pattern.Offset + pattern.Size))
            {
                object? value;

                try
                {
                    if (patternRegion.Tag is UStruct.UByteCodeDecompiler.Token token)
                    {
                        var decompiler = new UStruct.UByteCodeDecompiler((UStruct)hexViewControl.Target);
                        decompiler.JumpTo((ushort)token.Position);

                        value = token.Decompile(decompiler);
                    }
                    else if (patternRegion.Tag is IUnrealDecompilable decompilable)
                    {
                        value = decompilable.Decompile();
                    }
                    else value = string.Empty;
                }
                catch (Exception)
                {
                    value = null;
                }

                stream.Position += patternRegion.Size;

                return value;
            }

            return null;
        }
    }
}
