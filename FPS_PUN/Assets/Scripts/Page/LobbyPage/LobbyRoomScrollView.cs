using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyRoomScrollView : UGUIScrollView {

    public Action<LobbyRoomItemFunction> enterRoomAction;
    private Arrangement arragement = Arrangement.Vertical;
    protected override void Init()
    {
        base.Init();
        ItemSkin = transform.Find("item").gameObject;
        ItemSkin.AddComponent<LobbyRoomItemFunction>();
        UDragScroll uscr = ItemSkin.AddComponent<UDragScroll>();
        uscr.scroRect = ScroRect;
        SetData(640, 80, arragement, 6, 20, 10, 5);
    }
    public override void RefreshDisplay(List<object> data = null, bool restPos = false, bool isChange = false)
    {
        base.RefreshDisplay(data, restPos, isChange);
    }
    protected override void ItemAddListion(UGUIItemFunction func)
    {
        LobbyRoomItemFunction itemFunc = func as LobbyRoomItemFunction;
        itemFunc.enterRoomAction = OnEnterRoom;
    }

    private void OnEnterRoom(LobbyRoomItemFunction obj)
    {
        if (enterRoomAction != null) enterRoomAction(obj);
    }

    // Use this for initialization
    void Start () {
		
	}
	
}
