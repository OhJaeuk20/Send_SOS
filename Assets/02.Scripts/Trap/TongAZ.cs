using UnityEngine;

public class TongAZ : MonoBehaviour
{
    public GameObject projectile; // 발사할 오브젝트
    public Transform firePoint; // 오브젝트가 발사될 위치
    public float detectionRadius = 50f; // 플레이어를 감지할 반경
    public float fireInterval = 5f; // 발사 간격

    private GameObject player;
    private bool playerInRange;
    private float fireTimer;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); // 플레이어 오브젝트를 태그로 찾음
        fireTimer = fireInterval; // 타이머 초기화
    }

    void Update()
    {
        // 플레이어와 터렛 간의 거리 계산
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        // 플레이어가 감지 반경 내에 있는지 확인
        playerInRange = distanceToPlayer <= detectionRadius;

        // 플레이어가 감지 반경 내에 있을 때만 타이머 업데이트
        if (playerInRange)
        {
            fireTimer -= Time.deltaTime;
            if (fireTimer <= 0f)
            {
                FireProjectile();
                fireTimer = fireInterval; // 타이머 리셋
            }
        }
        else
        {
            fireTimer = fireInterval; // 플레이어가 반경 밖에 있으면 타이머 리셋
        }
    }

    void FireProjectile()
    {
        // 플레이어를 향해 회전
        Vector3 directionToPlayer = (player.transform.position - firePoint.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);

        // 오브젝트를 발사 위치에 생성하고, 플레이어를 향해 회전시킴
        Instantiate(projectile, firePoint.position, lookRotation);
    }

    // 감지 반경을 시각적으로 표시 (에디터에서만 보임)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
