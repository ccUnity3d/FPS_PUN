using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;

public abstract class CacheManager<T> : Singleton<T>, ICache where T : IInstance, new()
{
    //file.Length;字节长度//1-M = 1024-kb，1-kb = 1024-byte;
    //此处用不到
    //static private readonly string LocalReadOnlyHead = Application.dataPath + "/";
    static public readonly string LoadURLHead =
#if UNITY_STANDALONE_WIN || UNITY_EDITOR || UNITY_WEBPLAYER
                "file:///";
#elif UNITY_ANDROID   //安卓
                "jar:file://";
#elif UNITY_IPHONE  //iPhone
		        "file://";
#else
                string.Empty;
#endif
    static public readonly string LocalReadWriteURLHead = Application.persistentDataPath + "/";

    public bool isReady = false;
    /// <summary>
    /// 记录所有已缓存的服务器数据库id
    /// </summary>
    protected List<string> cacheList = new List<string>();
    /// <summary>
    /// 在配置cacheSchemeList加载完成前临时记录
    /// </summary>
    protected List<string> tempList = new List<string>();

    public virtual string GetSourceUrlById(string id) { return ""; }
    public virtual string GetIdByUrl(string url) { return ""; }
    public string GetLoadUrlByID(string id)
    {
        return LoadURLHead + GetSourceUrlById(id);
    }

    static protected string LocalLoadURLHead
    {
        get
        {
            return LoadURLHead + LocalReadWriteURLHead;
        }
    }
    //加载资源使用 此处应用不到
    //static protected string LocalLoadReadOnlyURLHead
    //{
    //    get
    //    {
    //        return LocalReadOnlyHead;
    //    }
    //}
    protected string Folding
    {
        get {
            return "alluserdata/";// + UnityIOSMsg.currentUser.uuid + "/Caches/";
        }
    }
    protected virtual string Name
    {
        get
        {
            return "CacheManager.catalog";
        }
    }
    public virtual string LoadURL
    {
        get
        {
            return LocalLoadURLHead + Folding + Name;
        }
    }
    public virtual string ReadWriteURL
    {
        get
        {
            return LocalReadWriteURLHead + Folding + Name;
        }
    }

    public virtual void AddCache(string url, object data)
    {
        string id = GetIdByUrl(url);
        //if (id == "resized")
        //{
        //    Debug.Log("resized");
        //}
            
        string souceUrl = GetSourceUrlById(id);
        FileInfo info = new FileInfo(souceUrl);
        if (info.Exists)
        {
            File.Delete(souceUrl);
        }
        else if (info.Directory.Exists == false)
        {
            info.Directory.Create();
        }

        Debug.Log("save:" + url);
        if (data is byte[])
        {
            byte[] bytes = data as byte[];
            using (FileStream stream = File.Open(souceUrl, FileMode.OpenOrCreate))
            {
                stream.Write(bytes, 0, bytes.Length);
            }
        }
        else if (data is string)
        {
            string json = data as string;
            using (StreamWriter writer = new StreamWriter(souceUrl, true, System.Text.Encoding.UTF8))
            {
                writer.WriteLine(json);
            }
        }
        if (isReady == true)
        {
            if (cacheList.IndexOf(id) == -1)
            {
                cacheList.Add(id);
                OnListChange();
            }
        }
        else
        {
            if (tempList.IndexOf(id) == -1) tempList.Add(id);
        }
    }
    public virtual bool HasCached(string url){
        string id = GetIdByUrl(url);
        string sourceUrl = GetSourceUrlById(id);
        if (isReady == true)
        {
            if (cacheList.IndexOf(id) != -1)
            {
                return true;
            }
            FileInfo info = new FileInfo(sourceUrl);
            if (info.Exists)
            {
                //记录中没有 属于垃圾缓存
                info.Delete();
            }
        }
        else
        {
            if (tempList.IndexOf(id) != -1)
            {
                return true;
            }
            ////临时记录中没有 加载后的记录可能有
            //FileInfo info = new FileInfo(sourceUrl);
            //if (info.Exists)
            //{
            //    info.Delete();
            //}
        }
        return false;
    }
    public SimpleOutterLoader LoadCache()
    {
        isReady = false;
        ClearCacheData();

        FileInfo info = new FileInfo(ReadWriteURL);
        if (info.Exists == false)
        {
            //没有缓存 即读到缓存为空 可以直接新建存入
            isReady = true;
           //this.dispatchEvent(new MyCacheEvent(MyCacheEvent.loadReady));
            return null;
        }
        SimpleOutterLoader outterLoader = LoaderPool.OutterLoad(LoadURL, SimpleLoadDataType.Json, OnLoaded);
        return outterLoader;
        //MyMono.MyStartCoroutine(LoadText, this, LoadURL);
    }

    private void OnLoaded(object msg)
    {
        SimpleOutterLoader loader = msg as SimpleOutterLoader;
        if (loader.state == SimpleLoadedState.Failed)
        {
            
        }
        else
        {
            string json = loader.loadedData.ToString();
            object obj = MyJsonTool.FromJson(json);
            List<object> listObj = obj as List<object>;
            for (int i = 0; i < listObj.Count; i++)
            {
                string id = listObj[i].ToString();
                if (cacheList.IndexOf(id) == -1)
                {
                    cacheList.Add(id);
                }
            }
        }
        bool changed = false;
        for (int i = 0; i < cacheList.Count; i++)
        {
            string tempid = cacheList[i];
            string souceUrl = GetSourceUrlById(tempid);
            FileInfo info = new FileInfo(souceUrl);
            if (info.Exists == false)
            {
                changed = true;
                cacheList.RemoveAt(i);
                i--;
            }
        }
        for (int i = 0; i < tempList.Count; i++)
        {
            if (cacheList.IndexOf(tempList[i]) != -1)
            {
                cacheList.Add(tempList[i]);
                changed = true;
            }
        }
        if (changed == true)
        {
            OnListChange();
        }

        isReady = true;
       // this.dispatchEvent(new MyCacheEvent(MyCacheEvent.loadReady));
    }

    public virtual void RemoveCache(string id)
    {
        string souceUrl = GetSourceUrlById(id);
        FileInfo info = new FileInfo(souceUrl);
        if (info.Exists == true)
        {
            info.Delete();
        }

        if (isReady)
        {
            if (cacheList.IndexOf(id) != -1)
            {
                cacheList.Remove(id);
                OnListChange();
            }
        }
        else
        {
            if (tempList.IndexOf(id) != -1)
            {
                tempList.Remove(id);
            }
        }
    }
    public virtual void ClearCache()
    {
        //先根据数据清空资源 后清空数据
        ClearCacheSoucre();
        ClearCacheData();
        OnListChange();
    }
    public virtual void OnListChange()
    {
        FileInfo info = new FileInfo(ReadWriteURL);
        if (info.Exists == true)
        {
            info.Delete();
        }
        if (info.Directory.Exists == false)
        {
            info.Directory.Create();
        }
        string json = MyJsonTool.ToJson(cacheList);
        using (StreamWriter writer = new StreamWriter(ReadWriteURL, true, System.Text.Encoding.UTF8))
        {
            //for (int i = 0; i < cacheList.Count; i++)
            //{
            //    writer.WriteLine(cacheList[i]);
            //}
            writer.WriteLine(json);
        }
    }
    private void ClearCacheSoucre()
    {
        for (int i = 0; i < cacheList.Count; i++)
        {
            string id = cacheList[i];
            string sourceUrl = GetSourceUrlById(id);

            FileInfo info = new FileInfo(sourceUrl);
            if (info.Exists == true)
            {
                info.Delete();
            }
        }
        FileInfo cacheinfo = new FileInfo(ReadWriteURL);
        if (cacheinfo.Exists == true)
        {
            cacheinfo.Delete();
        }

        for (int i = 0; i < tempList.Count; i++)
        {
            string serverId = tempList[i];
            string souceUrl = GetSourceUrlById(serverId);
            FileInfo info = new FileInfo(souceUrl);
            if (info.Exists == true)
            {
                info.Delete();
            }
        }
    }
    private void ClearCacheData() {
        cacheList.Clear();
        tempList.Clear();
    }

}

/// <summary>
/// SimpleOutterLoader 缓存的模型
/// </summary>
public class CacheModelManager : CacheManager<CacheModelManager>
{
    protected override string Name
    {
        get
        {
            return "AssetBundleServerCaches.catalog";
        }
    }
    public override bool HasCached(string url)
    {
        string id = GetIdByUrl(url);
        string sourceUrl = GetSourceUrlById(id);
        if (isReady == true)
        {
            if (cacheList.IndexOf(id) != -1)
            {
                Debug.LogWarning(sourceUrl + " cached = " + true);
                return true;
            }
            FileInfo info = new FileInfo(sourceUrl);
            if (info.Exists)
            {
                //记录中没有 属于垃圾缓存
                info.Delete();
            }
        }
        else
        {
            if (tempList.IndexOf(id) != -1)
            {
                Debug.LogWarning(sourceUrl + " cached = " + true);
                return true;
            }
            ////临时记录中没有 加载后的记录可能有
            //FileInfo info = new FileInfo(sourceUrl);
            //if (info.Exists)
            //{
            //    info.Delete();
            //}
        }
        Debug.LogWarning(sourceUrl + " cached = " + true);
        return false;
    }

    public override string GetIdByUrl(string url)
    {
        string id = "";
        string[] strs = url.Split('/');
        if (strs.Length > 1)
        {
            id = strs[strs.Length - 2];
        }
        return id;
    }

    //public override string GetSourceUrlById(string id)
    //{
    //    return LocalReadWriteURLHead + "alluserdata/" + UnityIOSMsg.currentUser.uuid + "/Assetbundle/" + id + "/model.assetbundle";
    //}
}

/// <summary>
/// SimpleOutterLoader 缓存的顶视图
/// </summary>
public class CacheTopViewManager : CacheManager<CacheTopViewManager>
{
    protected override string Name
    {
        get
        {
            return "TopViewServerCaches.catalog";
        }
    }

    public override string GetIdByUrl(string url)
    {
        string id = "";
        string[] strs = url.Split('/');
        if (strs.Length > 1)
        {
            id = strs[strs.Length - 2];
        }
        return id;
    }

    //public override string GetSourceUrlById(string id)
    //{
    //    return LocalReadWriteURLHead + "alluserdata/" + UnityIOSMsg.currentUser.uuid + "/Texture2D/" + id + "/top.png";
    //}
}

/// <summary>
/// SimpleOutterLoader 缓存的缩略图
/// </summary>
public class CacheResizedImageManager : CacheManager<CacheResizedImageManager>
{
    protected override string Name
    {
        get
        {
            return "ResizedImageServerCaches.catalog";
        }
    }

    public override string GetIdByUrl(string url)
    {
        string id = "";
        string[] strs = url.Split('/');
        if (strs.Length > 1)
        {
            id = strs[strs.Length - 2];
        }
        return id;
    }

    //public override string GetSourceUrlById(string id)
    //{
    //    //return LocalReadWriteURLHead + "alluserdata/" + UnityIOSMsg.currentUser.uuid + "/Texture2D/" + id + "/thumbnail.jpg";
    //}
}

/// <summary>
/// SimpleOutterLoader 缓存的材质贴图
/// </summary>
public class CacheMaterialTextureManager : CacheManager<CacheMaterialTextureManager>
{
    protected override string Name
    {
        get
        {
            return "MaterialTextureServerCaches.catalog";
        }
    }

    public override string GetIdByUrl(string url)
    {
        string id = "";
        string[] strs = url.Split('/');
        if (strs.Length > 1)
        {
            id = strs[strs.Length - 2];
        }
        return id;
    }

    //public override string GetSourceUrlById(string id)
    //{
    //    return LocalReadWriteURLHead + "alluserdata/" + UnityIOSMsg.currentUser.uuid + "/Texture2D/" + id + "/wallfloor.jpg";
    //}
}

public class CacheLocalOfferManager : CacheManager<CacheLocalOfferManager>
{
    protected override string Name
    {
        get
        {
            return "OfferLocalCaches.catalog";
        }
    }
    public override string GetIdByUrl(string url)
    {
        string id = url;
        //string[] strs = url.Split('/');
        //if (strs.Length > 1)
        //{
        //    id = strs[strs.Length - 2];
        //}
        return id;
    }

    //public override string GetSourceUrlById(string id)
    //{
    //    return LocalReadWriteURLHead + "alluserdata/" + UnityIOSMsg.currentUser.uuid + "/Offer/Local/" + id + ".midf";
    //}

    public int GetNewLocalcacheTempId()
    {
        int newtempId = -1;
        for (int i = 0; i < cacheList.Count; i++)
        {
            int tempid = int.Parse(cacheList[i]);
            if (tempid > newtempId)
            {
                newtempId = tempid;
            }
        }
        newtempId++;
        return newtempId;
    }

    public List<string> GetCacheOfferList()
    {
        return cacheList;
    }
}

public class CacheServerOfferManager : CacheManager<CacheServerOfferManager>
{
    protected override string Name
    {
        get
        {
            return "OfferServerCaches.catalog";
        }
    }

    public override string GetIdByUrl(string url)
    {
        string id = url;
        //string[] strs = url.Split('/');
        //if (strs.Length > 1)
        //{
        //    id = strs[strs.Length - 2];
        //}
        return id;
    }

    //public override string GetSourceUrlById(string id)
    //{
    //    return LocalReadWriteURLHead + "alluserdata/" + UnityIOSMsg.currentUser.uuid + "/Offer/Server/" + id + ".midf";
    //}


    public List<string> GetCacheOfferList()
    {
        return cacheList;
    }
}

public class CacheSchemeManager : CacheManager<CacheSchemeManager>
{
    public override bool HasCached(string httpUrl)
    {
        string id = GetIdByUrl(httpUrl);
        return HasCachedById(id);
    }
    public bool HasCachedById(string id)
    {
        string path = GetSourceUrlById(id);
        bool has = System.IO.File.Exists(path);
        Debug.Log(path + " has == false");
        return has;
    }
    public override void AddCache(string url, object data)
    {
        string id = GetIdByUrl(url);
        AddCacheById(id, data);
    }
    //public override string GetSourceUrlById(string id)
    //{
    //    return LocalReadWriteURLHead + "alluserdata/" + UnityIOSMsg.currentUser.uuid + "/Scheme/" + id + ".midf";
    //}
    public override string GetIdByUrl(string httpUrl)
    {
        string id = "";
        string[] strs = httpUrl.Split('/');
        if (strs.Length > 1)
        {
            id = strs[strs.Length - 2];
        }
        else
        {
            id = httpUrl;
        }
        return id;
    }
    public void AddCacheById(string id, object data)
    {
        //WWW w = new WWW();
        //WWWForm ww = new WWWForm();
        if (String.IsNullOrEmpty(id))
        {
            Debug.LogError(id+"is null");
        }
        string souceUrl = GetSourceUrlById(id);
        FileInfo info = new FileInfo(souceUrl);
        if (info.Exists)
        {
            File.Delete(souceUrl);
        }
        else if (info.Directory.Exists == false)
        {
            info.Directory.Create();
        }

        Debug.Log("save:" + souceUrl);
        if (data is byte[])
        {
            byte[] bytes = data as byte[];
            using (FileStream stream = File.Open(souceUrl, FileMode.OpenOrCreate))
            {
                stream.Write(bytes, 0, bytes.Length);
            }
        }
        else if (data is string)
        {
            string json = data as string;
            using (StreamWriter writer = new StreamWriter(souceUrl, true, System.Text.Encoding.UTF8))
            {
                writer.WriteLine(json);
            }
        }
    }
}

public interface ICache
{
    string LoadURL { get; }
    string ReadWriteURL { get; }
    SimpleOutterLoader LoadCache();
    //void OnLoaded(object loader);
    void AddCache(string url, object data);
    bool HasCached(string url);
    void RemoveCache(string url);
    void ClearCache();
    void OnListChange();
}



//public IEnumerator LoadText(object[] objs){
//    string url = objs[0].ToString();
//    WWW www = new WWW(url);
//    yield return www;
//    if (string.IsNullOrEmpty(www.error) == false)
//    {
//        //返回一个错误消息，在下载期间如果产生了一个错误的话www.error !=null
//    }
//    else
//    {
//        string json = System.Text.Encoding.UTF8.GetString(www.bytes);
//        int index = json.IndexOf("[");
//        if (index != 0)
//        {
//            json = json.Substring(index);
//        }
//        object obj = MyJsonTool.FromJson(json);
//        List <object> listObj = obj as List<object>;
//        //if(listObj==null)
//        for (int i = 0; i < listObj.Count; i++)
//        {
//            string id = listObj[i].ToString();
//            if (listObj.IndexOf(id) == -1)
//            {
//                listObj.Add(id);
//            }
//        }
//        //if (text.Length > 0)
//        //{
//        //    char num0 = text[0];
//        //    if (num0 < 48 || num0 > 57)
//        //    {
//        //        if (text.Length == 1)
//        //        {
//        //            text = "";
//        //        }
//        //        else {
//        //            text = text.Substring(1);
//        //        }
//        //    }
//        //}
//        //else
//        //{
//        //    //Debug.LogError(url + "加载到文件string.IsNullOrEmpty(text) == true");
//        //}
//        //string[] ids = text.Split(new string[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
//        //for (int i = 0; i < ids.Length; i++)
//        //{
//        //    if (cacheList.IndexOf(ids[i]) == -1)
//        //    {
//        //        cacheList.Add(ids[i]);
//        //    }
//        //}

//    }
//    bool changed = false;
//    for (int i = 0; i < cacheList.Count; i++)
//    {
//        string tempid = cacheList[i];
//        string souceUrl = GetSourceUrlById(tempid);
//        FileInfo info = new FileInfo(souceUrl);
//        if (info.Exists == false)
//        {
//            changed = true;
//            cacheList.RemoveAt(i);
//            i--;
//        }
//    }
//    for (int i = 0; i < tempList.Count; i++)
//    {
//        if (cacheList.IndexOf(tempList[i]) != -1)
//        {
//            cacheList.Add(tempList[i]);
//            changed = true;
//        }
//    }
//    if (changed == true)
//    {
//        OnListChange();
//    }

//    isReady = true;
//    this.dispatchEvent(new MyCacheEvent(MyCacheEvent.loadReady));
//}