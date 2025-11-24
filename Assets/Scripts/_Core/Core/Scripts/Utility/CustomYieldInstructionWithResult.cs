using UnityEngine;

namespace Core.Utility
{
    public abstract class CustomYieldInstructionWithResult<T> : CustomYieldInstruction
    {
        protected T result;
        protected bool finished = false;
        public override bool keepWaiting => !IsFinished();

        public virtual bool IsFinished() => finished;

        public virtual T GetResult() => result;
    }
}