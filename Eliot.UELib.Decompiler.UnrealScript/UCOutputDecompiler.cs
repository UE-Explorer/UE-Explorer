using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading;
using UELib.Annotations;
using UELib.Core;
using UELib.Decompiler.Common.Nodes;
using UELib.Decompiler.Nodes;
using UELib.UnrealScript;

namespace UELib.Decompiler.UnrealScript
{
    /// <summary>
    ///     Outputs an UnrealScript format from a <see cref="Node" /> tree.
    ///     The decompile process accepts any instance of an <see cref="UObject" />
    ///     and will pass it through a transformation pipe,
    ///     which turns the objects into nodes that can then be decompiled into the given output stream.
    /// </summary>
    [OutputDecompiler("UnrealScript")]
    public class UCOutputDecompiler : IOutputDecompiler<IAcceptable>, INodeVisitor
    {
        private readonly TextOutputStream _Output;
        private readonly UCDecompilerSettings _Settings;
        private readonly IVisitor<Node>[] _Transformers;

        public static readonly IVisitor<Node>[] DefaultTransformers = {
            // Build the declarations tree

            new DeclarationNodeTreeBuilder(),
            // Append the modifiers to the declarations
            //new ModifierNodeTreeBuilder(), new ClassDependsOnBuilder()
        };

        public UCOutputDecompiler(TextOutputStream outputStream) : this(
            outputStream,
            new UCDecompilerSettings(),
            DefaultTransformers)
        {
        }

        public UCOutputDecompiler(TextOutputStream outputStream,
            IVisitor<Node>[] transformers) : this(
            outputStream,
            new UCDecompilerSettings(),
            transformers)
        {
        }

        public UCOutputDecompiler(TextOutputStream outputStream,
            UCDecompilerSettings settings) : this(
            outputStream,
            settings,
            DefaultTransformers)
        {
        }

        public UCOutputDecompiler(TextOutputStream outputStream,
            UCDecompilerSettings settings,
            IVisitor<Node>[] transformers)
        {
            _Output = outputStream;
            _Settings = settings;
            _Transformers = transformers;
        }

        public void Visit(IAcceptable visitable) =>
            throw new NotImplementedException(
                $"The type '{visitable.GetType()}' of the visitable object is not supported.");

        public void Visit(INode node) => Visit((dynamic)node);

        public void Visit(MultiNode node) => throw new NotImplementedException();

        public void Visit(LineSeparatorNode node) => _Output.WriteLine();

        public void Visit(NumberLiteralNode node) => _Output.Write(PropertyDisplay.FormatLiteral(node.Value));

        public void Visit(StringLiteralNode node)
        {
            _Output.WriteDoubleQuote();
            _Output.Write(PropertyDisplay.FormatLiteral(node.Value));
            _Output.WriteDoubleQuote();
        }

        public void Visit(NameLiteralNode node)
        {
            _Output.WriteSingleQuote();
            _Output.Write(node.Value);
            _Output.WriteSingleQuote();
        }

        public void Visit<T>(StructLiteralNode<T> node) where T : struct => throw new NotImplementedException();

        public void Visit(StructLiteralNode<UVector> node)
        {
            _Output.WriteKeyword("vect");
            _Output.Write('(');
            _Output.WriteComma();
            _Output.WriteComma();
            _Output.Write(')');
        }

        public void Visit(ObjectLiteralNode node)
        {
            if (node.Value == null)
            {
                _Output.WriteKeyword("none");
                return;
            }

            Debug.Assert(node.Value.Class != null);
            _Output.WriteReference(node.Value.Class, node.Value.Class.Name);
            _Output.WriteSingleQuote();
            _Output.WriteReference(node.Value, node.Value.Name);
            _Output.WriteSingleQuote();
        }

        public void Visit(ArrayLiteralNode node) => throw new NotImplementedException();

        public void Visit(IdentifierNode node) => _Output.Write(node.Identifier);

        public void Visit(ModifierNode node)
        {
            _Output.WriteKeyword("modifier");

            var nodes = node.EnumerateChildren().ToList();
            if (nodes.Count == 0)
            {
                return;
            }

            _Output.Write('(');

            for (int index = 0; index < nodes.Count; index++)
            {
                var child = nodes[index];
                child.Accept(this);

                if (index != nodes.Count - 1)
                {
                    _Output.WriteComma();
                }
            }

            _Output.Write(')');
        }

        public void Visit(MemberInfoReferenceNode node) => throw new NotImplementedException();

        public void Visit<T>(ObjectDeclarationNode<T> node) where T : UObject => throw new NotImplementedException();
        public void Visit(ObjectDeclarationNode<UStruct> node)
        {
            var ctx = node.Object;

            _Output.WriteKeyword("struct");
            _Output.WriteSpace();
            
            _Output.WriteReference(ctx, node.Identifier);

            if (ctx.Super != null)
            {
                _Output.WriteSpace();
                _Output.WriteKeyword("extends");
                _Output.WriteSpace();

                OutputStructReference(ctx.Super, ctx);
            }
        }

        private void OutputTypeReference(UObject type, UStruct ctx)
        {
            _Output.WriteReference(type, type.Name);
        }

        private void OutputStructReference(UStruct type, UStruct ctx)
        {
            if (CanUseQualifiedIdentifier(type))
            {
                OutputSuperQualifiedIdentifier(type, ctx);
                return;
            }

            _Output.WriteReference(type, type.Name);
        }

        private bool CanUseQualifiedIdentifier(UObject type)
        {
            return _Settings.OutputQualifiedIdentifiers;
        }
        
        private bool CanUseQualifiedIdentifier(UStruct type)
        {
            // We cannot reference a UStruct using a qualified identifier if that struct is not a top-level struct.
            // i.e. a struct declared within another struct cannot be referenced using a qualified identifier.
            return _Settings.OutputQualifiedIdentifiers && type.Outer is UClass;
        }

        private bool CanUseQualifiedIdentifier(UEnum type)
        {
            return _Settings.OutputQualifiedIdentifiers && type.Outer is UClass;
        }

        private bool ShouldUseQualifiedIdentifier(UStruct type, UStruct ctx)
        {
            return type.Outer != ctx;
        }

        // (in most cases) UnrealScript qualified identifiers chaining is restricted to only "UClassIdentifier.TypeIdentifier".
        private void OutputSuperQualifiedIdentifier(UStruct super, UStruct ctx)
        {
            // We don't have to be explicit here.
            if (ShouldUseQualifiedIdentifier(super, ctx))
            {
                Debug.Assert(super.Outer != null, "super.Outer != null");
                _Output.WriteReference(super.Outer, super.Outer.Name);
                _Output.WriteDot();
                _Output.WriteReference(super, super.Name);
                return;
            }

            _Output.WriteReference(super, super.Name);
        }
        
        public void Visit(ObjectDeclarationNode<UClass> node)
        {
            var ctx = node.Object;

            _Output.WriteKeyword(ctx.IsClassInterface()
                ? "interface"
                : "class");
            _Output.WriteSpace();

            _Output.WriteReference(ctx, node.Identifier);

            if (ctx.Super != null)
            {
                _Output.WriteSpace();
                _Output.WriteKeyword("extends");
                _Output.WriteSpace();
                
                OutputStructReference(ctx.Super, ctx);
            }

            if (ctx.Within != null && (ctx.Within.Name != "Object" || _Settings.Mode != UCDecompilerMode.Clean))
            {
                _Output.WriteSpace();
                _Output.WriteKeyword("within");
                _Output.WriteSpace();

                OutputStructReference(ctx.Within, ctx);
            }

            _Output.WriteIndented(() =>
            {
                foreach (var child in node.EnumerateChildren<ModifierNode>())
                {
                    _Output.WriteLine();
                    child.Accept(this);
                }
            });

            _Output.WriteSemicolon();
            _Output.WriteLine();

            Node lastSibling = node;
            foreach (var sibling in node.EnumerateSiblings())
            {
                if (lastSibling.GetType() != sibling.GetType())
                {
                    _Output.WriteLine();
                }

                sibling.Accept(this);
                _Output.WriteLine();
                lastSibling = sibling;
            }
        }

        // In a UnrealScript context only the "object" type is permitted, and is usually written in lower-case.
        public void Visit(ArchetypeConstructionNode node)
        {
            _Output.WriteKeyword("begin");
            _Output.WriteSpace();
            _Output.WriteKeyword("object");

            var obj = (UObject)node.Archetype;
            Debug.Assert(obj != null);

            _Output.WriteSpace();
            _Output.WriteKeyword("name");
            _Output.WriteAssignment();
            _Output.WriteReference(obj, obj.Name);

            if (obj.ExportTable.Super == null)
            {
                _Output.WriteSpace();
                _Output.WriteKeyword("class");
                _Output.WriteAssignment();
                Debug.Assert(obj.Class != null);
                _Output.WriteReference(obj, obj.Class.Name);
            }

            // TODO: ScriptProperties

            _Output.WriteLine();
            _Output.WriteKeyword("end");
            _Output.WriteSpace();
            _Output.WriteKeyword("object");
        }

        public void Visit(ArchetypeParameterAssignmentNode node) => throw new NotImplementedException();

        public void Visit(ArchetypePropertyAssignmentNode node) => throw new NotImplementedException();

        public void Visit(ArchetypeShorthandAssignmentNode node) => throw new NotImplementedException();

        public void Visit(UStruct.UByteCodeDecompiler.Token token) => throw new NotImplementedException();

        public void Visit(ObjectDeclarationNode<UConst> node)
        {
            _Output.WriteKeyword("const");
            _Output.WriteSpace();

            _Output.WriteReference(node.Object, node.Identifier);

            _Output.WriteSpace();
            _Output.WriteAssignment();
            _Output.WriteSpace();

            string trimmed = node.Object.Value.Trim();
            _Output.Write(trimmed);
            _Output.WriteSemicolon();
        }

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
