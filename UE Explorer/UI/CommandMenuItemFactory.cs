using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using UEExplorer.Framework.UI.Commands;

namespace UEExplorer.UI
{
    internal static class CommandMenuItemFactory
    {
        public static IEnumerable<ToolStripItem> Create(
            IEnumerable<IContextCommand> commands,
            Action<IContextCommand> executeFunc) =>
            commands
                .Select(cmd =>
                {
                    var item = new ToolStripMenuItem(
                        cmd.Text,
                        cmd.Icon,
                        (sender, e) => executeFunc(cmd)
                    );

                    switch (cmd)
                    {
                        case IChildCommand childCmd:
                            {
                                var subCommands = childCmd.GetItems()
                                    ?.Select(subCmd =>
                                    {
                                        if (subCmd is IContextCommand subCtxCmd)
                                        {
                                            return new ToolStripMenuItem(
                                                subCtxCmd.Text,
                                                subCtxCmd.Icon,
                                                (sender, e) => executeFunc(subCtxCmd)
                                            );
                                        }

                                        return null;
                                    });

                                if (subCommands != null)
                                {
                                    item.DropDownItems.AddRange(subCommands.ToArray<ToolStripItem>());
                                }

                                break;
                            }
                    }

                    return item;
                });
    }
}
