using System;
using Core.Events;
using UnityEngine;

namespace Core.Utility
{
    public abstract class NewYieldInstruction : CustomYieldInstruction, IDisposable
    {
        public NewYieldInstruction()
        {
            ServiceBus.RegisterInstance(this);
        }

        public virtual void Dispose()
        {
            ServiceBus.UnregisterInstance(this);
        }

        public void Publish(string topic, params object[] args)
        {
            ServiceBus.Publish(topic, this, args);
        }
    }
}