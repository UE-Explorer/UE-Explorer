using UELib;

namespace UEExplorer.UI.Nodes
{
    /// <summary>
    /// This class can be decompiled and holds a reference to a decompilable object.
    /// </summary>
    public interface IWithDecompilableObject<out T> : IUnrealDecompilable
    {
        T Object { get; }
    }
}