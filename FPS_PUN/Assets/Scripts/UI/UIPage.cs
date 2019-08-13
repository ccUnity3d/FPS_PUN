using UnityEngine;
using System.Collections;
using System;

public class UIPage<T>:Singleton<T>, IPage  where T : IInstance, new()
{
    protected string prefabPath;

    private GameObject _skin;
    public RectTransform skinRectTrans;

    public GameObject skin
    {
        get
        {
            return _skin;
        }

        set
        {
            _skin = value;
        }
    }

    public string GetPrefabPath()
    {
        return prefabPath;
    }

    public void SetData(UnityEngine.Object arg1)
    {
        skin = (GameObject)arg1;
        skinRectTrans = skin.GetComponent<RectTransform>();
        skinRectTrans.sizeDelta = Vector2.zero;
        skinRectTrans.anchoredPosition3D = Vector3.zero;
        skinRectTrans.localScale = Vector3.one;
        Ready(arg1);
    }

    public void ChildPageSetData(UnityEngine.Object arg1)
    {
        skin = (GameObject)arg1;
        Ready(arg1);
    }

    protected virtual void Ready(UnityEngine.Object arg1)
    {
        
    }



}
