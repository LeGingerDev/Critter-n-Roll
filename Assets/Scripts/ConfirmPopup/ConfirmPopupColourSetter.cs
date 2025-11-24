using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmPopupColourSetter : MonoBehaviour
{
    [FoldoutGroup("Color References")]
    public List<ConfirmPopupColorData> colorDatas = new List<ConfirmPopupColorData>();

    [FoldoutGroup("UI References")]
    public Image headerBackground;
    [FoldoutGroup("UI References")]
    public Image headerStripes;
    [FoldoutGroup("UI References")]
    public Image footerBackground;
    [FoldoutGroup("UI References")]
    public Image footerStripes;
    [FoldoutGroup("UI References")]
    public Image popupBackground;

    [Button]
    public void Initialise(ConfirmPopup.ConfirmType confirmType)
    {
        ConfirmPopupColorData colorData = colorDatas.First(c => c.type == confirmType);
        headerBackground.color = colorData.headerFooterBackground;
        headerStripes.color = colorData.headerFooterStripes;
        footerBackground.color = colorData.headerFooterBackground;
        footerStripes.color = colorData.headerFooterStripes;
        popupBackground.color = colorData.popupBackground;
    }
}
