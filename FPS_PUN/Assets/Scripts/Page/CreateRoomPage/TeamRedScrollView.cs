using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;

public class TeamRedScrollView : UGUIScrollView {

    private Arrangement arragement = Arrangement.Vertical;
    public List<GameObject> skinPools = new List<GameObject>();
    protected override void Init()
    {
        base.Init();
        ItemSkin = transform.Find("item").gameObject;
        ItemSkin.AddComponent<TeamRedItemFunction>();
        UDragScroll uscr = ItemSkin.AddComponent<UDragScroll>();
        uscr.scroRect = ScroRect;
        SetData(640, 80, arragement, 6, 20, 10, 5);
        initObjectPool();
    }
    public int poolNumber= 10;
    private void initObjectPool() {
        for (int i = 0; i < poolNumber; i++)
        {
            GameObject cloneSkin = GameObject.Instantiate(ItemSkin);
            cloneSkin.transform.SetParent(ContentRectTrans);
            cloneSkin.transform.localPosition = GetLoaclPosByIndex(i);
            cloneSkin.transform.localScale = Vector3.one;
            cloneSkin.GetComponent<RectTransform>().SetSiblingIndex(i);
            cloneSkin.SetActive(false);
            skinPools.Add(cloneSkin);
        }
    }
    public override void Display(List<object> data)
    {
        base.Display(data);
    }
    public override void RefreshDisplay(List<object> data = null, bool restPos = false, bool isChange = false)
    {

        foreach (UGUIItemFunction item in itemDic.Values)
        {
            item.gameObject.SetActive(false);
            //skinList.Push(item.gameObject);
        }

        itemDic.Clear();

        List<object> playerData = new List<object>();
        foreach (var item in data)
        {
            Player player =  item as Player;
            if (player.CustomProperties["Team"].Equals("redTeam"))
            {
                playerData.Add(item);
            }
        } 
        if (actived == false) return;
  
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
            Player player = (Player)Msgs[i];
            var index = (int)player.CustomProperties["TeamNum"];
            skinClone = Instance(i);
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
    protected  GameObject Instance(int index)
    {
        var go = skinPools[index];
        go.SetActive(true);
        return go;
    }

}
