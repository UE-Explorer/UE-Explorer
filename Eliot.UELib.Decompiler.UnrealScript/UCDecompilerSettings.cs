using UELib.Decompiler.Common;

namespace UELib.Decompiler.UnrealScript
{
    public enum UCDecompilerMode
    {
        Default,
        Clean
    }

    public class UCDecompilerSettings : CommonDecompilerSettings
    {
        public UCDecompilerMode Mode { get; set; }
        public bool OutputQualifiedIdentifiers { get; set; }
    }
}
