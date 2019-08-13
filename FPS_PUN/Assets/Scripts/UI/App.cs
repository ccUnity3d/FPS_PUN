using Photon.Pun;
using UnityEngine;
using UnityEngine.EventSystems;

public class App : MonoBehaviour {

    public bool showErrorOnGUI = true;
    public bool isDebug = false;
    private MyTickManager tickManager;

    protected void Awake()
    {
        tickManager = MyTickManager.Instance;

        Application.targetFrameRate = 60;
        Application.runInBackground = true;
        Application.backgroundLoadingPriority = ThreadPriority.Normal;
        //防黑屏/
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        UIManager.Open(PageType.LoginPage);
       
    }

    protected void Update()
    {
        if (tickManager != null) tickManager.tick();
    }
    private int width = 0;
    private int height = 0;
    private void Adaption()
    {
        int w = Screen.width;
        int h = Screen.height;
        if (w == width && h == height)
        {
            return;
        }
        width = w;
        height = h;
    }



    private void OnApplicationFocus(bool onRunning)
    {
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            return;
        }

        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
 
        }
    }
}
