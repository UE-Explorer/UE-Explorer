using System.Collections.Generic;
using System.Threading;
using UEExplorer.Framework;

namespace UEExplorer
{
    public static class TextSearchHelpers
    {
        public static List<SourceLocation> FindText(string text, string keyword, CancellationToken cancellationToken)
        {
            var results = new List<SourceLocation>();

            if (text.Length < keyword.Length)
            {
                return results;
            }

            keyword = keyword.ToLower();
            int currentLine = 1;
            int currentColumn = 1;
            for (int i = 0; i < text.Length; ++i)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }

                if (text[i] == '\n')
                {
                    ++currentLine;
                    currentColumn = 1;
                    continue;
                }

                if (i + keyword.Length >= text.Length)
                {
                    break;
                }
                
                int startIndex = i;
                for (int j = 0; j < keyword.Length; ++j)
                {
                    if (char.ToLower(text[startIndex]) == keyword[j])
                    {
                        ++startIndex;
                        if (j != keyword.Length - 1)
                        {
                            continue;
                        }

                        var result = new SourceLocation(
                            currentLine, currentColumn,
                            startIndex - keyword.Length, keyword.Length);
                        results.Add(result);
                    }

                    break;
                }

                ++currentColumn;
            }

            return results;
        }

        public class DocumentResult
        {
            public object Document { get; set; }
            public List<SourceLocation> Results { get; set; }

            public override string ToString() => Document.ToString();
        }
    }
}
