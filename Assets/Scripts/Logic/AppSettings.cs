using UnityEngine;
using UnityEngine.UI;
using Mediapipe.Unity.Sample;
using Mediapipe.Unity.Sample.PoseLandmarkDetection;

public class AppSettings : MonoBehaviour {
    [Header("Bootstrap 组件引用")] [SerializeField]
    private Bootstrap bootstrap;
    [Header("当前图像源类型")] [SerializeField] private ImageSourceType currentImageSourceType = ImageSourceType.WebCamera;

    [Header("UI 按钮引用")] [SerializeField] private Button webCameraButton;
    [SerializeField] private Button videoButton;

    [Header("PoseLandmarkerRunner 引用")] [SerializeField]
    private PoseLandmarkerRunner poseLandmarkerRunner;

    private void Start() {
        if (bootstrap == null) {
            bootstrap = FindObjectOfType<Bootstrap>(); 
        }
        if (bootstrap == null) {
            Debug.LogError("AppSettings: 找不到 Bootstrap 组件！");
        }
        if (poseLandmarkerRunner == null) {
            poseLandmarkerRunner = FindObjectOfType<PoseLandmarkerRunner>();
        }
        if (poseLandmarkerRunner == null) {
            Debug.LogError("AppSettings: 找不到 PoseLandmarkerRunner 组件！");
        }
        BindButtonEvents();
        StartCoroutine(WaitForBootstrapAndSetImageSource());
    }

    private System.Collections.IEnumerator WaitForBootstrapAndSetImageSource() {
        while (!bootstrap.isFinished) {
            yield return null;
        }
        Debug.Log($"AppSettings: Bootstrap 初始化完成，设置图像源为 {currentImageSourceType}");
        ImageSourceProvider.Switch(currentImageSourceType);
    }

    private void BindButtonEvents() {
        if (webCameraButton != null)
            webCameraButton.onClick.AddListener(SwitchToWebCamera);
        if (videoButton != null)
            videoButton.onClick.AddListener(SwitchToVideo);
    }

    [ContextMenu("切换到摄像头")]
    public void SwitchToWebCamera() {
        ChangeImageSource(ImageSourceType.WebCamera);
    }

    [ContextMenu("切换到视频")]
    public void SwitchToVideo() {
        ChangeImageSource(ImageSourceType.Video);
    }

    private void ChangeImageSource(ImageSourceType newSourceType) {
        if (bootstrap == null) {
            Debug.LogError("AppSettings: Bootstrap 组件未找到，无法切换图像源！");
            return;
        }
        if (!bootstrap.isFinished) {
            Debug.LogWarning("AppSettings: Bootstrap 尚未初始化完成，请稍后再试！");
            return;
        }
        var field = typeof(Bootstrap).GetField("_defaultImageSource",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (field != null) {
            field.SetValue(bootstrap, newSourceType);
            currentImageSourceType = newSourceType;
            ImageSourceProvider.Switch(newSourceType);
            Debug.Log($"AppSettings: 图像源已切换到 {newSourceType}");
            StartCoroutine(RestartPoseLandmarkerRunner());
        } else {
            Debug.LogError("AppSettings: 无法访问 Bootstrap 的 _defaultImageSource 字段！");
        }
    }

    private System.Collections.IEnumerator RestartPoseLandmarkerRunner() {
        if (poseLandmarkerRunner == null) {
            Debug.LogWarning("AppSettings: PoseLandmarkerRunner 未找到，跳过重启");
            yield break;
        }
        Debug.Log("AppSettings: 停止 PoseLandmarkerRunner...");
        poseLandmarkerRunner.Stop();
        yield return null;
        Debug.Log("AppSettings: 重新启动 PoseLandmarkerRunner...");
        poseLandmarkerRunner.Play();
    }
}