using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyPageController : UIController<LobbyPageController> ,ILobbyCallbacks//, IInRoomCallbacks, IMatchmakingCallbacks
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
    }
    public override void sleep()
    {
        base.sleep();
        UITool.SetActionFalse(this.skin);

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
        PhotonNetwork.AddCallbackTarget(this);
        //lobbyRoomScrollView.RefreshDisplay(dataList);
    }

    private void OnEnterRoom(LobbyRoomItemFunction obj)
    {
      
    }

    private void OnExitLobby()
    {
       
    }

    private void OnCreateRoom()
    {
        UIManager.Close(PageType.LobbyPage);
        UIManager.Open(PageType.CreateRoomPage);
    }

    private void OnRandomEnterRoom()
    {
        
    }

    public void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby");
    }

    public void OnLeftLobby()
    {
        Debug.Log("OnLeftLobby");
    }

    public void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("OnRoomListUpdate"+ roomList.Count);
    }

    public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
    {
        Debug.Log("OnLobbyStatisticsUpdate");
    }
}
