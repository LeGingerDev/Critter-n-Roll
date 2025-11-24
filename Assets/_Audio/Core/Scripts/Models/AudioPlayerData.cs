using Audio.Core;
using System;
using UnityEngine;
using Utilities.Attributes;
namespace Audio.Models
{
    [Serializable]
    public class AudioPlayerData
    {
        [SerializeField, ConstDropdown(typeof(AudioConstIds))]
        public string clipId;
        [SerializeField]
        public bool isOneShot = true;
        [SerializeField]
        public bool positionBased;
    }
}