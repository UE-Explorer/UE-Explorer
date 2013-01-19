using System;
using System.Collections.Generic;

namespace UEExplorer
{
    public static class TextSearchHelpers
    {
        public class FindResult
        {
            public int TextIndex;
            public int TextLine;
            public int TextColumn;

            public override string ToString()
            {
                return String.Format( "({0}, {1})", TextLine, TextColumn );
            }
        };

        public class DocumentResult
        {
            public Object Document;
            public List<FindResult> Results;

            public override string ToString()
            {
                return Document.ToString();
            }
        };

        public static List<FindResult> FindText( string text, string keyword )
        {
            keyword = keyword.ToLower();
            var results = new List<FindResult>();

            var currentLine = 1;
            var currentColumn = 1;
            for( int i = 0; i < text.Length; ++ i )
            {
                if( text[i] == '\n' )
                {
                    ++ currentLine;
                    currentColumn = 1;
                    continue;
                }

                var startIndex = i;
                for( int j = 0; j < keyword.Length; ++ j )
                {
                    if( Char.ToLower( text[startIndex] ) == keyword[j] )
                    {
                        ++ startIndex;
                        if( j == keyword.Length - 1 )
                        {
                            var result = new FindResult
                            {
                                TextIndex = startIndex - keyword.Length,
                                TextLine = currentLine,
                                TextColumn = currentColumn
                            };
                            results.Add( result );
                            break;
                        }
                        continue;	
                    }
                    break;
                }
                ++ currentColumn;
            }
            return results;
        }
    }
}
