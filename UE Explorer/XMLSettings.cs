using System;
using System.Collections.Generic;
using UELib;

namespace UEExplorer
{
    [System.Reflection.ObfuscationAttribute(Exclude = true)]
    public class XMLSettings
    {																									  
        #region Unreal Packages Decompiler Related Members
        public string NTLPath = "NativesTableList_UDK-2012-05";

        public UnrealPackage.InitFlags InitFlags = UnrealPackage.InitFlags.All;
        public bool bForceVersion;
        public bool bForceLicenseeMode;
        public ushort Version;
        public ushort LicenseeMode;
        public string Platform = "PC";
        #endregion

        #region Unreal Cache Extractor Related Membbers
        public string InitialCachePath = String.Empty;
        #endregion

        #region DECOMPILER
        public bool bSuppressComments;
        public string PreBeginBracket = "%NEWLINE%%TABS%";
        public string PreEndBracket = "%NEWLINE%%TABS%";
        public int Indention = 4;
        public List<string> VariableTypes;
        #endregion

        #region THIRDPARY
        public string UEModelAppPath = String.Empty;
        public string HEXWorkshopAppPath = String.Empty;
        #endregion
    }
}