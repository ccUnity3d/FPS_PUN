using UnityEngine;
using System.Collections.Generic;
using System;
using Object = UnityEngine.Object;

public class ResourcesPool:Singleton<ResourcesPool>
{
    public Dictionary<string, LoadPoolData> pool = new Dictionary<string, LoadPoolData>();
    public List<string> loadinglist = new List<string>();
    public List<SimpleLoader> waitingLoaderlist = new List<SimpleLoader>();

    public bool TryGet(string key, out LoadPoolData value)
    {
        return pool.TryGetValue(key, out value);
    }

    public static void ClearPool()
    {
        Instance.clearPool();
    }

    public void clearPool()
    {
        List<string> clear = new List<string>();
        foreach (string item in pool.Keys)
        {
            bool cleared = pool[item].Clear();
            if (cleared == true)
            {
                clear.Add(item);
            }
        }
        for (int i = 0; i < clear.Count; i++)
        {
            pool.Remove(clear[i]);
        }
        //pool.Clear();
    }

    public void ClearWaitLoad()
    {
        waitingLoaderlist.Clear();
    }

    public ErrorData addError(string keyUrl)
    {
        ErrorData data;
        if (pool.ContainsKey(keyUrl))
        {
            //Debug.Log("重复添加：" + keyUrl);
            data = pool[keyUrl] as ErrorData;
            data.errorCount++;
        }
        else {
            data = new ErrorData(keyUrl);
            data.errorCount = 1;
            pool.Add(keyUrl, data);
        }
        return data;
    }

    public void RemoveError(object data)
    {
        string keyUrl = data.ToString();
        ErrorData errorData;
        if (pool.ContainsKey(keyUrl))
        {
            //Debug.Log("重复添加：" + keyUrl);
            errorData = pool[keyUrl] as ErrorData;
            if(errorData != null)pool.Remove(keyUrl);
        }
    }

    public PrefabData addPrefab(string keyUrl, object resouce, bool DonnotClear = false)
    {
        if (pool.ContainsKey(keyUrl))
        {
            if (pool[keyUrl] is ErrorData)
            {
                pool.Remove(keyUrl);
            }
            else {
                Debug.Log("重复添加：" + keyUrl);
                return pool[keyUrl] as PrefabData;
            }
        }
        PrefabData LoadPoolData = new PrefabData(keyUrl, resouce, DonnotClear);
        pool.Add(keyUrl, LoadPoolData);
        return LoadPoolData;
    }
    public void addTexture(string keyUrl, object resouce, bool DonnotClear = false)
    {
        if (pool.ContainsKey(keyUrl))
        {
            if (pool[keyUrl] is ErrorData)
            {
                pool.Remove(keyUrl);
            }
            else {
                Debug.Log("重复添加：" + keyUrl);
                return;
            }
        }
        //Debug.Log("添加：" + keyUrl);
        TextureData LoadPoolData = new TextureData(keyUrl, resouce, DonnotClear);
        pool.Add(keyUrl, LoadPoolData);
    }
    public void addJson(string keyUrl, object resouce, bool DonnotClear = false)
    {
        if (pool.ContainsKey(keyUrl))
        {
            if (pool[keyUrl] is ErrorData)
            {
                pool.Remove(keyUrl);
            }
            else {
                Debug.Log("重复添加：" + keyUrl);
                return;
            }
        }
        JsonData LoadPoolData = new JsonData(keyUrl, resouce, DonnotClear);
        pool.Add(keyUrl, LoadPoolData);
    }

    public void AddTextureUsage(string keyUrl, GameObject target)
    {
        LoadPoolData data;
        if (pool.TryGetValue(keyUrl, out data)) {
            if (data.type != PoolDataType.texture2D)
            {
                Debug.LogWarning(keyUrl + "不是图片");
                return;
            }
            TextureData textureData = data as TextureData;
            textureData.targets.Add(target.GetInstanceID());
        }
    }

    public bool addLoading(SimpleLoader loader)
    {
        for (int i = 0; i < loadinglist.Count; i++)
        {
            if (loadinglist[i] == loader.keyUrl)
            {
                Debug.LogWarning(" AddWaiting " + loader.keyUrl);
                waitingLoaderlist.Add(loader);
                return false;
            }
        }
        //Debug.LogWarning(" addLoading " + loader.keyUrl);
        loadinglist.Add(loader.keyUrl);
        return true;
    }

    public bool removeLoading(SimpleLoader loader)
    {
        for (int i = 0; i < loadinglist.Count; i++)
        {
            if(loadinglist[i] == loader.keyUrl)
            {
                loadinglist.RemoveAt(i);
                for (int k = 0; k < waitingLoaderlist.Count; k++)
                {
                    SimpleLoader item = waitingLoaderlist[k];
                    if (item.keyUrl == loader.keyUrl)
                    {
                        item.state = loader.state;
                        if (item.needClone == false)
                        {
                            item.loadedData = loader.loadedData;
                        }
                        else
                        {
                            LoadPoolData data;
                            if (pool.TryGetValue(loader.keyUrl, out data))
                            {
                                if(data is PrefabData)
                                {
                                    if (item.canceled == false && item.justEndReturn == false && item.onloaded != null)
                                    {
                                        item.loadedData = (data as PrefabData).GetNew();
                                    }
                                }
                            }
                            else
                            {
                                Debug.LogError("pool.TryGetValue == false keyUrl =" + loader.keyUrl);
                            }
                        }
                        item.EndOnly();
                        //item.Load();
                        waitingLoaderlist.RemoveAt(k);
                        k--;
                    }
                }
                return true;
            }
        }
        return false;
    }


    public void LoadError(SimpleLoader loader)
    {
        addError(loader.keyUrl);
        for (int i = 0; i < loadinglist.Count; i++)
        {
            if (loadinglist[i] == loader.keyUrl)
            {
                loadinglist.RemoveAt(i);
                for (int k = 0; k < waitingLoaderlist.Count; k++)
                {
                    SimpleLoader item = waitingLoaderlist[k];
                    if (item.keyUrl == loader.keyUrl)
                    {
                        //item.onloaded();
                        //item.progress = 1;
                        item.state = loader.state;
                        item.EndOnly();
                        waitingLoaderlist.RemoveAt(k);
                        k--;
                    }
                }
            }
        }
        MyCallLater.Add(1, RemoveError, loader.keyUrl);
    }
    
    public static void Dispos(GameObject target)
    {
        Instance.dispos(target);
    }

    /// <summary>
    /// 后面可能会分开成 disposTexture 和 disposGameObject
    /// </summary>
    /// <param name="target"></param>
    public void dispos(GameObject target)
    {
        if (target == null)
        {
            Debug.LogWarning("Dispos target == null");
            return;
        }
        int count = 0;
        int id = target.GetInstanceID();
        foreach (LoadPoolData item in pool.Values)
        {
           if (item.type == PoolDataType.texture2D)
            {
                TextureData textureData = item as TextureData;
                if (textureData.Remove(id))
                {
                    count++;
                    //texture是引用不能销毁物体  如需销毁在外部自己做此处不应提供if (target != null) GameObject.Destroy(target);
                }
            }
            if (item.type == PoolDataType.Prefab)
            {
                PrefabData prefabData = item as PrefabData;
                if (prefabData.Remove(id))
                {
                    count++;
                    //只有prefab实例会销毁  
                    if (target != null) GameObject.Destroy(target);
                }
            }
        }

        //foreach (LoadPoolData item in pool.Values)
        //{
        //    if (item.type == PoolDataType.Prefab)
        //    {
        //        PrefabData prefabData = item as PrefabData;
        //        prefabData.Remove(id);
        //        count++;
        //        只有prefab实例会销毁
        //        GameObject.Destroy(target);
        //    }
        //}

        if (count == 0)
        {
            Debug.LogWarning("Dispos未找到" + target.name);
        }
    }

    public static void DisposTexture(GameObject target)
    {
        Instance.disposTexture(target);
    }

    public void disposTexture(GameObject target)
    {
        int count = 0;
        int id = target.GetInstanceID();
        foreach (LoadPoolData item in pool.Values)
        {
            if (item.type == PoolDataType.texture2D)
            {
                TextureData textureData = item as TextureData;
                textureData.Remove(id);
                count++;
                //texture是引用不能销毁物体  如需销毁在外部自己做此处不应提供
            }
        }
        if (count == 0)
        {
            Debug.LogWarning("Dispos未找到" + target.name);
        }
    }



    private void Remove(string keyUrl)
    {
        if (pool.ContainsKey(keyUrl))
        {
            pool.Remove(keyUrl);
        }
    }

    

    internal void LoadErrorData(SimpleLoader loader)
    {
        addError(loader.keyUrl);
        for (int i = 0; i < loadinglist.Count; i++)
        {
            if (loadinglist[i] == loader.keyUrl)
            {
                loadinglist.RemoveAt(i);
                for (int k = 0; k < waitingLoaderlist.Count; k++)
                {
                    SimpleLoader item = waitingLoaderlist[k];
                    if (item.keyUrl == loader.keyUrl)
                    {
                        //item.onloaded();
                        //item.progress = 1;
                        item.state = loader.state;
                        item.EndOnly();
                        waitingLoaderlist.RemoveAt(k);
                        k--;
                    }
                }
            }
        }
    }
    public abstract class LoadPoolData
    {
        /// <summary>
        /// 未使用超过该时间删除
        /// </summary>
        public const float waitingTime = 30f;
        public string keyUrl;
        public object resouce;
        /// <summary>
        /// 加载的时间
        /// </summary>
        public float loadedTime;
        /// <summary>
        /// 最新使用时间
        /// </summary>
        private float _newUseTime;
        public float newUseTime
        {
            get { return _newUseTime; }
            set
            {
                _newUseTime = value;
                OnNewUse();
            }
        }

        public float newRemoveTime;

        public PoolDataType type;
        public bool DonnotClear;

        //public bool juseEndReturn;

        protected ResourcesPool resourcesPool
        {
            get { return ResourcesPool.Instance; }
        }


        protected virtual void OnNewUse()
        {

        }

        public abstract void disPos();

        public virtual bool Clear()
        {
            if (resouce == null) return false;
            if (DonnotClear == true) return false;
            disPos();
            return true;
        }
    }
    public class ErrorData : LoadPoolData
    {
        public readonly int MaxErrorCount = 3;
        public int errorCount = 0;

        public ErrorData(string keyUrl)
        {
            this.keyUrl = keyUrl;
        }

        public override void disPos()
        {

        }
    }
    public class PrefabData : LoadPoolData
    {
        public UnityEngine.Object prefab;
        /// <summary>
        /// instances  实例
        /// </summary>
        public Dictionary<int, GameObject> targets = new Dictionary<int, GameObject>();

        public object GetNew()
        {
            Debug.LogWarning("GameObject.Instantiate " + keyUrl);

            GameObject newObj = GameObject.Instantiate(prefab) as GameObject;
            targets.Add(newObj.GetInstanceID(), newObj);

            return newObj;
        }

        public PrefabData(string keyUrl, object resouce, bool DonnotClear = false)
        {
            type = PoolDataType.Prefab;
            this.loadedTime = Time.realtimeSinceStartup;
            this.newUseTime = this.loadedTime;
            this.keyUrl = keyUrl;
            this.resouce = resouce;
            this.DonnotClear = DonnotClear;
            prefab = resouce as UnityEngine.Object;
        }
        

        public bool Remove(int id)
        {
            if (targets.ContainsKey(id))
            {
                newRemoveTime = Time.realtimeSinceStartup;
                if (targets[id] != null)
                {
                    GameObject.DestroyImmediate(targets[id], true);
                }
                targets.Remove(id);
                if (targets.Count == 0)
                {
                    MyCallLater.Add(waitingTime, RemoveFromResoucePool);
                }
                return true;
            }
            return false;
        }
        
        private void RemoveFromResoucePool()
        {
            if (targets.Count > 0) return;
            float withoutUseTime = Time.realtimeSinceStartup - newRemoveTime;
            if (withoutUseTime >= waitingTime)
            {
                resourcesPool.Remove(keyUrl);
                disPos();
            }
            else
            {
                MyCallLater.Add(waitingTime - withoutUseTime, RemoveFromResoucePool);
            }
        }

        public override void disPos()
        {
            GameObject.DestroyImmediate(prefab, true);
            Resources.UnloadUnusedAssets();
        }

        public override bool Clear()
        {
            if (resouce == null) return false;
            if (DonnotClear == true) return false;
            foreach (GameObject item in targets.Values)
            {
                if (item != null) {
                    GameObject.DestroyImmediate(item, true);
                }
            }
            targets.Clear();
            disPos();
            return true;
        }
    }

    public class TextureData : LoadPoolData
    {
        public Texture texture;
        /// <summary>
        /// 引用物体列表
        /// </summary>
        public List<int> targets = new List<int>();

        public TextureData(string keyUrl, object resouce, bool DonnotClear = false)
        {
            type = PoolDataType.texture2D;
            this.keyUrl = keyUrl;
            this.resouce = resouce;
            this.DonnotClear = DonnotClear;
            texture = resouce as Texture;
        }

        public bool Remove(int id)
        {
            if (targets.IndexOf(id) != -1)
            {
                newRemoveTime = Time.realtimeSinceStartup;
                targets.Remove(id);
                if (targets.Count == 0)
                {
                    MyCallLater.Add(waitingTime, RemoveFromResoucePool);
                }
                return true;
            }
            return false;
        }

        private void RemoveFromResoucePool()
        {
            if (targets.Count > 0) return;
            float withoutUseTime = Time.realtimeSinceStartup - newRemoveTime;
            if (withoutUseTime >= waitingTime)
            {
                resourcesPool.Remove(keyUrl);
                disPos();
            }
            else
            {
                MyCallLater.Add(waitingTime - withoutUseTime, RemoveFromResoucePool);
            }
        }

        public override void disPos()
        {
            Debug.Log("释放图片:" + texture);
            GameObject.DestroyImmediate(texture, true);
            Resources.UnloadUnusedAssets();
        }
    }

    public class JsonData : LoadPoolData
    {
        public string json;
        public JsonData(string keyUrl, object resouce, bool DonnotClear = false)
        {
            type = PoolDataType.Json;
            this.keyUrl = keyUrl;
            this.resouce = resouce;
            this.DonnotClear = DonnotClear;
            json = resouce.ToString();
        }

        protected override void OnNewUse()
        {
            base.OnNewUse();
            MyCallLater.Remove(RemoveFromResoucePool);
            MyCallLater.Add(waitingTime, RemoveFromResoucePool);
        }

        private void RemoveFromResoucePool()
        {
            float withoutUseTime = Time.realtimeSinceStartup - newRemoveTime;
            if (withoutUseTime > waitingTime)
            {
                disPos();
                resourcesPool.Remove(keyUrl);
            }
            else
            {
                MyCallLater.Add(waitingTime - withoutUseTime, RemoveFromResoucePool);
            }
        }

        public override void disPos()
        {
            
        }
    }

    public enum PoolDataType
    {
        /// <summary>
        /// 基本不用
        /// </summary>
        Byte,
        /// <summary>
        /// 传输配置保存和读取类型
        /// </summary>
        Json,
        /// <summary>
        /// 远程图片
        /// </summary>
        texture2D,
        /// <summary>
        /// 暂时没有
        /// </summary>
        Prefab,
    }
}

/*
Directory.Exists(path);         // path表示路径参数;可判断文件路径是否存在
Directory.CreateDirectory(path);// path表示路径参数;可创建目录
File.Exists(path);              // path表示路径参数;可判断文件是否存在
*/
