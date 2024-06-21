using UnityEngine;

public class BackgroundMusicManager : MonoBehaviour
{
    public static BackgroundMusicManager Instance { get; private set; }
    public AudioClip backgroundMusicTitle;
    public AudioClip backgroundMusicOpening;
    public AudioClip backgroundMusicBelow210;
    public AudioClip backgroundMusicAbove210;
    public AudioClip backgroundMusicAbove650;
    public AudioClip backgroundMusicAbove1480;
    public AudioClip backgroundMusicAbove1850;
    public AudioClip backgroundMusicHappyEnd;
    public AudioClip backgroundMusicBadEnd;

    private AudioSource backgroundMusicSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            backgroundMusicSource = GetComponent<AudioSource>();
            // 초기 배경음악 설정
            backgroundMusicSource.clip = backgroundMusicTitle;
            backgroundMusicSource.Play();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetBackgroundMusic(AudioClip clip)
    {
        if (backgroundMusicSource != null && backgroundMusicSource.clip != clip)
        {
            backgroundMusicSource.clip = clip;
            backgroundMusicSource.Play();
        }
    }
}