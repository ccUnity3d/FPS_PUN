using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 添加组建后 在监听点击事件前必须先激活一次//
/// </summary>
public class ToggleButton : Button { 

    public bool onDown
    {
        get { return _onDown; }
        set {
            
            _onDown = value;
            //Debug.LogWarning(_onDown);
            ChangeShow(_onDown);
        }
    }

    protected GameObject background;
    private bool _onDown = false;

    protected override void Awake()
    {
        base.Awake();
        background = transform.Find("background").gameObject;
        this.transition = Transition.None;
        this.onClick.AddListener(ontoggle);
    }

    protected virtual void ontoggle()
    {
        onDown = !onDown;
    }
    
    private void ChangeShow(bool show)
    {
        background.SetActive(show);
    }

}

public class ConditionToggleButton : ToggleButton
{
    protected override void ontoggle()
    {
        
    }
}
