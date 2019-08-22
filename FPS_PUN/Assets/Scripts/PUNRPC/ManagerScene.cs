using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ManagerScene : MonoBehaviour {

    public static ManagerScene instance;

    private PhotonView photonView;
  
    void Awake() {
        if (instance==null)
        {
            instance = this;
        }
    }
    // Use this for initialization
    void Start() {
        photonView = this.gameObject.GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update() {

    }
    public void RPC_loadGameScene() {
        photonView.RPC("LoadGameScene", RpcTarget.All,"GameScene");
    }
    [PunRPC]
    public void LoadGameScene(string scene) {
        PhotonNetwork.LoadLevel(scene);
    }
}
