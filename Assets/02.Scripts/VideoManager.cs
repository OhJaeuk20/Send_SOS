using UnityEngine;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    private bool isVideoFinished = false;

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();

        // VideoPlayer의 loopPointReached 이벤트에 이벤트 핸들러 등록
        videoPlayer.loopPointReached += OnVideoFinished;

        // 영상 재생 시작
        videoPlayer.Play();
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        // 영상 재생이 끝났을 때 호출되는 이벤트 핸들러
        isVideoFinished = true;
        Debug.Log("Video finished playing!");
    }

    void Update()
    {
        if (isVideoFinished)
        {
            // 영상 재생이 끝났을 때 수행할 동작
            Debug.Log("Video has finished. Perform post-video actions here.");
        }
    }

    public bool IsVideoFinished()
    {
        return isVideoFinished;
    }
}