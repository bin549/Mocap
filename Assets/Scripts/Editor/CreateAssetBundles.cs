using System.IO;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

public class CreateAssetBundles
{
    [MenuItem("Assets/Build AssetBundles")]
    internal static void BuildAllAssetBundles()
    {
        var assetBundleDirectory = Application.streamingAssetsPath;
        if (!Directory.Exists(assetBundleDirectory))
        {
            var _ = Directory.CreateDirectory(assetBundleDirectory);
        }

        _ = BuildPipeline.BuildAssetBundles(assetBundleDirectory,
            BuildAssetBundleOptions.None,
            EditorUserBuildSettings.activeBuildTarget);
    }
}

#endif
