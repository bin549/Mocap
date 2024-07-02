using System.Collections;
using UnityEngine;

namespace Mediapipe.Unity.Sample
{
    public abstract class Solution : MonoBehaviour
    {
        private static readonly string _BootstrapName = nameof(Bootstrap);

        [SerializeField] private GameObject _bootstrapPrefab;

#pragma warning disable IDE1006
        protected virtual string TAG => GetType().Name;
#pragma warning restore IDE1006

        public Bootstrap bootstrap { get; private set; }
        protected bool isPaused;

        protected virtual IEnumerator Start()
        {
            bootstrap = FindBootstrap();
            yield return new WaitUntil(() => bootstrap.isFinished);

            Play();
        }

        public virtual void Play()
        {
            isPaused = false;
        }

        public virtual void Pause()
        {
            isPaused = true;
        }

        public virtual void Resume()
        {
            isPaused = false;
        }

        public virtual void Stop()
        {
            isPaused = true;
        }

        protected static void SetupAnnotationController<T>(AnnotationController<T> annotationController,
            ImageSource imageSource, bool expectedToBeMirrored = false) where T : HierarchicalAnnotation
        {
            annotationController.isMirrored =
                expectedToBeMirrored ^ imageSource.isHorizontallyFlipped ^ imageSource.isFrontFacing;
            annotationController.rotationAngle = imageSource.rotation.Reverse();
        }

        protected static void ReadFromImageSource(ImageSource imageSource, TextureFrame textureFrame)
        {
            var sourceTexture = imageSource.GetCurrentTexture();
            var textureType = sourceTexture.GetType();
            if (textureType == typeof(WebCamTexture))
            {
                textureFrame.ReadTextureFromOnCPU((WebCamTexture)sourceTexture);
            }
            else if (textureType == typeof(Texture2D))
            {
                textureFrame.ReadTextureFromOnCPU((Texture2D)sourceTexture);
            }
            else
            {
                textureFrame.ReadTextureFromOnCPU(sourceTexture);
            }
        }

        protected Bootstrap FindBootstrap()
        {
            var bootstrapObj = GameObject.Find(_BootstrapName);

            if (bootstrapObj == null)
            {
                Debug.Log("Initializing the Bootstrap GameObject");
                bootstrapObj = Instantiate(_bootstrapPrefab);
                bootstrapObj.name = _BootstrapName;
                DontDestroyOnLoad(bootstrapObj);
            }

            return bootstrapObj.GetComponent<Bootstrap>();
        }
    }
}
