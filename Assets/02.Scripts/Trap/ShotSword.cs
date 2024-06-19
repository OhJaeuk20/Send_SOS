using UnityEngine;

public class ShotSword : MonoBehaviour
{
    public float speed = 30f; // 발사체의 속도
    public float pushForce = 5f; // 플레이어에게 가할 힘의 크기
    public float playerHitDestroyDelay = 0.5f; // 플레이어에 맞았을 때 파괴 딜레이
    public float groundHitDestroyDelay = 2f; // 바닥에 맞았을 때 파괴 딜레이

    private Rigidbody rb;
    private bool hasHitPlayer = false; // 플레이어와 충돌했는지 여부를 추적

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed; // 발사체를 앞으로 발사
    }

    void OnCollisionEnter(Collision collision)
    {
        // 충돌한 오브젝트가 플레이어인지 확인
        if (collision.gameObject.CompareTag("Player") && !hasHitPlayer)
        {
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();

            if (playerRb != null)
            {
                // 발사체의 진행 방향으로 플레이어를 밀어냄
                Vector3 pushDirection = (collision.transform.position - transform.position).normalized;
                playerRb.AddForce(pushDirection * pushForce, ForceMode.Impulse);

                // 플레이어와 충돌한 경우 즉시 파괴되지 않도록 설정
                hasHitPlayer = true;
                Destroy(gameObject, playerHitDestroyDelay);
            }
        }
        else if (!collision.gameObject.CompareTag("Player"))
        {
            // 바닥이나 다른 물체와 충돌한 경우 일정 시간 후 파괴
            Destroy(gameObject, groundHitDestroyDelay);
        }
    }
}
