using System.Collections.Generic;
using System.IO;
using ICSharpCode.AvalonEdit.Document;
using UELib.Decompiler;

namespace UEExplorer.Framework.UI.Editor
{
    public class TextEditorOutputStream : TextOutputStream
    {
        private readonly TextDocument _Document;
        private readonly TextWriter _InnerWriter;
        private readonly List<ProgramLocation> _Locations = new List<ProgramLocation>();
        private int _Line, _Column;
        private int _Offset;

        public TextEditorOutputStream(TextDocument document, TextWriter innerWriter) : base(innerWriter)
        {
            _Document = document;
            _InnerWriter = innerWriter;
        }

        public IEnumerable<ProgramLocation> Locations => _Locations;

        public override void Write(char value)
        {
            base.Write(value);
            ++_Offset;
            ++_Column;
        }

        public override void WriteLine()
        {
            base.WriteLine();
            ++_Line;
            _Column = 0;
        }

        public override void WriteColumnAligned(int padding)
        {
            base.WriteColumnAligned(padding - _Column);
        }

        public override void WriteKeyword(string keyword)
        {
            int offset = _Offset;
            int line = _Line;
            int column = _Column;
            int length = keyword.Length;
            Write(keyword);

            var programLocation = new ProgramLocation(
                new SourceLocation(line, column, offset, length),
                StreamLocation.Empty);
            _Locations.Add(programLocation);
        }

        public override void WriteReference(object reference, string identifier)
        {
            int offset = _Offset;
            int line = _Line;
            int column = _Column;
            int length = identifier.Length;
            Write(identifier);

            var streamLocation = StreamLocationFactory.Create(reference);
            var programLocation = new ProgramLocation(
                new SourceLocation(line, column, offset, length),
                streamLocation);
            _Locations.Add(programLocation);
        }

        public override void WriteBegin(object subject, string name)
        {
            WriteLine();
            
            int offset = _Offset;
            int line = _Line;
            int column = _Column;
            int length = name.Length;
            Write(name);

            var streamLocation = StreamLocationFactory.Create(subject);
            var programLocation = new ProgramLocation(
                new SourceLocation(line, column, offset, length),
                streamLocation);
            _Locations.Add(programLocation);
            
            WriteLine();
        }

        public override void WriteEnd(object subject, string name)
        {
            WriteLine();
            
            int offset = _Offset;
            int line = _Line;
            int column = _Column;
            int length = name.Length;
            Write(name);

            var streamLocation = StreamLocationFactory.Create(subject);
            var programLocation = new ProgramLocation(
                new SourceLocation(line, column, offset, length),
                streamLocation);
            _Locations.Add(programLocation);
            
            WriteLine();
        }

        public override void Flush() => _InnerWriter.Flush();
    }
}
