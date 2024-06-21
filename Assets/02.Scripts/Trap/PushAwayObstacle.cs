using UnityEngine;

public class PushAwayObstacle : MonoBehaviour
{
    public float pushForce = 50.0f; // 밀어내는 힘의 크기

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody playerRb = other.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                Vector3 pushDirection = (other.transform.position - transform.position).normalized;
                playerRb.AddForce(pushDirection * pushForce, ForceMode.Impulse);
            }
        }
    }
}