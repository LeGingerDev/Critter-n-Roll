using UnityEngine;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "Level_", menuName = "LGD/Levels/Create Level")]
public class Level : ScriptableObject
{
    [FoldoutGroup("Level Settings")]
    public LevelDifficulty levelDifficulty;

    [FoldoutGroup("Level Settings")]
    [SerializeField] public string levelName;

    [FoldoutGroup("Level Settings")]
    [SerializeField] public string levelDescription;

    [FoldoutGroup("Visual & Audio")]
    [SerializeField] public Sprite levelVisual;

    [FoldoutGroup("Gameplay")]
    [SerializeField] public float baseLevelDuration = 60f;

    [FoldoutGroup("References")]
    [SerializeField] public LevelController levelPrefab;

    [FoldoutGroup("References")]
    [ShowIf("levelPrefab")]
    [PreviewField(75, ObjectFieldAlignment.Center)]
    [ReadOnly]
    [SerializeField] private GameObject _levelPrefabPreview;

    [FoldoutGroup("References")]
    [SerializeField] public Environment environment;

    // Getter functions as per your preference
    public string GetLevelName() => levelName;
    public string GetLevelDescription() => levelDescription;
    public Sprite GetLevelVisual() => levelVisual;
    public float GetBaseLevelDuration() => baseLevelDuration;
    public LevelController GetLevelPrefab() => levelPrefab;
    public Environment GetEnvironment() => environment;

    public float GetLevelDuration()
    {
        switch (levelDifficulty)
        {
            case LevelDifficulty.Easy:
                return baseLevelDuration;
            case LevelDifficulty.Medium:
                return baseLevelDuration * 1.5f;
            case LevelDifficulty.Hard:
                return baseLevelDuration * 2f;
            case LevelDifficulty.Insane:
                return baseLevelDuration * 2.5f;
            default:
                return baseLevelDuration;
        }
    }

    public float GetExperienceMultiplier()
    {
        return levelDifficulty switch
        {
            LevelDifficulty.Easy => 1.0f,
            LevelDifficulty.Medium => 1.5f,
            LevelDifficulty.Hard => 2.0f,
            LevelDifficulty.Insane => 3.0f,
            _ => 1.0f
        };
    }

#if UNITY_EDITOR
    // Update the preview field when the levelPrefab changes
    private void OnValidate()
    {
        if (levelPrefab != null)
        {
            _levelPrefabPreview = levelPrefab.gameObject;
        }
        else
        {
            _levelPrefabPreview = null;
        }
    }

    [FoldoutGroup("Automation")]
    [Button("Auto-Rename Assets", ButtonSizes.Large)]
    private void AutoRenameAssets()
    {
        if (string.IsNullOrEmpty(levelName))
        {
            Debug.LogWarning("Level name is empty! Please set a level name before auto-renaming.");
            return;
        }

        string formattedName = FormatAssetName();

        // Rename the ScriptableObject
        RenameScriptableObject(formattedName);

        // Rename the sprite if it exists
        if (levelVisual != null)
        {
            RenameAsset(levelVisual, formattedName);
        }

        // Rename the prefab if it exists
        if (levelPrefab != null)
        {
            RenameAsset(levelPrefab.gameObject, formattedName);
        }

        Debug.Log($"Successfully renamed assets to: {formattedName}");
    }

    private string FormatAssetName()
    {
        // Clean the level name (remove spaces, special characters if needed)
        string cleanLevelName = levelName.Replace(" ", "_");
        return $"{levelDifficulty}_{cleanLevelName}";
    }

    private void RenameScriptableObject(string newName)
    {
        string assetPath = AssetDatabase.GetAssetPath(this);
        if (!string.IsNullOrEmpty(assetPath))
        {
            AssetDatabase.RenameAsset(assetPath, newName);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }

    private void RenameAsset(Object asset, string newName)
    {
        if (asset == null) return;

        string assetPath = AssetDatabase.GetAssetPath(asset);
        if (!string.IsNullOrEmpty(assetPath))
        {
            AssetDatabase.RenameAsset(assetPath, newName);
        }
    }

    [FoldoutGroup("Automation")]
    [Button("Preview Asset Name", ButtonSizes.Medium)]
    private void PreviewAssetName()
    {
        if (string.IsNullOrEmpty(levelName))
        {
            Debug.Log("Please set a level name to preview the formatted name.");
            return;
        }

        string previewName = FormatAssetName();
        Debug.Log($"Assets will be renamed to: {previewName}");
    }
#endif
}