using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class LoadSceneRPC : MonoBehaviour {

    public static LoadSceneRPC instance;
    private PhotonView photonview;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        if (photonview == null)
        {
            photonview = GetComponent<PhotonView>();
        }
    }
    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}
   
    public void loadScene() {
        photonview.RPC("loadSceneRPC",RpcTarget.All,null);
    }
    [PunRPC]

    private void loadSceneRPC(PhotonMessageInfo info) {
        PhotonNetwork.LoadLevel("GameScene");
    }
}
