using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoClipItem : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string videoClipPath;
    public VideoClip videoClip;
    private RenderTexture videoTexture;
    public float sourceFps = 30f;
    public Image textureImage;

    private void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        textureImage = GetComponent<Image>();
    }

    public void SetVideoClipSource(string clipPath)
    {
        this.videoClipPath = clipPath;
        videoPlayer.url = clipPath;
        // VideoClip vclip = (VideoClip)Resources.Load(videoClipPath);
        if (videoPlayer.clip != null)
        {
            if (videoPlayer.clip.width == 0 || videoPlayer.clip.height == 0)
            {
                videoTexture = new RenderTexture(1920, 1080, 24);
            }
            else
            {
                videoTexture = new RenderTexture((int)videoPlayer.clip.width, (int)videoPlayer.clip.height, 24);
            }
        }
        else
        {
            videoTexture = new RenderTexture(1920, 1080, 24);
        }   
        videoPlayer.Play();
        videoPlayer.renderMode = VideoRenderMode.RenderTexture;
        videoPlayer.targetTexture = videoTexture;
        videoPlayer.Prepare();
        sourceFps = (float)videoPlayer.frameRate;
        videoPlayer.Pause();
    }

    public void OnVideoPlayerSourceUpdate(VideoPlayer videoPlayer)
    {
        videoPlayer.url = videoClipPath;
    }
}
