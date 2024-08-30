using Mediapipe.Unity.Sample;
using Mediapipe.Unity.Sample.PoseTracking;
using UnityEngine;
using UnityEngine.UI;

public class DeviceSelection : MonoBehaviour {
    [SerializeField] private Bootstrap _bootstrap;
    [SerializeField] private PoseTrackingSolution solution;
    [SerializeField] private Button webcamButton;
    [SerializeField] private Button videoButton;
    [SerializeField] private UiManager uiManager;
    public AvatarCameraController avatarCameraController;
    public ImageSourceType defaultImageSource;
    public GameObject selectedAvatar;

    private void Start() {
        webcamButton.onClick.AddListener(() => { this.OnDeviceWebcamSelected(ImageSourceType.WebCamera); });
        videoButton.onClick.AddListener(() => { this.OnDeviceVideoSelected(ImageSourceType.Video); });
    }

    private void OnDeviceWebcamSelected(ImageSourceType deviceType) {
        selectedAvatar.SetActive(true);
        defaultImageSource = deviceType;
        uiManager.ToggleUI();
        avatarCameraController.StartControl();
        solution.gameObject.SetActive(true);
        uiManager.videoSourceSelectionButton.gameObject.SetActive(false);
    }

    private void OnDeviceVideoSelected(ImageSourceType deviceType) {
        uiManager.videoSourceSelection.gameObject.SetActive(true);
        defaultImageSource = ImageSourceType.Video;
        gameObject.SetActive(false);
        this.OnAppInit();
    }
    
    public void OnAppInit() {
        solution.gameObject.SetActive(true);
    }

    public void OnAppBoot() {
        selectedAvatar.SetActive(true);
        avatarCameraController.StartControl();
    }
}
