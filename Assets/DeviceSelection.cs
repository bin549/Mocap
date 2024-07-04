using System.Collections;
using System.Collections.Generic;
using Mediapipe.Unity.Sample;
using Mediapipe.Unity.Sample.PoseTracking;
using UnityEngine;
using UnityEngine.UI;

public class DeviceSelection : MonoBehaviour
{
    [SerializeField] private Bootstrap _bootstrap;
    [SerializeField] private PoseTrackingSolution solution;
    [SerializeField] private Button webcamButton;
    [SerializeField] private Button videoButton;
    [SerializeField] private UiManager uiManager;
    public AvatarCameraController avatarCameraController;
    
    private void Start()
    {
        webcamButton.onClick.AddListener(() =>
        {
            _bootstrap.SwitchDevice(ImageSourceType.WebCamera);
            // solution.SetupScreen(solution.);
        });
        videoButton.onClick.AddListener(() =>
        {
            // _bootstrap.SwitchDevice(ImageSourceType.Video);
            // solution.SetupScreen(_bootstrap.ImageSource);
            uiManager.ToggleUI();
            avatarCameraController.StartControl();
        });
    }
}
