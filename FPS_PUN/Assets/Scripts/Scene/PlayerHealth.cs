using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerHealth : MonoBehaviour
{

    private PhotonView photonView;
    //---配置表格------//
    private int maxHp = 100;
    private int killScore = 10;
    private GameObject gun;      //player gun
    private float respawnTime = 5.0f;//玩家对象死亡后重生时间
    private float invincibleTime = 3.0F; // 玩家对象无敌时间

    private int team;
    private bool isAlive;  //玩家对象是否存活
    private int currentHP;//玩家对象当前生命值
    private bool invincible;//玩家对象是否无敌


    private Rigidbody rigidbody;
    private CapsuleCollider capsuleCollider;
    private Animator animator;
    private float timer;
    private IKController ikController;
    // Use this for initialization
    void Start()
    {
        Init();
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        photonView = GetComponent<PhotonView>();
        ikController = GetComponent<IKController>();
        if (!photonView.IsMine) return;
        photonView.RPC("UpdateHP", RpcTarget.Others, currentHP);
        if (PhotonNetwork.LocalPlayer.CustomProperties["Team"].Equals("redTeam"))
            team = 1;
        else team = 2;
        photonView.RPC("SetTeam", RpcTarget.Others, team);
    }


    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine) return;
        timer += Time.deltaTime;
        if (timer > invincibleTime && invincible == true)
        {
            photonView.RPC("SetInvincible", RpcTarget.All, false);
        }
        else if (timer <= invincibleTime && invincible == false) {
            photonView.RPC("SetInvincible", RpcTarget.All, true);
        }
    }
    void Init()
    {
        currentHP = maxHp;
        isAlive = true;
        timer = 0.0f;
        invincible = true;
    }
    [PunRPC]
    private void UpdateHP(int newHP)
    {
        currentHP = newHP;
        if (currentHP<=0)
        {
            isAlive = false;
            if (photonView.IsMine)
            {
                animator.SetBool("isDead",true);
                Invoke("PlayerSpawn",respawnTime);
            }
            rigidbody.useGravity = false;
            capsuleCollider.enabled = false;
            gun.SetActive(false);
            animator.applyRootMotion = true;
            ikController.enabled = false;
        }
    }
    [PunRPC]
    private void SetTeam(int newTeam)
    {
        team = newTeam;
    }
    [PunRPC]
    private void SetInvincible(bool isInvincible) {
        invincible = isInvincible;
    }
}