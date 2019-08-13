using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class DrapButton : Button, IDragHandler
{
    public DrapButton():base()
    {
        targetGraphic = GetComponent<Graphic>();
    }
    public Action<PointerEventData> onDragDele;
    public Action<PointerEventData> onPointerDownDele;
    public Action<PointerEventData> onPointerUpDele;
    public Action onPointerClickDele;
    private bool moved = false;

    public void OnDrag(PointerEventData eventData)
    {
        moved = true;
        if (onDragDele != null) onDragDele(eventData);
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        moved = false;
        if (onPointerDownDele != null) onPointerDownDele(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        if (onPointerUpDele != null) onPointerUpDele(eventData);
        if(moved == false && onPointerClickDele != null) onPointerClickDele();
    }
}
