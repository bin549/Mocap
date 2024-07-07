using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class AppSettings : MonoBehaviour
{
    private string videosFolderPath;
    private string modelsFolderPath;
    private string savedFolderPath;
    private static AppSettings _Instance;
    public bool isShowSkeleton = false;
    public bool isBVHRecorder = false;
    public Avatar avatar;
    [SerializeField] private Animator avatarAnimator;
    public GameObject environmentPivot;
    public GameObject avatarModelPivot;
    public GameObject[] environments;
    public GameObject[] avatarModels;
    private VideoSource _videoSource;
    [SerializeField] private Camera viewCamera;
    public bool isAppBoot = false;
    
    private void Awake()
    {
        if (_Instance != null)
        {
            Destroy(gameObject);
            return;
        }
    
        _Instance = this;
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            viewCamera.gameObject.SetActive(true);
        }
        if (Input.GetKeyUp(KeyCode.V))
        {
            viewCamera.gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        videosFolderPath = FolderUtils.CheckDirectory(Application.dataPath + @"/Videos");
        modelsFolderPath = FolderUtils.CheckDirectory(Application.dataPath + @"/Models");
        savedFolderPath = FolderUtils.CheckDirectory(Application.dataPath + @"/Resources");
        if (string.IsNullOrEmpty(savedFolderPath))
        {
            FolderUtils.SafeCreateDirectory(Application.dataPath + @"/Resources");
            savedFolderPath = FolderUtils.CheckDirectory(Application.dataPath + @"/Resources");
        }
    }
    
    public void SetAvatar(Avatar avatar)
    {
        this.avatar = avatar;
    }
    
    public Avatar GetAvatar()
    {
        if (avatar != null)
        {
            return avatar;
        }
    
        return null;
    }
    
    public Animator GetAvatarAnimator()
    {
        if (avatarAnimator != null)
        {
            return avatarAnimator;
        }
    
        return null;
    }
    
    public void SetAvatarAnimator(Animator avatarAnimator)
    {
        this.avatarAnimator = avatarAnimator;
    }
    
    public string GetVideosFolderPath()
    {
        return videosFolderPath;
    }
    
    public string[] GetModels()
    {
        string[] extensions = new string[]
        {
            ".fbx", ".obj"
        };
        return FolderUtils.GetFilterdFiles(modelsFolderPath, extensions);
    }
    
    
    public string GetModelsFolderPath()
    {
        return modelsFolderPath;
    }
    
    public string GetSavedFolderPath()
    {
        return savedFolderPath;
    }
    
    public void SetVideosFolderPath()
    {
        var path = FolderUtils.SelectFolder();
        if (!string.IsNullOrEmpty(path))
        {
            videosFolderPath = path;
        }
    }
    
    public void SetModelsFolderPath()
    {
        var path = FolderUtils.SelectFolder();
        if (!string.IsNullOrEmpty(path))
        {
            modelsFolderPath = path;
        }
    }
    
    
    
    public void SetSavedFolderPath()
    {
        var path = FolderUtils.SelectFolder();
        if (!string.IsNullOrEmpty(path))
        {
            savedFolderPath = path;
        }
    }
}
