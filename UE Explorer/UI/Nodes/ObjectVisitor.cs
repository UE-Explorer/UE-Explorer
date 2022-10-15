using UELib;

namespace UEExplorer.UI.Nodes
{
    public abstract class ObjectVisitor<TResult> : IVisitor<TResult>
    {
        public virtual TResult Visit(IAcceptable visitor)
        {
            return Visit((dynamic)visitor);
        }

        public virtual TResult Visit(dynamic obj)
        {
            return default;
        }
    }
}