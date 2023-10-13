using System;
using System.Threading;
using System.Threading.Tasks;
using UEExplorer.Framework.Tasks;
using UEExplorer.Properties;
using UELib;

namespace UEExplorer.PackageTasks
{
    public class InitializePackageTask : ActionTask
    {
        private readonly UnrealPackage _Linker;

        private int _ProgressCount, _ProgressMax;

        public InitializePackageTask(UnrealPackage linker) => _Linker = linker;

        public override Task Execute() =>
            new Task(() =>
            {
                _Linker.NotifyPackageEvent += LinkerOnNotifyPackageEvent;
                try
                {
                    _ProgressMax = _Linker.Exports.Count + _Linker.Objects.Count;
                    OnProgressChanged(new TaskProgressEventArgs(0, _ProgressMax));
                    _Linker.InitializePackage(UnrealPackage.InitFlags.Deserialize |
                                              UnrealPackage.InitFlags.Link);
                }
                catch (Exception exception)
                {
                    OnError(exception);
                }
                finally
                {
                    _Linker.NotifyPackageEvent -= LinkerOnNotifyPackageEvent;
                }
            }, CancellationToken);

        public override string Status() => string.Format(Resources.StatusInitializePackage, _Linker.PackageName);

        private void LinkerOnNotifyPackageEvent(object sender, UnrealPackage.PackageEventArgs e)
        {
            if (e.EventId != UnrealPackage.PackageEventArgs.Id.Object)
            {
                return;
            }

            OnProgressChanged(new TaskProgressEventArgs(_ProgressCount++, _ProgressMax));
        }
    }
}
