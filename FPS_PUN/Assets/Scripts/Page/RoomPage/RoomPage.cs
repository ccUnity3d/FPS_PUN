using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomPage : UIPage<RoomPage> {

    public RectTransform gameRoomPlaneRT;
    #region GameRoomPlane
    public RectTransform redTeamRT;
    public RectTransform blueTeamRT;
    public Button switchTeamButton;
    public Button startGameButton;
    public Text startGameText;
    public Button exitButton;
    public Text gameRoomNameText;
    #endregion
    public override void OnInstance()
    {
        base.OnInstance();
        prefabPath = "RoomPage.assetbundle";
    }
    protected override void Ready(Object arg1)
    {
        base.Ready(arg1);
        gameRoomPlaneRT = UITool.GetUIComponent<RectTransform>(skin.transform, "GameRoomPlane");
        redTeamRT = UITool.GetUIComponent<RectTransform>(gameRoomPlaneRT, "redTeam/Scroll Rect Mask");
        blueTeamRT = UITool.GetUIComponent<RectTransform>(gameRoomPlaneRT, "blueTeam/Scroll Rect Mask");
        switchTeamButton = UITool.GetUIComponent<Button>(gameRoomPlaneRT, "switchTeamButton");
        startGameButton = UITool.GetUIComponent<Button>(gameRoomPlaneRT, "startGameButton");
        startGameText = UITool.GetUIComponent<Text>(startGameButton.transform,"Text");
        exitButton = UITool.GetUIComponent<Button>(gameRoomPlaneRT, "exitButton");
        gameRoomNameText = UITool.GetUIComponent<Text>(gameRoomPlaneRT, "titleText");
    }
}
