using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoSourceSelection : MonoBehaviour {
    public VideoPlayer videoPlayer;
    public GameObject videoClipItem;
    [SerializeField] private AvatarCameraController avatarCameraController;
    [SerializeField] private UiManager _uiManager;
    [SerializeField] private AppSettings _appSettings;

    private void OnEnable() {
        this.videoPlayer.Pause();
        _uiManager.screen.gameObject.SetActive(false);
    }

    private void OnDisable() {
        if (!this.videoPlayer) {
            return;
        }
        this.videoPlayer.Play();
        _uiManager.screen.gameObject.SetActive(true);
    }

    private void Start() {
        foreach (var videoPath in this.GetVideos()) {
            VideoClipItem videoClip = GameObject.Instantiate(videoClipItem).GetComponent<VideoClipItem>();
            videoClip.gameObject.transform.SetParent(transform);
            videoClip.SetVideoClipSource(videoPath);
            videoClip.gameObject.GetComponent<Button>().onClick.AddListener(() => {
                if (!_appSettings.isAppBoot) {
                    _appSettings.isAppBoot = true;
                    _uiManager.ToggleUI();
                    _uiManager.deviceSelection.OnAppBoot();
                }
                videoClip.OnVideoPlayerSourceUpdate(this.videoPlayer);
                this.gameObject.SetActive(false);
                this.avatarCameraController.isInputDisable = false;
            });
        }
    }

    public string[] GetVideos() {
        return _appSettings.GetVideosFromStreamingAssets();
    }
}
