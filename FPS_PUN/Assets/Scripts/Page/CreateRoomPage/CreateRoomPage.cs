using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoomPage : UIPage<CreateRoomPage> {

    public RectTransform createRoomPlaneRT;
   
    #region CreateRoomPlane
    public InputField roomNameInputField;
    public ToggleGroup playerNumberTG;
    public Button confirmButton;
    public Button cancelButton;
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
        roomNameInputField = UITool.GetUIComponent<InputField>(createRoomPlaneRT, "roomNameTextInputField");
        playerNumberTG = UITool.GetUIComponent<ToggleGroup>(createRoomPlaneRT, "PlayerNumberToggleGroup");
        confirmButton = UITool.GetUIComponent<Button>(createRoomPlaneRT, "confirmButton");
        cancelButton= UITool.GetUIComponent<Button>(createRoomPlaneRT, "cancelButton");
       
    }
}
