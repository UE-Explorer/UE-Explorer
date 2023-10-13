using System;
using System.Linq;
using UELib.Core;
using UELib.Decompiler.Common.Nodes;
using UELib.Decompiler.Nodes;
using UELib.Decompiler.UnrealScript.Nodes;

namespace UELib.Decompiler.UnrealScript
{
    public class ClassDependsOnBuilder : IVisitor<Node>
    {
        public Node Visit(IAcceptable visitable)
        {
            if (!(visitable is ClassDeclarationNode classDeclarationNode))
            {
                return (Node)visitable;
            }

            var dependencies = classDeclarationNode.Object.ClassDependencies;
            if (dependencies == null ||
                !dependencies.Any())
            {
                return classDeclarationNode;
            }

            var dependsOnNode = new ModifierNode(ModifierNode.ModifierKind.Group);
            foreach (var dependency in dependencies)
            {
                var firstChildNode = new IdentifierNode(new UName(dependency.Class.NameTable))
                {
                    Sibling = dependsOnNode.Child
                };
                dependsOnNode.Child = firstChildNode;
            }

            dependsOnNode.Sibling = classDeclarationNode.Child;
            classDeclarationNode.Child = dependsOnNode;
            return classDeclarationNode;
        }

        public Node Visit(UStruct.UByteCodeDecompiler.Token token) => throw new NotImplementedException();
    }
}
