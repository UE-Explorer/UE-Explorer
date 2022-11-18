using System;
using UELib;
using UELib.Annotations;

namespace UEExplorer.Framework
{
    [Serializable]
    public class PackageReference
    {
        [NotNull] public readonly string FilePath;

        public PackageReference([NotNull] string filePath, [CanBeNull] UnrealPackage linker)
        {
            FilePath = filePath;
            Linker = linker;
        }

        [field: NonSerialized] [CanBeNull] public UnrealPackage Linker { get; internal set; }

        public bool IsActive() => Linker != null;
    }
}
