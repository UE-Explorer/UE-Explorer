namespace UEExplorer.Framework.UI.Commands
{
    public interface ICommandBuilder<out TResult>
    {
        TResult Build(object subject);
    }
}
