using UnityEngine;
using System.Collections;
using System.IO;


public class ManageResource : Singleton<ManageResource> {

    //可写的，持久存储的路径//
    public static string getMyPersistentPath(string name)
    {
        string path = "";
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            path = Application.persistentDataPath + "/" + name;
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            //path = Application.persistentDataPath + "/" + name;
            path = Application.temporaryCachePath + "/" + name;
        }
        else
        {
            path = Application.dataPath + "/" + name;
        }
        return path;
    }

    // streamingAsset 路径
    public static string getMyStreamingAssetsPath(string name)
    {
        string path = "";
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            path = Application.dataPath + "/Raw/" + name;
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            path = Application.temporaryCachePath + "/" + name;
        }
        else
        {
            path = Application.streamingAssetsPath + "/" + name;
        }
        return path;
    }
    //private static SchemeManifest schemeManifest { get { return SchemeManifest.instance; } }
    // 读取JSON 数据
    public static string ReadSchemeJson(string name,object obj)
    {
        // json 数据
        string jsonData =null;
        // 文件具体路径   Dir 文件夹
        string Direct = getMyPersistentPath("Scheme");
        // 文件的具体路径 在文件夹下面
        string path = Direct+"/"+name;
        // 判断文件夹是否存在
        if (!File.Exists(path))
        {
            DirectoryInfo dir = new DirectoryInfo(Direct);
            dir.Create();
            FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            fs.Close();
            //对象 -> json   序列化
            jsonData = MyJsonTool.ToJson(obj) ;
            return jsonData;
        }
        // 从绝对路径读取JSON
        using (StreamReader stream = new StreamReader(path, System.Text.Encoding.UTF8))
        {
            jsonData = stream.ReadToEnd();
        }
        return jsonData;
    }
    public static string WriteSchemeJson(string name,object obj)
    {
        string jsonData = null;
        // 文件具体路径
        string Direct = getMyPersistentPath("Scheme");
        // 文件的具体路径 在文件夹下面
        string path = Direct + "/" + name;
        jsonData = MyJsonTool.ToJson(obj);
        DirectoryInfo directoryInfo = new DirectoryInfo(Path.GetDirectoryName(path));
        if (!directoryInfo.Exists)
        {
            Debug.Log("文件不存在");
            directoryInfo.Create();
        }
        else
        {
            FileInfo info = new FileInfo(path);
            if (info.Exists)
            {
                File.Delete(path);
            }
        }
        using (StreamWriter stream = new StreamWriter(path, true, System.Text.Encoding.UTF8))
        {
            stream.WriteLine(jsonData);
        }
        return jsonData;
    }


}
