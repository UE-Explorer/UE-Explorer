using System;
using ICSharpCode.AvalonEdit;

namespace UEExplorer
{
    public static class EditorUtil
    {
        public static void FindText(TextEditor textEditor, string text)
        {
            var fails = 0;

            int currentIndex = textEditor.CaretOffset;
            if (currentIndex >= textEditor.Text.Length)
                return;

        searchAgain:
            int textIndex = textEditor.Text.IndexOf(text, currentIndex, StringComparison.OrdinalIgnoreCase);
            if (textIndex == -1)
            {
                currentIndex = 0;
                if (fails > 0)
                    return;

                ++fails;
                goto searchAgain;
            }

            var line = textEditor.TextArea.Document.GetLocation(textIndex);

            textEditor.ScrollTo(line.Line, line.Column);
            textEditor.Select(textIndex, text.Length);

            textEditor.CaretOffset = textIndex + text.Length;
        }
    }
}