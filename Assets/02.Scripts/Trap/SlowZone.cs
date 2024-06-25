using UnityEngine;

public class SlowZone : MonoBehaviour
{
    public string subtitleMessage = "세상에 쉬운 길은 없습니다...";
    public float subtitleDuration = 3f;
    public float slowFallSpeed = 0.1f; // Adjust as necessary

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UIManager.Instance.ShowSubtitle(subtitleMessage, subtitleDuration);
            PlayerMove playerMove = other.GetComponent<PlayerMove>();
            if (playerMove != null)
            {
                playerMove.SetFallSpeed(slowFallSpeed);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMove playerMove = other.GetComponent<PlayerMove>();
            if (playerMove != null)
            {
                playerMove.ResetFallSpeed(); // Reset to default fall speed
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMove playerMove = other.GetComponent<PlayerMove>();
            if (playerMove != null)
            {
                playerMove.SetFallSpeed(slowFallSpeed); // Ensure fall speed is consistently set while inside trigger
            }
        }
    }
}