using UELib.Core;
using UELib.Decompiler.Common.Nodes;
using UELib.Decompiler.Nodes;
using UELib.Flags;

namespace UELib.Decompiler.UnrealScript
{
    // Experimental approach to building a tree of modifiers.
    // This is expensive, but it provides us many ways to transform a tree.
    public class ModifierNodeTreeBuilder : INodeVisitor<Node>
    {
        public Node Visit(IAcceptable visitable) => Visit((dynamic)visitable);

        public Node Visit(UStruct.UByteCodeDecompiler.Token token) => null;

        public Node Visit(ObjectDeclarationNode<UClass> node)
        {
            var ctx = node.Object;
            if (ctx.HasClassFlag(ClassFlags.Abstract))
            {
                var mNode = new ModifierNode(ModifierNode.ModifierKind.Specifier)
                {
                    Sibling = new IdentifierNode(new UName("Abstract")), Child = node.Child
                };
                node.Child = mNode;
            }

            return node;
        }

        public Node Visit(INode node) => null;

        public Node Visit(MultiNode node) => node;

        public Node Visit(LineSeparatorNode node) => node;

        public Node Visit(NumberLiteralNode node) => node;

        public Node Visit(StringLiteralNode node) => node;

        public Node Visit(NameLiteralNode node) => node;

        public Node Visit<T>(StructLiteralNode<T> node) where T : struct => node;

        public Node Visit(StructLiteralNode<UVector> node) => node;

        public Node Visit(ObjectLiteralNode node) => node;

        public Node Visit(ArrayLiteralNode node) => node;

        public Node Visit(IdentifierNode node) => node;

        public Node Visit(ModifierNode node) => node;

        public Node Visit(MemberInfoReferenceNode node) => node;

        public Node Visit<T>(ObjectDeclarationNode<T> node) where T : UObject => node;

        public Node Visit(ObjectDeclarationNode<UStruct> node) => node;

        public Node Visit(ObjectDeclarationNode<UConst> node) => node;

        public Node Visit(ArchetypeConstructionNode node) => node;

        public Node Visit(ArchetypeParameterAssignmentNode node) => node;

        public Node Visit(ArchetypePropertyAssignmentNode node) => node;

        public Node Visit(ArchetypeShorthandAssignmentNode node) => node;
    }
}
