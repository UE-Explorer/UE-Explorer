using System.Windows.Forms;
using Krypton.Toolkit;

namespace UEExplorer.UI.Controls
{
    public class FilterTextBox : KryptonTextBox
    {
        public FilterTextBox()
        {
            TextBox.KeyUp += TextBoxOnKeyUp;
        }

        private void TextBoxOnKeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    Text = "";
                    e.Handled = true;
                    break;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
