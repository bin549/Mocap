using Mediapipe.Unity.Sample.PoseTracking;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class AvatarSelection : MonoBehaviour {
    public GameObject currentAvatar;
    public PoseTrackingSolution solution;
    public GameObject avatarItem;
    public VideoPlayer videoPlayer;
    [SerializeField] private AvatarCameraController avatarCameraController;
    [SerializeField] private UiManager _uiManager;
    [SerializeField] private AvatarUnit[] _avatarUnits;
    [SerializeField] private Transform avatarPivot;
    [SerializeField] private AppSettings _appSettings;

    private void Start() {
        foreach (var avatarUnit in _avatarUnits) {
            AvatarItem avatar = GameObject.Instantiate(avatarItem).GetComponent<AvatarItem>();
            avatar.gameObject.transform.SetParent(transform);
            Button avatarButton = avatar.gameObject.GetComponent<Button>();
            avatarButton.image.sprite = avatarUnit.sprite;
            avatarButton.onClick.AddListener(() => {
                GameObject.Destroy(currentAvatar);
                currentAvatar = GameObject.Instantiate(avatarUnit.avatar);
                currentAvatar.transform.SetParent(avatarPivot);
                _appSettings.motionDataRecorder.SetAnimator(currentAvatar.GetComponent<Animator>());
                currentAvatar.SetActive(true);
                solution.SetAvatar(currentAvatar.GetComponent<Mediapipe2UnitySkeletonController>());
                this.avatarCameraController.SetAvatar(currentAvatar.transform);
                this.gameObject.SetActive(false);
                this.avatarCameraController.isInputDisable = false;
            });
        }
    }

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
}
