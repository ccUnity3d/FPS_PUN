using System;
using System.Collections.Generic;
using UnityEngine;

public class UIManager :Singleton<UIManager> {

    private Dictionary<PageType, UICopntrollerData> ControllerDic = new Dictionary<PageType, UICopntrollerData>();
    private readonly bool DestroyWhenClose = false;


    public static RectTransform uiParant
    {
        get
        {
            return GlobalConfig.Instance.uiParant;
            //if (_uiParant == null) _uiParant = GameObject.Find("UI/Canvas/Anchor").GetComponent<RectTransform>();
            //return _uiParant;
        }
    }
    public override void OnInstance()
    {
        base.OnInstance();
        Inject();
    }

    public void Inject()
    {
        //MainPageUIController control = MainPageUIController.Instance;
        //SchemePageController schemeController = SchemePageController.Instance;
        //KeyPageController keyBoardController = KeyPageController.Instance;
        //SetWallController setWallController = SetWallController.Instance;
        //Instance.inject(PageType.MainPage, control);
        //UICopntrollerData Scheme = Instance.inject(PageType.Scheme, schemeController);
        //UICopntrollerData KeyBoard = Instance.inject(PageType.KeyBoard, keyBoardController);
        //UICopntrollerData SetWall = Instance.inject(PageType.SetWall, setWallController);
        //Scheme.state = SimpleLoadedState.Success;
        //KeyBoard.state = SimpleLoadedState.Success;
        //SetWall.state = SimpleLoadedState.Success;

        //ARPageController aRPageController = ARPageController.Instance;
        //UICopntrollerData arpage = Instance.inject(PageType.ARPage,aRPageController);
        //arpage.state = SimpleLoadedState.Success;

        LoginPageController loginctr = LoginPageController.Instance;
        UICopntrollerData login = Instance.inject(PageType.LoginPage,loginctr);
    }
    private UICopntrollerData inject(PageType page, IController control)
    {
        UICopntrollerData data;
        if (ControllerDic.TryGetValue(page, out data) == true)
        {
            Debug.LogError("Controller注册重复：" + page);
            return data;
        }
        data = new UICopntrollerData(control);
        ControllerDic.Add(page, data);
        return data;
    }



    public static void Open(PageType page)
    {
        Instance.open(page);
    }

    private void open(PageType page)
    {
        if (ControllerDic.ContainsKey(page) == false)
        {
            Debug.Log(page + "未注册");
            return;
        }
        ControllerDic[page].currentType = 1;
        switch (ControllerDic[page].state)
        {
            case SimpleLoadedState.None:
                Instance.LoadPanel(page);
                break;
            case SimpleLoadedState.Loading:
                break;
            case SimpleLoadedState.Failed:
                //上次加载失败
                break;
            case SimpleLoadedState.Success:
                if (ControllerDic[page].skin == null)
                {
                    ControllerDic[page].skin = ControllerDic[page].controller.getPanel.skin;
                }
                ControllerDic[page].skin.SetActive(true);
                ControllerDic[page].controller.awake();
                break;
            default:
                break;
        }
    }

    public static void Close(PageType page)
    {
        Instance.close(page);
    }
    private void close(PageType page)
    {
        if (ControllerDic.ContainsKey(page) == false)
        {
            Debug.Log(page + "未注册");
            return;
        }
        ControllerDic[page].currentType = 0;
        switch (ControllerDic[page].state)
        {
            case SimpleLoadedState.None:
                break;
            case SimpleLoadedState.Loading:
                break;
            case SimpleLoadedState.Failed:
                //上次加载失败
                break;
            case SimpleLoadedState.Success:
                ControllerDic[page].controller.sleep();
                if (DestroyWhenClose == true && ControllerDic[page].destroyable == true)
                {
                    ControllerDic[page].skin.SetActive(false);
                }
                else {
                    //ControllerDic[page].state = SimpleLoadedState.None;
                    //ResourcesPool.Dispos(ControllerDic[page].skin);
                }
                break;
            default:
                break;
        }
    }

    public static bool IsOpen(PageType pageType)
    {
        return Instance.isOpen(pageType);
    }
    public bool isOpen(PageType pageType)
    {
        return ControllerDic[pageType].currentType == 1;
    }

    /// <summary>
    /// 取反
    /// </summary>
    /// <param name="pageType"></param>
    public static void Toggle(PageType pageType)
    {
        Instance.toggle(pageType);
    }
    public void toggle(PageType pageType)
    {
        if (ControllerDic[pageType].currentType == 0)
        {
            open(pageType);
        }
        else
        {
            close(pageType);
        }
    }
    private void LoadPanel(PageType page)
    {
        string prefabPath = ControllerDic[page].controller.getPanel.GetPrefabPath();
        MyLoader loader = new MyLoader();
        loader.LoadPrefab(prefabPath, 0, onLoaded, page);
    }

    private void onLoaded(UnityEngine.Object arg1, object arg2)
    {
        GameObject goClone = arg1 as GameObject;
        PageType page = (PageType)arg2;
        goClone.SetActive(true);
        goClone.transform.SetParent(uiParant);
        ControllerDic[page].controller.SetData(goClone);

        if (ControllerDic[page].currentType == 0)
        {
            ControllerDic[page].controller.sleep();
            if (DestroyWhenClose == true && ControllerDic[page].destroyable == true)
            {
                ControllerDic[page].skin.SetActive(false);
            }
            else {
                ControllerDic[page].state = SimpleLoadedState.None;
                ResourcesPool.Dispos(ControllerDic[page].skin);
            }
        }
        else
        {
            ControllerDic[page].state = SimpleLoadedState.Success;
        }
    }
   

    public class UICopntrollerData
    {
        /// <summary>
        /// 控制器
        /// </summary>
        public IController controller;
        /// <summary>
        /// 皮肤
        /// </summary>
        public GameObject skin;
        /// <summary>
        /// 加载状态
        /// </summary>
        public SimpleLoadedState state = SimpleLoadedState.None;
        /// <summary>
        /// 0关闭 1打开
        /// </summary>
        public int currentType = 0;

        public bool destroyable = false;

        public UICopntrollerData(IController control)
        {
            this.controller = control;
        }
    }

}
