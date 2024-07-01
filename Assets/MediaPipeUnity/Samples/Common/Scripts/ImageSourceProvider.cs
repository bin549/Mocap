namespace Mediapipe.Unity.Sample
{
    public static class ImageSourceProvider
    {
        private static WebCamSource _WebCamSource;
        private static VideoSource _VideoSource;

        public static ImageSource ImageSource { get; private set; }

        public static ImageSourceType CurrentSourceType
        {
            get
            {
                if (ImageSource is WebCamSource)
                {
                    return ImageSourceType.WebCamera;
                }
                if (ImageSource is VideoSource)
                {
                    return ImageSourceType.Video;
                }
                return ImageSourceType.Unknown;
            }
        }

        internal static void Initialize(WebCamSource webCamSource, VideoSource videoSource)
        {
            _WebCamSource = webCamSource;
            _VideoSource = videoSource;
        }

        public static void Switch(ImageSourceType imageSourceType)
        {
            switch (imageSourceType)
            {
                case ImageSourceType.WebCamera:
                {
                    ImageSource = _WebCamSource;
                    break;
                }
                case ImageSourceType.Video:
                {
                    ImageSource = _VideoSource;
                    break;
                }
                case ImageSourceType.Unknown:
                default:
                {
                    throw new System.ArgumentException($"Unsupported source type: {imageSourceType}");
                }
            }
        }
    }
}
