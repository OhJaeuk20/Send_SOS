using UnityEngine;
using UnityEngine.SceneManagement; // 씬 관리 네임스페이스 추가
using System.Collections;

public class OpeningScene : MonoBehaviour
{
    public GameObject[] objects; // 오브젝트들을 배열로 설정
    public float displayDuration = 5f; // 각 오브젝트의 표시 시간
    public string nextSceneName; // 전환할 씬의 이름
    private Coroutine showObjectsCoroutine;

    void Start()
    {
        // 모든 오브젝트를 비활성화
        foreach (GameObject obj in objects)
        {
            obj.SetActive(false);
        }

        // 코루틴 시작
        showObjectsCoroutine = StartCoroutine(DisplayObjectsInSequence());
    }

    void Update()
    {
        // ESC 키 입력 처리
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SkipOpeningCutscene();
        }
    }

    IEnumerator DisplayObjectsInSequence()
    {
        // 오브젝트들을 순서대로 활성화
        foreach (GameObject obj in objects)
        {
            obj.SetActive(true); // 오브젝트 활성화
            yield return new WaitForSeconds(displayDuration); // 일정 시간 대기
            obj.SetActive(false); // 다음 오브젝트를 위해 현재 오브젝트 비활성화
        }

        // 모든 오브젝트를 다 보여준 후 지정된 씬으로 전환
        SceneManager.LoadScene(nextSceneName);
    }

    void SkipOpeningCutscene()
    {
        // 코루틴 중지
        if (showObjectsCoroutine != null)
        {
            StopCoroutine(showObjectsCoroutine);
        }

        // 모든 오브젝트를 비활성화
        foreach (GameObject obj in objects)
        {
            obj.SetActive(false);
        }

        // 지정된 씬으로 전환
        SceneManager.LoadScene(nextSceneName);
    }
}