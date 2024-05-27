using UnityEngine;

public class BouncePad : MonoBehaviour
{
    public float bounceForce = 20f; // 튕김 힘

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // 충돌한 객체가 플레이어인 경우
        {
            Rigidbody rb = other.GetComponent<Rigidbody>(); // 플레이어의 Rigidbody 컴포넌트 가져오기
            if (rb != null) // Rigidbody가 존재하는 경우
            {
                rb.velocity = Vector3.zero;
                rb.AddForce(transform.up * bounceForce, ForceMode.Impulse);
            }
        }
    }
}