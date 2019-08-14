using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UDragScroll : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IInitializePotentialDragHandler, IScrollHandler
{
    public ScrollRect scroRect;

    public void OnDrag(PointerEventData eventData)
    {
        scroRect.OnDrag(eventData);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        scroRect.OnBeginDrag(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        scroRect.OnEndDrag(eventData);
    }

    public void OnInitializePotentialDrag(PointerEventData eventData)
    {
        scroRect.OnInitializePotentialDrag(eventData);
    }

    public void OnScroll(PointerEventData eventData)
    {
        scroRect.OnScroll(eventData);
    }

}
