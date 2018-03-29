using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class UnityWebRequestLoader : LoaderBase
{
    private readonly int MaxCountRetry = 100*10000;//重复下载次数
    private int curCountRetry;

    override public Sprite Load(string resourcePath)
    {
        this.resourcePath = resourcePath;
        curCountRetry = MaxCountRetry;

        string url = getUrlName(resourcePath);
        UnityWebRequest request;
        if (isLoadBuddle)
        {
            request = UnityWebRequest.GetAssetBundle(url);
        }
        else
        {
            request = UnityWebRequest.Get(url);
        }

        request.Send();

        while (curCountRetry > 0 && (!request.isDone))
        {
            curCountRetry--;
        }

        Sprite obj = null;
        Debug.Log("[UnityWebRequestLoader] request:"+ request + " request.isDone:"+ request.isDone + " curCountRetry:"+ curCountRetry);
        if (request!= null && request.isDone)
        {
            obj = HandleUnityWebRequestRspn(request);
        }
        return obj;
    }
    override public void LoadAsync(string resourcePath, Loadhandle onLoadedFunc)
    {
        this.resourcePath = resourcePath;
        this.onLoadedFunc = onLoadedFunc;

        Debug.Log("LoadUnityWebRequest！");
        StartCoroutine(GetUnityWebRequest());
    }

    IEnumerator GetUnityWebRequest()
    {
        string url = getUrlName(resourcePath);
        UnityWebRequest request;
        if (isLoadBuddle)
        {
            request = UnityWebRequest.GetAssetBundle(url);
        }
        else
        {
            request = UnityWebRequest.Get(url);
        }
        //request.method = UnityWebRequest.kHttpVerbGET;  

        yield return request.Send();

        Debug.Log("[UnityWebRequestLoader Asyn] request:" + request + " request.isDone:" + request.isDone);

        Sprite obj = null;
        obj = HandleUnityWebRequestRspn(request);
        onLoadedFunc(obj);
    }

    Sprite HandleUnityWebRequestRspn(UnityWebRequest request)
    {
        if (request.isError)
        {
            Debug.Log(request.error);
            return null;
        }

        Sprite obj = null;

        Debug.Log("[HandleUnityWebRequestRspn] request.isDone:" + request.isDone);

        AssetBundle bundle = null;
        if (isLoadBuddle)
        {

            DownloadHandlerAssetBundle down = (DownloadHandlerAssetBundle)request.downloadHandler;
            bundle = down.assetBundle;

            obj = bundle.LoadAsset<Sprite>(resourcePath);
            Debug.Log("GetUnityWebRequest --[Buddle] obj:" + obj);
        }
        else
        {
            //if (request.responseCode == 200)
            //{
            //    string text = request.downloadHandler.text;
            //    byte[] results = request.downloadHandler.data;
            //}

            obj = CreateSprite(request.downloadHandler.data);
            Debug.Log("GetUnityWebRequest --[Sprite.Create] obj:" + obj);
        }

        //onLoadedFunc(obj);

        if (bundle != null)
        {
            bundle.Unload(false);
        }

        return obj;
    }
}
