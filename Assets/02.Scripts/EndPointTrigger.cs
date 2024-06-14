using UnityEngine;

public class EndPointTrigger : MonoBehaviour
{
    private GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.StopTimer();
            //엔딩 씬 스크립트 입력
        }
    }
}