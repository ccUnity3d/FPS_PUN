using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

/// <summary>
/// 连接流程        1.connection MasterServer   2. connection GameServer  
/// CallBack        1.OnConnectedToMaster   2.OnJoinedLobby  3.OnCreatedRoom   4.OnJoinedRoom
/// </summary>
[RequireComponent(typeof(PhotonView))]
public class PhotonConnection : MonoBehaviourPunCallbacks,IPunObservable
{ 
    private bool isConnecting;
    private TypedLobby typeLobby;
    private RoomOptions roomOptions;
    private void Start()
    {
        isConnecting = true;
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }
    //  masterServer      gameServer    

    private byte maxPlayersPerRoom = 10;

    /// <summary>
    /// 连接到Master 服务器
    /// </summary>
    public override void OnConnectedToMaster()
    {
        Debug.Log("connectedToMaster");
        if (isConnecting)
        {
            // 激活大厅 界面   TODO
            typeLobby = new TypedLobby { Name = "FPSLobby", Type = LobbyType.Default };
            PhotonNetwork.JoinLobby(typeLobby);
        }
    }
    public override void OnDisconnected(DisconnectCause cause) {

        switch (cause)
        {
            case DisconnectCause.None:
                Debug.Log("No error was tracked");
                break;
            case DisconnectCause.ExceptionOnConnect:
                Debug.Log("The server is not available or the address is wrong. Make sure the port is provided and the server is up");
                break;
            case DisconnectCause.Exception:
                Debug.Log("Some internal exception caused the socket code to fail. This may happen if you attempt to connect locally but the server is not available. In doubt: Contact Exit Games");
                break;
            case DisconnectCause.ServerTimeout:
                Debug.Log("The server disconnected this client due to timing out (missing acknowledgement from the client)");
                break;
            case DisconnectCause.ClientTimeout:
                Debug.Log("This client detected that the server's responses are not received in due time ");
                break;
            case DisconnectCause.DisconnectByServerLogic:
                Debug.Log("The server disconnected this client from within the room's logic");
                break;
            case DisconnectCause.DisconnectByServerReasonUnknown:
                Debug.Log("The server disconnected this client for unknown reasons");
                break;
            case DisconnectCause.InvalidAuthentication:
                break;
            case DisconnectCause.CustomAuthenticationFailed:
                break;
            case DisconnectCause.AuthenticationTicketExpired:
                break;
            case DisconnectCause.MaxCcuReached:
                break;
            case DisconnectCause.InvalidRegion:
                break;
            case DisconnectCause.OperationNotAllowedInCurrentState:
                break;
            case DisconnectCause.DisconnectByClientLogic:
                Debug.Log(" The client disconnected from within the logic");
                break;
            default:
                break;
        }
    }


    public override void OnConnected()
    {
        base.OnConnected();
      
    }
    /// <summary>
    /// 连接到游戏大厅
    /// </summary>
    public override void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby");

        //这里应该有按钮触发    离开大厅  创建房间
        PhotonNetwork.LeaveLobby();
    }

    
    /// <summary>
    /// 创建并进入房间  code: 成功
    /// </summary>
    public override void OnCreatedRoom()
    {
        Debug.Log("CreateRoome     : FPS");
        if (isConnecting)
        {
            PhotonNetwork.JoinRoom("FPS");
        }
    }
    /// <summary>
    /// 创建并进入房间 code: 失败
    /// </summary>
    /// <param name="returnCode"></param>
    /// <param name="message"></param>
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        // 创建失败   需要重新创建房间    TODO
        Debug.Log(message+ "    Can't join random room!");
    }
    /// <summary>
    /// 进入房间  code: 成功 
    /// </summary>
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("onjoinedRoom   : FPS");
    }
    /// <summary>
    /// 进入房间   code: 失败
    /// </summary>
    /// <param name="returnCode"></param>
    /// <param name="message"></param>
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        Debug.Log(message);
    }

    /// <summary>
    /// 离开大厅
    /// </summary>
    public override void OnLeftLobby()
    {
        base.OnLeftLobby();
        Debug.Log("OnLeftLobby");
        // 进入房间
        if (isConnecting)
        {
            //PhotonNetwork.JoinRandomRoom();//是否随机加入一个房间    如果没有可以加入的房间  会失败
            roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = maxPlayersPerRoom;
            PhotonNetwork.CreateRoom("FPS", roomOptions, typeLobby);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(null);
        }
    }
}

