using UnityEngine;
using UnityEngine.UI;

public class UISoundManager : MonoBehaviour
{
    public static UISoundManager instance; // 싱글톤 인스턴스

    public AudioClip panelOpenSound; // 패널 열릴 때 소리
    public AudioClip buttonClickSound; // 버튼 클릭 소리

    private AudioSource audioSource; // 오디오 소스

    void Awake()
    {
        // 싱글톤 패턴 구현
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시 오브젝트 파괴 방지
        }
        else
        {
            Destroy(gameObject); // 이미 존재하는 경우 파괴
        }

        // 오디오 소스 초기화
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void PlayPanelOpenSound()
    {
        if (panelOpenSound != null)
        {
            audioSource.PlayOneShot(panelOpenSound);
        }
    }

    public void PlayButtonClickSound()
    {
        if (buttonClickSound != null)
        {
            audioSource.PlayOneShot(buttonClickSound);
        }
    }
}