using System;
using UELib;
using UELib.Annotations;

namespace UEExplorer.Framework
{
    [Serializable]
    public class PackageReference
    {
        public readonly string FilePath;

        public PackageReference(string filePath, [CanBeNull] UnrealPackage linker)
        {
            FilePath = filePath;
            Linker = linker;
        }

        [field: NonSerialized] [CanBeNull] public UnrealPackage Linker { get; internal set; }

        public bool IsActive() => Linker != null;
    }
}
