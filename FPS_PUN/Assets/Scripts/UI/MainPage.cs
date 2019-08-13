using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class MainPage : UIPage<MainPage>
{

    public List<Transform> ChildTransfrom = new List<Transform>();
    public List<Button> two_D_No_Menu = new List<Button>();
    // 所有3D 操作按钮都添加在里面

    public Texture Logo;
    
    #region top
    public Transform top;
    public GridLayoutGroup menuGroup;

    public Button toFollow;
    #region left
    public ClickButton open;
    public Button save;
    public Button redo;
    public Button undo;
    #endregion

    #region center
    public ToggleButton template;
    public ToggleButton middleLine;
    public ToggleButton innerLine;
    [Tooltip("测量")]
    public ToggleButton measurement;
    public ToggleButton show;
    #endregion

    #region right
    public Button render;
    public Button share;
    [Tooltip("报价")]
    public Button offer;
    public ToggleButton cameraView;
    #endregion

    #endregion

    #region topLeft
    public Transform TopLeft;
    public Button exit;
    public RectTransform SaveBox;
    public Button cancelSave;
    public Button confirmSave;
    public Button exitSave;
    public Button scheme;
    public Text schemeName;
    public Button material;
    #endregion

    #region topRight
    public Transform topRight;
    public Button thereD;
    public Button twoD;
    #endregion

    #region Center
    public Transform centerRight;
    public Button addButton;
    #endregion

    #region BottomCenter

    #endregion

    #region BottomRight
    public Transform bottomRight;
    public Button query;
    public Button setup;
    #endregion
    public RectTransform loadSceneProgress;
    public Button loadExit;
    public RectTransform loadingScene;
    public Image loadProgressImage;
    public Text loadProgressText;
    public RectTransform LoadSucceed;


    #region ChildNode
    public Transform ChildNode;

    public GameObject SkinScheme; 

    public GameObject SkinTemplate;
    
    public GameObject SkinKeyBoard;

    public GameObject SkinShow;

    public GameObject SkinSelelctOptions;

    public GameObject SkinRender;

    public GameObject SkinGroup;

    public GameObject SkinHandShank;

    public GameObject SkinSetting;

    public GameObject SkinSetWall;

    public GameObject SkinMaterialScorll;

    public GameObject SkinCameraViewScroll;

    public GameObject SkinParticular;

    public GameObject saveScheme;

    #endregion

    public MainPage()
    {
        prefabPath = "MainPage.assetbundle";
    }
    protected override void Ready(Object arg1)
    { 
        Logo = null;
        RawImage image = UITool.GetUIComponent<RawImage>(skin.transform, "ChildNode/Scheme/CreateOffer/Content/OfferProduce/ScrollView/itemPictures/RawImage");
        if(image!=null) Logo = image.texture;

        //image = skin.transform.FindChild("RawImage").GetComponent<RawImage>();
        //image.gameObject.SetActive(false);
        #region Top
        top = skin.transform.Find("Top");
        TopLeft = skin.transform.Find("TopLeft");
        topRight = skin.transform.Find("TopRight");
        menuGroup = UITool.GetUIComponent<GridLayoutGroup>(top,"Menu");
        Transform tLeft = UITool.GetUIComponent<Transform>(top, "Menu/Left");
        open = UITool.AddUIComponent<ClickButton>(tLeft, "open");
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            UITool.SetActionFalse(open.gameObject);
        }
        save = UITool.AddUIComponent<ClickButton>(tLeft, "save");
        redo = UITool.AddUIComponent<ClickButton>(tLeft, "redo");
        undo = UITool.AddUIComponent<ClickButton>(tLeft, "undo");
        Transform tCenter = UITool.GetUIComponent<Transform>(top, "Menu/Center");
        template = UITool.AddUIComponent<ToggleButton>(tCenter, "template");
        innerLine = UITool.AddUIComponent<ToggleButton>(tCenter, "innerLine");
        middleLine = UITool.AddUIComponent<ToggleButton>(tCenter, "middleLine");
        measurement = UITool.AddUIComponent<ToggleButton>(tCenter, "measurement");
        show = UITool.AddUIComponent<ToggleButton>(tCenter, "show");
        Transform tRight = UITool.GetUIComponent<Transform>(top, "Menu/Right");
        render = UITool.AddUIComponent<ClickButton>(tRight, "render");
        // 改版 隐藏
        {
            render.gameObject.SetActive(false);
        }
        offer = UITool.AddUIComponent<ClickButton>(tRight, "offer");
        share = UITool.AddUIComponent<ClickButton>(tRight, "share");
        cameraView = UITool.AddUIComponent<ToggleButton>(tRight, "CameraView");
        cameraView.gameObject.SetActive(true);
        cameraView.gameObject.SetActive(false);
        
        Transform topLeft = skin.transform.Find("TopLeft");
        exit = UITool.GetUIComponent<Button>(topLeft, "exit");
        SaveBox = UITool.GetUIComponent<RectTransform>(topLeft,"exit/IsSave");
        cancelSave = UITool.GetUIComponent<Button>(SaveBox, "cancel");
        confirmSave = UITool.GetUIComponent<Button>(SaveBox, "confirm");
        exitSave = UITool.GetUIComponent<Button>(SaveBox, "exitSave");
        material = UITool.GetUIComponent<Button>(topLeft, "Material");
        scheme = UITool.AddUIComponent<ClickButton>(topLeft, "scheme");
        schemeName = UITool.GetUIComponent<Text>(topLeft, "scheme/name");
        Vector2 v2 = schemeName.rectTransform.anchoredPosition;
        v2.x = 0;
        schemeName.rectTransform.anchoredPosition = v2;
        thereD = UITool.GetUIComponent<Button>(topRight, "thereD");
        twoD = UITool.GetUIComponent<Button>(topRight, "twoD");
        //two_D_No_Menu.Add(open);
        two_D_No_Menu.Add(template);
        two_D_No_Menu.Add(innerLine);
        two_D_No_Menu.Add(middleLine);
        two_D_No_Menu.Add(measurement);


        toFollow = UITool.AddUIComponent<Button>(skin.transform, "Top/Menu/Right/SceneWalkthrough");
        #endregion

        #region Bottom
        // 渲染 部分还没有  写 。。。
        bottomRight = skin.transform.Find("BottomRight");
        setup = UITool.GetUIComponent<Button>(bottomRight, "set_up");
        query = UITool.GetUIComponent<Button>(bottomRight, "query");

        #endregion

        #region Center
        centerRight = skin.transform.Find("CenterRight");
        addButton = UITool.GetUIComponent<Button>(centerRight, "addButton");
        #endregion

        #region ChildNode
        ChildNode = UITool.GetUIComponent<RectTransform>(skin.transform, "ChildNode");

        SkinScheme = ChildNode.Find("Scheme").gameObject;
        //Scheme.Instance.SetData(skin);

        SkinTemplate = ChildNode.Find("Template").gameObject;
        //TemplatePage.Instance.SetData(skin);

        SkinRender = ChildNode.Find("Render").gameObject; //UITool.GetUIComponent<RectTransform>(ChildNode, "Render");
        //RenderPage.Instance.SetData(skin);

        SkinShow = ChildNode.Find("Show").gameObject; // UITool.GetUIComponent<RectTransform>(ChildNode, "Show");
        //ShowOrHide.Instance.SetData(skin);

        SkinSelelctOptions = skin.transform.Find("BottomCenter").gameObject;

        SkinMaterialScorll = ChildNode.Find("MaterialScroll").gameObject; // UITool.GetUIComponent<RectTransform>(ChildNode, "MaterialScroll");
        //MaterialPage.Instance.SetData(skin);

        SkinCameraViewScroll = ChildNode.Find("CameraView").gameObject;

        SkinParticular = ChildNode.Find("Particulars").gameObject;

        saveScheme = ChildNode.Find("SaveScheme").gameObject;

        SkinSetWall = ChildNode.Find("SetWall").gameObject; //UITool.GetUIComponent<RectTransform>(ChildNode, "SetWall");
        //SetWallPage.Instance.SetData(skin);

        SkinSetting = ChildNode.Find("Setting").gameObject; //UITool.GetUIComponent<RectTransform>(ChildNode, "Setting");
        //SettingPage.Instance.SetData(skin);

        SkinKeyBoard = ChildNode.Find("KeyBoard").gameObject; //UITool.GetUIComponent<RectTransform>(ChildNode,"KeyBoard");
        //KeyBoard.Instance.SetData(skin);

        SkinGroup = ChildNode.Find("GroupCollect").gameObject; // UITool.GetUIComponent<RectTransform>(ChildNode,"Group");           
        //GroupPage.Instance.SetData(skin);

        SkinHandShank = ChildNode.Find("HandShank").gameObject;
        UITool.SetActionFalse(SkinHandShank);
        #endregion
        loadSceneProgress = skin.transform.Find("LoadSceneProgress") as RectTransform;
        loadingScene = UITool.GetUIComponent<RectTransform>(loadSceneProgress, "LoadingScene");
        loadExit = UITool.GetUIComponent<Button>(loadSceneProgress, "LoadExit");
        loadProgressImage = UITool.GetUIComponent<Image>(loadingScene,"progress");
        loadProgressText = UITool.GetUIComponent<Text>(loadingScene, "progressText");
        LoadSucceed = skin.transform.Find("LoadSceneProgress/LoadSucceed") as RectTransform;
        UITool.SetActionFalse(loadSceneProgress.gameObject);
        UITool.SetActionFalse(loadingScene.gameObject);
        UITool.SetActionFalse(LoadSucceed.gameObject);

        ChildTransfrom.Add(top);
        ChildTransfrom.Add(topLeft);
        ChildTransfrom.Add(topRight);
        ChildTransfrom.Add(SkinSelelctOptions.transform);
        ChildTransfrom.Add(bottomRight);
        ChildTransfrom.Add(centerRight);
        ChildTransfrom.Add(ChildNode);
    }
}
