using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;
using UEExplorer.Properties;
using UELib.Core;
using UELib;

namespace UEExplorer.UI.Nodes
{
    public sealed class ObjectActionsBuilder : ObjectVisitor<List<(string, ContentNodeAction)>>
    {
        public override List<(string, ContentNodeAction)> Visit(object obj)
        {
            var actions = new List<(string, ContentNodeAction)>();

            void AddItem(string text, ContentNodeAction action)
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

            if (tag is IUnrealDecompilable) AddItem(Resources.NodeItem_ViewObject, ContentNodeAction.Decompile);
            if (tag is IBinaryData binaryDataObject && binaryDataObject.BinaryMetaData != null)
            {
                AddItem(Resources.UC_PackageExplorer_BuildItemNodes_View_Binary, ContentNodeAction.Binary);
            }

            if (tag is IUnrealViewable)
            {
                if (File.Exists(Program.Options.UEModelAppPath))
                {
                    AddItem(Resources.NodeItem_OpenInUEModelViewer, ContentNodeAction.DecompileExternal);
#if DEBUG
                    AddItem(Resources.NodeItem_ExportWithUEModelViewer, ContentNodeAction.ExportExternal);
#endif
                }
            }

            if (tag is IUnrealExportable exportableObj && exportableObj.CanExport())
            {
                AddItem(Resources.EXPORT_AS, ContentNodeAction.ExportAs);
            }


            if (tag is IBuffered bufferedObject && bufferedObject.GetBuffer() != null)
            {
                AddItem(Resources.NodeItem_ViewBuffer, ContentNodeAction.ViewBuffer);
            }

            var tableNode = tag as IContainsTable;
            if (tableNode?.Table != null)
            {
                AddItem(Resources.NodeItem_ViewTableBuffer, ContentNodeAction.ViewTableBuffer);
            }

            // === UObject tools

            if (!(tag is UObject uObject))
            {
                return actions;
            }

            if (uObject.Outer != null) AddItem(Resources.NodeItem_ViewOuter, ContentNodeAction.DecompileOuter);

            if (uObject is UStruct uStruct)
            {
                if (uStruct.ByteCodeManager != null)
                {
                    AddItem(Resources.NodeItem_ViewTokens, ContentNodeAction.DecompileTokens);
                    AddItem(Resources.NodeItem_ViewDisassembledTokens, ContentNodeAction.DisassembleTokens);
                }
            }

            if (uObject.ThrownException != null)
            {
                AddItem(Resources.NodeItem_ViewException, ContentNodeAction.ViewException);
            }

            return actions;
        }
    }
}