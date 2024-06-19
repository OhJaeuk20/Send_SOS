using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CutSceneManager : MonoBehaviour
{
    public Image[] images; // 이미지들을 배열로 설정
    public GameObject finalPanel; // 마지막에 활성화할 패널
    public float displayDuration = 5f; // 각 이미지의 표시 시간
    private Coroutine showImagesCoroutine;

    void Start()
    {
        // 모든 이미지를 비활성화
        foreach (Image img in images)
        {
            img.gameObject.SetActive(false);
        }

        if (finalPanel != null)
        {
            finalPanel.SetActive(false); // 마지막 패널도 비활성화
        }

        // 코루틴 시작
        showImagesCoroutine = StartCoroutine(ShowImagesInSequence());
    }

    void Update()
    {
        // ESC 키 입력 처리
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SkipCutscene();
        }
    }

    IEnumerator ShowImagesInSequence()
    {
        // 이미지를 순서대로 활성화
        foreach (Image img in images)
        {
            img.gameObject.SetActive(true); // 이미지 활성화
            yield return new WaitForSeconds(displayDuration); // 일정 시간 대기
            img.gameObject.SetActive(false); // 다음 이미지를 위해 현재 이미지 비활성화
        }

        // 모든 이미지를 다 보여준 후 마지막 패널 활성화
        if (finalPanel != null)
        {
            finalPanel.SetActive(true);
        }
    }

    void SkipCutscene()
    {
        // 코루틴 중지
        if (showImagesCoroutine != null)
        {
            StopCoroutine(showImagesCoroutine);
        }

        // 모든 이미지를 비활성화
        foreach (Image img in images)
        {
            img.gameObject.SetActive(false);
        }

        // finalPanel이 설정되어 있는 경우 활성화
        if (finalPanel != null)
        {
            finalPanel.SetActive(true);
        }
    }
}
