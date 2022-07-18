using UEExplorer.UI.Tabs;

namespace UEExplorer.UI
{
    public interface IActionPanel<T> : IObjectHandler<T>
    {
        void RestoreState(ref ActionState state);
        void StoreState(ref ActionState state);
    }
}