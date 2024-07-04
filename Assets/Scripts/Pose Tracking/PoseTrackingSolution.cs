using System.Collections;
using UnityEngine;
using Mediapipe.Unity;

namespace Mediapipe.Unity.Sample.PoseTracking
{
    public class PoseTrackingSolution : ImageSourceSolution<PoseTrackingGraph>
    {
        [SerializeField] private PoseLandmarkListAnnotationController _poseLandmarksAnnotationController;
        [SerializeField] private Mediapipe2UnitySkeletonController _mediapipe2UnitySkeletonController;

        public void SetAvatar(Mediapipe2UnitySkeletonController mediapipe2UnitySkeletonController)
        {
            this._mediapipe2UnitySkeletonController = mediapipe2UnitySkeletonController;
        }
        
        public PoseTrackingGraph.ModelComplexity modelComplexity
        {
            get => graphRunner.modelComplexity;
            set => graphRunner.modelComplexity = value;
        }

        public bool smoothLandmarks
        {
            get => graphRunner.smoothLandmarks;
            set => graphRunner.smoothLandmarks = value;
        }

        public bool enableSegmentation
        {
            get => graphRunner.enableSegmentation;
            set => graphRunner.enableSegmentation = value;
        }

        public bool smoothSegmentation
        {
            get => graphRunner.smoothSegmentation;
            set => graphRunner.smoothSegmentation = value;
        }

        public float minDetectionConfidence
        {
            get => graphRunner.minDetectionConfidence;
            set => graphRunner.minDetectionConfidence = value;
        }

        public float minTrackingConfidence
        {
            get => graphRunner.minTrackingConfidence;
            set => graphRunner.minTrackingConfidence = value;
        }

        public override void SetupScreen(ImageSource imageSource)
        {
            base.SetupScreen(imageSource);
        }

        protected override void OnStartRun()
        {
            if (!runningMode.IsSynchronous())
            {
                graphRunner.OnPoseDetectionOutput += OnPoseDetectionOutput;
                graphRunner.OnPoseLandmarksOutput += OnPoseLandmarksOutput;
                graphRunner.OnPoseWorldLandmarksOutput += OnPoseWorldLandmarksOutput;
            }
            var imageSource = ImageSourceProvider.ImageSource;
            SetupAnnotationController(_poseLandmarksAnnotationController, imageSource);
        }

        protected override void AddTextureFrameToInputStream(TextureFrame textureFrame)
        {
            graphRunner.AddTextureFrameToInputStream(textureFrame);
        }

        protected override IEnumerator WaitForNextValue()
        {
            var task = graphRunner.WaitNextAsync();
            yield return new WaitUntil(() => task.IsCompleted);

            var result = task.Result;
            _poseLandmarksAnnotationController.DrawNow(result.poseLandmarks);
            result.segmentationMask?.Dispose();
        }

        private void OnPoseDetectionOutput(object stream, OutputStream<Detection>.OutputEventArgs eventArgs)
        {
            var packet = eventArgs.packet;
            var value = packet == null ? default : packet.Get(Detection.Parser);
        }

        private void OnPoseLandmarksOutput(object stream,
            OutputStream<NormalizedLandmarkList>.OutputEventArgs eventArgs)
        {
            var packet = eventArgs.packet;
            var value = packet == null ? default : packet.Get(NormalizedLandmarkList.Parser);
            _poseLandmarksAnnotationController.DrawLater(value);
        }

        private void OnPoseWorldLandmarksOutput(object stream, OutputStream<LandmarkList>.OutputEventArgs eventArgs)
        {
            var packet = eventArgs.packet;
            var value = packet == null ? default : packet.Get(LandmarkList.Parser);
            _mediapipe2UnitySkeletonController.Refresh(value);
        }
    }
}
