using UELib;
using UELib.Core;

namespace UEExplorer.UI
{
    public abstract class ObjectVisitor<TResult> : IVisitor<TResult>
    {
        public virtual TResult Visit(IAcceptable visitor)
        {
            return Visit((dynamic)visitor);
        }

        public TResult Visit(UStruct.UByteCodeDecompiler.Token token) => throw new System.NotImplementedException();

        public virtual TResult Visit(dynamic subject)
        {
            return default;
        }
    }
}
