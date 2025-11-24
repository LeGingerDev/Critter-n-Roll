using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using System.Collections.Generic;

public class DifficultyVisualiser : MonoBehaviour
{
    [SerializeField]
    private Image _difficultyImage;
    [SerializeField]
    private Image _difficultyBorder;
    [SerializeField]
    private TextMeshProUGUI _difficultyText;
    [FoldoutGroup("Colors"), SerializeField]
    private List<Color> _difficultyColours = new List<Color>();
    [FoldoutGroup("Colors"), SerializeField]
    private List<Color> _difficultyColoursBorder = new List<Color>();
    public void Initialise(LevelDifficulty difficulty)
    {
        _difficultyBorder.color = GetBorderColor(difficulty);
        _difficultyImage.color = GetColor(difficulty);
        _difficultyText.text = difficulty.ToString();
    }

    public Color GetBorderColor(LevelDifficulty difficulty)
    {
        if ((int)difficulty < 0 || (int)difficulty >= _difficultyColoursBorder.Count)
        {
            Debug.LogWarning($"Difficulty {difficulty} is out of range for the defined border colors.");
            return Color.white;
        }
        return _difficultyColoursBorder[(int)difficulty];
    }

    public Color GetColor(LevelDifficulty difficulty)
    {
        if ((int)difficulty < 0 || (int)difficulty >= _difficultyColours.Count)
        {
            Debug.LogWarning($"Difficulty {difficulty} is out of range for the defined colors.");
            return Color.white; // Default color if out of range
        }
        return _difficultyColours[(int)difficulty];
    }
}