using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class SendMessage : MonoBehaviour , IOnEventCallback




{
    public void OnEvent(EventData photonEvent)
    {

           
    }

    // Use this for initialization
    void Awake() {
        byte evCode = 0;
        byte[] content = new byte[] {1,2,3,4,5,6 };
        RaiseEventOptions reo = new RaiseEventOptions();
        reo.CachingOption = EventCaching.AddToRoomCache;
        reo.InterestGroup = 0;
        int[] TargetActors = new int[PhotonNetwork.PlayerListOthers.Length]; 
        for (int i = 0; i < PhotonNetwork.PlayerListOthers.Length; i++)
        {
            TargetActors [i] = PhotonNetwork.PlayerListOthers[i].ActorNumber;
        }
        reo.TargetActors = TargetActors;
        reo.Receivers = ReceiverGroup.Others;
        PhotonNetwork.RaiseEvent(evCode,content, reo,SendOptions.SendReliable);
    }
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
