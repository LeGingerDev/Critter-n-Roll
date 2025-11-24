using Sirenix.OdinInspector;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor; // Needed for AssetDatabase
#endif
using UnityEngine;
namespace Audio.Models
{


    [CreateAssetMenu(fileName = "Audio_", menuName = "BagOfDucks/Audio/Create Audio File")]
    public class AudioClipSO : ScriptableObject
    {
        [ReadOnly]
        public string id;
        public string constName;

        public List<AudioClip> audioClips = new List<AudioClip>();
        public bool isLooping;
        public bool isRandomPitch;
        [ShowIf("isRandomPitch")]
        public float pitchVariance;
        [Range(0f, 2f)]
        public float volume;
        [Range(0f, 1f)]
        public float spatialBlend; // 0 = 2D, 1 = 3D

        public void UpdateConstName()
        {
            constName = constName.Replace(" ","_").ToUpper();
            id = constName.Replace("_", "-").ToLower();
        }

        /// <summary>
        /// Returns a random AudioClip from the list, or null if the list is empty.
        /// </summary>
        public AudioClip GetRandomClip()
        {
            if (audioClips == null || audioClips.Count == 0)
                return null;
            return audioClips[Random.Range(0, audioClips.Count)];
        }

        public float GetRandomPitch()
        {
            if(isRandomPitch)
                return 1f + Random.Range(-pitchVariance, pitchVariance);
            return 1f;
        }

        #region Editor

        [Button("Update Name")]
        private void UpdateName()
        {
            string formattedConstName = FormatConstName(constName);
            // Update the internal ScriptableObject name (visible in Inspector)
            this.name = $"AudioClipSO_{formattedConstName}";

            // Also rename the actual .asset file on disk to avoid Unity warnings
#if UNITY_EDITOR
            string assetPath = AssetDatabase.GetAssetPath(this);
            if (!string.IsNullOrEmpty(assetPath))
            {
                // If you want ".asset" extension explicitly, pass it in as the second parameter
                // but Unity will handle the extension automatically.
                AssetDatabase.RenameAsset(assetPath, $"AudioClipSO_{formattedConstName}");
                AssetDatabase.SaveAssets();
            }
#endif
        }

        private string FormatConstName(string input)
        {
            // Converts a string like "ENEMY_DEATH" or "enemy-death" into "EnemyDeath".
            if (string.IsNullOrEmpty(input))
                return input;

            // Replace hyphens with underscores in case they've been set, then lowercase everything.
            string normalized = input.Replace("-", "_").ToLower();
            string[] words = normalized.Split('_');
            for (int i = 0; i < words.Length; i++)
            {
                if (!string.IsNullOrEmpty(words[i]))
                    words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1);
            }
            return string.Join("", words);
        }

        private void OnValidate()
        {
            // Automatically update constName whenever the object is validated.
            UpdateConstName();
        }
        #endregion Editor
    }
}