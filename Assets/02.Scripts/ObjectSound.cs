using UnityEngine;

public class ObjectSound : MonoBehaviour
{
    public GameObject player; // 플레이어 오브젝트
    public AudioClip soundClip; // 재생할 소리 클립
    public float soundRange = 10f; // 소리를 재생할 거리

    private AudioSource audioSource;
    private bool isPlayerNearby = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            if (distanceToPlayer <= soundRange && !isPlayerNearby)
            {
                PlaySound();
                isPlayerNearby = true;
            }
            else if (distanceToPlayer > soundRange && isPlayerNearby)
            {
                isPlayerNearby = false;
            }
        }
    }

    void PlaySound()
    {
        if (audioSource != null && soundClip != null)
        {
            audioSource.PlayOneShot(soundClip);
        }
    }
}