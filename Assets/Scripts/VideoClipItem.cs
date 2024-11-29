using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoClipItem : MonoBehaviour {
    public VideoPlayer videoPlayer;
    public string videoClipPath;
    private RenderTexture videoTexture;
    public RawImage textureImage;
    private UnityAction<Texture2D> OnComplete;
    private int frameValue = 1;

    private void Awake() {
        videoPlayer = GetComponent<VideoPlayer>();
        textureImage = GetComponent<RawImage>();
    }

    public void SetVideoClipSource(string clipPath) {
        this.videoClipPath = clipPath;
        this.GetOneFrameTexture((texture2D) => { textureImage.texture = texture2D; });
    }

    private void GetOneFrameTexture(UnityAction<Texture2D> onComplete) {
        if (!string.IsNullOrEmpty(this.videoClipPath)) {
            videoPlayer.url = this.videoClipPath;
        }
        OnComplete = onComplete;
        videoPlayer.waitForFirstFrame = true;
        videoPlayer.sendFrameReadyEvents = true;
        videoPlayer.frameReady += frameReady;
        videoPlayer.Play();
    }

    private void frameReady(VideoPlayer source, long frameIdx) {
        frameValue++;
        if (frameValue >= 1) {
            OnComplete?.Invoke(TextureToTexture2D(source.texture));
            videoPlayer.frameReady -= frameReady;
            videoPlayer.sendFrameReadyEvents = false;
            videoPlayer.Stop();
        }
    }

    private Texture2D TextureToTexture2D(Texture texture) {
        Texture2D texture2D = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, false);
        RenderTexture renderTexture = RenderTexture.GetTemporary(texture.width, texture.height);
        Graphics.Blit(texture, renderTexture);
        RenderTexture.active = renderTexture;
        texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture2D.Apply();
        RenderTexture.ReleaseTemporary(renderTexture);
        return texture2D;
    }

    public void OnVideoPlayerSourceUpdate(VideoPlayer videoPlayer) {
        videoPlayer.url = videoClipPath;
    }
}
