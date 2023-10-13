using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UEExplorer.Framework;
using UEExplorer.Framework.Tasks;
using UEExplorer.Properties;
using UELib;
using UELib.Annotations;
using UELib.Core;

namespace UEExplorer.PackageTasks
{
    public class SearchInPackagesTask<T> : ActionTask
        where T : UObject
    {
        private readonly PackageReference _InPackageReference;
        private readonly string _SearchText;

        public SearchInPackagesTask(PackageReference inPackageReference, string searchText)
        {
            _InPackageReference = inPackageReference;
            _SearchText = searchText;
        }

        [CanBeNull] public List<List<TextSearchHelpers.DocumentResult>> Results { get; private set; }

        private static IEnumerable<IUnrealDecompilable> GetDecompilableObjects(UnrealPackage linker) =>
            linker.Objects
                .OfType<T>()
                .Where(c => c.ExportTable != null);

        public override Task Execute() =>
            new Task(() =>
            {
                var packageManager = ServiceHost.GetRequired<PackageManager>();

                int currentCount = 0;
                int packagesCount = packageManager.Packages.Count();

                Results = new List<List<TextSearchHelpers.DocumentResult>>(packagesCount);
                foreach (var packageReference in packageManager.EnumeratePackages())
                {
                    OnProgressChanged(new TaskProgressEventArgs(currentCount++, packagesCount));
                    if (packageReference.Linker?.Objects == null)
                    {
                        continue;
                    }

                    // Lazy solution :D
                    if (_InPackageReference != null && packageReference != _InPackageReference)
                    {
                        continue;
                    }

                    var packageResults = new List<TextSearchHelpers.DocumentResult>();
                    var objects = GetDecompilableObjects(packageReference.Linker);
                    foreach (var obj in objects)
                    {
                        if (CancellationToken.IsCancellationRequested)
                        {
                            break;
                        }

                        string textContent = obj.Decompile();
                        var findResults = TextSearchHelpers.FindText(textContent, _SearchText, CancellationToken);
                        if (!findResults.Any())
                        {
                            continue;
                        }

                        var document = new TextSearchHelpers.DocumentResult { Document = obj, Results = findResults };
                        packageResults.Add(document);
                    }

                    if (!packageResults.Any())
                    {
                        continue;
                    }

                    packageResults.Insert(0, new TextSearchHelpers.DocumentResult { Document = packageReference });
                    Results.Add(packageResults);
                }
            }, CancellationToken);

        public override string Status() => string.Format(Resources.Searching_for___0__, _SearchText);
    }
}
