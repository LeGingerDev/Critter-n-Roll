using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLeaderboardDisplay : LeaderboardDisplay<PlayerUserData>
{
    [SerializeField, FoldoutGroup("Visual Settings")]
    private Color _oddColor = Color.white;
    [SerializeField, FoldoutGroup("Visual Settings")]
    private Color _evenColor = Color.white;

    [SerializeField, FoldoutGroup("UI Elements")]
    private Image _backgroundImage;
    [SerializeField, FoldoutGroup("UI Elements")]
    private TextMeshProUGUI rankText;
    [SerializeField, FoldoutGroup("UI Elements")]
    private TextMeshProUGUI nameText;
    [SerializeField, FoldoutGroup("UI Elements")]
    private TextMeshProUGUI levelText;
    [SerializeField, FoldoutGroup("UI Elements")]
    private TextMeshProUGUI playedText;
    [SerializeField, FoldoutGroup("UI Elements")]
    private TextMeshProUGUI finishedText;
    [SerializeField, FoldoutGroup("UI Elements")]
    private TextMeshProUGUI winRateText;

    public override void InitializeEntry(PlayerUserData playerData, int rank)
    {
        _backgroundImage.color = GetHighlightColor(rank);

        if (rankText != null) rankText.text = rank.ToString();
        if (nameText != null) nameText.text = playerData.ChatData.GetDisplayName();
        if (levelText != null) levelText.text = playerData.GetLevel().ToString();
        if (playedText != null) playedText.text = playerData.GetNumberOfLevelsPlayed().ToString();
        if (finishedText != null) finishedText.text = playerData.GetNumberOfLevelsFinished().ToString();
        if (winRateText != null) winRateText.text = $"{playerData.GetWinRatePercentage():F0}%";
    }

    public Color GetHighlightColor(int rank)
    {
        if (rank % 2 == 0)
            return _evenColor;
        else
            return _oddColor;
    }


}
