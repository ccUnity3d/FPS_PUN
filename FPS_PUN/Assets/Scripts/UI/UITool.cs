using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using System.Collections.Generic;

public class UITool
{
    #region 查找
    public static T GetUIComponent<T>(Transform container, string UIPath) where T : UnityEngine.Component
    {
        if (container == null)
        {
            Debug.LogWarning("没有找到container  UIPath = " + UIPath);
            return null;
        }
        Transform tran = container.Find(UIPath);
        if (tran == null)
        {
            Debug.LogWarning(container.name + "中没有找到" + UIPath);
            return null;
        }
        T tempObj = tran.GetComponent<T>();
        if (tempObj == null)
        {
            Debug.LogWarning(container.name + "/" + UIPath + "没有找到" + typeof(T).Name);
            return null;
        }
        return tempObj;
    }

    public static T AddUIComponent<T>(GameObject skin) where T : UnityEngine.Component
    {
        T tempObj = skin.GetComponent<T>();
        if (tempObj == null)
        {
            tempObj = skin.AddComponent<T>();
        }
        return tempObj;
    }

    public static T AddUIComponent<T>(Transform container, string UIPath) where T : UnityEngine.Component
    {
        if (container == null)
        {
            Debug.LogWarning("没有找到container");
            return null;
        }
        Transform tran = container.Find(UIPath);
        if (tran == null)
        {
            Debug.LogWarning(container.name + "中没有找到" + UIPath);
            return null;
        }
        T tempObj = AddUIComponent<T>(tran.gameObject);
        return tempObj;
    }
    #endregion

    #region 添加
    public static T GetUIComponent<T>(string path) where T : UnityEngine.Component
    {
        T tempObj = GameObject.Find(path).GetComponent<T>();
        if (tempObj == null)
        {
            Debug.Log("没有发现这个路径");
            return null;
        }
        return tempObj;
    }
    #endregion

    #region 关闭
    public static void SetAction(GameObject obj)
    {
        obj.SetActive(!obj.activeSelf);
    }
    public static void SetActionTrue(GameObject obj)
    {
        obj.SetActive(true);
    }
    public static void SetActionFalse(GameObject obj)
    {
        obj.SetActive(false);
    }

    public static void SetActive(MonoBehaviour mono, bool show)
    {
        mono.gameObject.SetActive(show);
    }
    #endregion
}
