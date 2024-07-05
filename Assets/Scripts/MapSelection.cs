using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class MapSelection : MonoBehaviour
{
    [SerializeField] private GameObject sceneOne;
    [SerializeField] private GameObject sceneTwo;
    [SerializeField] private Button sceneOneButton;
    [SerializeField] private Button sceneTwoButton;
    [SerializeField] private Material sceneOneMaterial;
    [SerializeField] private Material sceneTwoMaterial;
    [SerializeField] private Light sceneOneLight;
    [SerializeField] private Light sceneTwoLight;

    [SerializeField] private AvatarCameraController avatarCameraController;
    public VideoPlayer videoPlayer;
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
        sceneOneButton.onClick.AddListener(() =>
        {
            sceneOne.SetActive(true);
            sceneTwo.SetActive(false);
            gameObject.SetActive(false);
            sceneOneLight.gameObject.SetActive(true);
            sceneTwoLight.gameObject.SetActive(false);
            RenderSettings.skybox = sceneOneMaterial;
            // RenderSettings.subtractiveShadowColor = Color.cyan;
            this.avatarCameraController.isInputDisable = false;
        });
        sceneTwoButton.onClick.AddListener(() =>
        {
            sceneOne.SetActive(false);
            sceneTwo.SetActive(true);
            gameObject.SetActive(false);
            sceneOneLight.gameObject.SetActive(false);
            sceneTwoLight.gameObject.SetActive(true);
            RenderSettings.skybox = sceneTwoMaterial;
            this.avatarCameraController.isInputDisable = false;
        });
    }
}
