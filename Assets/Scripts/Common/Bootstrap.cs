using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Video;

namespace Mediapipe.Unity.Sample {
    public class Bootstrap : MonoBehaviour {
        [Serializable]
        public enum AssetLoaderType {
            StreamingAssets,
            AssetBundle,
            Local,
        }

        [Header("基本设置")]
        [SerializeField] private ImageSourceType _defaultImageSource;
        [SerializeField] private InferenceMode _preferableInferenceMode;
        [SerializeField] private AssetLoaderType _assetLoaderType;
        [SerializeField] private Logger.LogLevel _logLevel = Logger.LogLevel.Debug;

        [Header("Glog 设置")]
        [SerializeField] private int _glogMinloglevel = Glog.Minloglevel;
        [SerializeField] private int _glogStderrthreshold = Glog.Stderrthreshold;
        [SerializeField] private int _glogV = Glog.V;

        [Header("WebCam 摄像头源")]
        [Tooltip("对于默认分辨率，将选择宽度最接近此值的分辨率")]
        [SerializeField] private int _preferredDefaultWebCamWidth = 1280;

        [SerializeField] private ImageSource.ResolutionStruct[] _defaultAvailableWebCamResolutions =
            new ImageSource.ResolutionStruct[] {
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

        [Header("静态图像源")]
        [SerializeField] private Texture[] _availableStaticImageSources;

        [SerializeField] private ImageSource.ResolutionStruct[] _defaultAvailableStaticImageResolutions =
            new ImageSource.ResolutionStruct[] {
                new ImageSource.ResolutionStruct(512, 512, 0),
                new ImageSource.ResolutionStruct(640, 480, 0),
                new ImageSource.ResolutionStruct(1280, 720, 0),
            };

        [Header("视频源")]
        [SerializeField] private VideoClip[] _availableVideoSources;

        public InferenceMode inferenceMode { get; private set; }
        public bool isFinished { get; private set; }
        private bool _isGlogInitialized;

        private void OnEnable() {
            var _ = StartCoroutine(Init());
        }

        private IEnumerator Init() {
            Debug.Log("The configuration for the sample app can be modified directly in the Bootstrap component inspector.");
#if !DEBUG && !DEVELOPMENT_BUILD
      Debug.LogWarning("Logging for the MediaPipeUnityPlugin will be suppressed. To enable logging, please check the 'Development Build' option and build.");
#endif

            Logger.MinLogLevel = _logLevel;

            Protobuf.SetLogHandler(Protobuf.DefaultLogHandler);

            Debug.Log("Setting global flags...");
            ResetGlogFlags();
            Glog.Initialize("MediaPipeUnityPlugin");
            _isGlogInitialized = true;

            Debug.Log("Initializing AssetLoader...");
            switch (_assetLoaderType) {
                case AssetLoaderType.AssetBundle: {
                    AssetLoader.Provide(new AssetBundleResourceManager("mediapipe"));
                    break;
                }
                case AssetLoaderType.StreamingAssets: {
                    AssetLoader.Provide(new StreamingAssetsResourceManager());
                    break;
                }
                case AssetLoaderType.Local: {
#if UNITY_EDITOR
                    AssetLoader.Provide(new LocalResourceManager());
                    break;
#else
            Debug.LogError("LocalResourceManager is only supported on UnityEditor." +
              "To avoid this error, consider switching to the StreamingAssetsResourceManager and copying the required resources under StreamingAssets, for example.");
            yield break;
#endif
                }
                default: {
                    Debug.LogError($"AssetLoaderType is unknown: {_assetLoaderType}");
                    yield break;
                }
            }

            DecideInferenceMode();
            if (inferenceMode == InferenceMode.GPU) {
                Debug.Log("Initializing GPU resources...");
                yield return GpuManager.Initialize();

                if (!GpuManager.IsInitialized) {
                    Debug.LogWarning(
                        "If your native library is built for CPU, change 'Preferable Inference Mode' to CPU from the Bootstrap component inspector");
                }
            }

            Debug.Log("Preparing ImageSource...");
            ImageSourceProvider.Initialize(
                BuildWebCamSource(), BuildStaticImageSource(),
                BuildVideoSource());
            ImageSourceProvider.Switch(_defaultImageSource);

            isFinished = true;
        }

        private void DecideInferenceMode() {
#if UNITY_EDITOR_OSX || UNITY_EDITOR_WIN
            if (_preferableInferenceMode == InferenceMode.GPU) {
                Debug.LogWarning("Current platform does not support GPU inference mode, so falling back to CPU mode");
            }
            inferenceMode = InferenceMode.CPU;
#else
      inferenceMode = _preferableInferenceMode;
#endif
        }

        private void OnApplicationQuit() {
            GpuManager.Shutdown();

            if (_isGlogInitialized) {
                Glog.Shutdown();
            }

            Protobuf.ResetLogHandler();
        }

        private void ResetGlogFlags() {
            Glog.Logtostderr = true;
            Glog.Minloglevel = _glogMinloglevel;
            Glog.Stderrthreshold = _glogStderrthreshold;
            Glog.V = _glogV;
        }

        private WebCamSource BuildWebCamSource() => new WebCamSource(
            _preferredDefaultWebCamWidth,
            _defaultAvailableWebCamResolutions
        );

        private StaticImageSource BuildStaticImageSource() => new StaticImageSource(
            _availableStaticImageSources,
            _defaultAvailableStaticImageResolutions
        );

        private VideoSource BuildVideoSource() => new VideoSource(_availableVideoSources);
    }
}