using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UELib;

namespace UEExplorer.Framework
{
    public class PackageManager : IDisposable
    {
        private readonly PackageCollection _Packages = new PackageCollection();

        public IEnumerable<PackageReference> Packages => _Packages;

        public void Dispose()
        {
            foreach (var packageReference in _Packages)
            {
                packageReference.Linker?.Dispose();
            }
        }

        public IEnumerable<PackageReference> EnumeratePackages()
        {
            foreach (var packageReference in Packages)
            {
                yield return packageReference;
            }
        }

        public PackageReference RegisterPackage(string filePath)
        {
            var packageReference = new PackageReference(filePath, null);
            RegisterPackage(packageReference);
            return packageReference;
        }

        public void RegisterPackage(PackageReference packageReference)
        {
            if (_Packages.Add(packageReference))
            {
                PackageRegistered?.Invoke(this, new PackageEventArgs(packageReference));
            }
        }

        public void LoadPackage(PackageReference packageReference)
        {
            Debug.Assert(packageReference.Linker == null, "package is already loaded");

            if (!File.Exists(packageReference.FilePath))
            {
                packageReference.Error =
                    new Exception($"Couldn't find package file '{packageReference.FilePath}'.");
                PackageError?.Invoke(this, new PackageEventArgs(packageReference));
                // abort loading events
                return;
            }

            try
            {
                var stream = new UPackageStream(packageReference.FilePath, FileMode.Open, FileAccess.Read);
                var linker = new UnrealPackage(stream);
                if (packageReference.Settings.EngineBuild.HasValue)
                {
                    linker.Build = new UnrealPackage.GameBuild(
                        packageReference.Settings.EngineBuild.Value.PackageVersion,
                        packageReference.Settings.EngineBuild.Value.PackageLicenseeVersion,
                        packageReference.Settings.EngineBuild.Value.Generation,
                        null,
                        0
                    );
                }
                else
                {
                    linker.BuildTarget = packageReference.Settings.BuildNameTarget;
                }

                // TODO: Auto-detection
                if (packageReference.Settings.CookerPlatform != PackageCookerPlatform.Auto)
                {
                    linker.CookerPlatform = (BuildPlatform)packageReference.Settings.CookerPlatform;
                }

                linker.Deserialize(stream);
                packageReference.Linker = linker;
            }
            catch (Exception ex)
            {
                packageReference.Error = new Exception("Load", ex);
                PackageError?.Invoke(this, new PackageEventArgs(packageReference));
            }

            PackageLoaded?.Invoke(this, new PackageEventArgs(packageReference));
        }

        public void InitializePackage(PackageReference packageReference, UnrealPackage.InitFlags flags =
            UnrealPackage.InitFlags.RegisterClasses |
            UnrealPackage.InitFlags.Construct)
        {
            Debug.Assert(packageReference.Linker != null);

            try
            {
                packageReference.Linker.InitializePackage(flags);
                PackageInitialized?.Invoke(this, new PackageEventArgs(packageReference));
            }
            catch (Exception ex)
            {
                packageReference.Error = new Exception("Initialize", ex);
                PackageError?.Invoke(this, new PackageEventArgs(packageReference));
            }
        }

        public void UnloadPackage(PackageReference packageReference)
        {
            if (packageReference.Linker != null)
            {
                packageReference.Linker.Dispose();
                packageReference.Linker = null;
            }

            PackageUnloaded?.Invoke(this, new PackageEventArgs(packageReference));
        }

        public void RemovePackage(PackageReference packageReference)
        {
            // Ensure we unload and dispose of this package.
            if (packageReference.Linker != null)
            {
                UnloadPackage(packageReference);
            }

            _Packages.Remove(packageReference);
            PackageRemoved?.Invoke(this, new PackageEventArgs(packageReference));
        }

        public event PackageEventHandler PackageRegistered;
        public event PackageEventHandler PackageLoaded;
        public event PackageEventHandler PackageInitialized;
        public event PackageEventHandler PackageUnloaded;
        public event PackageEventHandler PackageRemoved;
        public event PackageEventHandler PackageError;
    }
}
