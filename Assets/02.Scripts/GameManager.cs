using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public Slider volumeSlider;
    public Text timerText;
    private GameObject pausePanel;
    private GameObject optionPanel;
    private GameObject player;
    private Rigidbody playerRigidbody;

    public AudioClip buttonClickSound; // 버튼 클릭 소리
    public AudioClip panelOpenSound; // 패널 열림 소리
    private AudioSource audioSource; // 오디오 소스

    public Material skyboxAbove1900;
    public Material skyboxBelow1900;

    private bool isGamePaused = false;
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
            return;
        }
    }

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        InitializeGame(SceneManager.GetActiveScene(), LoadSceneMode.Single);

        // AudioSource 초기화
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    void InitializeGame(Scene scene, LoadSceneMode mode)
    {
        string currentSceneName = scene.name;

        if (currentSceneName == "Title")
        {
            InitializeOptionPanel();
            InitializeTitleButtons(); // 타이틀 씬의 버튼 리스너 초기화
            // 타이틀 씬에서 BGM 설정
            BackgroundMusicManager.Instance.SetBackgroundMusic(BackgroundMusicManager.Instance.backgroundMusicTitle);
        }
        else if (currentSceneName == "Play")
        {
            InitializePausePanel();
            InitializePlayer();
            InitializeTimerText();
            // Play 씬에서 BGM 설정
            BackgroundMusicManager.Instance.SetBackgroundMusic(BackgroundMusicManager.Instance.backgroundMusicBelow210);
            StartTimer(); // 타이머 시작
        }
        else if (currentSceneName == "Opening")
        {
            // 엔딩 씬에서 BGM 설정
            BackgroundMusicManager.Instance.SetBackgroundMusic(BackgroundMusicManager.Instance.backgroundMusicOpening);
        }
        else if (currentSceneName == "HappyEnd")
        {
            // 엔딩 씬에서 BGM 설정
            BackgroundMusicManager.Instance.SetBackgroundMusic(BackgroundMusicManager.Instance.backgroundMusicHappyEnd);
        }
        else if (currentSceneName == "BadEnd")
        {
            // 엔딩 씬에서 BGM 설정
            BackgroundMusicManager.Instance.SetBackgroundMusic(BackgroundMusicManager.Instance.backgroundMusicBadEnd);
        }
    }

    void InitializeOptionPanel()
    {
        if (optionPanel == null)
        {
            optionPanel = GameObject.Find("Option_Panel");
            if (optionPanel != null)
            {
                // 볼륨 슬라이더 초기 설정
                volumeSlider = optionPanel.GetComponentInChildren<Slider>();
                if (volumeSlider != null)
                {
                    volumeSlider.onValueChanged.AddListener(SetVolume);
                    // 초기 설정
                    volumeSlider.value = AudioListener.volume;
                }
                else
                {
                    Debug.LogWarning("Volume slider is not assigned or could not be found in the scene.");
                }
                optionPanel.SetActive(false);
            }
            else
            {
                Debug.LogWarning("Option panel is not assigned or could not be found in the scene.");
            }
        }
        else
        {
            // Option panel이 이미 초기화된 경우 볼륨 슬라이더만 설정
            volumeSlider = optionPanel.GetComponentInChildren<Slider>();
            if (volumeSlider != null)
            {
                volumeSlider.onValueChanged.AddListener(SetVolume);
                // 초기 설정
                volumeSlider.value = AudioListener.volume;
            }
            else
            {
                Debug.LogWarning("Volume slider is not assigned or could not be found in the scene.");
            }
        }
    }

    void InitializePausePanel()
    {
        if (pausePanel == null)
        {
            pausePanel = GameObject.Find("Pause_Panel");
            if (pausePanel != null)
            {
                // 볼륨 슬라이더 초기 설정
                volumeSlider = pausePanel.GetComponentInChildren<Slider>();
                if (volumeSlider != null)
                {
                    volumeSlider.onValueChanged.AddListener(SetVolume);
                    // 초기 설정
                    volumeSlider.value = AudioListener.volume;
                }
                else
                {
                    Debug.LogWarning("Volume slider is not assigned or could not be found in the scene.");
                }
                pausePanel.SetActive(false);
            }
            else
            {
                Debug.LogWarning("Pause panel is not assigned or could not be found in the scene.");
            }
        }
        else
        {
            // Pause panel이 이미 초기화된 경우 볼륨 슬라이더만 설정
            volumeSlider = pausePanel.GetComponentInChildren<Slider>();
            if (volumeSlider != null)
            {
                volumeSlider.onValueChanged.AddListener(SetVolume);
                // 초기 설정
                volumeSlider.value = AudioListener.volume;
            }
            else
            {
                Debug.LogWarning("Volume slider is not assigned or could not be found in the scene.");
            }
        }
    }

    void InitializeTimerText()
    {
        timerText = GameObject.Find("TimerText").GetComponent<Text>();
        if (timerText == null)
        {
            Debug.LogWarning("TimerText could not be found in the scene.");
        }
    }

    void InitializePlayer()
    {
        // 플레이어의 Rigidbody 컴포넌트 참조
        player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerRigidbody = player.GetComponent<Rigidbody>();
            if (playerRigidbody == null)
            {
                Debug.LogWarning("Player does not have a Rigidbody component.");
            }
        }
        else
        {
            Debug.LogWarning("Player is not assigned or could not be found in the scene.");
        }
    }

    void InitializeTitleButtons()
    {
        // Exit 버튼과 Option 버튼 리스너 설정
        GameObject exitButton = GameObject.Find("Button_Exit");
        if (exitButton != null)
        {
            Button btn = exitButton.GetComponent<Button>();
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() =>
            {
                OnClickExitButton();
                PlaySound(buttonClickSound); // 버튼 클릭 소리 재생
            });
        }
        else
        {
            Debug.LogWarning("ExitButton could not be found in the scene.");
        }

        GameObject optionButton = GameObject.Find("Button_Option");
        if (optionButton != null)
        {
            Button btn = optionButton.GetComponent<Button>();
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() =>
            {
                OnOptionButtonClicked();
                PlaySound(buttonClickSound); // 버튼 클릭 소리 재생
            });
        }
        else
        {
            Debug.LogWarning("OptionButton could not be found in the scene.");
        }
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Play")
        {
            // 플레이어의 y 좌표 확인 및 초기화
            if (player != null)
            {
                float playerY = player.transform.position.y;

                // 스카이박스 변경 로직
                if (playerY >= 1850f)
                {
                    RenderSettings.skybox = skyboxAbove1900;
                }
                else
                {
                    RenderSettings.skybox = skyboxBelow1900;
                }

                // BGM 변경 로직
                if (playerY >= 1850f)
                {
                    BackgroundMusicManager.Instance.SetBackgroundMusic(BackgroundMusicManager.Instance.backgroundMusicAbove1850);
                }
                else if (playerY >= 1480f)
                {
                    BackgroundMusicManager.Instance.SetBackgroundMusic(BackgroundMusicManager.Instance.backgroundMusicAbove1480);
                }
                else if (playerY >= 650f)
                {
                    BackgroundMusicManager.Instance.SetBackgroundMusic(BackgroundMusicManager.Instance.backgroundMusicAbove650);
                }
                else if (playerY >= 210f)
                {
                    BackgroundMusicManager.Instance.SetBackgroundMusic(BackgroundMusicManager.Instance.backgroundMusicAbove210);
                }
                else
                {
                    BackgroundMusicManager.Instance.SetBackgroundMusic(BackgroundMusicManager.Instance.backgroundMusicBelow210);
                }
            }

            if (isTiming)
            {
                elapsedTime = Time.time - startTime;
                UpdateTimerUI();
            }

            if (isMouseInputEnabled && Input.GetKeyDown(KeyCode.Escape))
            {
                TogglePause();
            }

            // 플레이어의 y 좌표 확인 및 초기화
            if (player != null && player.transform.position.y < -10f)
            {
                ResetPlayerPosition();
            }
        }
    }

    public void OnClickStartButton()
    {
        PlaySound(buttonClickSound);
        SceneManager.LoadScene("Opening");
    }

    public void OnClickExitButton()
    {
        PlaySound(buttonClickSound);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void TogglePause()
    {
        isGamePaused = !isGamePaused;
        if (isGamePaused)
        {
            PauseGame();
            if (pausePanel != null && !pausePanel.activeSelf) // pausePanel이 활성화되지 않은 경우에만 활성화
            {
                pausePanel.SetActive(true);
                PlaySound(panelOpenSound);
            }
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void OnOptionButtonClicked()
    {
        if (optionPanel != null)
        {
            optionPanel.SetActive(true);
            PlaySound(panelOpenSound); // 패널 열림 소리 재생
        }
        else
        {
            Debug.LogError("Option panel is not assigned or could not be found.");
        }
    }

    public void OnOptionCancelButtonClicked()
    {
        optionPanel = GameObject.Find("Option_Panel");
        if (optionPanel != null)
        {
            optionPanel.SetActive(false);
        }
        else
        {
            Debug.LogError("Option panel is not assigned or could not be found.");
        }
    }

    public void OnCancelButtonClicked()
    {
        pausePanel = GameObject.Find("Pause_Panel");
        ResumeGame();
        if (pausePanel != null) // pausePanel이 활성화된 경우에만 비활성화
        {
            pausePanel.SetActive(false);
            PlaySound(buttonClickSound);
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // 메인 메뉴로 나가기 버튼 클릭 시 호출될 메서드
    public void OnMainMenuButtonClicked()
    {
        PlaySound(buttonClickSound);
        ResumeGame();
        SceneManager.LoadScene("Title"); // MainMenu 씬으로 이동
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

        // 슬라이더의 핸들 위치 설정
        if (volumeSlider != null)
        {
            // 슬라이더의 min, max 값을 기반으로 핸들의 위치 계산
            float sliderValue = Mathf.InverseLerp(volumeSlider.minValue, volumeSlider.maxValue, volume);
            volumeSlider.value = sliderValue;
        }
        else
        {
            Debug.LogWarning("Volume slider is not assigned.");
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        InitializeGame(scene, mode);

        // 씬이 로드될 때 게임을 자동으로 재개
        if (scene.name == "Play") // 게임 씬의 이름을 사용하세요.
        {
            ResumeGame();
            StartTimer(); // 씬 로드 시 타이머 시작
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

    private void ResetPlayerPosition()
    {
        player.transform.position = new Vector3(0, 1.5f, 0);
        player.transform.rotation = Quaternion.identity; // 필요 시 회전 초기화

        // 플레이어의 속도 초기화
        if (playerRigidbody != null)
        {
            playerRigidbody.velocity = Vector3.zero;
            playerRigidbody.angularVelocity = Vector3.zero;
        }

        Debug.Log("Player position and velocity reset.");
    }

    // 오디오 재생 메서드
    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}