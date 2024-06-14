using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // GameManager에 접근하기 위한 참조
    private GameManager gameManager;

    void Start()
    {
        // GameManager의 인스턴스 가져오기
        gameManager = GameManager.Instance;
    }

    // 취소 버튼 클릭 시 호출될 메서드
    public void OnCancelButtonClicked()
    {
        gameManager.TogglePause(); // 일시 정지 해제
    }

    // 메인 메뉴로 나가기 버튼 클릭 시 호출될 메서드
    public void OnMainMenuButtonClicked()
    {
        SceneManager.LoadScene("Title"); // MainMenu 씬으로 이동
    }
}