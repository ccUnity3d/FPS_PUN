using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoomPage : UIPage<CreateRoomPage> {

    public RectTransform createRoomPlaneRT;
    public RectTransform gameRoomPlaneRT;
    #region CreateRoomPlane
    public InputField roomNameInputField;
    public ToggleGroup playerNumberTG;
    public Button confirmButton;
    public Button cancelButton;
    #endregion
    #region GameRoomPlane
    public RectTransform redTeamRT;
    public RectTransform blueTeamRT;
    public Button switchTeamButton;
    public Button startGameButton;
    public Text gameRoomNameText;
    #endregion
    public override void OnInstance()
    {
        base.OnInstance();
        prefabPath = "CreateRoomPage.assetbundle";
    }
    protected override void Ready(Object arg1)
    {
        base.Ready(arg1);
        createRoomPlaneRT = UITool.GetUIComponent<RectTransform>(skin.transform,"CreateRoomPlane");
        gameRoomPlaneRT = UITool.GetUIComponent<RectTransform>(skin.transform,"GameRoomPlane");
        roomNameInputField = UITool.GetUIComponent<InputField>(createRoomPlaneRT, "roomNameTextInputField");
        playerNumberTG = UITool.GetUIComponent<ToggleGroup>(createRoomPlaneRT, "PlayerNumberToggleGroup");
        confirmButton = UITool.GetUIComponent<Button>(createRoomPlaneRT, "confirmButton");
        cancelButton= UITool.GetUIComponent<Button>(createRoomPlaneRT, "cancelButton");
        redTeamRT = UITool.GetUIComponent<RectTransform>(gameRoomPlaneRT, "redTeam");
        blueTeamRT = UITool.GetUIComponent<RectTransform>(gameRoomPlaneRT, "blueTeam ");
        switchTeamButton = UITool.GetUIComponent<Button>(gameRoomPlaneRT, "switchTeamButton");
        startGameButton = UITool.GetUIComponent<Button>(gameRoomPlaneRT, "startGameButton");
        gameRoomNameText = UITool.GetUIComponent<Text>(gameRoomPlaneRT, "titleText");
        UITool.SetActionFalse(gameRoomPlaneRT.gameObject);
    }
}
