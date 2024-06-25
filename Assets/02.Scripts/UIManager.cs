using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public Text subtitleText;

    void Awake()
    {
        Instance = this;
    }

    public void ShowSubtitle(string message, float duration)
    {
        StartCoroutine(ShowSubtitleCoroutine(message, duration));
    }

    private IEnumerator ShowSubtitleCoroutine(string message, float duration)
    {
        subtitleText.text = message;
        subtitleText.enabled = true;
        yield return new WaitForSeconds(duration);
        subtitleText.enabled = false;
    }
}