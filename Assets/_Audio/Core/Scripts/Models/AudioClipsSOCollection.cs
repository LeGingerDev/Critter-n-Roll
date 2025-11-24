using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Audio.Models
{
    [CreateAssetMenu(fileName = "AudioClipsCollection", menuName = "BagOfDucks/Audio/Create Audio Clips Collection")]
    public class AudioClipsSOCollection : ScriptableObject
    {
        [SerializeField]
        private List<AudioClipSO> _audioClipsCollection = new List<AudioClipSO>();

        public AudioClipSO GetByID(string id)
        {
            return _audioClipsCollection.Find(x => x.id == id);
        }

        [Button("Find All Clips")]
        public void FindAllClips()
        {
#if UNITY_EDITOR
            // Define the folder to search.
            string searchPath = "Assets/_Audio";
            // Find all assets of type AudioClipSO in the specified folder.
            string[] guids = AssetDatabase.FindAssets("t:AudioClipSO", new string[] { searchPath });
            _audioClipsCollection.Clear();

            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                AudioClipSO clipSO = AssetDatabase.LoadAssetAtPath<AudioClipSO>(assetPath);
                if (clipSO == null)
                    continue;

                // Only add valid assets: must have an id, a constName, and at least one AudioClip.
                if (string.IsNullOrEmpty(clipSO.id) ||
                    string.IsNullOrEmpty(clipSO.constName) ||
                    clipSO.audioClips == null || clipSO.audioClips.Count == 0)
                {
                    continue;
                }

                _audioClipsCollection.Add(clipSO);
            }
            Debug.Log($"Found {_audioClipsCollection.Count} valid AudioClipSO assets.");
#endif
        }
    }
}
