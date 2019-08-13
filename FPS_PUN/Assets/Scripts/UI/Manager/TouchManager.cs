using System;
using UnityEngine;

public class TouchManager
{
    private static TouchManager instance;
    public static TouchManager Instance
    {
        get
        {
            if (instance == null) instance = new TouchManager();
            return instance;
        }
    }

    public int touchCount = 0;
    public bool doubleClick = false;
    public int doubleClickTouchCount = 0;

    public void update()
    {

    }
}


/*

    //public int OnableCount = 0;
    public int tempTouchCount = 0;
    public float touchedTime = 0;

    /// <summary>
    /// 输入无效 按规定算
    /// </summary>
    public bool disabled = false;
    /// <summary>
    /// 输入单击
    /// </summary>
    public bool clicked = false;
    /// <summary>
    /// 输入双击
    /// </summary>
    public bool doubleClicked = true;

    /// <summary>
    /// 
    /// </summary>
    public bool Down = false;
    /// <summary>
    /// 起手
    /// </summary>
    public bool Up = false;
    /// <summary>
    /// 按下后发生过移动
    /// </summary>
    public bool moved = false;
    /// <summary>
    /// 当前帧移动
    /// </summary>
    private bool onMove = false;
    /// <summary>
    /// 平均移动距离（单或多都可以）
    /// </summary>
    public Vector2 deltaMove = Vector2.zero;
    /// <summary>
    /// 双手指时手指距离变化量
    /// </summary>
    public float deltaDis = 0;
    /// <summary>
    /// 位置(单手指专用)
    /// </summary>
    public Vector2 pos = Vector2.zero;
    /// <summary>
    /// 输入状态激活成功
    /// </summary>
    public bool onable = false;
    
    /// <summary>
    /// 判断手指数的
    /// </summary>
    private float dateTime = 0.2f;
    private float clickedTime = 0;
    private Vector3 clickedPos = Vector3.zero;
    private float doubleClickDateTime = 0.5f;
    private float dateDis = 10;

    private float timeBeforAble = 0;
    



            if (tempTouchCount == 0)
        {
            Up = false;
            disabled = false;
            onable = false;
            moved = false;
            if (Input.touchCount == 0)
            {
                return;
            }
            ///输入状态待激活（如：可能有两只手指输入有先后 间隔很短）
            tempTouchCount = Input.touchCount;
            touchedTime = 0;
            pos = Input.GetTouch(0).position;
            Down = true;
            return;
        }

        Down = false;
        clicked = false;
        doubleClicked = false;
        onMove = false;
        deltaDis = 0;
        deltaMove = Vector2.zero;

        if (onable == false)
        {
            touchedTime += Time.deltaTime;
            if (touchedTime > dateTime)
            {
                onable = true;
            }
        }

        if (onable == true)
        {
            if (Input.touchCount == 0)
            {
                Up = true;
                if (moved == false)
                {
                    clicked = true;
                }
                tempTouchCount = 0;
            }
            else if (Input.touchCount != tempTouchCount)
            {
                disabled = true;
            }
            else
            {
                pos = Input.GetTouch(0).position;
                int movedCount = 0;
                Vector2 delta = Vector2.zero;
                for (int i = 0; i < tempTouchCount; i++)
                {
                    Touch tch = Input.GetTouch(i);
                    if (tch.phase == TouchPhase.Moved)
                    {
                        movedCount++;
                        delta += tch.deltaPosition;
                    }
                }
                if (movedCount > 0)
                {
                    onMove = true;
                    deltaMove = delta / movedCount;
                }

                if (movedCount != 0 && tempTouchCount == 2)
                {
                    deltaDis = (Input.GetTouch(1).deltaPosition - Input.GetTouch(0).deltaPosition).magnitude;
                }
            }
        }
        else
        {
            if (Input.touchCount == 0)
            {
                Up = true;
                clicked = true;
                if (Time.time - clickedTime < doubleClickDateTime)
                {
                    if (Vector3.Distance(pos, clickedPos) < dateDis)
                    {
                        doubleClicked = true;
                    }
                }
                clickedTime = Time.time;
                clickedPos = pos;
                tempTouchCount = 0;
            }
            else
            {
                if (Input.touchCount == tempTouchCount)
                {
                    pos = Input.GetTouch(0).position;
                    for (int i = 0; i < tempTouchCount; i++)
                    {
                        if (Input.GetTouch(i).phase == TouchPhase.Moved)
                        {
                            onable = true;
                            moved = true;
                            //deltaMove = touch.deltaPosition;
                            return;
                        }
                    }                    
                }
                else if (Input.touchCount > tempTouchCount)
                {
                    //输入状态瞬间变化 待激活（如：可能有两只手指输入有先后 间隔很短）
                    tempTouchCount = Input.touchCount;
                    pos = Input.GetTouch(0).position;
                }
                else
                {

                }                
            }

        }
*/
