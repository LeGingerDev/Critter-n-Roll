using System.Collections;
using UnityEngine;

public class DropdownSortFixer : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(WaitForSecond());
    }

    IEnumerator WaitForSecond()
    {
        yield return new WaitForSeconds(0.1f);
        Canvas canvas = GetComponent<Canvas>();
        canvas.overrideSorting = false;
    }
}
