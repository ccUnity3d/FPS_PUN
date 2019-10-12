using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
public class SceneManager : MonoBehaviour , IPunObservable
{

    public static SceneManager gm;
    public PhotonView photonView;
    public Hashtable playCustomProperties;
    public int currentScoreOfTeam1;
    public int currentScoreOfTeam2;
    private double startTime;
    private double endTime;
    private double countDown;
    const float photonCircleTime = 4294967.295f;
    private int loadedPlayerNum;
    private float gamePlayingTime;//设置游戏结束时间
    private GameObject localPlayer;
    private float checkPlayerTime = 30;//玩家游戏时间
    public List<Vector3> teamOneSpawnTransform = new List<Vector3>();
    public List<Vector3> teamTwoSpawnTransform = new List<Vector3>();
    public Camera mainCamera;
    public enum GameState {
        PreStart, Playing, GameWin, GameLose, Tie
    }

    public GameState state = GameState.PreStart;

    void Awake() {
        mainCamera = Camera.main;
    }
    void Start() {
        photonView.RPC("ConfirmLoad", RpcTarget.All, loadedPlayerNum);
        playCustomProperties = new Hashtable { { "Score", 0 } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(playCustomProperties);
        currentScoreOfTeam1 = 0;
        currentScoreOfTeam2 = 0;
        UpdateScore(currentScoreOfTeam1, currentScoreOfTeam2);
        
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("SetTime", RpcTarget.All, PhotonNetwork.Time, checkPlayerTime);
        }
    }
    void Update() {
        countDown = endTime - PhotonNetwork.Time;
        if (countDown >= photonCircleTime)
        {
            countDown -= photonCircleTime;
        }
        UpdateTimeLabel();
        switch (state)
        {
            case GameState.PreStart:
                if (PhotonNetwork.IsMasterClient)
                {
                    CheckPlayerConnected();
                }
                break;
            case GameState.Playing:
                break;
            case GameState.GameWin:
                break;
            case GameState.GameLose:
                break;
            case GameState.Tie:
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// 更新得分信息    显示得分面板
    /// </summary>
    /// <param name="team1"></param>
    /// <param name="team2"></param>
    [PunRPC]
    void UpdateScore(int team1, int team2) {
        currentScoreOfTeam1 = team1;
        currentScoreOfTeam2 = team2;
        Player [] players =  PhotonNetwork.PlayerList;
        List<PlayerInfo> teamOne = new List<PlayerInfo>();
        List<PlayerInfo> teamTwo = new List<PlayerInfo>();

        PlayerInfo tempPlayer;
        foreach (Player p in players)
        {
            tempPlayer = new PlayerInfo(p.NickName,(int)p.CustomProperties["Score"]);
            if (p.CustomProperties["Team"].ToString()=="team1")
            {
                teamOne.Add(tempPlayer);
            }
            else
            {
                teamTwo.Add(tempPlayer);
            }
        }
        teamTwo.Sort();
        teamOne.Sort();
    }

    /// <summary>
    /// 更新实时分数面板
    /// </summary>
    void UpdateRealTimeScorePanel() {


    }
    [PunRPC]
    void SetTime(double sTime, float dTime) {
        startTime = sTime;
        endTime = sTime + dTime;
    }
    /// <summary>
    /// 检查玩家时候连接
    /// </summary>
    void CheckPlayerConnected() {
        if (countDown <= 0.0f||loadedPlayerNum == PhotonNetwork.PlayerList.Length)
        {
            startTime = PhotonNetwork.Time;
            photonView.RPC("StartGame",RpcTarget.All,startTime);
        }
    }
    [PunRPC]
    void StartGame(double timer) {
        SetTime(timer,gamePlayingTime);
        gm.state = GameState.Playing;
        InstantiatePlayer();
        // 播放音效
    }
    void InstantiatePlayer() {
        playCustomProperties = PhotonNetwork.LocalPlayer.CustomProperties;
        if (playCustomProperties["Team"].ToString().Equals("redTeam"))
        {
            localPlayer =  PhotonNetwork.Instantiate("EthanPlayer", teamOneSpawnTransform[(int)playCustomProperties["TeamNum"]],Quaternion.identity,0);
        }
        else if(playCustomProperties["Team"].ToString().Equals("blueTeam"))
        {
            localPlayer = PhotonNetwork.Instantiate("RobotPlayer", teamTwoSpawnTransform[(int)playCustomProperties["TeamNum"]], Quaternion.identity, 0);
        }

        localPlayer.GetComponent<PlayerMove>().enabled = true;
        PlayerShoot playerShoot = localPlayer.GetComponent<PlayerShoot>();
        PlayerHealth playerHealth = localPlayer.GetComponent<PlayerHealth>();
        // TODO  显示玩家生命值
        Transform tempTransform = localPlayer.transform;
        mainCamera.transform.parent = tempTransform;
        mainCamera.transform.localPosition = playerShoot.shootingPosition;
        mainCamera.transform.rotation = Quaternion.identity;
        for (int i = 0; i < tempTransform.childCount; i++)
        {
            if (tempTransform.GetChild(i).name.Equals("Gun"))
            {
                tempTransform.GetChild(i).parent = mainCamera.transform;
                break;
            }
        }
    }
    [PunRPC]
    public void ConfirmLoad(int loadPlayerNum) {
        loadedPlayerNum = loadedPlayerNum++;
    }

    void UpdateTimeLabel() {

    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
     
    }
}
