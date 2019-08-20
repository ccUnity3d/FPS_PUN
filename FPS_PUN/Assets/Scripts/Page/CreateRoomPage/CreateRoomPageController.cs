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

      
    }
    /// <summary>
    /// 切换队伍
    /// </summary>
 

    private void OnCancelCreateRoom()
    {
        //TODO
        Debug.Log("CanacelCreateRoom");
        UIManager.Close(PageType.CreateRoomPage);
        UIManager.Open(PageType.LobbyPage);
    }
    /// <summary>
    /// 创建房间
    /// </summary>
    private void OnConfirmCreateRoom()
    {
        string nameRoom = createRoom.roomNameInputField.textComponent.text;
        PhotonNetwork.CreateRoom(nameRoom,new RoomOptions { MaxPlayers = 20});
    }
    /// <summary>
    /// callback  玩家进入房间
    /// </summary>
    /// <param name="newPlayer"></param>
    public void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("玩家进入房间");
    }
    /// <summary>
    /// callback  玩家离开房间
    /// </summary>
    /// <param name="otherPlayer"></param>
    public void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("玩家离开房间");
    }
    /// <summary>
    /// callback  更新房间属性
    /// </summary>
    /// <param name="propertiesThatChanged"></param>
    public void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        Debug.Log("更新房间属性");
    }
    /// <summary>
    /// callback  更新玩家属性
    /// </summary>
    /// <param name="targetPlayer"></param>
    /// <param name="changedProps"></param>
    public void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        Debug.Log("更新玩家属性");
  
    }
    /// <summary>
    /// callback   切换 MasterClient
    /// </summary>
    /// <param name="newMasterClient"></param>
    public void OnMasterClientSwitched(Player newMasterClient)
    {
        Debug.Log("切换MasterClient");
    }
    /// <summary>
    /// callback 更新好友列表
    /// </summary>
    /// <param name="friendList"></param>
    public void OnFriendListUpdate(List<FriendInfo> friendList)
    {
        Debug.Log("更新玩家列表");
    }
    /// <summary>
    /// callback  创建房间成功
    /// </summary>
    public void OnCreatedRoom()
    {
        Debug.Log("创建房间成功");
        Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount);
    }
    /// <summary>
    /// callback  创建房间失败
    /// </summary>
    /// <param name="returnCode"></param>
    /// <param name="message"></param>
    public void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("创建房间失败");
    }
    /// <summary>
    /// callback  加入房间成功
    /// </summary>
    public void OnJoinedRoom()
    {
        Debug.Log("加入房间");
        UIManager.Close(PageType.CreateRoomPage);
        UIManager.Open(PageType.RoomPage);
    }
    /// <summary>
    /// callback  加入房间失败
    /// </summary>
    /// <param name="returnCode"></param>
    /// <param name="message"></param>
    public void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("加入房间失败");
    }
    /// <summary>
    /// callback  随机加入房间失败
    /// </summary>
    /// <param name="returnCode"></param>
    /// <param name="message"></param>
    public void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("随机加入房间失败");
    }
    /// <summary>
    /// callback   离开房间
    /// </summary>
    public void OnLeftRoom()
    {
        Debug.Log("离开房间");
    }
}
