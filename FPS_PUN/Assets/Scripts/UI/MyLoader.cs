using System.Collections;
using System;
using UnityEngine;

public class MyLoader {

    public MyLoader()
    {
    }
    public object bringData;
    public Action<UnityEngine.Object, object> onLoaded;


    private string loadpath;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    /// <param name="type">加载类型 0本地Resouce 1本地StreamingAsset 2远程</param>
    /// <param name="data"></param>
    public void LoadPrefab(string path, int type, Action<UnityEngine.Object, object> Loaded, object data = null)
    {
        onLoaded = Loaded;
        bringData = data;
        if (type == 0)
        {
            loadpath = path.Split('.')[0];
            UnityEngine.Object obj = GameObject.Instantiate(Resources.Load(loadpath));
            if (Loaded != null) onLoaded(obj, bringData);
        }
        else if (type == 1)
        {
            loadpath = "UI/panel/" + path;
            LoaderPool.InnerLoad(loadpath, SimpleLoadDataType.prefabAssetBundle, OnLoaded, null);
            //loader.Load();
        }
        else
        {
            Debug.LogWarning("目前只有本地");
        }
    }

    private void OnLoaded(object obj)
    {
        SimpleLoader loader = obj as SimpleLoader;
        if (loader.state == SimpleLoadedState.Failed)
        {
            Debug.LogWarning("Load Fail :" + loader.uri);
            return;
        }
        if (onLoaded == null) return;
        UnityEngine.Object ob = loader.loadedData as UnityEngine.Object;
        onLoaded(ob, bringData);

    }
}
