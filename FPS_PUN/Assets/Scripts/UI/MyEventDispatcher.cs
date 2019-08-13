using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class MyEventDispatcher : IMyEventDispatcher
{
    private int hashCode = -1;

    //private static Dictionary<int, Dictionary<string, List<Action<MyEvent>>>> Dic;

    public Dictionary<string, List<Action<MyEvent>>> dic = new Dictionary<string, List<Action<MyEvent>>>();
    //{
    //    get
    //    {
    //        if (hashCode == -1)
    //        {
    //            hashCode = this.GetHashCode();
    //            if (hashCode == -1)
    //            {
    //                hashCode = 0;
    //            }
    //        }
    //        if (Dic == null)
    //        {
    //            Dic = new Dictionary<int, Dictionary<string, List<Action<MyEvent>>>>();
    //        }
    //        if (Dic.ContainsKey(hashCode) == false)
    //        {
    //            Dic.Add(hashCode, new Dictionary<string, List<Action<MyEvent>>>());
    //        }
    //        return Dic[hashCode];
    //    }
    //}

    /// <summary>
    /// 清空所有非永久监听的事件
    /// </summary>
    public static void ClearAllMyEvent()
    {
        //有些界面有永久性监听事件 不能清空
        //if (Dic != null) Dic.Clear();
    }

    public bool addEventListener(string type, Action<MyEvent> listener)
    {
        if (hasEventListener(type, listener) == true)
        {
            return false;
        }
        if (dic.ContainsKey(type) == false)
        {
            dic.Add(type, new List<Action<MyEvent>>());
        }        
        dic[type].Add(listener);
        return true;
    }
    
    public bool hasEventListener(string type)
    {
        if (dic.ContainsKey(type) == false)
        {
            return false;
        }
        if (dic[type].Count == 0)
        {
            return false;
        }
        return true;
    }
    public bool hasEventListener(string type, Action<MyEvent> listener)
    {
        if (dic.ContainsKey(type) == false)
        {
            return false;
        }
        if (dic[type].Contains(listener) == true)
        {
            return true;
        }
        return false;   
    }

    public bool removeEventListener(string type, Action<MyEvent> listener)
    {
        if (hasEventListener(type, listener))
        {
            dic[type].Remove(listener);
            if (dic[type].Count == 0)
            {
                dic.Remove(type);
            }
            return true;
        }
        return false;
    }

    public void removeEventListeners(string type)
    {
        if (hasEventListener(type))
        {
            dic.Remove(type);
        }
    }

    public int dispatchEvent(MyEvent myEvent)
    {
        if (dic.ContainsKey(myEvent.type) == false)
        {
            return 0;
        }
        List<Action<MyEvent>> list = dic[myEvent.type];
        int len = list.Count;
        for (int i = 0; i < len; i++)
        {
            list[i](myEvent);
        }
        return len;
    }

    public void ClearListioner()
    {
        dic.Clear();
    }
}

public class MyEvent
{
    public string type;
    public object data;
    public MyEvent(string type, object data = null)
    {
        this.type = type;
        this.data = data;
    }
}

public interface IMyEventDispatcher
{
    bool addEventListener(string type, Action<MyEvent> listener);
    bool removeEventListener(string type, Action<MyEvent> listener);
    void removeEventListeners(string type);
    bool hasEventListener(string type);
    bool hasEventListener(string type, Action<MyEvent> listener);
    int dispatchEvent(MyEvent myEvent);
}