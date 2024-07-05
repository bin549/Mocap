using System.Collections;
using System.Collections.Generic;
using Mediapipe.Unity.Sample.PoseTracking;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class AvatarSelection : MonoBehaviour
{
    public Button avatarItem;
    public GameObject currentAvatar;
    public GameObject selectedAvatar;
    public PoseTrackingSolution solution;
    public VideoPlayer videoPlayer;
    [SerializeField] private AvatarCameraController avatarCameraController;
    [SerializeField] private UiManager _uiManager;
    
    private void OnEnable()
    {
        this.videoPlayer.Pause();
        _uiManager.screen.gameObject.SetActive(false);
    }

    
    private void OnDisable()
    {
        if (!this.videoPlayer)
        {
            return;
        }
        this.videoPlayer.Play();
        _uiManager.screen.gameObject.SetActive(true);
    }

    private void Start()
    {
        avatarItem.onClick.AddListener(() =>
        {
            currentAvatar.gameObject.SetActive(false);
            selectedAvatar.gameObject.SetActive(true);
            solution.SetAvatar(selectedAvatar.GetComponent<Mediapipe2UnitySkeletonController>());
            this.gameObject.SetActive(false);
            this.avatarCameraController.isInputDisable = false;
        });
    }
}
