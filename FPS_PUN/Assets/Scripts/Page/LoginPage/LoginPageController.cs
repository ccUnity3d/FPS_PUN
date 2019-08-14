using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class LoginPageController : UIController<LoginPageController> , IConnectionCallbacks
{

    public LoginPage loginPage;

    public override void OnInstance()
    {
        base.OnInstance();
        panel = loginPage = LoginPage.Instance;
    }
    public override void awake()
    {
        base.awake();
        UITool.SetActionTrue(this.skin);
    }
    public override void sleep()
    {
        base.sleep();
        UITool.SetActionFalse(this.skin);

    }
    public override void ready()
    {
        base.ready();
        loginPage.loginButton.onClick.AddListener(OnLogin);
        loginPage.exitButton.onClick.AddListener(OnExit);
        loginPage.nicknameInputField.textComponent.text = PlayerPrefs.GetString("UserName");

        //loginPage.creatRoomButton.onClick.AddListener(OnCreateRoom);

        MyTickManager.Instance.add(LoginUpdate);
        PhotonNetwork.AddCallbackTarget(this);
    }
    ClientState ClientState = ClientState.PeerCreated;
#if (UNITY_EDITOR)
    private void LoginUpdate()
    {
        loginPage.connectionStateText.text = PhotonNetwork.NetworkClientState.ToString();
        if (PhotonNetwork.NetworkClientState != ClientState)
        {
            ClientState = PhotonNetwork.NetworkClientState;
            Debug.LogWarning(PhotonNetwork.NetworkClientState.ToString());
        }
    }
#endif
    private void OnLogin()
    {
        if (!PhotonNetwork.IsConnected)
            PhotonNetwork.ConnectUsingSettings();
        if (loginPage.nicknameInputField.textComponent.text=="")
        {
            loginPage.nicknameInputField.textComponent.text = "player" + Random.Range(1,100);
            PhotonNetwork.LocalPlayer.NickName = loginPage.nicknameInputField.textComponent.text;
            PlayerPrefs.SetString("UserName", loginPage.nicknameInputField.textComponent.text);
        }
    }

    private void OnExit()
    {
        Application.Quit();
    }

    //private void OnCreateRoom()
    //{
    //    PhotonNetwork.CreateRoom("FPS", new RoomOptions { MaxPlayers = 10 }, TypedLobby.Default);
    //}

    public void OnConnected()
    {
        Debug.Log("OnConnected");
    }

    public void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster");
     
        UIManager.Close(PageType.LoginPage);
        UIManager.Open(PageType.LobbyPage);
    }

    public void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("OnDisconnected");
    }

    public void OnRegionListReceived(RegionHandler regionHandler)
    {
        Debug.Log("OnRegionListReceived");
    }

    public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
    {
        Debug.Log("OnCustomAuthenticationResponse");
    }

    public void OnCustomAuthenticationFailed(string debugMessage)
    {
        Debug.Log("OnCustomAuthenticationFailed");
    }

    //  PhotonNetwork.Disconnect();
}
