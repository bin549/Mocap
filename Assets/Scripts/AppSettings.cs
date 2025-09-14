using UnityEngine;
using UnityEngine.Video;
using System.IO;

public class AppSettings : MonoBehaviour {
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
    public MotionDataRecorder motionDataRecorder;

    private void Awake() {
        if (_Instance != null) {
            Destroy(gameObject);
            return;
        }
        _Instance = this;
        DontDestroyOnLoad(this);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.V)) {
            viewCamera.gameObject.SetActive(true);
        }
        if (Input.GetKeyUp(KeyCode.V)) {
            viewCamera.gameObject.SetActive(false);
        }
    }

    private void Start() {
        // 在编辑器中优先使用Application.dataPath，编译后使用StreamingAssets
        string basePath;
        
        #if UNITY_EDITOR
        // 编辑器中直接使用Assets路径
        basePath = Application.dataPath;
        #else
        // 编译后使用StreamingAssets路径
        basePath = Application.streamingAssetsPath;
        if (string.IsNullOrEmpty(basePath)) {
            basePath = Application.dataPath;
        }
        #endif
        
        videosFolderPath = FolderUtils.CheckDirectory(basePath + @"/Videos");
        modelsFolderPath = FolderUtils.CheckDirectory(basePath + @"/Models");
        savedFolderPath = FolderUtils.CheckDirectory(basePath + @"/Resources");
        
        if (string.IsNullOrEmpty(videosFolderPath)) {
            FolderUtils.SafeCreateDirectory(basePath + @"/Videos");
            videosFolderPath = FolderUtils.CheckDirectory(basePath + @"/Videos");
        }
        
        if (string.IsNullOrEmpty(modelsFolderPath)) {
            FolderUtils.SafeCreateDirectory(basePath + @"/Models");
            modelsFolderPath = FolderUtils.CheckDirectory(basePath + @"/Models");
        }
        
        if (string.IsNullOrEmpty(savedFolderPath)) {
            FolderUtils.SafeCreateDirectory(basePath + @"/Resources");
            savedFolderPath = FolderUtils.CheckDirectory(basePath + @"/Resources");
        }
        
        Debug.Log($"视频文件夹路径: {videosFolderPath}");
    }

    public void SetAvatar(Avatar avatar) {
        this.avatar = avatar;
    }

    public Avatar GetAvatar() {
        if (avatar != null) {
            return avatar;
        }
        return null;
    }

    public Animator GetAvatarAnimator() {
        if (avatarAnimator != null) {
            return avatarAnimator;
        }
        return null;
    }

    public void SetAvatarAnimator(Animator avatarAnimator) {
        this.avatarAnimator = avatarAnimator;
    }

    public string GetVideosFolderPath() {
        return videosFolderPath;
    }
    
    public string[] GetVideosFromStreamingAssets() {
        string[] extensions = new string[] {
            ".mp4", ".mov"
        };
        
        #if UNITY_EDITOR
        // 编辑器中直接使用videosFolderPath
        Debug.Log($"编辑器模式 - 查找视频文件路径: {videosFolderPath}");
        string[] videos = FolderUtils.GetFilterdFiles(videosFolderPath, extensions);
        Debug.Log($"找到 {videos.Length} 个视频文件");
        foreach (string video in videos) {
            Debug.Log($"视频文件: {video}");
        }
        return videos;
        #else
        // 编译后优先尝试从StreamingAssets加载
        string streamingAssetsPath = Application.streamingAssetsPath + "/Videos";
        Debug.Log($"编译模式 - StreamingAssets路径: {streamingAssetsPath}");
        if (Directory.Exists(streamingAssetsPath)) {
            Debug.Log("StreamingAssets/Videos 目录存在");
            return FolderUtils.GetFilterdFiles(streamingAssetsPath, extensions);
        }
        
        Debug.Log("StreamingAssets/Videos 目录不存在，使用默认路径");
        // 如果StreamingAssets中没有，则使用默认路径
        return FolderUtils.GetFilterdFiles(videosFolderPath, extensions);
        #endif
    }

    public string[] GetModels() {
        string[] extensions = new string[] {
            ".fbx", ".obj"
        };
        return FolderUtils.GetFilterdFiles(modelsFolderPath, extensions);
    }


    public string GetModelsFolderPath() {
        return modelsFolderPath;
    }

    public string GetSavedFolderPath() {
        return savedFolderPath;
    }

    public void SetVideosFolderPath() {
        var path = FolderUtils.SelectFolder();
        if (!string.IsNullOrEmpty(path)) {
            videosFolderPath = path;
        }
    }

    public void SetModelsFolderPath() {
        var path = FolderUtils.SelectFolder();
        if (!string.IsNullOrEmpty(path)) {
            modelsFolderPath = path;
        }
    }

    public void SetSavedFolderPath() {
        var path = FolderUtils.SelectFolder();
        if (!string.IsNullOrEmpty(path)) {
            savedFolderPath = path;
        }
    }
}
