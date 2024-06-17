using UnityEngine;

public class Cannonball : MonoBehaviour
{
    public float explosionForce = 1000f; // 밀어내는 힘의 크기
    public float explosionRadius = 5f; // 밀어내는 범위

    void OnCollisionEnter(Collision collision)
    {
        // 대포알이 땅에 닿았을 때 충돌 이벤트
        Explode();
    }

    void Explode()
    {
        // 충돌 지점 주변의 플레이어를 감지
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            float distance = Vector3.Distance(player.transform.position, transform.position);
            if (distance <= explosionRadius)
            {
                Rigidbody rb = player.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    // 플레이어를 밀어내는 힘 적용
                    Vector3 explosionDirection = (player.transform.position - transform.position).normalized;
                    rb.AddForce(explosionDirection * explosionForce, ForceMode.Impulse);
                }
            }
        }

        // 대포알 제거
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        // 에디터에서 폭발 범위를 시각화하기 위한 기즈모
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
