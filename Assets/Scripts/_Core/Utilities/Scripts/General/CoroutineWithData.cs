using System.Collections;
using UnityEngine;

namespace General.Utility
{
    public class CoroutineWithData
    {
        public Coroutine Coroutine { get; private set; }
        public object Result { get; private set; }
        private IEnumerator target;

        public CoroutineWithData(MonoBehaviour owner, IEnumerator target)
        {
            this.target = target;
            this.Coroutine = owner.StartCoroutine(Run());
        }

        private IEnumerator Run()
        {
            while (target != null && target.MoveNext())
            {
                Result = target.Current;
                yield return Result;
            }

            target = null;
        }
    }
}