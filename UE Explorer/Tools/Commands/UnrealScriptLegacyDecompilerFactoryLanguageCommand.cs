using System.Drawing;
using System.Threading.Tasks;
using UEExplorer.Framework;
using UEExplorer.Framework.UI.Commands;
using UELib.Core;
using UELib.Decompiler.UnrealScript;

namespace UEExplorer.Tools.Commands
{
    internal sealed class UnrealScriptLegacyDecompilerFactoryLanguageCommand : DecompilerFactoryLanguageCommand,
        IContextCommand,
        IDecompilerFactoryCommand
    {
        private readonly UCLegacyDecompiler _Decompiler = new UCLegacyDecompiler();

        public bool CanExecute(object subject) =>
            TargetResolver.Resolve(subject) is UObject obj && _Decompiler.CanDecompile(obj);

        public Task Execute(object subject)
        {
            SetupPage(subject, _Decompiler);

            return Task.CompletedTask;
        }

        public override string Text => "UnrealScript (Legacy)";
        public override Image Icon { get; }
    }
}
