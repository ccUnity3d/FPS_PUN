using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ClickButton : Button {

    protected GameObject background;

    protected override void Awake()
    {
        base.Awake();
        this.transition = Transition.None;
        background = transform.Find("background").gameObject;
    }
    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        background.SetActive(true);
    }
    public override void OnPointerUp(PointerEventData eventData)
    {
        background.SetActive(false);
    }


    
}
