using UELib;
using UELib.Core;
using UELib.Engine;

namespace UEExplorer.UI.Nodes
{
    public abstract class ObjectVisitor<TResult> : IVisitor<TResult>
    {
        public virtual TResult Visit(IAcceptable visitor)
        {
            return Visit((dynamic)visitor);
        }

        public virtual TResult Visit(dynamic uMetaData)
        {
            return default;
        }

        public virtual TResult Visit(UMetaData uMetaData)
        {
            return default;
        }

        public virtual TResult Visit(UState uState)
        {
            return default;
        }

        public virtual TResult Visit(UStruct uStruct)
        {
            return default;
        }

        public virtual TResult Visit(UConst uConst)
        {
            return default;
        }

        public virtual TResult Visit(UEnum uEnum)
        {
            return default;
        }

        public virtual TResult Visit(UFont uFont)
        {
            return default;
        }

        public virtual TResult Visit(UModel uModel)
        {
            return default;
        }

        public virtual TResult Visit(UPalette uPalette)
        {
            return default;
        }

        public virtual TResult Visit(UTexture uTexture)
        {
            return default;
        }

        public virtual TResult Visit(USound uSound)
        {
            return default;
        }

        public virtual TResult Visit(UField uField)
        {
            return default;
        }

        public virtual TResult Visit(UObject uObject)
        {
            return default;
        }

        public virtual TResult Visit(UProperty uProperty)
        {
            return default;
        }

        public virtual TResult Visit(UScriptStruct uScriptStruct)
        {
            return default;
        }

        public virtual TResult Visit(UFunction uFunction)
        {
            return default;
        }

        public virtual TResult Visit(UClass uClass)
        {
            return default;
        }

        public virtual TResult Visit(UPackage uPackage)
        {
            return default;
        }

        public virtual TResult Visit(UTextBuffer uTextBuffer)
        {
            return default;
        }
    }
}