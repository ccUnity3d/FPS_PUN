using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class MyScrollRect : ScrollRect {
    public Action OnScrollDraged;
    public Action<PointerEventData> OnScrollDragedStart;
    public Action<PointerEventData> OnScrollDragedEnd;
    private bool moveEnd = false;

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
        if (OnScrollDraged != null) OnScrollDraged();
    }
    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        if (OnScrollDragedStart != null) OnScrollDragedStart(eventData);
    }
    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        if (OnScrollDragedEnd != null)
        {
            StopMovement();
            OnScrollDragedEnd(eventData);
        }
    }  
}
