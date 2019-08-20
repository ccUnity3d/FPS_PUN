using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class UGUIScrollView : MonoBehaviour
{
    protected MyEventDispatcher eventDispatcher = new MyEventDispatcher();

    public Dictionary<int, UGUIItemFunction> itemDic = new Dictionary<int, UGUIItemFunction>();

    public Stack<GameObject> skinList = new Stack<GameObject>();

    #region 测试用的数据
    //[Tooltip("单元格 宽")]
    //public int cellWidth;
    //[Tooltip("单元格 高")]
    //public int cellHeight;
    //[Tooltip("单元格 宽 间距")]
    //public int cellWidthSpace;
    //[Tooltip("单元格 高 间距")]
    //public int cellHeightSpace;
    //[Tooltip("视图显示数量")]
    //public int viewShowCount;
    //[Tooltip("每行 显示单元格 最大数量")]
    //public int maxPerLine;
    //[Tooltip("单元格 上限 刷新位置")]
    //public int upperLimitIndex;
    //[Tooltip("单元格 下限 刷新位置")]
    //public int lowerLimitIndex;
    #endregion

    public GameObject skinClone;

    #region  定义 ScrollView 
    [Tooltip("单元格 宽")]
    private int cellWidth;
    public int CellWidth
    {
        get
        {
            return cellWidth;
        }

        set
        {
            cellWidth = value;
        }
    }
    [Tooltip("单元格 高")]
    private int cellHeight;
    public int CellHeight
    {
        get
        {
            return cellHeight;
        }

        set
        {
            cellHeight = value;
        }
    }
    [Tooltip("单元格 宽 间距")]
    private int cellWidthSpace;
    public int CellWidthSpace
    {
        get
        {
            return cellWidthSpace;
        }

        set
        {
            cellWidthSpace = value;
        }
    }
    [Tooltip("单元格 高 间距")]
    private int cellHeightSpace;
    public int CellHeightSpace
    {
        get
        {
            return cellHeightSpace;
        }
        set
        {
            cellHeightSpace = value;
        }
    }
    [Tooltip("每行 显示单元格 最大数量")]
    private int maxPerLine;
    public int MaxPerLine
    {
        get
        {
            return maxPerLine;
        }

        set
        {
            maxPerLine = value;
        }
    }
    [Tooltip("单元格 上限 刷新位置")]
    private int upperLimitIndex;
    public int UpperLimitIndex
    {
        get
        {
            return upperLimitIndex;
        }

        set
        {
            upperLimitIndex = value;
        }
    }
    [Tooltip("单元格 下限 刷新位置")]
    private int lowerLimitIndex;
    public int LowerLimitIndex
    {
        get
        {
            return lowerLimitIndex;
        }

        set
        {
            lowerLimitIndex = value;
        }
    }
    #endregion
    [Tooltip("容器里 左上角第一个的索引")]
    private int currentIndex;
    public int CurrentIndex
    {
        get
        {
            return currentIndex;
        }

        set
        {
            currentIndex = value;
        }
    }
    [Tooltip("再次刷新的索引")]
    private int oldRefreshIndex;
    public int OldRefreshIndex
    {
        get
        {
            return oldRefreshIndex;
        }

        set
        {
            oldRefreshIndex = value;
        }
    }
    [Tooltip("滑动方式")]
    private Arrangement arrangement;
    public Arrangement _Arranement
    {
        get
        {
            return arrangement;
        }

        set
        {
            arrangement = value;
        }
    }
    [Tooltip("item预设")]
    private GameObject itemSkin;
    public GameObject ItemSkin
    {
        get
        {
            return itemSkin;
        }

        set
        {
            itemSkin = value;
        }
    }
    [Tooltip("容器")]
    private RectTransform contentRectTrans;
    public RectTransform ContentRectTrans
    {
        get
        {
            return contentRectTrans;
        }

        set
        {
            contentRectTrans = value;
        }
    }
    [Tooltip("scrollView 位置 ")]
    private RectTransform scrollRectTrans;
    public RectTransform ScrollRectTrans
    {
        get
        {
            return scrollRectTrans;
        }

        set
        {
            scrollRectTrans = value;
        }
    }
    [Tooltip("自己封装 ScrollRect")]
    private MyScrollRect scroRect;
    public MyScrollRect ScroRect
    {
        get
        {
            return scroRect;
        }

        set
        {
            scroRect = value;
        }
    }
    [Tooltip("服务器 传过来的数据")]
    protected List <object> msgs = new List<object>();
    protected RectTransform viewRect;

    public List<object> Msgs
    {
        get
        {
            return msgs;
        }

        set
        {
            msgs = value;
        }
    }

    protected bool actived = false;

    void Awake()
    {
        actived = true;
        Debug.Log(this.GetType().Name + "   Awake");
        ScroRect = this.gameObject.AddComponent<MyScrollRect>();
        //ScroRect.horizontal = false;
        ScroRect.OnScrollDraged = OnScrollDraged;
        ScrollRectTrans = this.GetComponent<RectTransform>();
        viewRect = new GameObject("Viewport",typeof(RectTransform)).GetComponent<RectTransform>();
        viewRect.SetParent(transform,false);
        viewRect.pivot = Vector2.up;
        viewRect.anchorMax = Vector2.one;
        viewRect.anchorMin = Vector2.zero;
        viewRect.anchoredPosition = Vector2.zero;
        viewRect.sizeDelta = Vector2.zero;
        viewRect.gameObject.AddComponent<CanvasRenderer>();
        viewRect.gameObject.AddComponent<Image>();
        viewRect.gameObject.AddComponent<Mask>().showMaskGraphic = false ;
        ContentRectTrans = new GameObject("Content",typeof(RectTransform)).GetComponent<RectTransform>();
        ContentRectTrans.SetParent(viewRect,false);
        ContentRectTrans.pivot = Vector2.up;
        ContentRectTrans.anchorMin = Vector2.up;
        ContentRectTrans.anchorMax = Vector2.up;
        ContentRectTrans.anchoredPosition = Vector2.zero;
        ContentRectTrans.sizeDelta = ScrollRectTrans.sizeDelta;
        ScroRect.content = ContentRectTrans;
        Init();
    }

    protected virtual void Init()
    {
        //ItemSkin = transform.Find("item").gameObject;
        //ItemSkin.AddComponent<UDragScroll>();
        //ItemSkin.AddComponent<UGUIItemFunction>();
        //switch (_Arranement)
        //{
        //    case Arrangement.Horizontal:
        //        SetData((int)ScrollRectTrans.sizeDelta.x, 200, _Arranement, 6, 20);
        //        break;
        //    case Arrangement.Vertical:
        //        SetData((int)ScrollRectTrans.sizeDelta.x, 200, _Arranement, 6, 20);
        //        break;
        //    default:
        //        break;
        //}
    }
    /// <summary>
    /// 设置容器单元格
    /// </summary>
    /// <param name="cellWidth">单元格宽</param>
    /// <param name="cellHeight">单元格高</param>
    /// <param name="arrangement">单元格滑动方向 [水平 or 垂直]</param>
    /// <param name="upperLimitIndex">超过最大上限，进行垃圾回收</param>
    /// <param name="lowerLimitIndex">超过最大下限，进行资源加载</param>
    /// <param name="cellHeightSpace">单元格与单元格之间的高度</param>
    /// <param name="cellWidthSpace">单元格与单元格之间的宽度</param>
    /// <param name="maxPerLine">这个根据 单元格滑动方向来定， 垂直 ，就是每行最多有多少个单元格  水平表示每列最多有多少单元格</param>
    protected void SetData(int cellWidth, int cellHeight, Arrangement arrangement, int upperLimitIndex=0, int lowerLimitIndex=0,
                           int cellHeightSpace=0, int cellWidthSpace=0, int maxPerLine=1)
    {
        this.CellWidth = cellWidth;
        this.CellHeight = cellHeight;
        this.CellWidthSpace = cellWidthSpace;
        this.CellHeightSpace = cellHeightSpace;
        this.MaxPerLine = maxPerLine;
        this._Arranement = arrangement;
        switch (arrangement)
        {
            case Arrangement.Horizontal:
                scroRect.vertical = false;
                break;
            case Arrangement.Vertical:
                scroRect.horizontal = false;
                break;
            default:
                scroRect.horizontal = true;
                scroRect.vertical = true;
                break;
        }
    }
    public virtual void Display(List<object> data)
    {
        if (actived == false) return;
        RefreshDisplay(data,true);
    }
    /// <summary>
    /// 刷新
    /// </summary>
    /// <param name="data">data = null  表示没有增加数据 </param>
    /// <param name="restPos"> restPos = false  表示不用重置位置</param>
    /// <param name="isChange">isChange = false  表示不需要刷新</param>
    public virtual void RefreshDisplay(List<object>  data = null, bool restPos = false, bool isChange = false )
    {
        if (actived == false) return;
        foreach (UGUIItemFunction item in itemDic.Values)
        {
            item.gameObject.SetActive(false);
            skinList.Push(item.gameObject);
        }

        itemDic.Clear();
        if (restPos == true) ResetPostion();
        if (data != null)
        {
            this.Msgs = data;
        }
        if (data != null || isChange)
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

    //protected virtual void AddItem(GameObject addSkin,int count =1)
    //{
    //    AddItemPerfab += count;
    //    ResetPostion();
    //    SetContentSize(count);
    //    GameObject addCloneSkin = GameObject.Instantiate(addSkin);
    //    addCloneSkin.transform.SetParent(ContentRectTrans);
    //    addCloneSkin.transform.localPosition = GetLoaclPosByIndex(0);
    //    addCloneSkin.transform.localScale = Vector3.one;
    //    addCloneSkin.SetActive(true);
    //}

    // 给每个Item 添加事件
    protected virtual void ItemAddListion(UGUIItemFunction func)
    {

    }
    // 获取Item 子物体
    protected virtual void ItemChildGameObject(GameObject obj = null)
    {
        skinClone.GetComponent<UDragScroll>().scroRect = ScroRect;
    }
    public virtual Vector3 GetLoaclPosByIndex(int index)
    {
        float x = 0f;
        float y = 0f;
        switch (arrangement)
        {
            case Arrangement.Horizontal:
                x = (index / MaxPerLine) * (CellWidth + CellWidthSpace);
                y = -(index % MaxPerLine) * (CellHeight + CellHeightSpace);
                break;
            case Arrangement.Vertical:
                x = (index % MaxPerLine) * (CellWidth + CellWidthSpace);
                y = -(index / MaxPerLine) * (CellHeight + CellHeightSpace);
                break;
        }
        return new Vector3(x, y);
    }

    protected virtual GameObject GetInstance()
    {
        if (skinList.Count > 0)
        {
            skinList.Peek().SetActive(true);
            return skinList.Pop();
        }
        GameObject cloneSkin = GameObject.Instantiate(ItemSkin);
        cloneSkin.SetActive(true);
        return cloneSkin;
    }
    protected virtual void ResetPostion()
    {
        if (actived == false) return;
        ContentRectTrans.anchoredPosition = Vector2.zero;
        CurrentIndex = 0;
    }
    public virtual void SetContentSize(int length)
    {
        if (length == 0)
        {
            ContentRectTrans.sizeDelta = Vector2.right * ContentRectTrans.sizeDelta.x;
            return;
        }
        int lineCount = length / maxPerLine;
        if (lineCount % maxPerLine != 0 &&  maxPerLine % 2 != 0)
        {
            lineCount += 1;
        }
        switch (arrangement)
        {
            case Arrangement.Horizontal:
                ContentRectTrans.sizeDelta = new Vector2(CellWidth * lineCount + CellWidthSpace * (lineCount - 1), ContentRectTrans.sizeDelta.y);
                break;
            case Arrangement.Vertical:
                ContentRectTrans.sizeDelta = new Vector2(ContentRectTrans.sizeDelta.x, (CellHeight * lineCount + CellHeightSpace * (lineCount - 1)));
                break;
        }
    }

    /// <summary>
    /// 显示区域顶点索引序号
    /// </summary>
    /// <returns></returns>
    protected int GetCurScrollPerLineIndex()
    {
        float value = 0;
        switch (this.arrangement)
        {
            case Arrangement.Horizontal:
                value = ContentRectTrans.anchoredPosition3D.x / (CellWidth + CellWidthSpace);
                break;
            case Arrangement.Vertical:
                value = ContentRectTrans.anchoredPosition3D.y / (CellHeight + CellHeightSpace);
                break;
            default:
                break;
        }
        int index = MaxPerLine * (int)value;
        return index;
    }
    private void OnScrollDraged()
    {
        int curPerLineEndInde = GetCurScrollPerLineIndex() * MaxPerLine;
        if (CurrentIndex != curPerLineEndInde)
        {
            CurrentIndex = curPerLineEndInde;
            if (CurrentIndex - OldRefreshIndex >= UpperLimitIndex || CurrentIndex - OldRefreshIndex <= -UpperLimitIndex )
            {
                OldRefreshIndex = CurrentIndex;
                LoadAndUnload();
            }
        }
    }
    private void LoadAndUnload()
    {//TODO:
        //currentIndex
        //Debug.Log("刷新");
    }


    internal void addEventListener(string typr, Action<MyEvent> func)
    {
        eventDispatcher.addEventListener(typr, func);
    }
    public void removeEventListener(string type, Action<MyEvent> action)
    {
        eventDispatcher.removeEventListener(type, action);
    }
    public void dispatchEvent(MyEvent myEvent)
    {
        //SchemeEvent mEvent = new SchemeEvent(type, data);
        eventDispatcher.dispatchEvent(myEvent);
    }
}
public enum Arrangement
{
    Horizontal,
    Vertical
}
