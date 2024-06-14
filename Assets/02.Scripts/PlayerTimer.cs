using UnityEngine;
using UnityEngine.UI;

public class PlayerTimer : MonoBehaviour
{
    public Text timerText; // 시간을 표시할 UI 텍스트
    private bool isTiming = false;
    private float startTime;
    private float elapsedTime;

    void Start()
    {
        StartTimer();
    }

    void Update()
    {
        if (isTiming)
        {
            elapsedTime = Time.time - startTime;
            UpdateTimerUI();
        }
    }

    void StartTimer()
    {
        startTime = Time.time;
        isTiming = true;
    }

    void StopTimer()
    {
        isTiming = false;
        UpdateTimerUI(); // 최종 시간 업데이트
    }

    void UpdateTimerUI()
    {
        int hours = Mathf.FloorToInt(elapsedTime / 3600f);
        int minutes = Mathf.FloorToInt((elapsedTime % 3600) / 60f);
        float seconds = elapsedTime % 60f;
        timerText.text = string.Format("{0:00}:{1:00}:{2:00.00}", hours, minutes, seconds);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ENDPOINT"))
        {
            StopTimer();
        }
    }
}
