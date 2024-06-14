using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject pausePanel;
    public Slider volumeSlider;
    public GameObject player;
    public Text timerText;

    private bool isGamePaused = false;
    private float startDelay = 2.0f; // 시작 시 마우스 입력을 무시할 시간
    private bool isMouseInputEnabled = true;
    private float startTime;
    private bool isTiming = false;
    private float elapsedTime;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // 볼륨 슬라이더 초기 설정
        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }
        else
        {
            Debug.LogWarning("Volume slider is not assigned in the inspector.");
        }

        // 게임 시작 시 마우스 입력을 비활성화
        StartCoroutine(DisableMouseInputTemporarily(startDelay));

        // 씬 로드 시 이벤트 핸들러 추가
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Pause Panel 확인 및 초기 상태 설정
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Pause panel is not assigned in the inspector.");
        }

        // 타이머 텍스트 UI 초기화
        if (timerText == null)
        {
            Debug.LogWarning("Timer text UI is not assigned in the inspector.");
        }
        else
        {
            StartTimer(); // 타이머 시작
        }
    }

    void Update()
    {
        if (isTiming)
        {
            elapsedTime = Time.time - startTime;
            UpdateTimerUI();
        }

        if (isMouseInputEnabled && Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isGamePaused = !isGamePaused;
        if (isGamePaused)
        {
            PauseGame();
            if (pausePanel != null)
            {
                pausePanel.SetActive(true);
            }
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            ResumeGame();
            if (pausePanel != null)
            {
                pausePanel.SetActive(false);
            }
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void PauseGame()
    {
        Time.timeScale = 0f;
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f;
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
    }

    private IEnumerator DisableMouseInputTemporarily(float delay)
    {
        isMouseInputEnabled = false;

        // 마우스 커서를 숨기고 고정
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        yield return new WaitForSeconds(delay);

        isMouseInputEnabled = true;

        // 마우스 커서를 다시 숨기고 고정
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 씬이 로드될 때 게임을 자동으로 재개
        if (scene.name == "GameScene") // 게임 씬의 이름을 사용하세요.
        {
            ResumeGame();
        }
    }

    public void StartTimer()
    {
        startTime = Time.time;
        isTiming = true;
    }

    public void StopTimer()
    {
        isTiming = false;
        UpdateTimerUI(); // 최종 시간 업데이트
    }

    private void UpdateTimerUI()
    {
        int hours = Mathf.FloorToInt(elapsedTime / 3600f);
        int minutes = Mathf.FloorToInt((elapsedTime % 3600) / 60f);
        float seconds = elapsedTime % 60f;
        if (timerText != null)
        {
            timerText.text = string.Format("{0:00}:{1:00}:{2:00.00}", hours, minutes, seconds);
        }
    }
}
