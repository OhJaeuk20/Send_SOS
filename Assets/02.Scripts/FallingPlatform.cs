using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    public float fallDelay = 1.0f; // 발판이 떨어지기 전 대기 시간
    public float destroyDelay = 2.0f; // 발판이 사라지기 전 대기 시간

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true; // 시작할 때 발판이 고정되어 있도록 설정
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Invoke("StartFalling", fallDelay);
        }
    }

    void StartFalling()
    {
        rb.isKinematic = false; // 발판이 중력에 영향을 받도록 설정
        Invoke("DestroyPlatform", destroyDelay);
    }

    void DestroyPlatform()
    {
        Destroy(gameObject); // 발판 제거
    }
}
