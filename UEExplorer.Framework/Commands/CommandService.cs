using System;
using System.Threading.Tasks;

namespace UEExplorer.Framework.Commands
{
    public class CommandService
    {
        public Task Execute<TCommand>(object subject)
            where TCommand : ICommand<object>
        {
            var command = (ICommand<object>)Activator.CreateInstance(typeof(TCommand));
            return Execute(command, subject);
        }
        
        public Task Execute<TCommand, TSubject>(TSubject subject)
            where TCommand : ICommand<TSubject>
            where TSubject : class
        {
            var command = (ICommand<object>)Activator.CreateInstance(typeof(TCommand));
            return Execute(command, subject);
        }
        
        public Task Execute<TSubject>(ICommand<TSubject> command, TSubject subject)
            where TSubject : class
        {
            return command.Execute(subject);
        }
    }
}
