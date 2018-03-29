using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResLoader
{
    public enum LoadTypes
    {
        TypeResource, TypeWWW, TypeUnityWebRequest, TypeAssetDataBase
    }

    private static ResLoader instance = null;
    public static ResLoader Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ResLoader();
            }
            return instance;
        }
    }

    private LoaderBase curLoader;

    public Sprite Load(string resourcePath, LoadTypes loadType)
    {
        this.curLoader = GetLoader(loadType);

        Sprite loadedAsset;
        loadedAsset = curLoader.Load(resourcePath);
        return loadedAsset;
    }

    public void LoadAsync(string resourcePath, LoadTypes loadType, LoaderBase.Loadhandle onLoadedFunc)
    {
        this.curLoader = GetLoader(loadType);

        curLoader.LoadAsync(resourcePath, onLoadedFunc);
    }

    void StopCurLoader()
    {
        if (curLoader != null)
        {
            curLoader.StopLoad();
        }
    }

    public LoaderBase GetLoader(LoadTypes loadType)
    {
        LoaderBase loader = null;

        Debug.Log("[GetLoader] loadType:" + loadType);
        switch (loadType)
        {
            case LoadTypes.TypeResource:
                loader = GetLoaderComponent<ResourcesLoader>();
                break;
            case LoadTypes.TypeWWW:
                loader = GetLoaderComponent<WWWLoader>();
                break;
            case LoadTypes.TypeUnityWebRequest:
                loader = GetLoaderComponent<UnityWebRequestLoader>();
                break;
            case LoadTypes.TypeAssetDataBase:
                loader = GetLoaderComponent<AssetDataBaseLoader>();
                break;
        }

        return loader;
    }

    public T GetLoaderComponent<T>() where T : LoaderBase
    {
        return SingletonTemplate<T>.Instance;

        //T loader = gameObject.GetComponent<T>();
        //if (loader == null) loader = gameObject.AddComponent<T>();
        //return loader;
    }
}
   
