using System.Collections;
using UnityEngine;
using UnityEngine.Video;

namespace Mediapipe.Unity.Sample
{
    public class Bootstrap : MonoBehaviour
    {
        public enum AssetLoaderType
        {
            StreamingAssets,
            AssetBundle,
            Local,
        }

        [SerializeField] private DeviceSelection deviceSelection;
        [SerializeField] private InferenceMode _preferableInferenceMode;
        [SerializeField] private AssetLoaderType _assetLoaderType;
        [SerializeField] private Logger.LogLevel _logLevel = Logger.LogLevel.Debug;

        [Header("Glog settings")] private int _glogMinloglevel = Glog.Minloglevel;
        private int _glogStderrthreshold = Glog.Stderrthreshold;
        private int _glogV = Glog.V;

        [Header("WebCam Source")]
        [Tooltip("For the default resolution, the one whose width is closest to this value will be chosen")]
        [SerializeField]
        private int _preferredDefaultWebCamWidth = 1280;

        [SerializeField] private ImageSource.ResolutionStruct[] _defaultAvailableWebCamResolutions =
            new ImageSource.ResolutionStruct[]
            {
                new ImageSource.ResolutionStruct(176, 144, 30),
                new ImageSource.ResolutionStruct(320, 240, 30),
                new ImageSource.ResolutionStruct(424, 240, 30),
                new ImageSource.ResolutionStruct(640, 360, 30),
                new ImageSource.ResolutionStruct(640, 480, 30),
                new ImageSource.ResolutionStruct(848, 480, 30),
                new ImageSource.ResolutionStruct(960, 540, 30),
                new ImageSource.ResolutionStruct(1280, 720, 30),
                new ImageSource.ResolutionStruct(1600, 896, 30),
                new ImageSource.ResolutionStruct(1920, 1080, 30),
            };

        public ImageSourceType defaultImageSource => deviceSelection.defaultImageSource;
        public InferenceMode preferableInferenceMode => _preferableInferenceMode;
        public AssetLoaderType assetLoaderType => _assetLoaderType;
        public Logger.LogLevel logLevel => _logLevel;
        public WebCamSource webCamSource;
        public VideoSource videoSource;
        
        public void ResetGlogFlags()
        {
            Glog.Logtostderr = true;
            Glog.Minloglevel = _glogMinloglevel;
            Glog.Stderrthreshold = _glogStderrthreshold;
            Glog.V = _glogV;
        }

        public WebCamSource BuildWebCamSource()
        {
            return webCamSource;
        }

        public VideoSource BuildVideoSource()
        {
            return videoSource;
        }

        public InferenceMode inferenceMode { get; private set; }
        public bool isFinished { get; private set; }
        private bool _isGlogInitialized;

        private void OnEnable()
        {
            var _ = StartCoroutine(Init());
        }

        private IEnumerator Init()
        {
            Debug.Log("The configuration for the sample app can be modified using AppSettings.asset.");
#if !DEBUG && !DEVELOPMENT_BUILD
      Debug.LogWarning("Logging for the MediaPipeUnityPlugin will be suppressed. To enable logging, please check the 'Development Build' option and build.");
#endif
            Logger.MinLogLevel = logLevel;
            Protobuf.SetLogHandler(Protobuf.DefaultLogHandler);
            this.ResetGlogFlags();
            Glog.Initialize("MediaPipeUnityPlugin");
            _isGlogInitialized = true;
            switch (this.assetLoaderType)
            {
                case AssetLoaderType.AssetBundle:
                {
                    AssetLoader.Provide(new AssetBundleResourceManager("mediapipe"));
                    break;
                }
                case AssetLoaderType.StreamingAssets:
                {
                    AssetLoader.Provide(new StreamingAssetsResourceManager());
                    break;
                }
                case AssetLoaderType.Local:
                {
#if UNITY_EDITOR
                    AssetLoader.Provide(new LocalResourceManager());
                    break;
#else
            Debug.LogError("LocalResourceManager is only supported on UnityEditor." +
              "To avoid this error, consider switching to the StreamingAssetsResourceManager and copying the required resources under StreamingAssets, for example.");
            yield break;
#endif
                }
                default:
                {
                    Debug.LogError($"AssetLoaderType is unknown: {this.assetLoaderType}");
                    yield break;
                }
            }

            DecideInferenceMode();
            if (inferenceMode == InferenceMode.GPU)
            {
                yield return GpuManager.Initialize();
                if (!GpuManager.IsInitialized)
                {
                    Debug.LogWarning(
                        "If your native library is built for CPU, change 'Preferable Inference Mode' to CPU from the Inspector Window for AppSettings");
                }
            }
            ImageSourceProvider.Initialize(this.BuildWebCamSource(), this.BuildVideoSource());
            ImageSourceProvider.Switch(this.defaultImageSource);
            isFinished = true; 
        }

        private void DecideInferenceMode()
        {
#if UNITY_EDITOR_OSX || UNITY_EDITOR_WIN
            if (this.preferableInferenceMode == InferenceMode.GPU)
            {
                Debug.LogWarning("Current platform does not support GPU inference mode, so falling back to CPU mode");
            }
            inferenceMode = InferenceMode.CPU;
#else
      inferenceMode = this.preferableInferenceMode;
#endif
        }

        private void OnApplicationQuit()
        {
            GpuManager.Shutdown();
            if (_isGlogInitialized)
            {
                Glog.Shutdown();
            }

            Protobuf.ResetLogHandler();
        }
    }
}
