using System;
using ICSharpCode.AvalonEdit;

namespace UEExplorer
{
   public static class EditorUtil
    {
        public static void FindText(TextEditorPanel editor, string text)
        {
            var fails = 0;

            int currentIndex = editor.textEditor.CaretOffset;
            if (currentIndex >= editor.textEditor.Text.Length)
                return;

        searchAgain:
            int textIndex = editor.textEditor.Text.IndexOf(text, currentIndex, StringComparison.OrdinalIgnoreCase);
            if (textIndex == -1)
            {
                currentIndex = 0;
                if (fails > 0)
                    return;

                ++fails;
                goto searchAgain;
            }

            var line = editor.textEditor.TextArea.Document.GetLocation(textIndex);

            editor.textEditor.ScrollTo(line.Line, line.Column);
            editor.textEditor.Select(textIndex, text.Length);

            editor.textEditor.CaretOffset = textIndex + text.Length;
        }
    }
}