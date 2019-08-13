using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class LoginPageController : UIController<LoginPageController> , IConnectionCallbacks,ILobbyCallbacks,IInRoomCallbacks, IMatchmakingCallbacks
{

    public LoginPage loginPage;

    public override void OnInstance()
    {
        base.OnInstance();
        panel = loginPage = LoginPage.Instance;
    }
    public override void sleep()
    {
        base.sleep();

    }
    public override void ready()
    {
        base.ready();
        loginPage.loginButton.onClick.AddListener(OnLogin);
        loginPage.exitButton.onClick.AddListener(OnExit);
        loginPage.nicknameInputField.textComponent.text = PlayerPrefs.GetString("UserName");

        loginPage.creatRoomButton.onClick.AddListener(OnCreateRoom);

        MyTickManager.Instance.add(LoginUpdate);
        PhotonNetwork.AddCallbackTarget(this);
    }

#if (UNITY_EDITOR)
    private void LoginUpdate()
    {
        loginPage.connectionStateText.text = PhotonNetwork.NetworkClientState.ToString();
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

    private void OnCreateRoom()
    {
        PhotonNetwork.CreateRoom("FPS",new RoomOptions { MaxPlayers = 10},TypedLobby.Default);
    }

    public void OnConnected()
    {
        Debug.Log("OnConnected");
    }

    public void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster");
        TypedLobby typedLobby = new TypedLobby { Name = "FPS", Type = LobbyType.Default };
        PhotonNetwork.JoinLobby(typedLobby);
    }

    public void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("OnDisconnected");
    }

    public void OnRegionListReceived(RegionHandler regionHandler)
    {
        throw new System.NotImplementedException();
    }

    public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
    {
        throw new System.NotImplementedException();
    }

    public void OnCustomAuthenticationFailed(string debugMessage)
    {
        throw new System.NotImplementedException();
    }

    public void OnJoinedLobby()
    {
        Debug.Log(PhotonNetwork.LocalPlayer.NickName);
    }

    public void OnLeftLobby()
    {
        Debug.Log("OnLeftLobby");
    }

    public void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("OnRoomListUpdate");
    }

    public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
    {
        throw new System.NotImplementedException();
       
    }

    public void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("OnPlayerEnteredRoom");
    }

    public void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("OnPlayerLeftRoom");
    }

    public void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        Debug.Log("OnRoomPropertiesUpdate");
    }

    public void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        Debug.Log("OnPlayerPropertiesUpdate");
    }

    public void OnMasterClientSwitched(Player newMasterClient)
    {
        Debug.Log("OnMasterClientSwitched");
    }

    public void OnFriendListUpdate(List<FriendInfo> friendList)
    {
        Debug.Log("OnFriendListUpdate");
    }

    public void OnCreatedRoom()
    {
        Debug.Log("OnCreatedRoom");
        Debug.Log(PhotonNetwork.PlayerList.Length);
    }

    public void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("OnCreateRoomFailed");
    }

    public void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom");
    }

    public void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("OnJoinRoomFailed");
    }

    public void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("OnJoinRandomFailed");
    }

    public void OnLeftRoom()
    {
        Debug.Log("OnLeftRoom");
    }
    //  PhotonNetwork.Disconnect();
}
