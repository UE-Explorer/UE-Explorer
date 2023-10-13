using UELib.Core;
using UELib.Decompiler.Common.Nodes;
using UELib.Decompiler.Nodes;

namespace UELib.Decompiler.UnrealScript.Nodes
{
    public class StructDeclarationNode : ObjectDeclarationNode<UStruct>
    {
        public StructDeclarationNode(UStruct @object) : base(@object)
        {
        }

        public override void Accept(INodeVisitor visitor) => visitor.Visit(this);

        public override TResult Accept<TResult>(INodeVisitor<TResult> visitor) => visitor.Visit(this);
    }
}
