using UELib.Core;
using UELib.Decompiler.Common;
using UELib.Decompiler.Nodes;
using UELib.Decompiler.UnrealScript.Nodes;

namespace UELib.Decompiler.UnrealScript
{
    public class DeclarationNodeTreeBuilder : IVisitor<Node>
    {
        private readonly ArchetypeNodeTreeBuilder _ArchetypeTreeBuilder = new ArchetypeNodeTreeBuilder();

        public Node Visit(IAcceptable visitable)
        {
            switch (visitable)
            {
                case UObject obj: return Visit(obj);
            }

            return null;
        }

        public Node Visit(UStruct.UByteCodeDecompiler.Token token) => null;

        public Node Visit(UObject obj)
        {
            switch (obj)
            {
                case UConst uConst:
                    return new ConstDeclarationNode(uConst);

                case UClass uClass:
                    {
                        Node siblingNode = null;
                        foreach (var field in uClass.EnumerateFields())
                        {
                            var fieldNode = field.Accept(this);
                            fieldNode.Sibling = siblingNode;
                            siblingNode = fieldNode;
                        }

                        return new ClassDeclarationNode(uClass) { Sibling = siblingNode };
                    }

                case UStruct uStruct:
                    {
                        Node siblingNode = null;
                        foreach (var field in uStruct.EnumerateFields())
                        {
                            var fieldNode = field.Accept(this);
                            fieldNode.Sibling = siblingNode;
                            siblingNode = fieldNode;
                        }

                        return new StructDeclarationNode(uStruct) { Sibling = siblingNode };
                    }
            }

            return obj.Accept(_ArchetypeTreeBuilder);
        }
    }
}
