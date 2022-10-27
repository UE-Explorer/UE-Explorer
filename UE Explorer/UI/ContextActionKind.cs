namespace UEExplorer.UI
{
    public enum ContextActionKind
    {
        /// <summary>
        /// Will use one of the below actions depending on the node's case.
        /// </summary>
        Auto,
        
        Location,
        
        Decompile,
        DecompileExternal,
        
        Play,

        Binary,
        
        ExportAs,
        ExportExternal,
        
        DecompileTokens,
        DisassembleTokens,
        
        ViewException,
        ViewBuffer,
        ViewTableBuffer
    }
}