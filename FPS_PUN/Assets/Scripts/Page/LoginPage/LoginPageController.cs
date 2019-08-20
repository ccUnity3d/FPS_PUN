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
        PhotonNetwork.AddCallbackTarget(this);
    }
    public override void sleep()
    {
        base.sleep();
        UITool.SetActionFalse(this.skin);
        PhotonNetwork.RemoveCallbackTarget(this);
    }
    public override void ready()
    {
        base.ready();
        loginPage.loginButton.onClick.AddListener(OnLogin);
        loginPage.exitButton.onClick.AddListener(OnExit);
        loginPage.nicknameInputField.textComponent.text = PlayerPrefs.GetString("UserName");

        //loginPage.creatRoomButton.onClick.AddListener(OnCreateRoom);

        //MyTickManager.Instance.add(LoginUpdate);
       
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
        if (loginPage.nicknameInputField.textComponent.text=="")
        {
            loginPage.nicknameInputField.textComponent.text = "player" + Random.Range(1,100);
        }
        PhotonNetwork.LocalPlayer.NickName = loginPage.nicknameInputField.textComponent.text;
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + "   NickName");
        PlayerPrefs.SetString("UserName", loginPage.nicknameInputField.textComponent.text);
        PlayerPrefs.Save();
        if (!PhotonNetwork.IsConnected)
            PhotonNetwork.ConnectUsingSettings();
    }

    private void OnExit()
    {
        Application.Quit();
    }

    //private void OnCreateRoom()
    //{
    //    PhotonNetwork.CreateRoom("FPS", new RoomOptions { MaxPlayers = 10 }, TypedLobby.Default);
    //}
    /// <summary>
    /// 已经连接
    /// </summary>
    public void OnConnected()
    {
        Debug.Log("OnConnected");
    }
    /// <summary>
    /// 连接到MasterServer
    /// </summary>
    public void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster");
        UIManager.Close(PageType.LoginPage);
        UIManager.Open(PageType.LobbyPage);
    }
    /// <summary>
    /// 断开连接 
    /// </summary>
    /// <param name="cause"></param>
    public void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("OnDisconnected");
    }
    /// <summary>
    /// 接收区域列表
    /// </summary>
    /// <param name="regionHandler"></param>
    public void OnRegionListReceived(RegionHandler regionHandler)
    {
        Debug.Log("OnRegionListReceived");
    }
    /// <summary>
    /// 用户验证响应
    /// </summary>
    /// <param name="data"></param>
    public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
    {
        Debug.Log("OnCustomAuthenticationResponse");
    }
    /// <summary>
    /// 用户验证失败
    /// </summary>
    /// <param name="debugMessage"></param>
    public void OnCustomAuthenticationFailed(string debugMessage)
    {
        Debug.Log("OnCustomAuthenticationFailed");
    }

    //  PhotonNetwork.Disconnect();
}
