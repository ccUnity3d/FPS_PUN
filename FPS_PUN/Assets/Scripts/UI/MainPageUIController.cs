using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class MainPageUIController : UIController<MainPageUIController> {

    
    //各种管理器和控制器
    public MainPage mainpage;

    //public OptionsPage optionsPage { get { return OptionsPage.Instance; } }

    //public MainPageUIData mainpageData;

    //public MainPageStateMachine machine
    //{
    //    get {
    //        return MainPageStateMachine.Instance;
    //    }
    //}
    //private CameraControler cameraControl
    //{
    //    get {
    //        return CameraControler.Instance;
    //    }
    //}
    //private Mode2DPrefabs mode2dprefab
    //{
    //    get {
    //        return Mode2DPrefabs.Instance;
    //    }
    //}
    //private CameraStateMachine cameraMachine
    //{
    //    get {
    //        return CameraStateMachine.Instance;
    //    }
    //}
    //private InputStateMachine inputMachine
    //{
    //    get {
    //        return InputStateMachine.Instance;
    //    }
    //}
    //private SchemePageController schemeController
    //{
    //    get
    //    {
    //        return SchemePageController.Instance;
    //    }
    //}
    //private SchemeManifest schemeManifest
    //{
    //    get
    //    {
    //        return SchemeManifest.Instance;
    //    }
    //}
    //private CacheTopViewManager topViewCache
    //{
    //    get {
    //        return CacheTopViewManager.Instance;
    //    }
    //}
 

    //private CacheResizedImageManager resizedImageCache
    //{
    //    get {
    //        return CacheResizedImageManager.Instance;
    //    }
    //}
    //private CacheLocalOfferManager localOfferCache
    //{
    //    get {
    //        return CacheLocalOfferManager.Instance;
    //    }
    //}

    //private CacheServerOfferManager serverOfferCache
    //{
    //    get
    //    {
    //        return CacheServerOfferManager.Instance;
    //    }
    //}

    //private JsonCacheManager jsonCacheManager
    //{
    //    get
    //    {
    //        return JsonCacheManager.Instance;
    //    }
    //}

    //protected UndoHelper undoHelper
    //{
    //    get
    //    {
    //        return UndoHelper.Instance;
    //    }
    //}
    //public override void sleep()
    //{
    //    base.sleep();
    //    mainpage.skin.SetActive(false);
    //}
    public MainPageUIController()
    {
        panel = mainpage = MainPage.Instance;
        //data = mainpageData = MainPageUIData.Instance;
    }
    public override void ready()
    {
        base.ready();
        //#region Top
        //mainpage.exit.onClick.AddListener(onExit);
        //mainpage.cancelSave.onClick.AddListener(onCancelSave);
        //mainpage.confirmSave.onClick.AddListener(onConfirmSave);
        //mainpage.exitSave.onClick.AddListener(onExitSave);
        //mainpage.scheme.onClick.AddListener(onSchem);
        //mainpage.open.onClick.AddListener(onOpen);
        //mainpage.save.onClick.AddListener(onSave);
        //mainpage.redo.onClick.AddListener(onReDo);
        //mainpage.undo.onClick.AddListener(onUndo);
        //mainpage.template.onClick.AddListener(onTemp);
        //mainpage.innerLine.onClick.AddListener(onInner);
        //mainpage.middleLine.onClick.AddListener(onMiddle);
        //mainpage.measurement.onClick.AddListener(onMeasurement);
        //mainpage.show.onClick.AddListener(onShow);
        //mainpage.render.onClick.AddListener(onRender);
        //mainpage.offer.onClick.AddListener(onOffer);
        //mainpage.cameraView.onClick.AddListener(onCameraView);
        //mainpage.share.onClick.AddListener(onShare);
        //mainpage.twoD.onClick.AddListener(onTwoD);
        //mainpage.thereD.onClick.AddListener(onThreeD);
        //mainpage.material.onClick.AddListener(onMaterial);
        //mainpage.loadExit.onClick.AddListener(Exit);

        //mainpage.toFollow.onClick.AddListener(ToFollow);
        //#endregion

        //#region Center
        //mainpage.addButton.onClick.AddListener(onAdd);
        //#endregion
        
        //#region Bottom
        //mainpage.query.onClick.AddListener(onQuery);
        //mainpage.setup.onClick.AddListener(onSet_Up);

        //#endregion
        
        //#region ChildNode
        //SchemePageController.Instance.SetData(mainpage.SkinScheme);
        //KeyPageController.Instance.SetData(mainpage.SkinKeyBoard);
        //SetWallController.Instance.SetData(mainpage.SkinSetWall);
        //OptionsController.Instance.SetData(mainpage.SkinSelelctOptions);
        //FollowController.Instance.SetData(mainpage.SkinHandShank);
        //#endregion

        //machine.Ready();
        
        //#region initialize 
        //machine.setState(MainPageFreeState.Name);
        //cameraControl.mainCamera = mode2dprefab.mainCamera;
        //cameraControl.uiCamera = mode2dprefab.uiCamera;
        ////cameraControl.helpCamera = mode2dprefab.helpCamera;
        ////CameraData data2D = new CameraData(LayerMask.GetMask("UI"), Vector3.forward * -30, Vector3.zero, true, 5);
        ////CameraData data3D = new CameraData(LayerMask.GetMask("Default"), Vector3.forward * -12 + Vector3.up * 19, Vector3.right * 53);
        ////data2D.setCamera(cameraControl.helpCamera); 
        ////data3D.setCamera(cameraControl.mainCamera);
        ////CameraTextureData cameraData = new CameraTextureData(cameraControl.helpCamera);
        //cameraMachine.setState(CameraState2D.NAME);
        //inputMachine.setState(FreeState2D.NAME);
        //#endregion


        //#region aboutCache
        //// 版本 隐藏

        ////if (localOfferCache.isReady)
        ////{
        ////    LocalOfferLoadReady(null);
        ////}
        ////else
        ////{
        ////    localOfferCache.addEventListener(MyCacheEvent.loadReady, LocalOfferLoadReady);
        ////}
        ////if (serverOfferCache.isReady)
        ////{
        ////    ServerOfferLoadReady(null);
        ////}
        ////else
        ////{
        ////    serverOfferCache.addEventListener(MyCacheEvent.loadReady, ServerOfferLoadReady);
        ////}

        ////mainpageData.addEventListener(MainPageUIDataEvent.SchemeIdChanged, OnSchemeIdChange);
        //#endregion

        //this.addEventListener(LoadEvent.OpenLoadingPage, OpenLoadingPage);
        //this.addEventListener(LoadEvent.RefreshLoadingPage, RefreshLoadingPage);
        //this.addEventListener(LoadEvent.CloseLoadingPage, CloseLoadingPage);


        //#region 
        //    {
        //    MsgToIOS msg = new MsgToIOS();
        //    msg.code = "101001";
        //    UnityIOSMsg.sendToIOS(msg);
        //    }   

        //if (Application.platform != RuntimePlatform.IPhonePlayer)
        //{
        //    { MsgFromIOS msg = new MsgFromIOS();
        //        msg.code = "201000";
        //        msg.info = new MsgFromIOS.InfoFromIOS();
        //        Dictionary<string, object> userInfo = new Dictionary<string, object>();
        //        userInfo.Add("uuid", UnityIOSMsg.currentUser.uuid);
        //        msg.info.userInfo = userInfo;
        //        UnityIOSMsg.Instance.receiveFromIOSMsg(msg);
        //    }
        //    { MsgFromIOS msg = new MsgFromIOS();
        //        msg.code = "201001";
        //        msg.info = new MsgFromIOS.InfoFromIOS();
        //        msg.info.enterType = "1";
        //        UnityIOSMsg.Instance.receiveFromIOSMsg(msg);
        //    }
        //}
        //#endregion



    }

    //private void onExitSave()
    //{
    //    UITool.SetActionFalse(mainpage.SaveBox.gameObject);
        
    //}

    //private void OpenLoadingPage(MyEvent obj)
    //{
    //    UITool.SetActionTrue(mainpage.loadSceneProgress.gameObject);
    //    UITool.SetActionTrue(mainpage.loadingScene.gameObject);
    //    mainpage.loadProgressText.text = string.Format("加载模型...0%");
    //}

    //private void CloseLoadingPage(MyEvent obj)
    //{
    //    UITool.SetActionFalse(mainpage.loadSceneProgress.gameObject);
    //    UITool.SetActionFalse(mainpage.loadingScene.gameObject);
    //}

    //private void RefreshLoadingPage(MyEvent obj)
    //{
    //    float progress = (float)obj.data ;
    //    mainpage.loadProgressImage.fillAmount = progress;
    //    mainpage.loadProgressText.text = string.Format("加载模型...{0}%", Mathf.RoundToInt(progress*100));
    //}

    //private void onConfirmSave()
    //{
    //    //UITool.SetActionFalse(mainpage.SaveBox.gameObject);
    //    onSave();
    //   // setState(SaveState.Name);
    //}

    //private void onCancelSave()
    //{
    //    //UITool.SetActionFalse(mainpage.SaveBox.gameObject);
    //    Debug.Log("onCancelSave");
    //    setState(ExitState.Name);
    //}

    //private void onCameraView()
    //{
    //    Debug.Log("相机视图");
    //    if (cameraMachine.CurrentState is CameraStateFollow)
    //    {
    //        cameraMachine.setState(CameraState3D.NAME);
    //    }
    //    // if (inputMachine.currentInputIs2D == true) setState(ToThreeDState.Name);
    //    setState(CameraViewState.Name);
    //}

    //private void LocalOfferLoadReady(MyEvent obj)
    //{
    //    List<string> cacheOfferList = localOfferCache.GetCacheOfferList();
    //    for (int i = 0; i < cacheOfferList.Count; i++)
    //    {
    //        int tempId = int.Parse(cacheOfferList[i]);
    //        if (mainpageData.HasPriceData(tempId.ToString()))
    //        {
    //            continue;
    //        }
    //        LoaderPool.CacheLoad(tempId, SimpleLoadDataType.JsonOffer, OnPriceLoaded);
    //    }
    //}

    //private void OnPriceLoaded(object obj)
    //{
    //    SimpleCacheLoader loader = obj as SimpleCacheLoader;
    //    if (loader.state == SimpleLoadedState.Failed)
    //    {
    //        //Debug.LogError(loader.uri);
    //        return;
    //    }
    //    string json = loader.loadedData.ToString();
    //    object jsonObj = MyJsonTool.FromJson(json);
    //    PriceData priceData = new PriceData();
    //    priceData.Deserialize(jsonObj as Dictionary<string, object>);
    //    mainpageData.AddPriceData(priceData);
    //}

    //private void ServerOfferLoadReady(MyEvent obj)
    //{
    //    List<string> cacheOfferList = serverOfferCache.GetCacheOfferList();
    //    for (int i = 0; i < cacheOfferList.Count; i++)
    //    {
    //        string id = cacheOfferList[i];
    //        if (mainpageData.HasPriceData(id))
    //        {
    //            continue;
    //        }
    //        LoaderPool.CacheLoad(id, SimpleLoadDataType.JsonOffer, null);
    //    }
    //}

    //private void OnSchemeIdChange(MyEvent obj)
    //{
    //    MainPageUIDataEvent e = obj as MainPageUIDataEvent;
    //    MsgFromIOS.InfoFromIOS info = (MsgFromIOS.InfoFromIOS)e.data;
    //    Dictionary<string, PriceData> outDic;
    //    if (mainpageData.allPriceDic.TryGetValue(info.tempId.ToString(), out outDic))
    //    {
    //        mainpageData.allPriceDic.Remove(info.tempId.ToString());
    //        mainpageData.allPriceDic.Add(info.projectId, outDic);
    //        foreach (PriceData offer in outDic.Values)
    //        {
    //            offer.targetServerId = info.projectId;
    //            jsonCacheManager.AddOfferCache(offer);
    //        }
    //    }
    //}

    //public void SetCollected(string seekId, bool v)
    //{
    //    if (v==true && mainpageData.productCollectedList.IndexOf(seekId) == -1)
    //    {
    //        mainpageData.productCollectedList.Add(seekId);
    //        return;
    //    }
    //    if (mainpageData.productCollectedList.IndexOf(seekId)!=-1)
    //    {
    //        mainpageData.productCollectedList.Remove(seekId);
    //    }
    //    //throw new NotImplementedException();
    //}

    public override void awake()
    {
        base.awake();
        //OptionsController.Instance.ShowSelect();
    }

    //#region Set State
    //private void setState(string name)
    //{
    //    machine.setState(name);
    //}
    //private bool isCurrentState(string name)
    //{
    //    return machine.IsCurrentState(name);
    //}
    //#endregion

    //#region topFunction
   
    //private void onExit()
    //{
    //    if (undoHelper.currentData.SaveId != 0)
    //    {
    //        this.addEventListener(SchemeEvent.SchemeSuccess, ScehemSuccess);
    //        MessageBox.Instance.ShowOkCancelClose("保存方案", "是否保存当前设计方案",  onCancelSave, onConfirmSave);
    //        return;
    //    }
    //    if (cameraMachine.CurrentState  is CameraStateFollow)
    //    {
    //        cameraMachine.setState(CameraState3D.NAME);
    //    }
    //    Exit();
    //}
    //private void Exit()
    //{
    //    Debug.Log("Exit");
    //    setState(ExitState.Name);
    //}

    //private void ToFollow()
    //{
    //    FollowController.Instance.changeFollow();
    //}


    //private void onSchem()
    //{
    //    setState(EditorSchemeState.Name);
    //}
    //private void onOpen()
    //{
    //    if (cameraMachine.CurrentState is CameraStateFollow)
    //    {
    //        cameraMachine.setState(CameraState3D.NAME);
    //    }
    //    setState(OpenOtherSchemeState.Name);

    //}
    //private void onUndo()
    //{
    //    if (cameraMachine.CurrentState is CameraStateFollow)
    //    {
    //        cameraMachine.setState(CameraState3D.NAME);
    //    }
    //    setState(UndoState.Name);
        
    //    Debug.Log("后退");
    //}
    //private void onReDo()
    //{
    //    if (cameraMachine.CurrentState is CameraStateFollow)
    //    {
    //        cameraMachine.setState(CameraState3D.NAME);
    //    }
    //    setState(RedoState.Name);
    //    Debug.Log("前进");
    //}
    ////private Camera camera3d;
    //private void onSave()
    //{
   
    
    //    //this.addEventListener(SchemeEvent.SchemeSuccess,ScehemSuccess);
    //    setState(SaveState.Name);

    //    Debug.Log("保存");
    //}
    //private void ScehemSuccess(MyEvent e)
    //{
    //    this.removeEventListener(SchemeEvent.SchemeSuccess, ScehemSuccess);
    //    setState(ExitState.Name);
    //}
    //private void onTemp()
    //{
    //    setState(TemplateState.Name);
    //    Debug.Log("模板");
    //}
    //private void onInner()
    //{
    //    setState(InnerLineState.Name);
    //    Debug.Log("内线");
    //}
    //private void onMiddle()
    //{
    //    setState(MiddleLineState.Name);
    //    Debug.Log("中线");
    //}
    //private void onShow()
    //{
    //    if (cameraMachine.CurrentState is CameraStateFollow)
    //    {
    //        cameraMachine.setState(CameraState3D.NAME);
    //    }
    //    setState(ShowState.Name);
    //    Debug.Log("显示");
    //}

    //private void onMeasurement()
    //{
    //    setState(MeasurementState.Name);
    //    Debug.Log("测量");
    //}
    //private void onRender()
    //{
    //    if (cameraMachine.CurrentState is CameraStateFollow)
    //    {
    //        cameraMachine.setState(CameraState3D.NAME);
    //    }
    //    if (inputMachine.currentInputIs2D == true) setState(ToThreeDState.Name);
    //    setState(RenderState.Name);
    //    Debug.Log("渲染");
    //}
    //private void onOffer()
    //{
    //    //Debug.Log("清单");
    //    setState(OfferState.Name);
    //}
    //private void onShare()
    //{
    //    setState(ShareState.Name);
    //    Debug.Log("分享");
    //}
    //public void onTwoD()
    //{
    //    setState(ToTwoDState.Name);
    //    Debug.Log("2D");
    //    //切换显示按钮change(!is2d);
    //}
    //public void onThreeD()
    //{
    //    setState(ToThreeDState.Name);
    //    Debug.Log("3D");
    //}
    //private void onMaterial()
    //{
    //    //if (inputMachine.currentInputIs2D == true) setState(ToThreeDState.Name);
    //    setState(MaterialState.Name);
    //    Debug.Log("材质");
    //}

 
    //#endregion

    //#region rightFunction
    //public void onAdd()
    //{
    //    //setState(AddGoodsState.Name);
    //    setState(ARState.Name);
    //    Debug.Log("添加商品/贴图");
    //}
    //private void onQuery()
    //{
    //    setState(QueryState.Name);
    //    Debug.Log("疑问");
    //}
    //private void onSet_Up()
    //{
    //    setState(DefaultSetState.Name);
    //    Debug.Log("设置");
    //}
    //#endregion

    //#region Show InputPage
    //public void OpenInput(float value, Text onChange, Action dele = null)
    //{
    //    OpenInput(value.ToString(), onChange, dele);
    //}
    ///// <summary>
    ///// 打开输入框
    ///// </summary>
    ///// <param name="defaultValue">默认值</param>
    ///// <param name="onChange">回调</param>
    //public void OpenInput(string defaultValue, Text onChange, Action dele = null)
    //{
    //    onChange.text = defaultValue;
    //    KeyPageController.Instance.onChange = dele;
    //    KeyPageController.Instance.Text = onChange;
    //    KeyPageController.Instance.Number = defaultValue.ToString();
    //    KeyPageController.Instance.Open();
    //}

    ///// <summary>
    ///// 打开输入框
    ///// </summary>
    ///// <param name="defaultValue">默认值</param>
    ///// <param name="onChange">回调</param>
    //public void OpenInputWithLimit(string defaultValue, Text onChange, float min, float max,Action dele = null)
    //{
    //    InputPageData data = new InputPageData(onChange, min, max, dele); 
    //    onChange.text = defaultValue;
    //    KeyPageController.Instance.onChange = data.mydele;
    //    KeyPageController.Instance.Text = onChange;
    //    KeyPageController.Instance.Number = defaultValue.ToString();
    //    KeyPageController.Instance.Open();
    //}

    //public void CloseInput()
    //{
    //    KeyPageController.Instance.Close();
    //}
    //#endregion

    //public void openChildPage(RectTransform rect)
    //{
    //    UITool.SetActionTrue(rect.gameObject);
    //}

    //public class InputPageData
    //{
    //    private Action dele;
    //    private float max;
    //    private float min;
    //    private Text onChange;

    //    public InputPageData(Text onChange, float min, float max, Action dele)
    //    {
    //        this.onChange = onChange;
    //        this.min = min;
    //        this.max = max;
    //        this.dele = dele;
    //    }


    //    public void mydele()
    //    {
    //        float value;
    //        if (float.TryParse(onChange.text, out value))
    //        {
    //            if (value < min)
    //            {
    //                value = min;
    //            }
    //            else if (value > max)
    //            {
    //                value = max;
    //            }
    //            onChange.text = value.ToString();
    //            dele();
    //        }
    //        else
    //        {
    //            Debug.Log("float.TryParse false value = " + onChange.text);
    //        }
    //    }

    //}

}
