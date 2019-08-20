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
        nameText.text = (string)itemData.NickName;
        if (itemData.IsMasterClient)
        {
                stateText.text = "房主";
        }
        else {
            if ((bool)itemData.CustomProperties["isReady"])
            {
                stateText.text = "准备";
            }
            else
            {
                stateText.text = "未准备";
            }
        }
        
    }
}
