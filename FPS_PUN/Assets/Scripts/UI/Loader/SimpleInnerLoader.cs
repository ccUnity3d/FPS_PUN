using UnityEngine;
using System.Collections;
using System;

public class SimpleInnerLoader : SimpleLoader {

    static public readonly string StreamingAssetsURL =
#if UNITY_STANDALONE_WIN || UNITY_EDITOR || UNITY_WEBPLAYER
                "file://" + Application.dataPath + "/StreamingAssets/";
#elif UNITY_ANDROID   //安卓
                "jar:file://" + Application.dataPath + "!/assets/";

#elif UNITY_IPHONE  //iPhone
		        "file://" + Application.dataPath + "/Raw/";
#else
                string.Empty;
#endif

    private string ur =
#if UNITY_STANDALONE_WIN || UNITY_EDITOR || UNITY_WEBPLAYER
        "Web/";
#elif UNITY_ANDROID   //安卓
        "Android/";
#elif UNITY_IPHONE  //iPhone
        "IOS/";
#else
        "";
#endif

    public override string url
    {
        get {
            return StreamingAssetsURL + ur + uri;
        }
    }

    public SimpleInnerLoader(string uri, SimpleLoadDataType type, Action<object> onloaded, object bringData)
    {
        this.uri = uri;
        this.type = type;
        this.onloaded = onloaded;
        this.bringData = bringData;
        state = SimpleLoadedState.None;
    }

    public override void StartLoad()
    {
        base.StartLoad();
        MyMono.MyStartCoroutine(load, this, null);
    }

    protected override void stopLoad()
    {
        MyMono.MyStopCoroutine(load, this, null);
        base.stopLoad();
    }


    private IEnumerator load(object[] arg1)
    {
        Debug.LogWarning("IEnumerator load:" + uri);

        www = new WWW(url);

        if (checkProgress)
        {
            MyTickManager.Add(Progress);
        }
        yield return www;
        if (checkProgress)
        {
            MyTickManager.Remove(Progress);
        }

        //if (www == null)
        //{
        //    state = SimpleLoadedState.Failed;
        //    loadedData = null;
        //    string message = string.Format("加载文件失败:{0}", url);
        //    Debug.Log(message);
        //    error = true;
        //}
        //else {
            if (string.IsNullOrEmpty(www.error))

            {
                string realName = System.IO.Path.GetFileNameWithoutExtension(url);
                AssetBundle bundle = www.assetBundle;
                state = SimpleLoadedState.Success;
                switch (type)
                {
                    case SimpleLoadDataType.prefabAssetBundle:
                        UnityEngine.Object[] objs = bundle.LoadAllAssets();
                        for (int i = 0; i < objs.Length; i++)
                        {
                            if (objs[i].GetType() == typeof(GameObject)) {
                                if (objs[i].name == realName)
                                {
                                    UnityEngine.Object data = objs[i];
                                    ResourcesPool.PrefabData prefab = resourcePool.addPrefab(keyUrl, data, true);
                                    if (canceled == false) loadedData = prefab.GetNew();
                                }
                            }
                        }
                        bundle.Unload(false);
                        break;
                    case SimpleLoadDataType.texture2D:
                        if (bundle != null)
                        {
                            loadedData = bundle.LoadAsset<Texture2D>(realName);
                            resourcePool.addTexture(keyUrl, loadedData, true);
                            bundle.Unload(false);
                        }
                        else
                        {
                            loadedData = www.texture;
                            resourcePool.addTexture(keyUrl, loadedData, true);
                        }
                        break;
                    case SimpleLoadDataType.Json:
                        if (bundle != null)
                        {
                            loadedData = bundle.mainAsset;
                            resourcePool.addJson(keyUrl, loadedData, true);
                            bundle.Unload(true);
                        } 
                        else
                        {
                            loadedData = www.text;
                            resourcePool.addJson(keyUrl, loadedData, true);
                        }
                        break;
                    case SimpleLoadDataType.Byte:
                    default:
                        if (bundle != null)
                        {
                            bundle.Unload(true);
                        }
                        loadedData = null;
                        string message = string.Format("加载文件类型：{0} 失败:", type);
                        Debug.Log(message);
                        break;
                }
            }
            else
            {
                state = SimpleLoadedState.Failed;
                loadedData = null;
                string message = string.Format("加载文件失败:{0}", url);
                Debug.Log(message);
            }
        //}
        OnLoaded();
    }

}
