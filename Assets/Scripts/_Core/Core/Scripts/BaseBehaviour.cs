using Core.Events;
using Sirenix.OdinInspector;
using UnityEngine;
using Utilities.General;

namespace Core
{
    public class BaseBehaviour : SerializedMonoBehaviour
    {
        protected virtual void OnEnable()
        {
            ServiceBus.RegisterInstance(this);
        }

        protected virtual void OnDisable()
        {
            ServiceBus.UnregisterInstance(this);
        }

        public void Publish(string topic, params object[] args)
        {
            Debug.Log("Publishing: " + topic);
            ServiceBus.Publish(topic, this, args);
        }

        #region Random

        public int RandomRange(int min, int max) => OTBGRandom.Range(this, min, max);
        public int RandomRange(int max) => OTBGRandom.Range(this, 0, max);
        public float RandomRange(float min, float max) => OTBGRandom.Range(this, min, max);
        public float RandomRange(float max) => OTBGRandom.Range(this, 0, max);
        public bool RandomBool() => OTBGRandom.RandomBool(this);
        public float RandomValue() => OTBGRandom.Range(this, 0f, 1f);

        #endregion Random
    }
}