using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;

public class TeamRedScrollView : UGUIScrollView {


    protected override void Init()
    {
        base.Init();
    }
    public override void Display(List<object> data)
    {
        base.Display(data);
    }
    public override void RefreshDisplay(List<object> data = null, bool restPos = false, bool isChange = false)
    {
        List<object> playerData = new List<object>();
        foreach (var item in data)
        {
            Player player =  item as Player;
            if (player.CustomProperties["Team"].Equals("RedTeam"))
            {
                playerData.Add(item);
            }
        } 

        if (actived == false) return;
        foreach (UGUIItemFunction item in itemDic.Values)
        {
            item.gameObject.SetActive(false);
            skinList.Push(item.gameObject);
        }

        itemDic.Clear();
        if (restPos == true) ResetPostion();
        if (playerData != null)
        {
            this.Msgs = playerData;
        }
        if (playerData != null || isChange)
        {
            SetContentSize(this.Msgs.Count);
        }

        for (int i = 0; i < this.Msgs.Count; i++)
        {
            if ((i < CurrentIndex - UpperLimitIndex) && (CurrentIndex > LowerLimitIndex) && !isChange)
            {
                return;
            }
            skinClone = GetInstance();
            skinClone.transform.SetParent(ContentRectTrans);
            skinClone.transform.localPosition = GetLoaclPosByIndex(i);
            skinClone.transform.localScale = Vector3.one;
            skinClone.GetComponent<RectTransform>().SetSiblingIndex(i);
            UGUIItemFunction func = skinClone.GetComponent<UGUIItemFunction>();
            func.scroRect = ScroRect;
            func.data = this.Msgs[i];
            func.index = i;
            itemDic.Add(i, func);
            ItemAddListion(func);
            ItemChildGameObject(skinClone);
        }
    }

}
