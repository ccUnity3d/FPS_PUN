using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 代替MonoBehavior里的StartCoroutine
/// </summary>
public class MyMono {

    private static MyMono mymono;

    public static MyMono getInstance
    {
        get
        {
            if (mymono == null)
            {
                mymono = new MyMono();
            }
            return mymono;
        }
    }
    public MyMono()
    {
    }

    private Stack<GameObject> objList = new Stack<GameObject>();

    /// <summary>
    /// 同一个方法 第二次调用会把第一次在使用的关掉
    /// </summary>
    private Dictionary<object, Dictionary<int, List<ItemMono>>> objRunningDic = new Dictionary<object, Dictionary<int, List<ItemMono>>>();
    
    private GameObject _parent;

    private GameObject parent
    {
        get
        {
            if (_parent == null)
            {
                _parent = new GameObject("MyMono");
            }
            return _parent;
        }
    }

    /// <summary>
    /// int hashCode = func.GetHashCode(); 如果已在运行则会移除在运行的
    /// </summary>
    /// <param name="func"></param>
    /// <param name="target"></param>
    /// <param name="objs"></param>
    public static void MyStartCoroutine(Func<object[], IEnumerator> func, object target, params object[] objs)
    {
        getInstance.myStartCoroutine(func, true, target, objs);
    }

    public void myStartCoroutine(Func<object[], IEnumerator> func, bool sameable = true, object target = null,  params object[] objs)
    {
        int hashCode = func.GetHashCode();
       
        ItemMono itemMono = GetItemMono();
        itemMono.setData(func, target, sameable, objs);

        Dictionary<int, List<ItemMono>> dic = null;
        if (objRunningDic.TryGetValue(target,out dic) == false)
        {
            dic = new Dictionary<int, List<ItemMono>>();
            objRunningDic.Add(target, dic);
        }

        if (dic.ContainsKey(hashCode) == true)
        {
            if (itemMono.sameable == false)
            {
                List<ItemMono> list = dic[hashCode];
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].sameable == false)
                    {
                        list[i].EndAndRemove();
                    }
                }
            }
        }
        else
        {
            dic.Add(hashCode, new List<ItemMono>());
        }

        dic[hashCode].Add(itemMono);

        itemMono.StartCoroutine(itemMono.MyDele());
    }

    private ItemMono GetItemMono()
    {
        GameObject obj = null;
        if (objList.Count > 0)
        {
            obj = objList.Pop();
            obj.SetActive(true);
        }
        if (obj != null)
        {
            return obj.GetComponent<ItemMono>();
        }
        obj = new GameObject("item");
        obj.transform.parent = parent.transform;
        return obj.AddComponent<ItemMono>();
    }

    public static void MyStopCoroutine(Func<object[], IEnumerator> func, object target = null, params object[] objs)
    {
        if (mymono.objRunningDic.ContainsKey(target) == false)
        {
            return;
        }
        int hashCode = func.GetHashCode();
        if (mymono.objRunningDic[target].ContainsKey(hashCode) == false)
        {
            return;
        }
        List<ItemMono> list = mymono.objRunningDic[target][hashCode];
        for (int i = 0; i < list.Count; i++)
        {
            list[i].EndAndRemove();
        }
    }

    /// <summary>
    /// target 不能为空 
    /// </summary>
    /// <param name="target"></param>
    public static void StopAllCoroutine(object target)
    {
        if (target != null)
        {
            if (mymono.objRunningDic.ContainsKey(target) == false)
            {
                return;
            }
            Dictionary<int, List<ItemMono>> dic = mymono.objRunningDic[target];
            foreach (List<ItemMono> itemList in dic.Values)
            {
                for (int i = 0; i < itemList.Count; i++)
                {
                    ItemMono item = itemList[i];
                    item.JustEnd();
                }
            }
            dic.Clear();
        }
    }

    private void sleep()
    {
        GameObject.Destroy(parent);
    }

    public void recycle(GameObject gameObj)
    {
        gameObj.SetActive(false);
        objList.Push(gameObj);
    }

    public void Remove(ItemMono item)
    {
        if (mymono.objRunningDic.ContainsKey(item.target) == false)
        {
            return;
        }
        int hashcode = item.func.GetHashCode();
        if(mymono.objRunningDic[item.target].ContainsKey(hashcode) == false)
        {
            return;
        }
        mymono.objRunningDic[item.target][hashcode].Remove(item);
    }
}

public class ItemMono : MonoBehaviour
{
    public ItemMono()
    {
        mymono = MyMono.getInstance;
    }
    private MyMono mymono;

    public Func<object[], IEnumerator> func;
    public object target;
    /// <summary>
    /// 是否可重复
    /// </summary>
    public bool sameable = false;

    private object[] objs;
    

    public void setData(Func<object[], IEnumerator> func1, object target1, bool sameable1, object[] objs1)
    {
        gameObject.name = func1.Method.Name;
        this.func = func1;
        this.target = target1;
        this.sameable = sameable1;
        this.objs = objs1;
    }

    public IEnumerator MyDele()
    {
        yield return StartCoroutine(func(objs));
        EndAndRemove();
    }

    public void EndAndRemove()
    {
        mymono.Remove(this);
        JustEnd();
    }

    /// <summary>
    /// 不移除
    /// </summary>
    public void JustEnd()
    {
        mymono.recycle(gameObject);
    }
}