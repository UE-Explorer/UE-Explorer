using System;
using System.ComponentModel;
using System.IO;
using UELib;
using UELib.Annotations;

namespace UEExplorer.Framework
{
    [Flags]
    [Serializable]
    public enum PackageReferenceFlags
    {
        NoAutoLoad = 1 << 0,
    }

    [Serializable]
    public enum PackageCookerPlatform
    {
        /// <summary>
        /// Automatically determine the platform.
        /// </summary>
        Auto,

        /// <summary>
        /// The package should be treated as cooked for PC.
        /// </summary>
        PC,

        /// <summary>
        /// The package should be treated as cooked for Console.
        /// Console cooked packages are usually fully compressed,
        /// use the .xxx extension,
        /// and all editor data is completely stripped out.
        /// </summary>
        Console,
    }

    [Serializable]
    public struct PackageEngineBuild
    {
        public uint PackageVersion;
        public ushort PackageLicenseeVersion;

        public UnrealPackage.GameBuild.BuildName BuildName;
        public BuildGeneration Generation;
    }

    [Serializable]
    public struct PackageSettings
    {
        [DisplayName("Package Build")]
        public UnrealPackage.GameBuild.BuildName BuildNameTarget { get; set; }
        
        /// <summary>
        /// Some game packages, esp heavily modified ones cannot be detected as such.
        /// For such packages it is necessary to manually configure the build.
        ///
        /// If not null, override the auto-detected engine build.
        /// If null, fall back to <see cref="BuildNameTarget"></see>
        /// </summary>
        public PackageEngineBuild? EngineBuild { get; set; }

        /// <summary>
        /// A package cooked for console is drastically different, and usually cannot be detected automatically.
        /// For such packages it is necessary to manually configure the platform.
        /// </summary>
        [DisplayName("Package Platform")]
        public PackageCookerPlatform CookerPlatform { get; set; }
    }

    [Serializable]
    public class PackageReference : IComparable<PackageReference>, IComparable<string>
    {
        [NotNull] public readonly string FilePath;
        public readonly DateTime RegisterDate;

        public PackageSettings Settings;
        public PackageReferenceFlags Flags;

        public PackageReference([NotNull] string filePath, [CanBeNull] UnrealPackage linker)
        {
            FilePath = filePath;
            RegisterDate = DateTime.Now;

            Linker = linker;
        }

        [field: NonSerialized] [CanBeNull] public UnrealPackage Linker { get; internal set; }
        [field: NonSerialized] [CanBeNull] public Exception Error { get; internal set; }

        [DisplayName("Path")]
        public string ShortFilePath => Path.Combine(
            //"...",
            Path.GetFileName(Path.GetDirectoryName(FilePath)) ?? string.Empty,
            Path.GetFileNameWithoutExtension(FilePath));

        public int CompareTo(PackageReference other) => other.FilePath.Equals(FilePath)
            ? 0
            : RegisterDate.CompareTo(other.RegisterDate);

        public int CompareTo(string other) => string.Compare(FilePath, other, StringComparison.Ordinal);

        public bool IsActive() => Linker != null;

        public override int GetHashCode() => FilePath.GetHashCode();

        public override string ToString() => ShortFilePath;
    }
}
