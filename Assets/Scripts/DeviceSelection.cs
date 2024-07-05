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
    public ImageSourceType defaultImageSource;
    public GameObject selectedModel;
    
    private void Start()
    {
        webcamButton.onClick.AddListener(() =>
        {
            this.OnDeviceSelected(ImageSourceType.WebCamera);
        });
        videoButton.onClick.AddListener(() =>
        {
            this.OnDeviceSelected(ImageSourceType.Video);
        });
    }

    private void OnDeviceSelected(ImageSourceType deviceType)
    {
        selectedModel.SetActive(true);
        defaultImageSource = deviceType;
        uiManager.ToggleUI();
        avatarCameraController.StartControl();
        solution.gameObject.SetActive(true);
    }
}
