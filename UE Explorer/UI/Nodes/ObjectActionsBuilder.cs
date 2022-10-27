using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;
using UEExplorer.Properties;
using UELib.Core;
using UELib;

namespace UEExplorer.UI.Nodes
{
    public sealed class ObjectActionsBuilder : ObjectVisitor<List<(string, ContextActionKind)>>
    {
        public override List<(string, ContextActionKind)> Visit(object obj)
        {
            var actions = new List<(string, ContextActionKind)>();

            void AddItem(string text, ContextActionKind action)
            {
                actions.Add((text, action));
            }

            // Can be null!
            object tag;
            switch (obj)
            {
                // See if we can work with the TreeNode's tag?
                case TreeNode treeNode:
                    tag = treeNode.Tag;
                    if (tag is UObjectTableItem item && item.Object != null)
                    {
                        tag = item.Object;
                    }

                    break;

                // Maybe just an UObject
                default:
                    tag = obj;
                    break;
            }

            // Not a workable target?
            if (tag == null)
            {
                return actions;
            }

            if (tag is USound)
            {
                AddItem(Resources.NodeItem_Play, ContextActionKind.Play);
            }

            if (tag is IUnrealDecompilable) AddItem(Resources.NodeItem_Decompile, ContextActionKind.Decompile);
            if (tag is IBinaryData binaryDataObject && binaryDataObject.BinaryMetaData != null)
            {
                AddItem(Resources.NodeItem_ViewBinaryFields, ContextActionKind.Binary);
            }

            if (tag is IUnrealViewable)
            {
                if (File.Exists(Program.Options.UEModelAppPath))
                {
                    AddItem(Resources.NodeItem_OpenInUEModelViewer, ContextActionKind.DecompileExternal);
#if DEBUG
                    AddItem(Resources.NodeItem_ExportWithUEModelViewer, ContextActionKind.ExportExternal);
#endif
                }
            }

            if (tag is IUnrealExportable exportableObj && exportableObj.CanExport())
            {
                AddItem(Resources.NodeItem_ExportAs, ContextActionKind.ExportAs);
            }

            if (tag is IBuffered bufferedObject && bufferedObject.GetBuffer() != null)
            {
                AddItem(Resources.NodeItem_ViewBuffer, ContextActionKind.ViewBuffer);
            }

            var tableNode = tag as IContainsTable;
            if (tableNode?.Table != null)
            {
                AddItem(Resources.NodeItem_ViewTableBuffer, ContextActionKind.ViewTableBuffer);
            }

            // === UObject tools

            if (!(tag is UObject uObject))
            {
                return actions;
            }

            if (uObject is UStruct uStruct)
            {
                if (uStruct.ByteCodeManager != null)
                {
                    AddItem(Resources.NodeItem_ViewTokens, ContextActionKind.DecompileTokens);
                    AddItem(Resources.NodeItem_ViewDisassembledTokens, ContextActionKind.DisassembleTokens);
                }
            }

            if (uObject.ThrownException != null)
            {
                AddItem(Resources.NodeItem_ViewException, ContextActionKind.ViewException);
            }

            return actions;
        }
    }
}