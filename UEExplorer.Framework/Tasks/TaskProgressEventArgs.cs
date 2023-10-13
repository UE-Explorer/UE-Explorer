using System;

namespace UEExplorer.Framework.Tasks
{
    public class TaskProgressEventArgs : EventArgs
    {
        public int Count, Max;

        public TaskProgressEventArgs(int count, int max)
        {
            Count = count;
            Max = max;
        }
    }
}