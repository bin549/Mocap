using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public Button avatarSelectionButton;
    public GameObject avatarSelectionPanel;
    public Button videoSourceSelectionButton;
    public GameObject videoSourceSelectionPanel;
    public Button mapSelectionButton;
    public GameObject mapSelectionPanel;
    public Button settingsButton;
    public GameObject settingsPanel;
    public GameObject deviceSelectionPanel;
    [SerializeField] private AvatarCameraController _avatarCameraController;


    private void Start()
    {
        avatarSelectionButton.onClick.AddListener((() =>
        {
            this.SetPanelActive(avatarSelectionPanel);
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            deviceSelectionPanel.SetActive(!deviceSelectionPanel.activeSelf);
        }
    }

    private void SetPanelActive(GameObject panel)
    {
        panel.SetActive(!panel.activeSelf);       
        this._avatarCameraController.isInputDisable = panel.activeSelf; 
    }
}
