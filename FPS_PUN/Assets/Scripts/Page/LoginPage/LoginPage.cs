using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LoginPage : UIPage<LoginPage> {

    public Button loginButton;
    public Button exitButton;
    public InputField nicknameInputField;
    public Text connectionStateText;

    public Button creatRoomButton;
    public LoginPage()
    {
        prefabPath = "LoginPage.assetbundle";
    }

    protected override void Ready(Object arg1)
    {
        base.Ready(arg1);
        loginButton = UITool.GetUIComponent<Button>(skin.transform, "LoginPlane/loginButton");
        exitButton = UITool.GetUIComponent<Button>(skin.transform,"LoginPlane/exitButton");
        nicknameInputField = UITool.GetUIComponent<InputField>(skin.transform,"LoginPlane/nickname_InputField");
        connectionStateText = UITool.GetUIComponent<Text>(skin.transform,"connectionStateText");

        creatRoomButton = UITool.GetUIComponent<Button>(skin.transform, "LoginPlane/creatRoomButton");

    }

}
