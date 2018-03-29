using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WWWLoader : LoaderBase
{
    private WWW www;

    private readonly int MaxCountRetry = 10;//重复下载次数
    private int curCountRetry;

    private Sprite loadedAsset;
    override public Sprite Load(string resourcePath)
    {
        this.resourcePath = resourcePath;
        Debug.Log("WWWLoader Load!");
        curCountRetry = MaxCountRetry;

        www = new WWW(getUrlName(resourcePath));
        while (curCountRetry > 0 && (www == null || !www.isDone) )
        {
            curCountRetry--;
        }
        Debug.Log("www:"+ www + (www!=null ? " www.isDone:" + www.isDone : "") + " curCountRetry:" + curCountRetry);

        loadedAsset = HandleLoadWWWRspn(www);

        //尝试方式 失败
        //loadedAsset = CreateEmptySprite();
        //LoadAsync(resourcePath, LoadHandleSync);

        //loadedAsset = LoadWayWWW();

        disposeWWW();
        return loadedAsset;
    }

    //void LoadHandleSync(Sprite obj)
    //{
    //    Debug.Log("obj:" + obj);
    //    loadedAsset.texture.LoadRawTextureData(obj.texture.GetRawTextureData());
    //}

    Sprite LoadWayWWW()
    {
        Sprite loadedAsset = null;
        UnityEngine.Object result = null;

        while (result == null)
        {
            WWWColloection wwwColloection = LoadWWW() as WWWColloection;
            if (wwwColloection != null)
            {
                result = (wwwColloection.Current as WWW).assetBundle;
            }
            //foreach (UnityEngine.Object obj in LoadWWW())
            //{
            //    result = obj;
            //}
            if (curCountRetry > 0)
            {
                if (result != null)
                {
                    break;
                }
                curCountRetry--;
            }
            else
            {
                break;
            }
        }
        if (result != null)
        {
            AssetBundle bundle = result as AssetBundle;
            loadedAsset = bundle.LoadAsset<Sprite>(resourcePath);
        }
        return loadedAsset;
    }
    override public void LoadAsync(string resourcePath, Loadhandle onLoadedFunc)
    {
        this.resourcePath = resourcePath;
        this.onLoadedFunc = onLoadedFunc;

        StartCoroutine(LoadWWWAsync());
    }

    IEnumerator LoadWWWAsync()
    {
        www = new WWW(getUrlName(resourcePath));

        yield return WWWColloection.createWWWColloection(www);
        //yield return www;

        Sprite loadAssets = HandleLoadWWWRspn(www);
        if (loadAssets != null)
        {
            onLoadedFunc(loadAssets);
        }
    }

    Sprite HandleLoadWWWRspn(WWW www)
    {
        if (www.error != null)
        {
            Debug.LogError("Load Bundle Faile Error Is " + www.error);
            //yield break;
            return null;
        }

        Sprite obj;
        AssetBundle bundle = null;
        if (isLoadBuddle)
        {
            //AssetBundle方式

            bundle = www.assetBundle;
            string picName = resourcePath;

            //obj = bundle.LoadAsset<Sprite>("Desert");
            obj = bundle.LoadAsset<Sprite>(picName);
            Debug.Log("LoadBundleWWW--[Buddle]-- obj:" + obj);
            //obj = bundle.LoadAsset<Sprite>("desert");//名字大小写无关？？？？？
            //Debug.Log("LoadBundleWWW --[Buddle]-- obj:" + obj);
        }
        else
        {
            obj = CreateSprite(www.bytes);
            Debug.Log("LoadBundleWWW--[www.bytes]-- obj:" + obj);
        }

        //onLoadedFunc(obj);

        if (bundle != null)
        {
            bundle.Unload(false);//AssetBundle方式需要调用
        }
        return obj;
    }
    public IEnumerable LoadWWW()
    {
        Debug.Log("IEnumerable LoadWWW");
        www = new WWW(getUrlName(resourcePath));
        yield return WWWColloection.createWWWColloection(www);
        //yield return www.assetBundle;
    }

    private void disposeWWW()
    {
        if (www != null)
        {
            www.Dispose();
        }
        www = null;
    }
}

public class WWWColloection : IEnumerator
{
    private WWW www;

    public WWWColloection(WWW www)
    {
        this.www = www;
    }

    public static WWWColloection createWWWColloection(WWW www)
    {
        Debug.Log("createWWWColloection www:"+ www);
        if (www == null) return null;
        return new WWWColloection(www);
    }
    public bool MoveNext()
    {
        return false;
    }

    public void Reset()
    {
    }
    public object Current
    {
        get
        {
            return www;
        }
    }
}
