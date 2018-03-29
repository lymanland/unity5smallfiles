using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoaderBase : MonoBehaviour {
    public delegate void Loadhandle(Sprite loadedAsset);
    protected Loadhandle onLoadedFunc;

    protected string resourcePath;

    public bool isLoadBuddle = true;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    virtual public Sprite Load(string resourcePath)
    {
        this.resourcePath = resourcePath;

        return null;
    }

    virtual public void LoadAsync(string resourcePath, Loadhandle onLoadedFunc)
    {
        this.resourcePath = resourcePath;
        this.onLoadedFunc = onLoadedFunc;
    }

    virtual public void StopLoad()
    {
        StopAllCoroutines();
    }

    public string getUrlName(string resourcePath)
    {
        string sPath;
        if (isLoadBuddle)
        {
            sPath = Application.streamingAssetsPath + "/" + resourcePath;//文件assetbuddle
        }
        else
        {
            //错误方式sPath = Application.dataPath + "Assets/Resources/" + resourcePath + ".jpg";
            //"BgChange/AssetsAssets/Resources/Desert.jpg"
            sPath = Application.dataPath + "/Resources/" + resourcePath + ".jpg";
        }

        string fullPath = "file://" + sPath;
        Debug.Log("getUrlName：" + fullPath);
        return fullPath;
    }

    public Sprite CreateSprite(byte[] data, int width = 400, int height = 400)
    {
        Texture2D tex = new Texture2D(width, height);
        tex.LoadImage(data);
        //www.LoadImageIntoTexture(tex);
        Sprite obj = Sprite.Create(tex, new Rect(0, 0, width, height), Vector2.zero);
        return obj;
    }

    public Sprite CreateEmptySprite(int width = 400, int height = 400)
    {
        Texture2D tex = new Texture2D(width, height);
        Sprite obj = Sprite.Create(tex, new Rect(0, 0, width, height), Vector2.zero);
        return obj;
    }

    public Sprite CreateSpriteWWW(WWW www, int width = 400, int height = 400)
    {
        Sprite obj;

        Texture2D tex = new Texture2D(width, height);
        tex.LoadImage(www.bytes);
        //www.LoadImageIntoTexture(tex);
        obj = Sprite.Create(tex, new Rect(0, 0, width, height), Vector2.zero);

        ///**
        //Texture2D 方式一
        tex.LoadImage(www.bytes);
        //www.LoadImageIntoTexture(tex);
        obj = Sprite.Create(tex, new Rect(0, 0, width, height), Vector2.zero);
        // */

        /****
        //Texture2D 方式二
        Texture2D tex = www.texture;
        obj = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
        */

        return obj;
    }
}
