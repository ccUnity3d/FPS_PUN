using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPage :UIPage<LobbyPage> {


    public Button exitLobbyButton;
    public Button createButton;
    public Button randomEnterRoomButton;
    public RectTransform RoomListScrollRect;
    public override void OnInstance()
    {
        prefabPath = "LobbyPage.assetbundle";
    }
    protected override void Ready(Object arg1)
    {
        base.Ready(arg1);
        exitLobbyButton = UITool.GetUIComponent<Button>(skin.transform, "exitButton");
        createButton = UITool.GetUIComponent<Button>(skin.transform, "createRoomButton");
        randomEnterRoomButton = UITool.GetUIComponent<Button>(skin.transform, "randomEnterRoomButton");
        RoomListScrollRect = UITool.GetUIComponent<RectTransform>(skin.transform, "Lobby-Popup/Scroll Rect Mask");
    }

}
