using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UEExplorer.Properties;
using UELib.Core;
using UELib;

namespace UEExplorer.UI.Nodes
{
    public class ObjectActionsBuilder : ObjectVisitor<List<(string, ContentNodeAction)>>
    {
        public override List<(string, ContentNodeAction)> Visit(object target)
        {
            var actions = new List<(string, ContentNodeAction)>();
            var addItem = (Action<string, ContentNodeAction>)((text, action) =>
            {
                actions.Add((text, action));
            });

            // Can be null!
            object tag;
            switch (target)
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
                    tag = target;
                    break;
            }

            // Not a workable target?
            if (tag == null)
            {
                return actions;
            }

            if (tag is IUnrealDecompilable) addItem(Resources.NodeItem_ViewObject, ContentNodeAction.Decompile);
            if (tag is IBinaryData binaryDataObject && binaryDataObject.BinaryMetaData != null)
            {
                addItem(Resources.UC_PackageExplorer_BuildItemNodes_View_Binary, ContentNodeAction.Binary);
            }
            if (tag is IUnrealViewable)
            {
                if (File.Exists(Program.Options.UEModelAppPath))
                {
                    addItem(Resources.NodeItem_OpenInUEModelViewer, ContentNodeAction.DecompileExternal);
#if DEBUG
                    addItem(Resources.NodeItem_ExportWithUEModelViewer, ContentNodeAction.ExportExternal);
#endif
                }
            }

            if (tag is IUnrealExportable exportableObj && exportableObj.CanExport())
            {
                addItem(Resources.EXPORT_AS, ContentNodeAction.ExportAs);
            }

            if (tag is IBuffered bufferedObject && bufferedObject.GetBuffer() != null)
            {
                addItem(Resources.NodeItem_ViewBuffer, ContentNodeAction.ViewBuffer);

            }
            
            var tableNode = tag as IContainsTable;
            if (tableNode?.Table != null)
            {
                addItem(Resources.NodeItem_ViewTableBuffer, ContentNodeAction.ViewTableBuffer);
            }

            // === UObject tools

            UObject obj;
            if (tag is UObject uObject)
            {
                obj = uObject;
            }
            else
            {
                return actions;
            }

            if (obj.Outer != null) addItem(Resources.NodeItem_ViewOuter, ContentNodeAction.DecompileOuter);

            if (obj is UStruct uStruct)
            {
                if (uStruct.ByteCodeManager != null)
                {
                    addItem(Resources.NodeItem_ViewTokens, ContentNodeAction.DecompileTokens);
                    addItem(Resources.NodeItem_ViewDisassembledTokens, ContentNodeAction.DisassembleTokens);
                }
            }

            if (obj.ThrownException != null)
            {
                addItem(Resources.NodeItem_ViewException, ContentNodeAction.ViewException);
            }

            return actions;
        }
    }
}
