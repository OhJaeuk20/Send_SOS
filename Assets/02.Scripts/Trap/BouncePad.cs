using UnityEngine;

public class BouncePad : MonoBehaviour
{
    private AudioSource m_AudioSource;

    void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();    
    }

    public float bounceHeight = 10f; // 원하는 튕김 높이

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // 충돌한 객체가 플레이어인 경우
        {
            Rigidbody rb = other.GetComponent<Rigidbody>(); // 플레이어의 Rigidbody 컴포넌트 가져오기
            if (rb != null) // Rigidbody가 존재하는 경우
            {
                rb.velocity = Vector3.zero; // 기존 속도 초기화
                float bounceVelocity = Mathf.Sqrt(2 * bounceHeight * Physics.gravity.magnitude); // 일정한 높이로 설정할 속도 계산
                rb.velocity = new Vector3(rb.velocity.x, bounceVelocity, rb.velocity.z); // 속도 설정
                m_AudioSource.Play();
            }
        }
    }
}