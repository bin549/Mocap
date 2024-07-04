using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public Button modelsButton;
    public GameObject modelsPanel;
    public Button videoSourcesButton;
    public GameObject videoSourcesPanel;
    public Button mapsButton;
    public GameObject mapsPanel;
    public Button settingsButton;
    public GameObject settingsPanel;

    private void Start()
    {
        modelsButton.onClick.AddListener((() =>
        {
            modelsPanel.SetActive(!modelsPanel.activeSelf);
        }));
        videoSourcesButton.onClick.AddListener((() =>
        {
            videoSourcesPanel.SetActive(!videoSourcesPanel.activeSelf);
        }));
        mapsButton.onClick.AddListener((() =>
        {
            mapsPanel.SetActive(!mapsPanel.activeSelf);
        }));
        settingsButton.onClick.AddListener((() =>
        {
            settingsPanel.SetActive(!settingsPanel.activeSelf);
        }));
    } 
}
