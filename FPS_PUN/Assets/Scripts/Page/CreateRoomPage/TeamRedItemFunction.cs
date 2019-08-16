using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;

public class TeamRedItemFunction : UGUIItemFunction {

    public Text nameText;
    public Text stateText;
    public Player itemData {
        get { return  data as Player; }
    }
    protected override void Awake()
    {
        nameText = UITool.GetUIComponent<Text>(this.transform,"name");
        stateText = UITool.GetUIComponent<Text>(this.transform, "state");
    }
    public override void Render()
    {
        base.Render();
        nameText.text = (string)itemData.CustomProperties["name"];
        stateText.text = (string)itemData.CustomProperties["state"];
    }
}
