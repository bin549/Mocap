using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoSourceSelection : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    private void OnEnable()
    {
        this.videoPlayer.Pause();
    }

    private void OnDisable()
    {
        this.videoPlayer.Play();
    }

    private void Start()
    {
        
    }

 private   void Update()
    {
        
    }
}
