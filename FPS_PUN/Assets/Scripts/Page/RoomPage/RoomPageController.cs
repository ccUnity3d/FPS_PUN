using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine.UI;

public class RoomPageController : UIController<RoomPageController> ,IInRoomCallbacks{

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
       
        refreshRoomList();
    
    }
    private ExitGames.Client.Photon.Hashtable costomProperties;
    private void refreshRoomList() {
        Debug.Log(" refreshRoomList  "+PhotonNetwork.PlayerList.Length);
        int teamSize = PhotonNetwork.CurrentRoom.MaxPlayers / 2;
        List<object> players = new List<object>();
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            // 每个客户只控制一名玩家。其他人不是本地的。
            if (p.IsLocal)
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

        GameObject[] redTeams = redTeamScrollView.skinPools.ToArray();
        GameObject[] blueTeams = blueTeamScrollView.skinPools.ToArray();

        for (int i = 0; i < teamSize; i++)
        {
            if (!redTeams[i].activeSelf)
            {
                redTeams[i].SetActive(true);
                //redTeams[i].SetActive(true);
                costomProperties = new ExitGames.Client.Photon.Hashtable()
                {
                    { "Team","redTeam"},
                    { "TeamNum",i},
                    { "isReady",false},
                    { "Score",0}
                };
               var  nameText = UITool.GetUIComponent<Text>(redTeams[i].transform, "name");
                nameText.text = PhotonNetwork.LocalPlayer.NickName;
               var stateText = UITool.GetUIComponent<Text>(redTeams[i].transform, "state");
                if (PhotonNetwork.IsMasterClient)
                {
                    stateText.text = "房主";
                }
                else
                {
                    stateText.text = "未准备";
                }
                PhotonNetwork.LocalPlayer.SetCustomProperties(costomProperties);
                break;
            }
            else if(!blueTeams[i].activeSelf)
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
                var nameText = UITool.GetUIComponent<Text>(blueTeams[i].transform, "name");
                nameText.text = PhotonNetwork.LocalPlayer.NickName;
                var stateText = UITool.GetUIComponent<Text>(blueTeams[i].transform, "state");
                if (PhotonNetwork.IsMasterClient)
                {
                    stateText.text = "房主";
                }
                else
                {
                    stateText.text = "未准备";
                }
                PhotonNetwork.LocalPlayer.SetCustomProperties(costomProperties);
                break;
            }
        }
    }
    private void OnSwitchTeam()
    {
        // TODO
        Debug.Log("SwitchTeam");
    }
    /// <summary>
    /// 开始游戏
    /// </summary>
    private void OnExitRoom()
    {
        // TODO
        Debug.Log("OnExitRoom");
        PhotonNetwork.LeaveRoom();
        UIManager.Close(PageType.RoomPage);
        UIManager.Open(PageType.LobbyPage);
    }
    private void OnStartGame()
    {
        // TODO
        Debug.Log("StartGame");
    }
    /// <summary>
    /// 玩家进入房间
    /// </summary>
    /// <param name="newPlayer"></param>
    public void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("OnPlayerEnteredRoom:"+newPlayer.NickName);
    }
    /// <summary>
    /// 玩家离开房间
    /// </summary>
    /// <param name="otherPlayer"></param>
    public void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("OnPlayerLeftRoom");
        Debug.Log("OnPlayerEnteredRoom:" + otherPlayer.NickName);
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
    }
    /// <summary>
    /// MasterClient切换
    /// </summary>
    /// <param name="newMasterClient"></param>
    public void OnMasterClientSwitched(Player newMasterClient)
    {
        Debug.Log("OnMasterClientSwitched");
    }
}
