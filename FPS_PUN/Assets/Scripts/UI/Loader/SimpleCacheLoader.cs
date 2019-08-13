using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// 目前只有Json
/// </summary>
public class SimpleCacheLoader : SimpleLoader
{
#if UNITY_STANDALONE_WIN || UNITY_EDITOR || UNITY_WEBPLAYER
    static public readonly string LocalURL = "file:///" + Application.persistentDataPath + "/";
#elif UNITY_ANDROID   //安卓
    static public readonly string LocalURL = "jar:file://" + Application.persistentDataPath + "/";
#elif UNITY_IPHONE  //iPhone
	static public readonly string LocalURL =  "file://" + Application.persistentDataPath + "/";
#else
    static public readonly string LocalURL =string.Empty;
#endif
    static public readonly string LocalWriteURL = Application.persistentDataPath + "/";

    private string id;
    private int tempId;
    private bool isTemped = false;

    public override string url
    {
        get
        {
            if (isTemped == true)
            {
                if (type == SimpleLoadDataType.JsonScheme)
                {
                    return schemeCache.GetLoadUrlByID(tempId.ToString());
                }
                else if (type == SimpleLoadDataType.JsonOffer)
                {
                    return localOfferCache.GetLoadUrlByID(tempId.ToString());
                }
            }
            else
            {
                if (type == SimpleLoadDataType.JsonScheme)
                {
                    return schemeCache.GetLoadUrlByID(id);
                }
                else if (type == SimpleLoadDataType.JsonOffer)
                {
                    return serverOfferCache.GetLoadUrlByID(id);
                }
            }
            return "";
        }
    }
    
    private CacheSchemeManager schemeCache
    {
        get
        {
            return CacheSchemeManager.Instance;
        }
    }

    private CacheLocalOfferManager localOfferCache
    {
        get
        {
            return CacheLocalOfferManager.Instance;
        }
    }

    private CacheServerOfferManager serverOfferCache
    {
        get
        {
            return CacheServerOfferManager.Instance;
        }
    }

    public SimpleCacheLoader(string id, SimpleLoadDataType type, Action<object> onloaded, object bringdata)
    {
        isTemped = false;
        this.id = id;
        this.type = type;
        this.onloaded = onloaded;
        this.bringData = bringdata;
        state = SimpleLoadedState.None;
    }

    public SimpleCacheLoader(int tempId, SimpleLoadDataType type, Action<object> onloaded, object bringData)
    {
        isTemped = true;
        this.tempId = tempId;
        this.type = type;
        this.onloaded = onloaded;
        this.bringData = bringData;
        state = SimpleLoadedState.None;
    }
    
    public override void StartLoad()
    {
        base.StartLoad();
        //加载的Json不保存
        //ResourcesPool.LoadPoolData data;
        //if (resourcePool.TryGet(url, out data))
        //{
        //    data.newUseTime = Time.realtimeSinceStartup;
        //    MyCallLater.Add(LoadInPool, 0, data);
        //    return;
        //}
        MyMono.MyStartCoroutine(LoadCache, this, url);
    }

    protected override void stopLoad()
    {
        MyMono.MyStopCoroutine(LoadCache, this, url);
        base.stopLoad();
    }

    private IEnumerator LoadCache(object[] arg1)
    {
        string url = arg1[0].ToString();
        www = new WWW(url);
        yield return www;
        if (string.IsNullOrEmpty(www.error) == false)
        {
            Debug.LogError(www.error);
            state = SimpleLoadedState.Failed;
        }
        else {
            string json = System.Text.Encoding.UTF8.GetString(www.bytes);
            int index = json.IndexOf("{");
            if (index != 0)
            {
                json = json.Substring(index);
            }
            //同一个Json可能会变化 不记录上次的 因为有可能是过时的
            //if(localHas == false && string.IsNullOrEmpty(uri) == false) SaveToLocal(www.bytes);
            loadedData = json;
            state = SimpleLoadedState.Success;
        }

        OnLoaded();
    }
}
