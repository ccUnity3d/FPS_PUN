﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine.UI;

public class RoomPageController : UIController<RoomPageController>, IInRoomCallbacks, IMatchmakingCallbacks
{

    public RoomPage roomPage;
    public TeamRedScrollView redTeamScrollView;
    public TeamBlueScrollView blueTeamScrollView;
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
        panel = roomPage = RoomPage.Instance;
    }
    public override void ready()
    {
        base.ready();
        roomPage.switchTeamButton.onClick.AddListener(OnSwitchTeam);
        roomPage.startGameButton.onClick.AddListener(OnStartGame);


        redTeamScrollView = UITool.AddUIComponent<TeamRedScrollView>(roomPage.redTeamRT.gameObject);
        blueTeamScrollView = UITool.AddUIComponent<TeamBlueScrollView>(roomPage.blueTeamRT.gameObject);
        roomPage.exitButton.onClick.AddListener(OnExitRoom);


        redTeams = redTeamScrollView.skinPools.ToArray();
        blueTeams = blueTeamScrollView.skinPools.ToArray();

        refreshRoomList();

        bool isReady = (bool)PhotonNetwork.LocalPlayer.CustomProperties["isReady"];
        //costomProperties = new ExitGames.Client.Photon.Hashtable() { { "isReady", !isReady } };
        //PhotonNetwork.LocalPlayer.SetCustomProperties(costomProperties);
        if (!PhotonNetwork.IsMasterClient)
        {
            if (isReady)
            {
                roomPage.startGameText.text = "准备";
            }
            else
            {
                roomPage.startGameText.text = "取消准备";
            }
        }
    }

    GameObject[] redTeams;
    GameObject[] blueTeams;
    private ExitGames.Client.Photon.Hashtable costomProperties;
    int teamSize { get { return PhotonNetwork.CurrentRoom.MaxPlayers / 2; } }
    private void refreshRoomList(bool isShowLocalPlayer = false)
    {

        List<object> players = new List<object>();
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            // 每个客户只控制一名玩家。其他人不是本地的。
            if (!isShowLocalPlayer && p.IsLocal)
            {
                Debug.Log("本地玩家");
                continue;
            }
            Debug.Log("添加玩家信息");
            players.Add(p);
        }
        //  刷新 2个战队

        redTeamScrollView.Display(players);

        blueTeamScrollView.Display(players);

        for (int i = 0; i < teamSize; i++)
        {
            if (!redTeams[i].activeSelf)
            {
                //redTeams[i].SetActive(true);
                //redTeams[i].SetActive(true);
                costomProperties = new ExitGames.Client.Photon.Hashtable()
                {
                    { "Team","redTeam"},
                    { "TeamNum",i},
                    { "isReady",false},
                    { "Score",0}
                };

                PhotonNetwork.LocalPlayer.SetCustomProperties(costomProperties);
                players.Add(PhotonNetwork.LocalPlayer);
                redTeamScrollView.Display(players);
                break;
            }
            else if (!blueTeams[i].activeSelf)
            {
                blueTeams[i].SetActive(true);
                //redTeams[i].SetActive(true);
                costomProperties = new ExitGames.Client.Photon.Hashtable()
                {
                    { "Team","blueTeam"},
                    { "TeamNum",i},
                    { "isReady",false},
                    { "Score",0}
                };
                PhotonNetwork.LocalPlayer.SetCustomProperties(costomProperties);
                players.Add(PhotonNetwork.LocalPlayer);
                blueTeamScrollView.Display(players);
                break;
            }
        }
    }
    private void OnSwitchTeam()
    {
        // TODO
            
        Debug.Log("SwitchTeam");
        List<object> players = new List<object>();
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            // 每个客户只控制一名玩家。其他人不是本地的。

            players.Add(p);
        }
        costomProperties = PhotonNetwork.LocalPlayer.CustomProperties;
        bool isSwitched = false;
        if (costomProperties["Team"].ToString().Equals("redTeam"))
        {
            for (int i = 0; i < teamSize; i++)
            {
                if (!blueTeams[i].activeSelf)
                {
                    isSwitched = true;
                    costomProperties = new ExitGames.Client.Photon.Hashtable() {
                        { "Team","blueTeam"},{ "TeamNum",i}
                    };
                    PhotonNetwork.LocalPlayer.SetCustomProperties(costomProperties);
                    blueTeamScrollView.Display(players);
                    redTeamScrollView.Display(players);
                    Debug.Log("blueTeam");
                    break;
                }
            }
        }
        else if (costomProperties["Team"].ToString().Equals("blueTeam"))
        {
            for (int i = 0; i < teamSize; i++)
            {
                if (!redTeams[i].activeSelf)
                {
                    isSwitched = true;
                    costomProperties = new ExitGames.Client.Photon.Hashtable() {
                        { "Team","redTeam"},{ "TeamNum",i}
                    };
                    PhotonNetwork.LocalPlayer.SetCustomProperties(costomProperties);
                    redTeamScrollView.Display(players);
                    blueTeamScrollView.Display(players);
                    Debug.Log("redTeam");

                    break;
                }
            }
        }
        if (!isSwitched)
        {
            Debug.Log("另一队伍已满，无法切换");
        }
        else
        {
            Debug.Log("切换成功");
        }
    }
    /// <summary>
    /// 开始游戏
    /// </summary>
    private void OnExitRoom()
    {
        // TODO
     
        PhotonNetwork.LeaveRoom();
      
  
        //TypedLobby typedLobby = new TypedLobby { Name = "FPS", Type = LobbyType.Default };
        //PhotonNetwork.JoinLobby(typedLobby);
    }
    private void OnStartGame()
    {
        // TODO
        Debug.Log("StartGame");

        if (PhotonNetwork.IsMasterClient)
        {
            foreach (Player p in PhotonNetwork.PlayerList)
            {
                if (p.IsLocal) continue;
                if ((bool)p.CustomProperties["isReady"] == false)
                {
                    Debug.Log("有人未准备，游戏无法开始");
                    return;
                }
            }
            PhotonNetwork.CurrentRoom.IsOpen = false;
            costomProperties = PhotonNetwork.LocalPlayer.CustomProperties;
            costomProperties["isReady"] = true;
            //costomProperties = new ExitGames.Client.Photon.Hashtable() { { "isReady", true } };
            PhotonNetwork.LocalPlayer.SetCustomProperties(costomProperties);
            LoadSceneRPC.instance.loadScene();
            //ManagerScene.instance.RPC_loadGameScene();
            // 调用RPC
        }
        else
        {
            OnClickReady();
        }
        refreshPlayList();
    }

    private void refreshPlayList()
    {
        List<object> players = new List<object>();
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            players.Add(p);
        }
        redTeamScrollView.Display(players);
        blueTeamScrollView.Display(players);
    }
    private void OnClickReady()
    {

        costomProperties = PhotonNetwork.LocalPlayer.CustomProperties;
        bool isReady = (bool)costomProperties["isReady"];
        costomProperties["isReady"] = !isReady;
        PhotonNetwork.LocalPlayer.SetCustomProperties(costomProperties);
        if (isReady)
        {
            roomPage.startGameText.text = "准备";
        }
        else
        {
            roomPage.startGameText.text = "取消准备";
        }
    }
    /// <summary>
    /// 玩家进入房间
    /// </summary>
    /// <param name="newPlayer"></param>
    public void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("OnPlayerEnteredRoom:" + newPlayer.NickName);
        List<object> players = new List<object>();
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            if (p.IsLocal)
            {
                continue;
            }
            players.Add(p);
        }
        redTeamScrollView.Display(players);
        blueTeamScrollView.Display(players);
    }
    /// <summary>
    /// 玩家离开房间
    /// </summary>
    /// <param name="otherPlayer"></param>
    public void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("OnPlayerLeftRoom");
        Debug.Log("OnPlayerEnteredRoom:" + otherPlayer.NickName);
        refreshPlayList();
    }
    /// <summary>
    /// 房间属性更新
    /// </summary>
    /// <param name="propertiesThatChanged"></param>
    public void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        Debug.Log("OnRoomPropertiesUpdate");
    }
    /// <summary>
    /// 玩家属性更新
    /// </summary>
    /// <param name="targetPlayer"></param>
    /// <param name="changedProps"></param>
    public void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        Debug.Log("OnPlayerPropertiesUpdate");
        refreshPlayList();
    }
    /// <summary>
    /// MasterClient切换
    /// </summary>
    /// <param name="newMasterClient"></param>
    public void OnMasterClientSwitched(Player newMasterClient)
    {
        //Debug.Log("OnMasterClientSwitched");
        refreshPlayList();
    }

    public void OnFriendListUpdate(List<FriendInfo> friendList)
    {
        throw new System.NotImplementedException();
    }

    public void OnCreatedRoom()
    {
      
    }

    public void OnCreateRoomFailed(short returnCode, string message)
    {
       
    }

    public void OnJoinedRoom()
    {
        
    }

    public void OnJoinRoomFailed(short returnCode, string message)
    {
       
    }

    public void OnJoinRandomFailed(short returnCode, string message)
    {
       
    }

    public void OnLeftRoom()
    {
        Debug.Log("OnLeftRoom");
        Debug.Log(PhotonNetwork.NetworkClientState);
        UIManager.Close(PageType.RoomPage);
        UIManager.Open(PageType.LobbyPage);
    }
}
