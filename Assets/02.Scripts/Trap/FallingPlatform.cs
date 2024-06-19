using UnityEngine;
using System.Collections;

public class FallingPlatform : MonoBehaviour
{
    public float fallDelay = 1.0f; // 발판이 떨어지기 전 대기 시간
    public float destroyDelay = 2.0f; // 발판이 사라지기 전 대기 시간
    public float respawnTime = 5.0f; // 발판이 리스폰되는 시간

    private Rigidbody rb;
    private Vector3 initialPosition;
    private bool isFalling = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true; // 시작할 때 발판이 고정되어 있도록 설정
        initialPosition = transform.position;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isFalling)
        {
            StartCoroutine(FallAfterDelay());
        }
    }

    IEnumerator FallAfterDelay()
    {
        yield return new WaitForSeconds(fallDelay);

        isFalling = true;
        rb.isKinematic = false; // 발판이 중력에 영향을 받도록 설정

        yield return new WaitForSeconds(destroyDelay);

        RespawnPlatform();
    }

    void RespawnPlatform()
    {
        rb.isKinematic = true; // 발판을 다시 고정시킴
        isFalling = false;
        transform.position = initialPosition; // 초기 위치로 되돌림
        StartCoroutine(ReactivateAfterDelay());
    }

    IEnumerator ReactivateAfterDelay()
    {
        yield return new WaitForSeconds(respawnTime);
        gameObject.SetActive(true); // 발판을 다시 활성화
    }
}
