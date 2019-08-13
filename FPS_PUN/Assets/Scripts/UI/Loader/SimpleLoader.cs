using UnityEngine;
using System;
using System.Collections.Generic;

public abstract class SimpleLoader : MyEventDispatcher{

    public static MyEventDispatcher StaticEventDispatcher = new MyEventDispatcher();
    private static Stack<SimpleLoader> loaderStack = new Stack<SimpleLoader>();
    private static List<SimpleLoader> loaders = new List<SimpleLoader>();
    private const int LoaderCount = 5;

    public string uri;
    public SimpleLoadDataType type;
    public Action<object> onloaded;
    public bool checkProgress;
    public object bringData;
    public object loadedData;
    public SimpleLoadedState state;
    protected ResourcesPool resourcePool { get { return ResourcesPool.Instance; } }
    protected LoaderPool loaderPool { get { return LoaderPool.Instance; } }
    //protected List<WaitingLoad> waitings = new List<WaitingLoad>();
    protected WWW www;
    public bool canceled = false;
    //protected bool error = false;
    public bool justEndReturn = false;
    public float progress = 0;

    public virtual string url
    {
        get {
            return uri;
        }
    }

    public virtual string keyUrl
    {
        get
        {
            return url;
        }
    }

    public bool needClone
    {
        get {
            switch (type)
            {
                case SimpleLoadDataType.Byte:
                case SimpleLoadDataType.Json:
                case SimpleLoadDataType.JsonScheme:
                case SimpleLoadDataType.JsonOffer:
                case SimpleLoadDataType.texture2D:
                case SimpleLoadDataType.texture2DAssetBundle:
                default:
                    return false;
                case SimpleLoadDataType.prefabAssetBundle:
                case SimpleLoadDataType.Prefab:
                    return true;
            }
        }
    }

    public void Load()
    {
        if (string.IsNullOrEmpty(url))
        {
            Debug.LogError("NeedLoadstring.IsNullOrEmpty(url) == true");
            return;
        }
        //Debug.LogWarning("NeedLoad"+url);
        state = SimpleLoadedState.Loading;

        ResourcesPool.LoadPoolData data;
        if (resourcePool.TryGet(keyUrl, out data))
        {
            data.newUseTime = Time.realtimeSinceStartup;
            //Debug.LogError("LoadInPool  +  keyUrl = " + keyUrl);
            MyCallLater.Add(LoadInPool, 0, data);

            return;
        }

        bool add = resourcePool.addLoading(this);
        if (add)
        {
            if (loaders.Count < LoaderCount)
            {
                loaders.Add(this);
                StartLoad();
            }
            else {
                loaderStack.Push(this);
            }
        }        
    }

    public static void CancelLoad()
    {
        UnityEngine.Debug.Log("dispatchEvent(new LoadEvent(LoadEvent.Cancel));");
        StaticEventDispatcher.dispatchEvent(new LoadEvent(LoadEvent.Cancel));
        for (int i = 0; i < loaders.Count; i++)
        {
            loaders[i].stopLoad();
        }
        loaders.Clear();
        loaderStack.Clear();
        ResourcesPool.Instance.loadinglist.Clear();
        ResourcesPool.Instance.waitingLoaderlist.Clear();
    }

    public static void RemoveLoader(SimpleLoader loader)
    {
        int loadIndex = loaders.IndexOf(loader);
        int waitingIndex = -1;
        if (loadIndex != -1)
        {
            loader.stopLoad();
        }
        else
        {
            waitingIndex = ResourcesPool.Instance.waitingLoaderlist.IndexOf(loader);
            if (waitingIndex != -1)
            {
                ResourcesPool.Instance.waitingLoaderlist.RemoveAt(waitingIndex);
            }
        }
    }

    protected virtual void stopLoad()
    {
        
    }

    private void LoadInPool(object data)
    {
        //Debug.LogWarning("LoadInPool:"+uri);
        ResourcesPool.LoadPoolData poolData = data as ResourcesPool.LoadPoolData;
        if (poolData is ResourcesPool.ErrorData)
        {
            ResourcesPool.ErrorData errorData = poolData as ResourcesPool.ErrorData;
            errorData.errorCount++;
            //if (errorData.errorCount > errorData.MaxErrorCount)
            //{
            this.state = SimpleLoadedState.Failed;
            this.EndOnly();
            return;
            //}
        }
        switch (poolData.type)
        {
            case ResourcesPool.PoolDataType.Json:
                loadedData = poolData.resouce;
                break;
            case ResourcesPool.PoolDataType.texture2D:
                loadedData = poolData.resouce;
                break;
            case ResourcesPool.PoolDataType.Prefab:
                ResourcesPool.PrefabData prefabData = poolData as ResourcesPool.PrefabData;
                if (canceled == false)
                {
                    if (justEndReturn == false)
                    {
                        loadedData = prefabData.GetNew();
                    }
                }
                break;
            case ResourcesPool.PoolDataType.Byte:
            default:
                loadedData = null;
                Debug.LogWarning("没有此类型的池！");
                break;
        }

        EndOnly();
    }

    public virtual void StartLoad()
    {
        //Debug.LogWarning("StartLoad" + url);
    }

    public virtual void OnLoaded()
    {
        LoadNext();
        EndOnly();
        if (state == SimpleLoadedState.Failed)
        {
            resourcePool.LoadError(this);
        }
        else {
            bool remove = resourcePool.removeLoading(this);
        }
    }

    public void EndOnly()
    {
        progress = 1;
        this.dispatchEvent(new LoadEvent(LoadEvent.Progress, progress));
        this.dispatchEvent(new LoadEvent(LoadEvent.Complete));
        //if (state == SimpleLoadedState.Failed)
        //{
        //    return;
        //}
        if (canceled == false && onloaded != null)
        {
            onloaded(this);
        }        
    }

    protected void LoadNext()
    {
        loaders.Remove(this);
        if (loaderStack.Count > 0)
        {
            SimpleLoader loader = loaderStack.Pop();
            loaders.Add(loader);
            loader.StartLoad();
        }
    }

    public void Cancel()
    {
        canceled = true;
    }

    protected void Progress()
    {
        if (www != null)
        {
            progress = www.progress;
            this.dispatchEvent(new LoadEvent(LoadEvent.Progress, progress));
        }
    }

    //public void AddWaiting(SimpleLoadDataType type, Action<object> onloaded, object bringData)
    //{
    //    WaitingLoad wait = new WaitingLoad(type, onloaded, bringData);
    //    AddWaiting(wait);
    //}
    //private void AddWaiting(WaitingLoad wait)
    //{
    //    waitings.Add(wait);
    //}

    //protected struct WaitingLoad
    //{
    //    public WaitingLoad(SimpleLoadDataType type, Action<object> onloaded, object bringData)
    //    {
    //        this.onloaded = onloaded;
    //        this.type = type;
    //        this.bringData = bringData;
    //    }
    //    public SimpleLoadDataType type;
    //    public Action<object> onloaded;
    //    public object bringData;
    //}
}
