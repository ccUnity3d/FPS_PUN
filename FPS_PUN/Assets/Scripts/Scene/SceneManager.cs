using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
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
    public List<Vector3> teamOneSpawnTransform = new List<Vector3>();
    public List<Vector3> teamTwoSpawnTransform = new List<Vector3>();

    public enum GameState {
        PreStart, Playing, GameWin, GameLose, Tie
    }

    public GameState state = GameState.PreStart;

    void Start() {
        photonView.RPC("ConfirmLoad", RpcTarget.All);
        playCustomProperties = new Hashtable { { "Score", 0 } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(playCustomProperties);
        currentScoreOfTeam2 = 0;
        currentScoreOfTeam1 = 0;
        UpdateScore(currentScoreOfTeam1, currentScoreOfTeam2);
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("SetTime", RpcTarget.All, PhotonNetwork.Time);
        }
    }
    void Update() {
        countDown = endTime - PhotonNetwork.Time;
        if (countDown >= photonCircleTime)
        {
            countDown -= photonCircleTime;
        }
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
    /// 更新得分信息
    /// </summary>
    /// <param name="team1"></param>
    /// <param name="team2"></param>
    [PunRPC]
    void UpdateScore(int team1, int team2) {

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
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
     
    }
}
