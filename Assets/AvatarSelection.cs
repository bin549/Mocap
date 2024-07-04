using System.Collections;
using System.Collections.Generic;
using Mediapipe.Unity.Sample.PoseTracking;
using UnityEngine;
using UnityEngine.UI;

public class AvatarSelection : MonoBehaviour
{
    public Button avatarItem;
    public GameObject currentAvatar;
    public GameObject selectedAvatar;
    public PoseTrackingSolution solution;
    public UiManager uiManager;
    
    private void Start()
    {
        avatarItem.onClick.AddListener(() =>
        {
            currentAvatar.gameObject.SetActive(false);
            selectedAvatar.gameObject.SetActive(true);
            solution.SetAvatar(selectedAvatar.GetComponent<Mediapipe2UnitySkeletonController>());
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
