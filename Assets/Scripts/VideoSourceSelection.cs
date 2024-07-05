using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoSourceSelection : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public VideoClip[] videoClips;
    public VideoClip videoClip;
    public Button videoClipItem;
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
        this.videoClipItem.onClick.AddListener(() =>
        {
            this.UpdateVideoClip();
            this.gameObject.SetActive(false);
            this.avatarCameraController.isInputDisable = false;
        });
    }

    public void UpdateVideoClip()
    {
        this.videoPlayer.clip = videoClip;
    }
}
