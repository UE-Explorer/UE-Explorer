using System;
using System.Threading;
using System.Threading.Tasks;

namespace UEExplorer.Framework.Tasks
{
    public abstract class ActionTask
    {
        public CancellationToken CancellationToken { get; internal set; }

        public event TaskProgressEventHandler ProgressChanged;
        public event EventHandler Completed;
        public event EventHandler<Exception> Error;

        public abstract Task Execute();

        protected void OnProgressChanged(TaskProgressEventArgs e) => ProgressChanged?.Invoke(this, e);

        protected void OnError(Exception exception) => Error?.Invoke(this, exception);

        public void OnCompleted() => Completed?.Invoke(this, EventArgs.Empty);

        public abstract string Status();
    }
}
