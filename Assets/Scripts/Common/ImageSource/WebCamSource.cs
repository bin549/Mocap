using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if UNITY_ANDROID
using UnityEngine.Android;
#endif

namespace Mediapipe.Unity {
    public class WebCamSource : ImageSource {
        private readonly int _preferableDefaultWidth = 1280;

        private const string _TAG = nameof(WebCamSource);

        private readonly ResolutionStruct[] _defaultAvailableResolutions;

        public WebCamSource(int preferableDefaultWidth, ResolutionStruct[] defaultAvailableResolutions) {
            _preferableDefaultWidth = preferableDefaultWidth;
            _defaultAvailableResolutions = defaultAvailableResolutions;
        }

        private static readonly object _PermissionLock = new object();
        private static bool _IsPermitted = false;

        private WebCamTexture _webCamTexture;

        private WebCamTexture webCamTexture {
            get => _webCamTexture;
            set {
                if (_webCamTexture != null) {
                    _webCamTexture.Stop();
                }
                _webCamTexture = value;
            }
        }

        public override int textureWidth => !isPrepared ? 0 : webCamTexture.width;
        public override int textureHeight => !isPrepared ? 0 : webCamTexture.height;

        public override bool isVerticallyFlipped => isPrepared && webCamTexture.videoVerticallyMirrored;

        public override bool isFrontFacing => isPrepared && (webCamDevice is WebCamDevice valueOfWebCamDevice) &&
                                              valueOfWebCamDevice.isFrontFacing;

        public override RotationAngle rotation =>
            !isPrepared ? RotationAngle.Rotation0 : (RotationAngle)webCamTexture.videoRotationAngle;

        private WebCamDevice? _webCamDevice;

        private WebCamDevice? webCamDevice {
            get => _webCamDevice;
            set {
                if (_webCamDevice is WebCamDevice valueOfWebCamDevice) {
                    if (value is WebCamDevice valueOfValue && valueOfValue.name == valueOfWebCamDevice.name) {
                        // not changed
                        return;
                    }
                } else if (value == null) {
                    // not changed
                    return;
                }
                _webCamDevice = value;
                resolution = GetDefaultResolution();
            }
        }

        public override string sourceName =>
            (webCamDevice is WebCamDevice valueOfWebCamDevice) ? valueOfWebCamDevice.name : null;

        private WebCamDevice[] _availableSources;

        private WebCamDevice[] availableSources {
            get {
                if (_availableSources == null) {
                    _availableSources = WebCamTexture.devices;
                }

                return _availableSources;
            }
            set => _availableSources = value;
        }

        public override string[] sourceCandidateNames => availableSources?.Select(device => device.name).ToArray();

#pragma warning disable IDE0025
        public override ResolutionStruct[] availableResolutions {
            get {
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        if (webCamDevice is WebCamDevice valueOfWebCamDevice) {
          return valueOfWebCamDevice.availableResolutions.Select(resolution => new ResolutionStruct(resolution)).ToArray();
        }
#endif
                return webCamDevice == null ? null : _defaultAvailableResolutions;
            }
        }
#pragma warning restore IDE0025

        public override bool isPrepared => webCamTexture != null;
        public override bool isPlaying => webCamTexture != null && webCamTexture.isPlaying;

        private IEnumerator Initialize() {
            Debug.Log("WebCamSource: 开始获取权限...");
            yield return GetPermission();
            Debug.Log($"WebCamSource: 权限获取完成，状态: {_IsPermitted}");

            if (!_IsPermitted) {
                Debug.LogError("WebCamSource: 权限获取失败，退出初始化");
                yield break;
            }

            if (webCamDevice != null) {
                Debug.Log($"WebCamSource: 设备已存在: {webCamDevice?.name}");
                yield break;
            }

            Debug.Log("WebCamSource: 获取可用摄像头设备...");
            availableSources = WebCamTexture.devices;
            Debug.Log($"WebCamSource: 找到 {availableSources?.Length ?? 0} 个摄像头设备");

            if (availableSources != null && availableSources.Length > 0) {
                webCamDevice = availableSources[0];
                Debug.Log($"WebCamSource: 选择设备: {webCamDevice?.name}");
            } else {
                Debug.LogError("WebCamSource: 没有找到可用的摄像头设备！");
            }
        }

        private IEnumerator GetPermission() {
            lock (_PermissionLock) {
                if (_IsPermitted) {
                    yield break;
                }

#if UNITY_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
          Permission.RequestUserPermission(Permission.Camera);
          yield return new WaitForSeconds(0.1f);
        }
#elif UNITY_IOS
        if (!Application.HasUserAuthorization(UserAuthorization.WebCam)) {
          yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        }
#endif

#if UNITY_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
          Debug.LogWarning("Not permitted to use Camera");
          yield break;
        }
#elif UNITY_IOS
        if (!Application.HasUserAuthorization(UserAuthorization.WebCam)) {
          Debug.LogWarning("Not permitted to use WebCam");
          yield break;
        }
#endif
                _IsPermitted = true;

                yield return new WaitForEndOfFrame();
            }
        }

        public override void SelectSource(int sourceId) {
            if (sourceId < 0 || sourceId >= availableSources.Length) {
                throw new ArgumentException($"Invalid source ID: {sourceId}");
            }

            webCamDevice = availableSources[sourceId];
        }

        public override IEnumerator Play() {
            Debug.Log("WebCamSource: 开始初始化...");
            yield return Initialize();
            Debug.Log($"WebCamSource: 初始化完成，权限状态: {_IsPermitted}");

            if (!_IsPermitted) {
                Debug.LogError("WebCamSource: 没有摄像头权限！");
                throw new InvalidOperationException("Not permitted to access cameras");
            }

            Debug.Log($"WebCamSource: 初始化WebCamTexture，设备: {webCamDevice?.name}");
            InitializeWebCamTexture();
            Debug.Log("WebCamSource: 开始播放WebCamTexture...");
            webCamTexture.Play();
            yield return WaitForWebCamTexture();
            Debug.Log($"WebCamSource: WebCamTexture播放完成，尺寸: {webCamTexture.width}x{webCamTexture.height}");
        }

        public override IEnumerator Resume() {
            if (!isPrepared) {
                throw new InvalidOperationException("WebCamTexture is not prepared yet");
            }
            if (!webCamTexture.isPlaying) {
                webCamTexture.Play();
            }
            yield return WaitForWebCamTexture();
        }

        public override void Pause() {
            if (isPlaying) {
                webCamTexture.Pause();
            }
        }

        public override void Stop() {
            if (webCamTexture != null) {
                webCamTexture.Stop();
            }
            webCamTexture = null;
        }

        public override Texture GetCurrentTexture() => webCamTexture;

        private ResolutionStruct GetDefaultResolution() {
            var resolutions = availableResolutions;
            return resolutions == null || resolutions.Length == 0
                ? new ResolutionStruct()
                : resolutions.OrderBy(resolution => resolution, new ResolutionStructComparer(_preferableDefaultWidth))
                    .First();
        }

        private void InitializeWebCamTexture() {
            Stop();
            if (webCamDevice is WebCamDevice valueOfWebCamDevice) {
                webCamTexture = new WebCamTexture(valueOfWebCamDevice.name, resolution.width, resolution.height,
                    (int)resolution.frameRate);
                return;
            }
            throw new InvalidOperationException("Cannot initialize WebCamTexture because WebCamDevice is not selected");
        }

        private IEnumerator WaitForWebCamTexture() {
            const int timeoutFrame = 2000;
            var count = 0;
            Debug.Log("Waiting for WebCamTexture to start");
            yield return new WaitUntil(() => count++ > timeoutFrame || webCamTexture.width > 16);

            if (webCamTexture.width <= 16) {
                throw new TimeoutException("Failed to start WebCam");
            }
        }

        private class ResolutionStructComparer : IComparer<ResolutionStruct> {
            private readonly int _preferableDefaultWidth;

            public ResolutionStructComparer(int preferableDefaultWidth) {
                _preferableDefaultWidth = preferableDefaultWidth;
            }

            public int Compare(ResolutionStruct a, ResolutionStruct b) {
                var aDiff = Mathf.Abs(a.width - _preferableDefaultWidth);
                var bDiff = Mathf.Abs(b.width - _preferableDefaultWidth);
                if (aDiff != bDiff) {
                    return aDiff - bDiff;
                }
                if (a.height != b.height) {
                    return a.height - b.height;
                }
                return (int)(a.frameRate - b.frameRate);
            }
        }
    }
}