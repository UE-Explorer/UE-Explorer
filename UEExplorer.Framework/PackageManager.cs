using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using UELib;

namespace UEExplorer.Framework
{
    public class PackageManager : IComponent
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

        public ISite Site { get; set; }
        public event EventHandler Disposed;

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
            if (_Packages.Add(packageReference))
            {
                PackageRegistered?.Invoke(this, new PackageEventArgs(packageReference));
            }

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

            var linker = UnrealLoader.LoadPackage(packageReference.FilePath);
            packageReference.Linker = linker;

            PackageLoaded?.Invoke(this, new PackageEventArgs(packageReference));
        }

        public void InitializePackage(PackageReference packageReference, UnrealPackage.InitFlags flags =
            UnrealPackage.InitFlags.RegisterClasses |
            UnrealPackage.InitFlags.Construct)
        {
            Debug.Assert(packageReference.Linker != null);

            packageReference.Linker.InitializePackage(flags);
            PackageInitialized?.Invoke(this, new PackageEventArgs(packageReference));
        }

        public void UnloadPackage(PackageReference packageReference)
        {
            Debug.Assert(packageReference.Linker != null);

            packageReference.Linker.Dispose();
            packageReference.Linker = null;
            _Packages.Remove(packageReference);

            PackageUnloaded?.Invoke(this, new PackageEventArgs(packageReference));
        }

        public event PackageEventHandler PackageRegistered;
        public event PackageEventHandler PackageLoaded;
        public event PackageEventHandler PackageInitialized;
        public event PackageEventHandler PackageUnloaded;
    }
}
