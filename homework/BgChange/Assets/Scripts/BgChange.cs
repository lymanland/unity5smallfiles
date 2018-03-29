using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BgChange : MonoBehaviour {
    private Image bgShow;
    private Image bgHide;

    public ResLoader.LoadTypes loadType;
    public bool isLoadAsync = false;

    private string [] picNames = new string[]  { "Desert", "Jellyfish", "Lighthouse", "Penguins", "Tulips" };
    private int curPicIndex =  0;

    public float showTimeSec = 6;
    private float showTimeTrigger = 0;
    public float fadeTimeSec = 3;
    private float fadeTimeTrigger = 0;

    void Awake()
    {
        bgShow = transform.Find("ImageBG_Show").GetComponent<Image>();
        bgHide = transform.Find("ImageBG_Hide").GetComponent<Image>();
    }

    // Use this for initialization
    void Start () {
        showTimeTrigger = showTimeSec;
        fadeTimeTrigger = fadeTimeSec;

        DoLoadhandle();//写错函数LoadResource 每次以一次都默认方式，还以为loadType有问题
    }

    // Update is called once per frame
    void Update()
    {
        float alpha;
        if (showTimeTrigger < showTimeSec)
        {
            showTimeTrigger += Time.deltaTime;
            alpha = showTimeTrigger / showTimeSec;
            bgShow.color = new Color(1, 1, 1, alpha);
        }

        if (fadeTimeTrigger < fadeTimeSec)
        {
            fadeTimeTrigger += Time.deltaTime;
            alpha = 1 - (fadeTimeTrigger / fadeTimeSec);
            bgHide.color = new Color(1, 1, 1, alpha);
        }
    }

    // 使用资源
    void LoadRspnHandle(Sprite loadedAsset)
    {
        Debug.Log("LoadRspnHandle##:" + loadedAsset);
        bgHide.sprite = bgShow.sprite;

        bgShow.color = new Color(1, 1, 1, 0);
        bgShow.sprite = loadedAsset;

        showTimeTrigger = 0;
        fadeTimeTrigger = 0;
    }

    public void OnPreCLick()
    {
        curPicIndex = (curPicIndex - 1 + picNames.Length) % picNames.Length;
        DoLoadhandle();
    }

    public void OnNextCLick()
    {
        curPicIndex = (curPicIndex + 1 + picNames.Length) % picNames.Length;
        DoLoadhandle();
    }

    void DoLoadhandle()
    {
        string resourcePath = GetResourcePath();

        if (isLoadAsync)
        {
            ResLoader.Instance.LoadAsync(resourcePath, loadType, LoadRspnHandle);
        }
        else
        {
            Sprite loadedAsset = ResLoader.Instance.Load(resourcePath, loadType);
            // 使用资源
            LoadRspnHandle(loadedAsset);
        }
    }

    string GetResourcePath()
    {
        //不需要扩展名

        //开始没有建Resources文件夹 Resources文件夹需要用户自己新建，可以放在Asset文件夹里任意层级的子目录中，
        //多加了前缀“Resources”显示不出
        string resourcePath = "" + picNames[curPicIndex];//1开始没有建Resources文件夹 2多加了前缀“Resources”显示不出
        Debug.Log("resourcePath:" + resourcePath);
        return resourcePath;
    }
}
