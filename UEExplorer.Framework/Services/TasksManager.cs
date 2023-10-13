using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UEExplorer.Framework.Tasks;
using UELib.Annotations;

namespace UEExplorer.Framework.Services
{
    public class TasksManager
    {
        private readonly Queue<ActionTask> _Tasks = new Queue<ActionTask>();

        private int _SessionMax;

        [CanBeNull] public ActionTask CurrentTask { get; private set; }

        public event EventHandler<ActionTask> TaskStart, TaskStop;
        public event TaskProgressEventHandler ProgressChanged;

        public void Process()
        {
            if (CurrentTask != null || _Tasks.Count == 0)
            {
                return;
            }

            var nextTask = _Tasks.Dequeue();
            CurrentTask = nextTask;

            TaskStart?.Invoke(null, CurrentTask);
            OnProgressChanged(new TaskProgressEventArgs(_Tasks.Count, _SessionMax));

            if (_Tasks.Count == 0)
            {
                _SessionMax = 0;
            }

            nextTask
                .Execute()
                .RunSynchronously(TaskScheduler.Current);

            nextTask.OnCompleted();

            TaskStop?.Invoke(null, CurrentTask);
            CurrentTask = null;

            OnProgressChanged(new TaskProgressEventArgs(_Tasks.Count, _SessionMax));
        }

        public void Enqueue(ActionTask actionTask, CancellationToken cancellationToken)
        {
            actionTask.CancellationToken = cancellationToken;
            
            ++_SessionMax;
            _Tasks.Enqueue(actionTask);
        }

        private void OnProgressChanged(TaskProgressEventArgs e) => ProgressChanged?.Invoke(null, e);
    }
}
