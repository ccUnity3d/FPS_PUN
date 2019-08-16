using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RoomPageController : UIController<RoomPageController> {

    public RoomPage roomPage;
    public TeamRedScrollView redTeamScrollView;
    public TeamBlueScrollView blueTeamScrollView;
    public override void awake()
    {
        base.awake();
    }
    public override void sleep()
    {
        base.sleep();
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

        refreshRoomList();
    }
    private ExitGames.Client.Photon.Hashtable costomProperties;
    private void refreshRoomList() {

        int teamSize = PhotonNetwork.PlayerList.Length / 2;
        List<object> players = new List<object>();
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            // 每个客户只控制一名玩家。其他人不是本地的。
            if (p.IsLocal)
            {
                continue;
            }
            players.Add(p);
        }
        for (int i = 0; i < teamSize; i++)
        {
            if (i%2==0)
            {
                // 设置玩家属性
                if (PhotonNetwork.IsMasterClient)
                {
                    costomProperties = new ExitGames.Client.Photon.Hashtable()
                    {
                        { "Team","red"},{"TeamNum",i },{"isReady",false },{"Score" ,0}
                    };
                }
                PhotonNetwork.LocalPlayer.SetCustomProperties(costomProperties);
                PhotonNetwork.CurrentRoom.IsOpen = false;
            }
            else
            {
                // 设置玩家属性
                if (PhotonNetwork.IsMasterClient)
                {
                    costomProperties = new ExitGames.Client.Photon.Hashtable()
                {
                    { "Team","red"},{"TeamNum",i },{"isReady",false },{"Score" ,0}
                };
                }
                PhotonNetwork.LocalPlayer.SetCustomProperties(costomProperties);
                PhotonNetwork.CurrentRoom.IsOpen = false;
            }
        }

        //  刷新 2个战队

            redTeamScrollView.Display(players);

            blueTeamScrollView.Display(players);


    }
    private void OnSwitchTeam()
    {
        // TODO
        Debug.Log("SwitchTeam");
    }
    /// <summary>
    /// 开始游戏
    /// </summary>
    private void OnStartGame()
    {
        // TODO
        Debug.Log("StartGame");
    }
}
