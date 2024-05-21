using System.Collections;
using UnityEngine;

public class FloorTrap : MonoBehaviour
{
    public Transform trapDoor; // 바닥 함정의 Transform
    public float openDelay = 1.0f; // 바닥 함정이 열리기 전 지연 시간
    public float openSpeed = 2.0f; // 바닥 함정이 열리는 속도
    public float launchForce = 20.0f; // 플레이어를 날려보낼 힘

    private bool isOpening = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isOpening)
        {
            StartCoroutine(OpenTrapDoor(other.gameObject));
        }
    }

    IEnumerator OpenTrapDoor(GameObject player)
    {
        isOpening = true;
        yield return new WaitForSeconds(openDelay);

        float elapsedTime = 0f;
        Vector3 initialRotation = trapDoor.rotation.eulerAngles;
        Vector3 targetRotation = initialRotation + new Vector3(90f, 0f, 0f);

        while (elapsedTime < openSpeed)
        {
            trapDoor.rotation = Quaternion.Euler(Vector3.Lerp(initialRotation, targetRotation, elapsedTime / openSpeed));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        trapDoor.rotation = Quaternion.Euler(targetRotation);

        // 플레이어를 날려보내는 로직
        Rigidbody playerRb = player.GetComponent<Rigidbody>();
        if (playerRb != null)
        {
            playerRb.AddForce(Vector3.up * launchForce, ForceMode.Impulse);
        }

        isOpening = false;
    }
}
