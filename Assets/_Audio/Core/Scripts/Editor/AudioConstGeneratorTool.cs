#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using Audio.Models; // Ensure this points to your AudioClipSO definition

namespace Audio.Core.Editor
{
    public static class AudioConstGeneratorTool
    {
        [MenuItem("BagOfDucks/Generate Audio Consts")]
        public static void GenerateAudioConstIds()
        {
            // Define the folder where your AudioClipSO assets are located.
            string searchPath = "Assets/_Audio";
            // Find all assets of type AudioClipSO in the folder.
            string[] guids = AssetDatabase.FindAssets("t:AudioClipSO", new string[] { searchPath });

            if (guids.Length == 0)
            {
                Debug.LogWarning("No AudioClipSO assets found in " + searchPath);
                return;
            }

            List<AudioClipSO> validAudioAssets = new List<AudioClipSO>();
            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                AudioClipSO asset = AssetDatabase.LoadAssetAtPath<AudioClipSO>(assetPath);
                if (asset == null)
                    continue;

                // Exclude assets with no id, no constName, or no Audio Clips.
                if (string.IsNullOrEmpty(asset.id) ||
                    string.IsNullOrEmpty(asset.constName) ||
                    asset.audioClips == null || asset.audioClips.Count == 0)
                {
                    continue;
                }

                validAudioAssets.Add(asset);
            }

            if (validAudioAssets.Count == 0)
            {
                Debug.LogWarning("No valid AudioClipSO assets found that meet all criteria.");
                return;
            }

            // Build the content for the generated file.
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("// Auto-generated file. Do not modify manually.");
            sb.AppendLine("namespace Audio.Core");
            sb.AppendLine("{");
            sb.AppendLine("    public static class AudioConstIds");
            sb.AppendLine("    {");
            foreach (AudioClipSO asset in validAudioAssets)
            {
                // Assuming asset.constName is a valid C# identifier.
                sb.AppendLine($"        public const string {asset.constName} = \"{asset.id}\";");
            }
            sb.AppendLine("    }");
            sb.AppendLine("}");

            // Define the fixed output directory and file path.
            string targetDir = "Assets/_Audio/Core/Scripts";
            if (!Directory.Exists(targetDir))
            {
                Directory.CreateDirectory(targetDir);
            }
            string filePath = Path.Combine(targetDir, "AudioConstIds.cs");

            File.WriteAllText(filePath, sb.ToString());
            Debug.Log("AudioConstIds generated at: " + filePath);

            // Refresh the AssetDatabase to show the new file in Unity.
            AssetDatabase.Refresh();
        }
    }
}
#endif
