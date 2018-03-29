using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor; // 这个文件在手机上没有，需要使用条件编译
#endif

public class AssetDataBaseLoader : LoaderBase
{
    override public Sprite Load(string resourcePath)
    {
        this.resourcePath = resourcePath;
        Debug.Log("LoadAssetDataBase！");

        Sprite loadedAsset;
        string fullPath = "Assets/Resources/" + resourcePath + ".jpg";
        // 加载资源
        //loadedAsset = AssetDatabase.LoadAssetAtPath(fullPath, typeof(Sprite)) as Sprite;
        loadedAsset = AssetDatabase.LoadAssetAtPath<Sprite>(fullPath);

        //备注需要后缀名，开始所以显示不出来

        // 使用资源
        //FageBg(loadedAsset);
        // 卸载资源：注意，卸载方法是在Resources类中
        //Resources.UnloadAsset(loadedAsset);
        // 调用GC来清理垃圾，资源不是立刻就被清掉的

        return loadedAsset;
    }

    override public void LoadAsync(string resourcePath, Loadhandle onLoadedFunc)
    {
        this.resourcePath = resourcePath;
        this.onLoadedFunc = onLoadedFunc;
        StartCoroutine(GetAssetDataBaseRes());
    }

    IEnumerator GetAssetDataBaseRes()
    {
        yield return null;
        Sprite loadedAsset = Load(resourcePath);
        onLoadedFunc(loadedAsset);
    }
}
