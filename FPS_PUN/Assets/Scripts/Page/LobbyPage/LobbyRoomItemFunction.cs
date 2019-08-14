using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyRoomItemFunction : UGUIItemFunction {


    public Text NoText;
    public Text nameText;
    public Text roomNumberText;
    public Button enterButton;

    public Action<LobbyRoomItemFunction> enterRoomAction;
    private RoomInfo itemData {
        get { return data as RoomInfo; }
    }
    protected override void Awake()
    {
        NoText = UITool.GetUIComponent<Text>(this.transform,"number");
        nameText = UITool.GetUIComponent<Text>(this.transform, "name");
        roomNumberText = UITool.GetUIComponent<Text>(this.transform, "roomNumber");
        enterButton = UITool.GetUIComponent<Button>(this.transform,"enter");
        enterButton.onClick.AddListener(OnEnterRoom);
    }
    void Start()
    {
        enterButton.onClick.AddListener(OnEnterRoom);
    }

    private void OnEnterRoom()
    {
        if (enterRoomAction != null) enterRoomAction(this);
    }

    public override void Render()
    {
        nameText.text = itemData.Name;
        roomNumberText.text = itemData.PlayerCount.ToString();
    }
}
