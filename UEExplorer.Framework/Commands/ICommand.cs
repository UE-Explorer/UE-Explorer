using System.Reflection;
using System.Threading.Tasks;

namespace UEExplorer.Framework.Commands
{
    public interface ICommand
    {
        /// <summary>
        /// If accepted, the command is determined applicable.
        /// </summary>
        /// <param name="subject"></param>
        bool CanExecute(object subject);

        Task Execute(object subject);
    }

    public interface ICommand<in T> where T : class
    {
        /// <summary>
        /// If accepted, the command is determined applicable.
        /// </summary>
        /// <param name="subject"></param>
        bool CanExecute(T subject);

        Task Execute(T subject);
    }

    public static class CommandExtensions
    {
        public static string GetCategory(this ICommand command)
        {
            var categoryAttribute = command.GetType().GetCustomAttribute<CommandCategoryAttribute>();
            return categoryAttribute?.Category;
        }
        
        public static string GetCategory(this ICommand<object> command)
        {
            var categoryAttribute = command.GetType().GetCustomAttribute<CommandCategoryAttribute>();
            return categoryAttribute?.Category;
        }
    }
}
