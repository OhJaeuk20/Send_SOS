using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndingCredit : MonoBehaviour
{
    public float scrollSpeed = 200f;
    public float endPositionY = 2000f; // 크레딧이 끝날 위치

    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        // 텍스트를 위로 스크롤
        rectTransform.anchoredPosition += new Vector2(0, scrollSpeed * Time.deltaTime);

        // 크레딧이 끝 위치에 도달했을 때 다음 씬으로 전환
        if (rectTransform.anchoredPosition.y >= endPositionY || Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Title");
        }
    }
}
