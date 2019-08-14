using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class CreateRoomPageController : UIController<CreateRoomPageController>,IInRoomCallbacks, IMatchmakingCallbacks
{
    public CreateRoomPage createRoom;
    public TeamScrollView redTeamScrollView;
    public TeamScrollView blueTeamScrollView;
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

    public override void OnInstance()
    {
        base.OnInstance();
        panel = createRoom = CreateRoomPage.Instance;
    }
    public override void ready()
    {
        base.ready();
        createRoom.confirmButton.onClick.AddListener(OnConfirmCreateRoom);
        createRoom.cancelButton.onClick.AddListener(OnCancelCreateRoom);

        createRoom.switchTeamButton.onClick.AddListener(OnSwitchTeam);
        createRoom.startGameButton.onClick.AddListener(OnStartGame);

        redTeamScrollView =  UITool.AddUIComponent<TeamScrollView>(createRoom.redTeamRT.gameObject);
        blueTeamScrollView = UITool.AddUIComponent<TeamScrollView>(createRoom.blueTeamRT.gameObject);
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnSwitchTeam()
    {
        throw new NotImplementedException();
    }

    private void OnStartGame()
    {
        throw new NotImplementedException();
    }

    private void OnCancelCreateRoom()
    {
        throw new NotImplementedException();
    }

    private void OnConfirmCreateRoom()
    {
        string nameRoom = createRoom.roomNameInputField.textComponent.text;
        PhotonNetwork.CreateRoom(nameRoom,new RoomOptions { MaxPlayers = 20});
        UITool.SetActionFalse(createRoom.createRoomPlaneRT.gameObject);
        UITool.SetActionTrue(createRoom.gameRoomPlaneRT.gameObject);
    }

    public void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("玩家进入房间");
    }

    public void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("玩家离开房间");
    }

    public void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        Debug.Log("更新房间属性");
    }

    public void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        Debug.Log("更新玩家属性");
    }

    public void OnMasterClientSwitched(Player newMasterClient)
    {
        Debug.Log("切换MasterClient");
    }

    public void OnFriendListUpdate(List<FriendInfo> friendList)
    {
        Debug.Log("更新玩家列表");
    }

    public void OnCreatedRoom()
    {
        Debug.Log("创建房间成功");
    }

    public void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("创建房间失败");
    }

    public void OnJoinedRoom()
    {
        Debug.Log("加入房间");
        Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount);
    }

    public void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("加入房间失败");
    }

    public void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("随机加入房间失败");
    }

    public void OnLeftRoom()
    {
        Debug.Log("离开房间");
    }
}
