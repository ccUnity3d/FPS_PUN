
public enum SimpleLoadDataType
{
    /// <summary>
    /// 基本不用
    /// </summary>
    Byte,
    /// <summary>
    /// 外部加载Json使用的类型 传输配置保存和读取类型
    /// </summary>
    Json,
    /// <summary>
    /// 方案-Cache加载Json使用的类型
    /// </summary>
    JsonScheme,
    /// <summary>
    /// 报价-Cache加载Json使用的类型
    /// </summary>
    JsonOffer,
    /// <summary>
    /// 远程图片
    /// </summary>
    texture2D,
    /// <summary>
    /// 打包的UGUIPanel和远程模型
    /// </summary>
    prefabAssetBundle,

    /// <summary>
    /// 暂时没有
    /// </summary>
    Prefab,
    /// <summary>
    /// 暂时没有
    /// </summary>
    texture2DAssetBundle,
}

public enum SimpleLoadedState
{
    None,
    Loading,
    Failed,
    Success
}

public enum AdressType
{
    /// <summary>
    /// 直接打包进程序的资源
    /// </summary>
    Resources,
    /// <summary>
    /// 只读文件夹 "file://" + Application.dataPath + "/StreamingAssets/" + xxx;
    /// </summary>
    StreamingAssets,
    /// <summary>
    /// 沙盒 可读可写文件夹 "file://" + Application.persistentDataPath + xxx
    /// </summary>
    PersistentDataPath,
    /// <summary>
    /// 远程 完整路径加载 "http://" + xxx
    /// </summary>
    Http
}
