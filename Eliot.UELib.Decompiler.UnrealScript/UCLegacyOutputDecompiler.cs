using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading;
using UELib.Annotations;
using UELib.Core;

namespace UELib.Decompiler.UnrealScript
{
    [OutputDecompiler("UnrealScript")]
    public class UCLegacyOutputDecompiler : IOutputDecompiler<IAcceptable>, IVisitor
    {
        private readonly TextOutputStream _Output;
        private readonly UCDecompilerSettings _Settings;
        private readonly IVisitor<UObject>[] _Transformers;

        public static readonly IVisitor<UObject>[] DefaultTransformers = { };

        public UCLegacyOutputDecompiler(TextOutputStream outputStream) : this(
            outputStream,
            new UCDecompilerSettings(),
            DefaultTransformers)
        {
        }

        public UCLegacyOutputDecompiler(TextOutputStream outputStream,
            IVisitor<UObject>[] transformers) : this(
            outputStream,
            new UCDecompilerSettings(),
            transformers)
        {
        }

        public UCLegacyOutputDecompiler(TextOutputStream outputStream,
            UCDecompilerSettings settings) : this(
            outputStream,
            settings,
            DefaultTransformers)
        {
        }

        public UCLegacyOutputDecompiler(TextOutputStream outputStream,
            UCDecompilerSettings settings,
            IVisitor<UObject>[] transformers)
        {
            _Output = outputStream;
            _Settings = settings;
            _Transformers = transformers;
        }

        public void Visit(IAcceptable visitable)
        {
            switch (visitable)
            {
                case IUnrealDecompilable decompilable:
                    _Output.Write(decompilable.Decompile());
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        public void Visit(UStruct.UByteCodeDecompiler.Token token) => throw new NotImplementedException();

        private IAcceptable TransformToNode([NotNull] IAcceptable visitable)
        {
            Debug.Assert(visitable != null);
            return _Transformers.Aggregate(visitable, (current, transformer) => current.Accept(transformer) ?? current);
        }

        public void Decompile([NotNull] IAcceptable visitable, CancellationToken cancellationToken)
        {
            Contract.Assert(visitable != null, "Cannot decompile for null");

            var transformed = TransformToNode(visitable);
            Debug.Assert(transformed != null);

            transformed.Accept(this);

            _Output.Flush();
        }
    }
}
