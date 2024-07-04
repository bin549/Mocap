using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public Button modelSelectionButton;
    public GameObject modelSelectionPanel;
    public Button videoSourceSelectionButton;
    public GameObject videoSourceSelectionPanel;
    public Button mapSelectionButton;
    public GameObject mapSelectionPanel;
    public Button settingsButton;
    public GameObject settingsPanel;
    [SerializeField] private AvatarCameraController _avatarCameraController;


    private void Start()
    {
        modelSelectionButton.onClick.AddListener((() =>
        {
            this.SetPanelActive(modelSelectionPanel);
        }));
        videoSourceSelectionButton.onClick.AddListener((() =>
        {
            this.SetPanelActive(videoSourceSelectionPanel);
        }));
        mapSelectionButton.onClick.AddListener((() =>
        {
            this.SetPanelActive(mapSelectionPanel);
        }));
        settingsButton.onClick.AddListener((() =>
        {
            this.SetPanelActive(settingsPanel);
        }));
    }

    private void SetPanelActive(GameObject panel)
    {
        panel.SetActive(!panel.activeSelf);       
        this._avatarCameraController.isInputDisable = panel.activeSelf; 
    }
}
