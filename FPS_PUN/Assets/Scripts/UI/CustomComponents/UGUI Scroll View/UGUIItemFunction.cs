using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System;

public abstract class UGUIItemFunction : MonoBehaviour {


    public int index;
    public ScrollRect scroRect;
    private object _data;
    public object data
    {
        get { return _data;}
        set { _data = value;
            Render();
        }
    }
    
    protected abstract void Awake();
        

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    public virtual void Render()
    {

    }
}
