using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
public class VideoController : MonoBehaviour
{
    VideoPlayer videoPlayer;

    private void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.loopPointReached += OnVideoEnd; // Subscribe to the event
    }

    private void OnDestroy()
    {
        videoPlayer.loopPointReached -= OnVideoEnd; // Unsubscribe from the event
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        SceneManager.LoadScene("maze"); // Load the scene named "maze"
    }
}