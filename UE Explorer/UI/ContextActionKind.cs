namespace UEExplorer.UI
{
    public enum ContextActionKind
    {
        Action,

        /// <summary>
        /// Will use one of the below actions depending on the node's case.
        /// </summary>
        Auto,
        
        Location,
        
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
