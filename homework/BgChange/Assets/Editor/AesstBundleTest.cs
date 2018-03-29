using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class AesstBundleTest: MonoBehaviour {
    [MenuItem("Custom Editor/Create AssetBunldes ALL")]
    static void CreateAssetBunldesALL()
    {
        BuildPipeline.BuildAssetBundles("Assets/StreamingAssets", BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
    }

    [MenuItem("Custom Editor/Create AssetBunldes ALL2")]
    static void CreateAssetBunldesALL2()
    {
        // Create the array of bundle build details.
        AssetBundleBuild[] buildMap = new AssetBundleBuild[1];

        buildMap[0].assetBundleName = "enemybundle";
        string[] enemyAssets = new string[2];
        enemyAssets[0] = "Assets/Resources/Desert.jpg";
        enemyAssets[1] = "Assets/Resources/Jellyfish.jpg";
        buildMap[0].assetNames = enemyAssets;

        BuildPipeline.BuildAssetBundles("Assets/StreamingAssets", buildMap, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
    }
}
