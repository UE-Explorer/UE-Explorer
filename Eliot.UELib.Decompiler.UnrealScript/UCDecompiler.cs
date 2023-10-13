using System;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;
using UELib.Core;

namespace UELib.Decompiler.UnrealScript
{
    public class UCDecompiler : IDecompiler<IOutputDecompiler<IAcceptable>, IAcceptable>
    {
        public Type GetOutputDecompilerType() => typeof(UCOutputDecompiler);

        public bool CanDecompile(IAcceptable visitable) => visitable is UField;

        public Task Run(IAcceptable visitable,
            IOutputDecompiler<IAcceptable> outputDecompiler,
            CancellationToken cancellationToken)
        {
            Contract.Assert(CanDecompile(visitable));
            return Task.Run(() => outputDecompiler.Decompile(visitable, cancellationToken), cancellationToken);
        }
    }

    public class UCLegacyDecompiler : IDecompiler<IOutputDecompiler<IAcceptable>, IAcceptable>
    {
        public Type GetOutputDecompilerType() => typeof(UCLegacyOutputDecompiler);

        public bool CanDecompile(IAcceptable visitable) => visitable is IUnrealDecompilable;

        public Task Run(IAcceptable visitable,
            IOutputDecompiler<IAcceptable> outputDecompiler,
            CancellationToken cancellationToken)
        {
            Contract.Assert(CanDecompile(visitable));
            return Task.Run(() => outputDecompiler.Decompile(visitable, cancellationToken), cancellationToken);
        }
    }
}
