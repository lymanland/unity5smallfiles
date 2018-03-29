using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LuaInterface;

namespace AQ
{
    public class Test : MonoBehaviour
    {
        static Test _instance;
        public static Test Instance
        {
            get
            {
                return _instance;
            }
        }

        private void Awake()
        {
            _instance = this;
        }

        private void Start()
        {
            TestFunc("prefabs/ui/petpackage", "PetPackageMain", null);
        }

        public void TestFunc(string abName, string assetName, LuaFunction func)
        {
            StartCoroutine(_TestFunc(abName, assetName));
        }

        IEnumerator _TestFunc(string abName, string assetName)
        {
            var realTime = System.DateTime.Now.Hour * 3600 + System.DateTime.Now.Minute * 60 + System.DateTime.Now.Second;
            Debug.Log("realTime:" + realTime);
            string[] strs = {
                            "E:/GitProject/WinGameClient/AssetBundles/Windows/shaders",
                            "E:/GitProject/WinGameClient/AssetBundles/Windows/postprcessing",
                            "E:/GitProject/WinGameClient/AssetBundles/Windows/material/ui",
                            "E:/GitProject/WinGameClient/AssetBundles/Windows/font/chinese",
                            "E:/GitProject/WinGameClient/AssetBundles/Windows/texture/common",
                            "E:/GitProject/WinGameClient/AssetBundles/Windows/texture/uiatlas/petpackage",
                            "E:/GitProject/WinGameClient/AssetBundles/Windows/texture/uiatlas/common",
                            "E:/GitProject/WinGameClient/AssetBundles/Windows/texture/uiimage/petpackage",
                            "E:/GitProject/WinGameClient/AssetBundles/Windows/texture/uiatlas/energy",
                            "E:/GitProject/WinGameClient/AssetBundles/Windows/texture/uiatlas/dynamic/family/rect_98_98",
                            "E:/GitProject/WinGameClient/AssetBundles/Windows/texture/uiatlas/dynamic/role/body_rect_640_720",
                            "E:/GitProject/WinGameClient/AssetBundles/Windows/texture/uiatlas/dynamic/family/rect_72_72",
                            "E:/GitProject/WinGameClient/AssetBundles/Windows/texture/uiatlas/dynamic/genius/word",
                            "E:/GitProject/WinGameClient/AssetBundles/Windows/texture/uiatlas/dynamic/rarity/petpackage",
                            "E:/GitProject/WinGameClient/AssetBundles/Windows/models/ornament/petpackage"};
            WWW[] ws = new WWW[strs.Length];

            string url = Application.dataPath.Replace("Assets", "AssetBundles/Windows/") + abName;
            Debug.Log(url);
            url = "file://" + url;
            WWW www = WWW.LoadFromCacheOrDownload(url, realTime 
                + strs.Length);
            yield return www;
            if (string.IsNullOrEmpty(www.error) == false)
            {
                Debug.Log(www.error);
                yield break;
            }
            for (int i = 0; i < strs.Length; i++)
            {
                var name = strs[i];
                Debug.Log(name);
                WWW tmp = WWW.LoadFromCacheOrDownload("file://" + name, i+realTime);
                ws[i] = tmp;
                yield return tmp;
                if (string.IsNullOrEmpty(tmp.error) == false)
                {
                    Debug.Log(www.error);
                    yield break;
                }
            }
            yield return null;
            GameObject prefab = www.assetBundle.LoadAsset<GameObject>(assetName);
            GameObject root = GameObject.Find("SCENE");
            Instantiate<GameObject>(prefab, root.transform);
            //yield return new WaitForSeconds(1);
            yield return null;
            foreach(var w in ws)
            {
                w.assetBundle.Unload(false);
            }
            www.assetBundle.Unload(false);
        }
    }
}

