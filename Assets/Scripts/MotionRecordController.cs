using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordController : MonoBehaviour
{
    [SerializeField] private MotionDataRecorder motionDataRecorder;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            this.RecordMotion();
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            this.RecordMotion();
        }
    }

    private void RecordMotion()
    {
        this.UnityAnimRecording();
    }

    private void UnityAnimRecording()
    {
        if (!motionDataRecorder.isRecording)
        {
            motionDataRecorder.RecordStart();
        }
        else
        {
            try
            {
                motionDataRecorder.RecordEnd();
            }
            catch (System.Exception e)
            {
                Debug.LogError("Fail！" + e.Message + e.StackTrace);
            }
        }
    }
}
