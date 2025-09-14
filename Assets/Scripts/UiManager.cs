using UnityEngine;
using UnityEngine.UI;


public class UiManager : MonoBehaviour {
    public GameObject[] uiPanels;
    public Button avatarSelectionButton;
    public AvatarSelection avatarSelectionPanel;
    public Button videoSourceSelectionButton;
    public VideoSourceSelection videoSourceSelection;
    public Button mapSelectionButton;
    public GameObject mapSelectionPanel;
    public Button settingsButton;
    public GameObject settingsPanel;
    public DeviceSelection deviceSelection;
    [SerializeField] private AvatarCameraController _avatarCameraController;
    public RawImage screen;

    private void Start() {
        avatarSelectionButton.onClick.AddListener((() => { this.SetPanelActive(avatarSelectionPanel.gameObject); }));
        videoSourceSelectionButton.onClick.AddListener((() => {
            this.SetPanelActive(videoSourceSelection.gameObject);
        }));
        mapSelectionButton.onClick.AddListener((() => { this.SetPanelActive(mapSelectionPanel); }));
        settingsButton.onClick.AddListener((() => { this.SetPanelActive(settingsPanel); }));
        videoSourceSelection.videoPlayer.Pause();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            QuitApp();
        }
    }

    public void QuitApp() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void ToggleUI() {
        if (deviceSelection.defaultImageSource == ImageSourceType.WebCamera) {
            deviceSelection.gameObject.SetActive(!deviceSelection.gameObject.activeSelf);
        }
        foreach (var uiPanel in uiPanels) {
            uiPanel.SetActive(!deviceSelection.gameObject.activeSelf);
        }
        if (deviceSelection.gameObject.activeSelf) {
            videoSourceSelection.videoPlayer.Pause();
        } else {
            videoSourceSelection.videoPlayer.Play();
        }
    }

    private void SetPanelActive(GameObject panel) {
        panel.SetActive(!panel.activeSelf);
        this._avatarCameraController.isInputDisable = panel.activeSelf;
    }
}
