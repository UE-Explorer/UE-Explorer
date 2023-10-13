using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using UEExplorer.Framework;
using UEExplorer.Framework.Commands;
using UEExplorer.Framework.UI.Commands;
using UEExplorer.Properties;
using UEExplorer.UI.Forms;
using UELib;

namespace UEExplorer.Tools.Commands
{
    [CommandCategory(CommandCategories.View)]
    internal class OpenBufferPageMenuCommand : IMenuCommand, IContextCommand
    {
        public bool CanExecute(object subject)
        {
            object resolvedTarget = TargetResolver.Resolve(subject);
            return resolvedTarget is IBuffered buffered
                   && buffered.GetBufferSize() != 0;
        }

        public Task Execute(object subject)
        {
            object resolvedTarget = TargetResolver.Resolve(subject);

            var target = (IBuffered)resolvedTarget;
            Debug.Assert(target != null, nameof(target) + " != null");

            if (target.GetBufferSize() == 0)
            {
                return Task.CompletedTask;
            }

            var hexDialog = new HexViewerForm(target, target.GetBufferId(true));
            hexDialog.Show();

            return Task.CompletedTask;
        }

        [Localizable(true)] public string Text => Resources.OpenBufferCommand;
        public Keys ShortcutKeys => Keys.B;
        public Image Icon => Resources.Binary;

        public IEnumerable<IMenuCommand> GetItems()
        {
            return new IMenuCommand[] { new OpenTableBufferPageMenuCommand() };
        }
    }

    // TODO: Deprecate, instead offer a Table node under the object node
    internal class OpenTableBufferPageMenuCommand : MenuCommand, IContextCommand
    {
        public bool CanExecute(object subject)
        {
            object resolvedTarget = TargetResolver.Resolve(subject);
            return resolvedTarget is IContainsTable tableObject
                   && tableObject is IBuffered buffered
                   && buffered.GetBufferSize() != 0;
        }

        public Task Execute(object subject)
        {
            object resolvedTarget = TargetResolver.Resolve(subject);

            var target = (IBuffered)resolvedTarget;
            Debug.Assert(target != null, nameof(target) + " != null");

            if (target.GetBufferSize() == 0)
            {
                return Task.CompletedTask;
            }

            var hexDialog = new HexViewerForm(target, target.GetBufferId(true));
            hexDialog.Show();

            return Task.CompletedTask;
        }

        [Localizable(true)] public override string Text => Resources.TableIdentifier;
        public override Image Icon => Resources.Code;
    }
}
