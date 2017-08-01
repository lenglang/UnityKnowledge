using UnityEngine;
using UnityEditor;
    public class BuildPipelineHelper
    {
        public static string BuildPlayer(string[] levels, string locationPathName, BuildTarget target, BuildOptions options)
        {
            if (EditorUserBuildSettings.activeBuildTarget != target)
            {
                Debug.LogError("请先切换平台至 " + target);
                return "";
            }

            return BuildPipeline.BuildPlayer(levels, locationPathName, target, options);
        }

        public static AssetBundleManifest BuildAssetBundles(string outputPath, AssetBundleBuild[] builds, BuildAssetBundleOptions assetBundleOptions, BuildTarget targetPlatform)
        {
            if (EditorUserBuildSettings.activeBuildTarget != targetPlatform)
            {
                Debug.LogError("请先切换平台至 " + targetPlatform);
                return null;
            }

            return BuildPipeline.BuildAssetBundles(outputPath, builds, assetBundleOptions, targetPlatform);
        }
    }