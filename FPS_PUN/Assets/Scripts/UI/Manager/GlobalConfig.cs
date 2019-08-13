using UnityEngine;

public class GlobalConfig : Singleton<GlobalConfig>
{

    public GlobalConfig()
    {

    }

    public static bool isMyDebug = false;

    public static bool isNeedSDK = true;

    public static bool running = false;

    /*����˵��*/
    /*
     ���MainCamera�����������
     ����Screen�ķֱ�����α仯  ֻҪMainCamera��orthographicSize���� MainCamera��Ұheight��Ӧ�ĳ����߶ȴ�С�̶����䡣 
     * ���磺MainCamera��orthographicSize = 1����ô����Screen�ķֱ����Ƕ��٣�MainCamera��Ұ�߶ȶ���6.25��λ�ĳ�����С��
     */
    /*********/

    /// <summary>
    /// MainCamera������Size��Ӧ�����ĵ�λ����/
    /// </summary>
    public static float cameraSceneScale
    {
        get { return 1f; }
    }

    /// <summary>
    /// ����ui�ı�׼�ֱ��ʿ�ȣ�����Ӧ�ڴ˻����ϸı�
    /// </summary>
    public static float perfectWith = 960;
    /// <summary>
    /// ����ui��׼�ֱ��ʸ߶ȣ�����Ӧ�ڴ˻����ϸı�
    /// </summary>
    public static float perfectHeight = 540;

    private static GameObject _aimParentObj;

    public static GameObject UIObjInScene;

    /// <summary>
    /// ����ui��ê����
    /// </summary>
    public static GameObject UIParentObj
    {
        get
        {
            if (_aimParentObj == null)
            {
                _aimParentObj = GameObject.Find("UI/Canvas/Anchor").gameObject;
            }
            return _aimParentObj;
        }
    }
    private static Canvas canvas;
    public static Canvas Canvas
    {
        get
        {
            if (canvas == null)
            {
                canvas = GetUIComponent<Canvas>("UI/Canvas");
            }
            return canvas;
        }
    }

    private  RectTransform _uiParant;
    public  RectTransform uiParant
    {
        get
        {
            if (_uiParant == null) _uiParant = GetUIComponent<RectTransform>("UI/Canvas/Anchor");  
            return _uiParant;
        }
    }

    private Transform _Scene;
    public Transform Scene
    {
        get
        {
            if (_Scene == null) _Scene = GetUIComponent<Transform>("Scene"); 
            return _Scene;
        }
    }

    private Transform _uiParentTran;
    public Transform uiParentTran
    {
        get
        {
            if (_uiParentTran == null) _uiParentTran = GetUIComponent<Transform>("UI/Canvas/uiScene"); //GameObject.Find("UI/Canvas/uiScene").transform;
            return _uiParentTran;
        }
    }

    public static T GetUIComponent<T>(string path) where T : UnityEngine.Component
    {
        T tempObj = GameObject.Find(path).GetComponent<T>();
        if (tempObj == null)
        {
            Debug.Log("û�з������·��");
            return null;
        }
        return tempObj;
    }
}
