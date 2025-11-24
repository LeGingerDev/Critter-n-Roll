using System;
using UnityEngine;

namespace Utilities.Data
{
    [Serializable]
    public class ValueChange
    {
        public ValueChange(float value, float maxValue)
        {
            this.value = value;
            this.maxValue = maxValue;
        }

        public ValueChange(int value, int maxValue)
        {
            this.value = value;
            this.maxValue = maxValue;
        }

        public float value;
        public float maxValue;

        public float GetPercentage()
        {
            return (value / maxValue);
        }

        public float GetRoundedToInt()
        {
            return Mathf.Round(value);
        }

        public float GetRoundDownToOneDecimal()
        {
            return Mathf.Floor(value * 10) / 10;
        }
    }
}