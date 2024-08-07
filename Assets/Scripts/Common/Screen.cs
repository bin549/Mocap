using UnityEngine;
using UnityEngine.UI;

namespace Mediapipe.Unity
{
    public class Screen : MonoBehaviour
    {
        [SerializeField] private RawImage _screen;

        private ImageSource _imageSource;

        public Texture texture
        {
            get => _screen.texture;
            set => _screen.texture = value;
        }

        public UnityEngine.Rect uvRect
        {
            set => _screen.uvRect = value;
        }

        public void Initialize(ImageSource imageSource)
        {
            _imageSource = imageSource;
            // Resize(_imageSource.textureWidth, _imageSource.textureHeight);
            Rotate(_imageSource.rotation.Reverse());
            ResetUvRect(RunningMode.Async);
            texture = imageSource.GetCurrentTexture();
        }

        public void Resize(int width, int height)
        {
            _screen.rectTransform.sizeDelta = new Vector2(width, height);
        }

        public void Rotate(RotationAngle rotationAngle)
        {
            _screen.rectTransform.localEulerAngles = rotationAngle.GetEulerAngles();
        }

        public void ReadSync(TextureFrame textureFrame)
        {
            if (!(texture is Texture2D))
            {
                texture = new Texture2D(_imageSource.textureWidth, _imageSource.textureHeight, TextureFormat.RGBA32,
                    false);
                ResetUvRect(RunningMode.Sync);
            }

            textureFrame.CopyTexture(texture);
        }

        private void ResetUvRect(RunningMode runningMode)
        {
            var rect = new UnityEngine.Rect(0, 0, 1, 1);

            if (_imageSource.isVerticallyFlipped && runningMode == RunningMode.Async)
            {
                rect = FlipVertically(rect);
            }

            if (_imageSource.isFrontFacing)
            {
                var rotation = _imageSource.rotation;
                if (rotation == RotationAngle.Rotation0 || rotation == RotationAngle.Rotation180)
                {
                    rect = FlipHorizontally(rect);
                }
                else
                {
                    rect = FlipVertically(rect);
                }
            }

            uvRect = rect;
        }

        private UnityEngine.Rect FlipHorizontally(UnityEngine.Rect rect)
        {
            return new UnityEngine.Rect(1 - rect.x, rect.y, -rect.width, rect.height);
        }

        private UnityEngine.Rect FlipVertically(UnityEngine.Rect rect)
        {
            return new UnityEngine.Rect(rect.x, 1 - rect.y, rect.width, -rect.height);
        }
    }
}
