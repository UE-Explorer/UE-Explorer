namespace UEExplorer.UI
{
    public enum ContentNodeAction
    {
        /// <summary>
        /// Will use one of the below actions depending on the node's case.
        /// </summary>
        Auto,
        
        Decompile,
        DecompileExternal,

        Binary,
        
        ExportAs,
        ExportExternal,
        
        DecompileOuter,
        DecompileScriptProperties,
        DecompileClassReplication,
        
        DecompileTokens,
        DisassembleTokens,
        
        ViewException,
        ViewBuffer,
        ViewTableBuffer
    }
}