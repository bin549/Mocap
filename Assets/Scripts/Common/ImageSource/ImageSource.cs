using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

namespace Mediapipe.Unity {
    public abstract class ImageSource {
        [Serializable]
        public struct ResolutionStruct {
            public int width;
            public int height;
            public double frameRate;

            public ResolutionStruct(int width, int height, double frameRate) {
                this.width = width;
                this.height = height;
                this.frameRate = frameRate;
            }

            public ResolutionStruct(Resolution resolution) {
                width = resolution.width;
                height = resolution.height;
                frameRate = resolution.refreshRate;
            }

            public Resolution ToResolution() {
                return new Resolution() { width = width, height = height, refreshRate = (int)frameRate };
            }

            public override string ToString() {
                var aspectRatio = $"{width}x{height}";
                var frameRateStr = frameRate.ToString("#.##");
                return frameRate > 0 ? $"{aspectRatio} ({frameRateStr}Hz)" : aspectRatio;
            }
        }

        public ResolutionStruct resolution { get; protected set; }

        public TextureFormat textureFormat => isPrepared
            ? TextureFormatFor(GetCurrentTexture())
            : throw new InvalidOperationException("ImageSource is not prepared");

        public virtual int textureWidth => resolution.width;
        public virtual int textureHeight => resolution.height;
        public virtual double frameRate => resolution.frameRate;

        public float focalLengthPx { get; } = 2.0f; // TODO: calculate at runtime
        public virtual bool isHorizontallyFlipped { get; set; } = false;
        public virtual bool isVerticallyFlipped { get; } = false;
        public virtual bool isFrontFacing { get; } = false;
        public virtual RotationAngle rotation { get; } = RotationAngle.Rotation0;

        public abstract string sourceName { get; }
        public abstract string[] sourceCandidateNames { get; }
        public abstract ResolutionStruct[] availableResolutions { get; }
        public abstract bool isPrepared { get; }

        public abstract bool isPlaying { get; }
        public abstract void SelectSource(int sourceId);

        public void SelectResolution(int resolutionId) {
            var resolutions = availableResolutions;
            if (resolutionId < 0 || resolutionId >= resolutions.Length) {
                throw new ArgumentException($"Invalid resolution ID: {resolutionId}");
            }
            resolution = resolutions[resolutionId];
        }

        public abstract IEnumerator Play();

        public abstract IEnumerator Resume();

        public abstract void Pause();

        public abstract void Stop();

        public abstract Texture GetCurrentTexture();

        protected static TextureFormat TextureFormatFor(Texture texture) {
            return GraphicsFormatUtility.GetTextureFormat(texture.graphicsFormat);
        }

        public Experimental.ImageTransformationOptions GetTransformationOptions(bool expectedToBeMirrored = false) {
            var shouldFlipHorizontally = (isFrontFacing || expectedToBeMirrored) ^ isHorizontallyFlipped;
            var shouldFlipVertically = isVerticallyFlipped;
            return Experimental.ImageTransformationOptions.Build(shouldFlipHorizontally, shouldFlipVertically,
                rotation);
        }
    }
}