﻿using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyPageController : UIController<LobbyPageController> ,ILobbyCallbacks, IMatchmakingCallbacks
{


    private List<RoomInfo> roominfoList;
    public LobbyPage lobbyPage;
    public LobbyRoomScrollView lobbyRoomScrollView;
    public override void OnInstance()
    {
        base.OnInstance();
        panel = lobbyPage = LobbyPage.Instance;
    }
    List<object> dataList {
        get {
            List<object> temp = new List<object>();
            foreach (var item in roominfoList)
            {
                temp.Add(item);
            }
            return temp;
        }
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
        lobbyPage.exitLobbyButton.onClick.AddListener(OnExitLobby);
        lobbyPage.createButton.onClick.AddListener(OnCreateRoom);
        lobbyPage.randomEnterRoomButton.onClick.AddListener(OnRandomEnterRoom);
        lobbyRoomScrollView = UITool.AddUIComponent<LobbyRoomScrollView>(lobbyPage.RoomListScrollRect.gameObject);
        lobbyRoomScrollView.enterRoomAction = OnEnterRoom;

        TypedLobby typedLobby = new TypedLobby { Name = "FPS", Type = LobbyType.Default };
        PhotonNetwork.JoinLobby(typedLobby);
  
        //lobbyRoomScrollView.RefreshDisplay(dataList);
    }
    /// <summary>
    /// 进入房间 
    /// </summary>
    /// <param name="obj"></param>
    private void OnEnterRoom(LobbyRoomItemFunction obj)
    {
        //TODO
        RoomInfo roomInfo = (RoomInfo)obj.data;
        PhotonNetwork.JoinRoom(roomInfo.Name);
    }
    /// <summary>
    /// 离开大厅
    /// </summary>
    private void OnExitLobby()
    {
        PhotonNetwork.LeaveLobby();
        UIManager.Close(PageType.LobbyPage);
        UIManager.Open(PageType.LoginPage);
    }
    /// <summary>
    /// 创建房间
    /// </summary>
    private void OnCreateRoom()
    {
        UIManager.Close(PageType.LobbyPage);
        UIManager.Open(PageType.CreateRoomPage);
    }
    /// <summary>
    /// 随机加入房间
    /// </summary>
    private void OnRandomEnterRoom()
    {
        //TODO
        PhotonNetwork.JoinRandomRoom();
    }
    /// <summary>
    /// callback  已经加入大厅
    /// </summary>
    public void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby：已经加入大厅");
    }
    /// <summary>
    /// callback  离开大厅
    /// </summary>
    public void OnLeftLobby()
    {
        Debug.Log("OnLeftLobby");
        PhotonNetwork.Disconnect();
    }
    /// <summary>
    /// callback  更新房间列表
    /// </summary>
    /// <param name="roomList"></param>
    public void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        List<object> rooms = new List<object>();
        Debug.Log("OnRoomListUpdate"+ roomList.Count);
        for (int i = 0; i < roomList.Count; i++)
        {
            rooms.Add(roomList[i]);
        }
        lobbyRoomScrollView.Display(rooms);
    }
    /// <summary>
    /// callback  更新大厅信息    APPSeting  设置
    /// </summary>
    /// <param name="lobbyStatistics"></param>
    public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
    {
        Debug.Log("OnLobbyStatisticsUpdate");
    }

    public void OnFriendListUpdate(List<FriendInfo> friendList)
    {
        //throw new NotImplementedException();
    }

    public void OnCreatedRoom()
    {
        //throw new NotImplementedException();
    }

    public void OnCreateRoomFailed(short returnCode, string message)
    {
        //throw new NotImplementedException();
    }

    public void OnJoinedRoom()
    {
        Debug.Log("JoinedRoom");

        //throw new NotImplementedException();
    }

    public void OnJoinRoomFailed(short returnCode, string message)
    {
        //throw new NotImplementedException();
    }

    public void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.LogWarningFormat("{0}:{1}",returnCode,message);
        UIManager.Close(PageType.LobbyPage);
        UIManager.Open(PageType.CreateRoomPage);
    }

    public void OnLeftRoom()
    {
        //throw new NotImplementedException();
    }
}
