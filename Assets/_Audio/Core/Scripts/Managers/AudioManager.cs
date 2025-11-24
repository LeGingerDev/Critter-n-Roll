using Audio.Models;
using Core.Singleton;
using System;
using UnityEngine;

namespace Audio.Managers
{
    public partial class AudioManager : MonoSingleton<AudioManager>
    {
        [SerializeField]
        private AudioClipsSOCollection _audioClipsCollection;


    }
}

