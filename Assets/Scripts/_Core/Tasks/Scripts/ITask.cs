using System.Collections;

namespace Tasks
{
    public interface ITask
    {
        public IEnumerator ExecuteInternal();
        public bool CanExecute();
        public bool IsFinished();
    }
}