using System;
using System.Collections.Generic;
using UnityEngine;

public class LoaderPool : Singleton<LoaderPool>
{
    public static void InnerLoad(string uri, SimpleLoadDataType type, Action<object> onloaded, object bringData)
    {
        Instance.getInnerPool(uri, type, onloaded, bringData);
    }
    public SimpleInnerLoader getInnerPool(string uri, SimpleLoadDataType type, Action<object> onloaded, object bringData)
    {
        SimpleInnerLoader loader;
        loader = new SimpleInnerLoader(uri, type, onloaded, bringData);
        loader.Load();
        return loader;
    }
    public static SimpleOutterLoader OutterLoad(string httpurl, SimpleLoadDataType type, Action<object> onloaded, object bringData = null, Action<GameObject, object> onloadedBforClone = null)
    {
        Debug.Log("httpurl = " + httpurl);
        if (string.IsNullOrEmpty(httpurl))
        {
            return null;
        }
        return Instance.getOutterPool(httpurl, type, onloaded, bringData, onloadedBforClone);
    }

    public SimpleOutterLoader getOutterPool(string httpurl, SimpleLoadDataType type, Action<object> onloaded, object bringData = null, Action<GameObject, object> onloadedBforClone = null)
    {
        SimpleOutterLoader loader;
        loader = new SimpleOutterLoader(httpurl, type, onloaded, bringData, onloadedBforClone);
        loader.Load();
        return loader;
    }

    public static SimpleOutterLoader WaitOutterLoad(string httpurl, SimpleLoadDataType type, Action<object> onloaded, object bringData, Action<GameObject, object> onloadedBforClone = null)
    {
        return Instance.waitOutterLoad(httpurl, type, onloaded, bringData, onloadedBforClone);
    }


    public SimpleOutterLoader waitOutterLoad(string httpurl, SimpleLoadDataType type, Action<object> onloaded, object bringData, Action<GameObject, object> onloadedBforClone = null)
    {
        SimpleOutterLoader loader;
        loader = new SimpleOutterLoader(httpurl, type, onloaded, bringData, onloadedBforClone);
        loader.justEndReturn = true;
        loader.Load();
        return loader;
    }

    public static SimpleCacheLoader CacheLoad(int tempId, SimpleLoadDataType type, Action<object> onloaded, object bringdata = null)
    {
        return Instance.cacheLoad(tempId, type, onloaded, bringdata);
    }

    public SimpleCacheLoader cacheLoad(int tempId, SimpleLoadDataType type, Action<object> onloaded, object bringdata = null)
    {
        SimpleCacheLoader loader;
        loader = new SimpleCacheLoader(tempId, type, onloaded, bringdata);
        loader.Load();
        return loader;
    }

    public static SimpleCacheLoader CacheLoad(string id, SimpleLoadDataType type, Action<object> onloaded, object bringdata = null)
    {
        return Instance.cacheLoad(id, type, onloaded, bringdata);
    }
    public SimpleCacheLoader cacheLoad(string id, SimpleLoadDataType type, Action<object> onloaded, object bringdata = null)
    {
        SimpleCacheLoader loader;
        loader = new SimpleCacheLoader(id, type, onloaded, bringdata);
        loader.Load();
        return loader;
    }

    public static SimpleOutterLoader OnlyOutterLoad(string httpurl, SimpleLoadDataType type, Action<object> onloaded, object bringData)
    {
        return Instance.onlyOutterLoad(httpurl, type, onloaded, bringData);
    }


    public SimpleOutterLoader onlyOutterLoad(string httpurl, SimpleLoadDataType type, Action<object> onloaded, object bringData)
    {
        SimpleOutterLoader loader;
        loader = new SimpleOutterLoader(httpurl, type, onloaded, bringData);
        loader.justEndReturn = true;
        loader.justLoad = true;
        loader.Load();
        return loader;
    }
}