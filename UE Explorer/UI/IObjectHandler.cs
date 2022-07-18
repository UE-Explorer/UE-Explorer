namespace UEExplorer.UI
{
    public interface IObjectHandler<T>
    {
        ContentNodeAction Action { get; }
        T Object { get; set; }
    }
}
