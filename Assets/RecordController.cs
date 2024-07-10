using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordController : MonoBehaviour
{
    public bool isRecording = false;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            this.isRecording = true;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            this.isRecording = false;
        }
    }
}
