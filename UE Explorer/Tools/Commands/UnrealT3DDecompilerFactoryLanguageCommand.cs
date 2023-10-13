using System.Drawing;
using System.Threading.Tasks;
using UEExplorer.Framework;
using UEExplorer.Framework.UI.Commands;
using UELib.Core;
using UELib.Decompiler.T3D;

namespace UEExplorer.Tools.Commands
{
    internal sealed class UnrealT3DDecompilerFactoryLanguageCommand : DecompilerFactoryLanguageCommand,
        IContextCommand,
        IDecompilerFactoryCommand
    {
        private readonly T3DDecompiler _Decompiler = new T3DDecompiler();

        public UnrealT3DDecompilerFactoryLanguageCommand() => PageUniqueName = "DecompilerT3D";

        public bool CanExecute(object subject) =>
            TargetResolver.Resolve(subject) is UObject obj && _Decompiler.CanDecompile(obj);

        public Task Execute(object subject)
        {
            SetupPage(subject, _Decompiler);

            return Task.CompletedTask;
        }

        public override string Text => "UnrealT3D";
        public override Image Icon { get; }
    }
}
