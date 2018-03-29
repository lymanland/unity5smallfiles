using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesLoader : LoaderBase
{
    override public Sprite Load(string resourcePath)
    {
        this.resourcePath = resourcePath;

        Sprite loadedAsset;

        //loadedAsset = Resources.Load(resourcePath) as Sprite;//?????后面两种都可以了，这种还不能显示
        //Debug.Log("loadedAsset:" + loadedAsset);
        //bg.sprite = loadedAsset;

        //loadedAsset = Resources.Load(resourcePath, typeof(Sprite)) as Sprite;
        //Debug.Log("loadedAsset:" + loadedAsset);
        //bg.sprite = loadedAsset;

        loadedAsset = Resources.Load<Sprite>(resourcePath);
        Debug.Log("loadedAsset:" + loadedAsset);

        return loadedAsset;
    }

    override public void LoadAsync(string resourcePath, Loadhandle onLoadedFunc)
    {
        this.resourcePath = resourcePath;
        this.onLoadedFunc = onLoadedFunc;
        StartCoroutine(LoadResourceAsync());
    }

    IEnumerator LoadResourceAsync()
    {
        Sprite loadedAsset;
        ResourceRequest request;
        //request = Resources.LoadAsync(resourcePath);//?????后面两种都可以了，这种还不能显示
        //request = Resources.LoadAsync(resourcePath, typeof(Sprite));
        request = Resources.LoadAsync<Sprite>(resourcePath);
        yield return request;

        Debug.Log("异步加载Resource完成了！");
        loadedAsset = request.asset as Sprite;
        if (!loadedAsset)
        {
            Debug.Log("异步加载Resource显示失败！");
        }
        else
        {
            onLoadedFunc(loadedAsset);
        }
    }
}
