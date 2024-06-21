using UnityEngine;
using System.Collections;

public class ExplosionEffect : MonoBehaviour
{
    public float explosionForce = 20f; // 밀어내는 힘의 크기
    public float explosionRadius = 10f; // 밀어내는 범위
    public float delay = 3f; // 폭발까지의 지연 시간
    public GameObject explosionRangePrefab; // 폭발 범위를 나타내는 프리팹
    public GameObject explosionEffectPrefab; // 폭발 이펙트를 나타내는 프리팹
    public GameObject explosionSoundPrefab; // 폭발 사운드를 나타내는 프리팹
    public GameObject shellPrefab; // 쉘 프리팹
    public float shellStartHeight = 20f; // 쉘이 시작되는 높이

    private bool hasExploded = false;
    private GameObject explosionRangeInstance;
    private GameObject explosionEffectInstance;
    private GameObject explosionSoundInstance;
    private GameObject shellInstance;

    void Start()
    {
        // 폭발을 지연시키기 위한 코루틴 시작
        StartCoroutine(ExplodeAfterDelay());

        // 폭발 범위 프리팹을 인스턴스화
        if (explosionRangePrefab != null)
        {
            explosionRangeInstance = Instantiate(explosionRangePrefab, transform.position, Quaternion.identity);
            UpdateExplosionRange();
        }

        // 쉘 프리팹을 인스턴스화하여 폭발 지점으로 이동시키기
        if (shellPrefab != null)
        {
            Vector3 shellStartPos = new Vector3(transform.position.x, transform.position.y + shellStartHeight, transform.position.z);
            Quaternion shellRotation = Quaternion.Euler(90f, 0f, 0f); // 쉘의 로테이션을 X축으로 90도 회전
            shellInstance = Instantiate(shellPrefab, shellStartPos, shellRotation);
            StartCoroutine(MoveShellToTarget(shellInstance, transform.position, delay));
        }
    }

    IEnumerator ExplodeAfterDelay()
    {
        yield return new WaitForSeconds(delay);

        // 폭발 발생
        Explode();
    }

    void Explode()
    {
        if (hasExploded) return;
        hasExploded = true;

        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, explosionRadius);
        bool playerFound = false;

        foreach (Collider hit in colliders)
        {
            // 플레이어를 찾음
            if (hit.CompareTag("Player"))
            {
                playerFound = true;
                Rigidbody rb = hit.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddExplosionForce(explosionForce, explosionPos, explosionRadius, 1.0f, ForceMode.Impulse);
                }
            }
        }

        if (playerFound)
        {
            Debug.Log("Player found and affected by explosion");
        }
        else
        {
            Debug.Log("No player found within explosion radius");
        }

        // 폭발 이펙트 인스턴스화 및 재생
        if (explosionEffectPrefab != null)
        {
            explosionEffectInstance = Instantiate(explosionEffectPrefab, explosionPos, Quaternion.identity);
        }

        // 폭발 사운드 인스턴스화 및 재생
        if (explosionSoundPrefab != null)
        {
            explosionSoundInstance = Instantiate(explosionSoundPrefab, explosionPos, Quaternion.identity);
            AudioSource audioSource = explosionSoundInstance.GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.Play(); // 사운드 재생
                Destroy(explosionSoundInstance, audioSource.clip.length); // 재생 후 사운드 파괴
            }
            else
            {
                Debug.LogWarning("No AudioSource component found in explosion sound prefab.");
            }
        }

        // 폭발 범위 삭제
        DestroyExplosionRange();

        // 폭발 프리팹 파괴
        Destroy(shellInstance);
        Destroy(gameObject);
    }

    IEnumerator MoveShellToTarget(GameObject shell, Vector3 targetPosition, float travelTime)
    {
        Vector3 startPosition = shell.transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < travelTime)
        {
            shell.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / travelTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        shell.transform.position = targetPosition;

        // 쉘이 도착한 후 쉘 파괴
        Destroy(shell);
    }

    void UpdateExplosionRange()
    {
        if (explosionRangeInstance != null)
        {
            float scaleMultiplier = explosionRadius; // 예시로 5f는 폭발의 최대 범위를 나타냅니다.
            explosionRangeInstance.transform.localScale = Vector3.one * scaleMultiplier;
        }
    }

    void DestroyExplosionRange()
    {
        if (explosionRangeInstance != null)
        {
            Destroy(explosionRangeInstance);
        }
    }

    // 에디터에서 폭발 범위를 시각화하기 위한 기즈모
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
