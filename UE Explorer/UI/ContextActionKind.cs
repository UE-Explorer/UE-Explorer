namespace UEExplorer.UI
{
    public enum ContextActionKind
    {
        Action,
        Location,

        /// <summary>
        /// Will use one of the below actions depending on the node's case.
        /// </summary>
        Auto,
        
        Decompile,
        DecompileExternal,
        
        Open,

        Binary,
        
        ExportAs,
        ExportExternal,
        
        DecompileTokens,
        DisassembleTokens,
        
        ViewException,
        ViewBuffer,
        ViewTableBuffer,
    }
}
