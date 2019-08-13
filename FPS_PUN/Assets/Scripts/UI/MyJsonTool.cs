using System;
using System.Collections.Generic;
using UnityEngine;

public class MyJsonTool {

    //public static WebJsonProjectData ReadProjectJson(object json)
    //{
    //    WebJsonProjectData data = new WebJsonProjectData();
    //    data.Deserialize(json as Dictionary<string, object>);
    //    return data;
    //}

    //public static OriginalInputData ProjectDataToOriginalInputData(WebJsonProjectData project)
    //{
    //    return null;
    //}
    //public static WebJsonProjectData OriginalInputDataToProjectJson(OriginalInputData input)
    //{
    //    return null;
    //}
    /// <summary>
    /// 把字节序列化恢复为对象过程称为对象的反序列化。
    /// </summary>
    /// <param name="json"></param>
    /// <returns></returns>
    public static object FromJson(string json)
    {
        return Json.Deserialize(json);
    }


    /// <summary>
    /// 把对象转换为字节序列的过程称为对象的序列化。
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static string ToJson(object obj)
    {
        //Debug.LogWarning("ToJson*******************************");
        //如果包含OrigInputData 需要originalInputData.BeforetSerializeFieldDo();
        return Json.Serialize(obj);
        //JsonUtility.ToJson(obj);
    }

    public static List<int> getIntListValue(Dictionary<string, object> from, string property)
    {
        object value = getValue(from, property);

        if (value == null)
        {
            return null;
        }
        List<object> objs = value as List<object>;
        if (objs == null)
        {
            return null;
        }
        List<int> list = new List<int>();
        for (int i = 0; i < objs.Count; i++)
        {
            list.Add(int.Parse(objs[i].ToString()));
        }

        return list;
    }
    public static List<string> getStringListValue(Dictionary<string, object> from, string property)
    {
        object value = getValue(from, property);

        if (value == null)
        {
            return null;
        }
        List<object> objs = value as List<object>;
        if (objs == null)
        {
            return null;
        }
        List<string> list = new List<string>();
        for (int i = 0; i < objs.Count; i++)
        {
            list.Add(objs[i].ToString());
        }

        return list;
    }

    public static T getEnumValue<T>(Dictionary<string, object> from, string property)
    {
        string value = getStringValue(from, property);
        T t;
        try
        {
            t = (T)System.Enum.Parse(typeof(T), value);
        }
        catch (Exception)
        {
            t = default(T);
        }
        return t;
    }

    public static string getStringValue(Dictionary<string, object> from, string property)
    {
        object value = getValue(from, property);

        if (value == null)
        {
            return "";
        }

        return value.ToString();
    }

    public static int getIntValue(Dictionary<string, object> from, string property)
    {
        object value = getValue(from, property);

        if (value == null)
        {
            return 0;
        }
        int intValue;
        if (int.TryParse(value.ToString(), out intValue) == false)
        {
            Debug.LogWarning("int 转化失败 ：" + value);
        }
        return intValue;
    }

    public static float getFloatValue(Dictionary<string, object> from, string property)
    {
        object value = getValue(from, property);

        if (value == null)
        {
            return 0;
        }

        return float.Parse(value.ToString());
    }

    public static bool getBoolValue(Dictionary<string, object> from, string property)
    {
        object value = getValue(from, property);

        if (value == null)
        {
            return false;
        }

        return bool.Parse(value.ToString());
    }

    public static object getValue(Dictionary<string, object> from, string property)
    {
        if (from == null)
        {
            Debug.LogError("Dictionary<string, object> from = " + null);
            return null;
        }
        object value;
        if (from.TryGetValue(property, out value))
        {
            return value;
        }
        Debug.LogError("不存在property = "+ property);
        return null;
    }

    public static Vector3 getVector3(Dictionary<string, object> from, string property)
    {
        Vector V = getVector(from, property);
        return V.getV3();
    }

    public static Vector2 getVector2(Dictionary<string, object> from, string property)
    {
        Vector V = getVector(from, property);
        return V.getV2();
    }

    private static Vector getVector(Dictionary<string, object> from, string property)
    {
        object obj = getValue(from, property);
        if (obj != null)
        {
            vector.DeSerialize(obj as Dictionary<string, object>);
            return vector;
        }
        return emtyvector;
    }

    private static Vector vector = new Vector();
    private readonly static Vector emtyvector = new Vector();
    public class Vector
    {
        public float x = 0;
        public float y = 0;
        public float z = 0;

        public void DeSerialize(Dictionary<string, object> dic)
        {
            x = 0;
            y = 0;
            z = 0;
            foreach (string key in dic.Keys)
            {
                switch (key)
                {
                    case "x":
                        x = getFloatValue(dic, key);
                        break;
                    case "y":
                        y = getFloatValue(dic, key);
                        break;
                    case "z":
                        z = getFloatValue(dic, key);
                        break;
                    default:
                        break;
                }
            }
        }

        public Vector2 getV2()
        {
            return x * Vector2.right + y * Vector2.up;
        }
        public Vector3 getV3()
        {
            return x * Vector3.right + y * Vector3.up + z * Vector3.forward;
        }
    }

}
