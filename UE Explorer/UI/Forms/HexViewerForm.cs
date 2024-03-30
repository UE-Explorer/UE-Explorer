using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using UEExplorer.Properties;
using UELib;
using UELib.Core;

namespace UEExplorer.UI.Forms
{
    public partial class HexViewerForm : Form
    {
        private readonly string _PackageFilePath;
        private readonly IBuffered _Target;

        private HexViewerForm()
        {
            InitializeComponent();

            WindowState = Settings.Default.HexViewerState;
            Size = Settings.Default.HexViewerSize;
            Location = Settings.Default.HexViewerLocation;
            ViewASCIIItem.Checked = Settings.Default.HexViewer_ViewASCII;
            ViewByteItem.Checked = Settings.Default.HexViewer_ViewByte;
        }

        public HexViewerForm(IBuffered target, string packageFilePath) : this()
        {
            Debug.Assert(target != null);

            _Target = target;
            _PackageFilePath = packageFilePath;

            switch (target)
            {
                case UnrealPackage _:
                    editToolStripMenuItem.Enabled = false;
                    break;
            }
        }

        private void HexViewerForm_Load(object sender, EventArgs e)
        {
            SizeLabel.Text = string.Format(SizeLabel.Text,
                _Target.GetBufferSize().ToString(CultureInfo.InvariantCulture)
            );

            HexPanel.SetHexData(_Target);
            Text = $"{Text} - {HexPanel.Target.GetBufferId(true)}";

            OnHexPanelOffsetChanged(0);
            HexPanel.OffsetChangedEvent += OnHexPanelOffsetChanged;
            HexPanel.BufferModifiedEvent += () => { SaveItem.Enabled = true; };
        }

        private void HexViewDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (WindowState != FormWindowState.Normal)
            {
                Settings.Default.HexViewerLocation = RestoreBounds.Location;
                Settings.Default.HexViewerSize = RestoreBounds.Size;
            }
            else
            {
                Settings.Default.HexViewerLocation = Location;
                Settings.Default.HexViewerSize = Size;
            }

            Settings.Default.HexViewerState = WindowState == FormWindowState.Minimized
                ? FormWindowState.Normal
                : WindowState;
            Settings.Default.Save();
        }

        private void OnHexPanelOffsetChanged(int selectedOffset)
        {
            ToolStripStatusLabel_Position.Text = string.Format
            (
                Resources.HexView_Position,
                HexPanel.Target.GetBufferPosition(),
                selectedOffset,
                HexPanel.Target.GetBufferPosition()
            );
        }

        private void ViewASCIIToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            HexPanel.DrawASCII = !HexPanel.DrawASCII;
            Settings.Default.HexViewer_ViewASCII = HexPanel.DrawASCII;
            Settings.Default.Save();
        }

        private void ViewByteToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            HexPanel.DrawByte = !HexPanel.DrawByte;
            Settings.Default.HexViewer_ViewByte = HexPanel.DrawByte;
            Settings.Default.Save();
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(BitConverter.ToString(HexPanel.Buffer).Replace('-', ' '));
        }

        private void CopyAsViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            const int firstColumnWidth = 8;
            const int secondColumnWidth = 32;
            const int thirdColumnWidth = 16;
            const string columnMargin = "  ";
            const string byteValueWidth = "  ";
            const char charValueWidth = ' ';
            const char valueMargin = ' ';
            const int columnWidth = 16;

            byte[] buffer = HexPanel.Buffer;
            string input = Resources.HexView_Offset.PadRight(firstColumnWidth, valueMargin) + columnMargin
                + "0  1  2  3  4  5  6  7  8  9  A  B  C  D  E  F ".PadRight(secondColumnWidth, valueMargin) +
                columnMargin
                + "0 1 2 3 4 5 6 7 8 9 A B C D E F ".PadRight(thirdColumnWidth, valueMargin)
                + "\r\n";

            var lines = (int)Math.Ceiling((double)buffer.Length / columnWidth);
            for (var i = 0; i < lines; ++i)
            {
                input += $"\r\n{i * columnWidth:X8}" + columnMargin;
                for (var j = 0; j < columnWidth; ++j)
                {
                    int index = i * columnWidth + j;
                    if (index >= buffer.Length)
                    {
                        input += byteValueWidth;
                    }
                    else
                    {
                        input += $"{buffer[index]:X2}";
                    }

                    if (j < columnWidth - 1)
                    {
                        input += valueMargin;
                    }
                }

                input += columnMargin;

                for (var j = 0; j < columnWidth; ++j)
                {
                    int index = i * columnWidth + j;
                    if (index >= buffer.Length)
                    {
                        input += charValueWidth;
                    }
                    else
                    {
                        input += HexViewerControl.FilterByte(buffer[index]).ToString(CultureInfo.InvariantCulture);
                    }

                    if (j < columnWidth - 1)
                    {
                        input += valueMargin;
                    }
                }
            }

            Clipboard.SetText(input);
        }

        private void ExportBinaryFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fsd = new SaveFileDialog { FileName = HexPanel.Target.GetBufferId() };
            if (fsd.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllBytes(fsd.FileName, HexPanel.Buffer);
            }
        }

        private void ImportBinaryFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var target = HexPanel.Target;
            var osd = new OpenFileDialog { FileName = target.GetBufferId() };
            if (osd.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            byte[] buffer = File.ReadAllBytes(osd.FileName);
            if (buffer.Length != HexPanel.Buffer.Length)
            {
                MessageBox.Show(Resources.CANNOT_IMPORT_BINARY_NOTEQUAL_LENGTH);
                return;
            }

            HexPanel.Buffer = buffer;
            HexPanel.Refresh();

            var result = MessageBox.Show(
                Resources.SAVE_QUESTION_WARNING,
                Resources.SAVE_QUESTION,
                MessageBoxButtons.YesNo
            );
            if (result != DialogResult.Yes)
            {
                return;
            }

            if (ReplaceBuffer(target, buffer))
            {
                ReloadObject();
            }
        }

        private void SaveModificationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ReplaceBuffer(HexPanel.Target, HexPanel.Buffer))
            {
                SaveItem.Enabled = false;
            }
        }

        private void ReloadPackageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReloadPackage();
        }

        private void RedeserializeObjectOnlyUseAtOwnRiskToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReloadObject();

            // Try renew the buffer, in case modifications were made outside of Hex Viewer.
            ReplaceBuffer(HexPanel.Target, HexPanel.Buffer);
        }

        private void ReloadObject()
        {
            try
            {
                if (HexPanel.Target is UObject obj)
                {
                    obj.BeginDeserializing();
                    HexPanel.Reload();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(string.Format(Resources.COULDNT_RELOAD_OBJECT, e), Resources.Error,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ReloadPackage()
        {
            // TEMP: Hacky solution
            var tab = ((ProgramForm)(Owner)).Tabs.GetTab(_PackageFilePath);
            if (tab != null)
            {
                ((ProgramForm)Owner).Tabs.CloseTab(tab);
            }
            ((ProgramForm)Owner).LoadFromFile(_PackageFilePath);
            Close();
        }

        private bool ReplaceBuffer(IBuffered target, byte[] buffer)
        {
            target.GetBuffer().Dispose();
            string packageFilePath = _PackageFilePath;
            using (var package = UnrealLoader.LoadPackage(packageFilePath, FileAccess.ReadWrite))
            {
                package.Stream.Seek(target.GetBufferPosition(), SeekOrigin.Begin);
                try
                {
                    package.Stream.Write(buffer, 0, buffer.Length);
                    package.Stream.Flush();
                    package.Stream.Dispose();
                    return true;
                }
                catch (IOException exc)
                {
                    MessageBox.Show(string.Format(Resources.COULDNT_SAVE_EXCEPTION, exc));
                }
            }

            return false;
        }

        private void HelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FAQControl.Visible = !FAQControl.Visible;
        }

        private void CopyAddressToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText($"0x{HexPanel.Target.GetBufferPosition():X8}");
        }

        private void CopySizeInHexToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText($"0x{HexPanel.Target.GetBufferSize():X8}");
        }

        private void HEXWorkshopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool appExists = File.Exists(Program.Options.HEXWorkshopAppPath);
            if (!appExists)
            {
                if (MessageBox.Show(string.Format(Resources.PLEASE_SELECT_PATH, "Hex Workshop"),
                        Resources.NOT_AVAILABLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) ==
                    DialogResult.Cancel)
                {
                    return;
                }

                var ofd = new OpenFileDialog
                {
                    Filter = "Hex Workshop(HWorks32.exe)|HWorks32.exe",
                    FileName = Path.Combine("%ProgramW6432%", "BreakPoint Software", "Hex Workshop v6", "HWorks32.exe")
                };
                if (ofd.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                Program.Options.HEXWorkshopAppPath = ofd.FileName;
                Program.SaveConfig();
            }

            string appPath = Program.Options.HEXWorkshopAppPath;
            string filePath = _PackageFilePath;
            int pos = HexPanel.Target.GetBufferPosition();
            int size = HexPanel.Target.GetBufferSize();

            var appArguments = $"\"{filePath}\" /GOTO:{pos} /SELECT:{size}";
            var appInfo = new ProcessStartInfo(appPath, appArguments)
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = false
            };
            var app = Process.Start(appInfo);
        }
    }
}
