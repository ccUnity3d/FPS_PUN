using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour {

    public string playerName;
    public int TeamScore;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public PlayerInfo()
    {

    }
    public PlayerInfo(string name, int score)
    {
        playerName = name;
        TeamScore = score;
    }
}
