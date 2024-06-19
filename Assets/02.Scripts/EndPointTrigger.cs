using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPointTrigger : MonoBehaviour
{
    private GameManager gameManager;
    private PlayerInventory playerInventory;

    void Start()
    {
        gameManager = GameManager.Instance;
        playerInventory = FindObjectOfType<PlayerInventory>();

        if (playerInventory == null)
        {
            Debug.LogWarning("PlayerInventory를 찾을 수 없습니다.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.StopTimer();

            if (playerInventory != null)
            {
                int itemCount = playerInventory.ItemCount;

                // 아이템 개수에 따른 엔딩 씬 분기
                if (itemCount >= 4) // 예: 4개 이상의 아이템을 수집한 경우
                {
                    LoadHappyEnding();
                }
                else
                {
                    LoadBadEnding();
                }
            }
        }
    }

    private void LoadHappyEnding()
    {
        Debug.Log("Good Ending으로 이동합니다.");
        SceneManager.LoadScene("HappyEnd"); // GoodEndingScene이라는 씬 이름을 사용
    }

    private void LoadBadEnding()
    {
        Debug.Log("Bad Ending으로 이동합니다.");
        SceneManager.LoadScene("BadEnd"); // BadEndingScene이라는 씬 이름을 사용
    }
}
