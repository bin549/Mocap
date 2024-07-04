using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoSourceSelection : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public VideoClip videoClip;
    public Button videoClipItem;

    private void OnEnable()
    {
        this.videoPlayer.Pause();
    }

    private void OnDisable()
    {
        if (!this.videoPlayer)
        {
            return;
        }
        this.videoPlayer.Play();
    }

    private void Start()
    {
        this.videoClipItem.onClick.AddListener(() =>
        {
            this.UpdateVideoClip();
            this.gameObject.SetActive(false);
        });
    }

    public void UpdateVideoClip()
    {
        videoPlayer.clip = videoClip;
    }

 private   void Update()
    {
        
    }
}
