using UnityEngine;
using System.Collections;
using System;

public class SimpleOutterLoader : SimpleLoader {

    static public readonly string serverURL = "http://midea-prod-assets.s3.cn-north-1.amazonaws.com.cn/";
    static private readonly string modelName = "product";//以后的打包都是product.prefab打包成model.assetBoundle
#if UNITY_STANDALONE_WIN || UNITY_EDITOR || UNITY_WEBPLAYER
    static public readonly string LocalURL = "file:///" + Application.persistentDataPath + "/";
#elif UNITY_ANDROID   //安卓
    static public readonly string LocalURL = "jar:file://" + Application.persistentDataPath + "/";
#elif UNITY_IPHONE  //iPhone
	static public readonly string LocalURL =  "file://" + Application.persistentDataPath + "/";
#else
    static public readonly string LocalURL =string.Empty;
#endif

    public string httpUrl;
    public override string url
    {
        get {
            if (localHas)
            {
                return localloadurl;
            }
            return httpUrl;
        }
    }

    public string localloadurl
    {
        get
        {
            return LocalURL + uri;
        }
    }
   
    public override string keyUrl
    {
        get
        {
            return httpUrl;
        }
    }
    private ICache cacheManager
    {
        get
        {
            switch (souceType)
            {
                case SouceType.AssetBoundle:
                    return CacheModelManager.Instance;
                case SouceType.TopView:
                    return CacheTopViewManager.Instance;
                case SouceType.MaterialTexture:
                    return CacheMaterialTextureManager.Instance;
                case SouceType.ResizedImage:
                    return CacheResizedImageManager.Instance;
                case SouceType.Default:
                default:
                    return null;
            }
        }
    }

    private bool localHas = true;
    private SouceType souceType = SouceType.Default;
    public Action<GameObject, object> OnLoadprefabBeforClone;
    internal bool justLoad = false;

    public SimpleOutterLoader(string url, SimpleLoadDataType type, Action<object> onloaded, object bringData = null, Action<GameObject, object> onloadedBforClone = null)
    {
        this.httpUrl = url;
        this.type = type;
        this.onloaded = onloaded;
        this.bringData = bringData;
        this.OnLoadprefabBeforClone = onloadedBforClone;
        state = SimpleLoadedState.None;
        this.uri = getUri(httpUrl);
    }

    private string getUri(string httpUrl)
    {
        string[] strs = httpUrl.Split('/');
        if (strs.Length < 2)
        {
            Debug.LogError("httpUrl = " + httpUrl + " httpUrl.Split('/').Length < 2 " + httpUrl);
            return "";
        }
        string resource = strs[strs.Length - 1];
        string fold = strs[strs.Length - 2] + "/";

        string[] str2s = resource.Split('.');
        if (str2s.Length < 2)
        {
            Debug.LogWarning("resource.Split('.').Length < 2 " + httpUrl);
            return "";
        }
        string ResourceName = resource;
        if (souceType == SouceType.Default)
        {
            string name = str2s[0];
            switch (name)
            {
                case "Top":
                    souceType = SouceType.TopView;
                    ResourceName = "top." + str2s[str2s.Length - 1];
                    break;
                case "top":
                    souceType = SouceType.TopView;
                    ResourceName = name + "." + str2s[str2s.Length - 1];
                    break;
                case "wallfloor":
                    souceType = SouceType.MaterialTexture;
                    ResourceName = name + "." + str2s[str2s.Length - 1];
                    break;
                case "model":
                    souceType = SouceType.AssetBoundle;
                    ResourceName = name + "." + str2s[str2s.Length - 1];
                    break;
                //case "iso":
                //case "thumbnail":
                //case "thumbnail_":
                //break;
                default:
                    if (name.IndexOf("thumbnail") != -1)
                    {
                        souceType = SouceType.ResizedImage;
                        ResourceName = "thumbnail." + str2s[str2s.Length - 1];
                    }
                    break;
            }
        }
        string ending = str2s[str2s.Length - 1];
        switch (ending)
        {
            case "png":
            //case "jpg":
            //    fold = "alluserdata/" + UnityIOSMsg.currentUser.uuid + "/Texture2D/" + fold;
            //    break;
            //case "assetbundle":
            //    fold = "alluserdata/" + UnityIOSMsg.currentUser.uuid + "/Assetbundle/" + fold;
            //    break;
            //case "midf":
            //    fold = "alluserdata/" + UnityIOSMsg.currentUser.uuid + "/OutMidf/" + fold;
            //    break;
            default:
                break;
        }
        return fold + ResourceName;
    }

    public override void StartLoad()
    {
        base.StartLoad();
        localHas = false;
        if (cacheManager != null)
        {
            localHas = cacheManager.HasCached(httpUrl);
            //Debug.LogError(httpUrl + " localHas == " + localHas);
        }
        MyMono.MyStartCoroutine(load, this, null);
    }


    protected override void stopLoad()
    {
        MyMono.MyStopCoroutine(load, this, null);
        base.stopLoad();
    }


    private IEnumerator load(object[] arg1)
    {
        //加载成功 但是加载到的数据不对
        bool loadedIsWrong = false;
        string path = url;
        path = path.Replace("midea-products.oss-cn-shanghai.aliyuncs.com/", "pms.3dshome.net/");
        Debug.LogWarning("OutterLoad:" + path);
        www = new WWW(path);

        if (checkProgress)
        {
            MyTickManager.Add(Progress);
        }
        yield return www;
        //yield return new WaitForSeconds(2);//模拟慢网速
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
        //}
        //else {
        if (string.IsNullOrEmpty(www.error))
        {
            state = SimpleLoadedState.Success;
            switch (type)
            {
                case SimpleLoadDataType.prefabAssetBundle:

                    //string realName = System.IO.Path.GetFileNameWithoutExtension(url);
                    AssetBundle bundle = www.assetBundle;
                    if (bundle != null)
                    {
                        UnityEngine.Object[] objs = bundle.LoadAllAssets();
                        for (int i = 0; i < objs.Length; i++)
                        {
                            if (objs[i].GetType() == typeof(GameObject))
                            {
                                if (objs[i].name == modelName)
                                {
                                    GameObject data = objs[i] as GameObject;
                                    data.name = uri;
                                    if (OnLoadprefabBeforClone != null) OnLoadprefabBeforClone(data, bringData);
                                    if (justLoad == false)
                                    {
                                        ResourcesPool.PrefabData prefab = resourcePool.addPrefab(httpUrl, data);
                                        if (canceled == false && justEndReturn == false) loadedData = prefab.GetNew();
                                    }
                                    //loadedData = objs[i];
                                    //resourcePool.addPrefab(url, loadedData);
                                }
                            }
                        }
                        if (localHas == false && string.IsNullOrEmpty(uri) == false)
                        {
                            if(cacheManager!=null) cacheManager.AddCache(httpUrl, www.bytes);
                        }
                        if (justLoad == false)
                        {
                            bundle.Unload(false);
                        }
                        else
                        {
                            bundle.Unload(true);
                        }
                    }
                    else
                    {
                        loadedIsWrong = true;
                        Debug.LogError("加载到的资源不是AssetBundle！path = " + www.url);
                    }
                    break;
                case SimpleLoadDataType.texture2D:
                    if (localHas == false && string.IsNullOrEmpty(uri) == false)
                    {
                        if (cacheManager != null) cacheManager.AddCache(httpUrl, www.bytes);
                    }
                    if (www.assetBundle != null)
                    {
                        if (justLoad == false)
                        {
                            loadedData = www.assetBundle.mainAsset as Texture2D; ;
                            resourcePool.addTexture(httpUrl, loadedData);
                            www.assetBundle.Unload(false);
                        }
                        else
                        {
                            www.assetBundle.Unload(true);
                        }
                    }
                    else
                    {
                        if (justLoad == false)
                        {
                            loadedData = www.texture;
                            resourcePool.addTexture(httpUrl, loadedData);
                        }
                    }
                    break;
                case SimpleLoadDataType.Json:
                    string json = System.Text.Encoding.UTF8.GetString(www.bytes);
                    int index = json.IndexOf("{");
                    if(index == -1) index = json.IndexOf("[");
                    if (index > 0)
                    {
                        json = json.Substring(index);
                    }
                    //同一个Json可能会变化 不记录上次的 因为有可能是过时的
                    //if(localHas == false && string.IsNullOrEmpty(uri) == false) SaveToLocal(www.bytes);
                    if (justLoad == false)
                    {
                        loadedData = json;
                    }
                    break;
                case SimpleLoadDataType.Byte:
                default:
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
            string message = string.Format("加载文件失败:{0} Error:{1}", www.url, www.error);
            Debug.LogWarning(message);
        }
        www.Dispose();
        //Debug.LogWarning("OnLoaded");
        if (loadedIsWrong == false)//没加载到资源 或者 加载到且正确
        {
            OnLoaded();
        }
        else {
            //加载资源且错误 等于加载失败
            this.state = SimpleLoadedState.Failed;
            LoadNext();
            EndOnly();
            resourcePool.LoadErrorData(this);
        }
    }
    
    //private void progress()
    //{
    //    if (www != null)
    //    {
    //        //this.dispatchEvent(new EventX(EventX.PROGRESS, www.progress));
    //    }
    //}

    private enum SouceType
    {
        Default,
        AssetBoundle,
        TopView,
        MaterialTexture,
        ResizedImage,
    }
}
