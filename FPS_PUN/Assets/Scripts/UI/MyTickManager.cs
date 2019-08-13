using System;
using System.Collections.Generic;
using UnityEngine;

public class MyTickManager
{

    private static MyTickManager instance;
    public static MyTickManager Instance
    {
        get {
            if (instance == null) instance = new MyTickManager();
            return instance;
        }
    }

    private List<Action> actionList = new List<Action>();
    private List<Action> temp = new List<Action>();

    //private List<float> tempAddTime = new List<float>();
    //private List<Action> tempAdd = new List<Action>();

    public static void Add(Action act)
    {
        Instance.add(act);
    }

    public static void Remove(Action act)
    {
        Instance.remove(act);
    }

    public void add(Action act)
    {
        //tempAddTime.Add(Time.realtimeSinceStartup);
        //tempAdd.Add(act);
        if (actionList.Contains(act))
        {
            return;
        }
        actionList.Add(act);
    }

    public void remove(Action act)
    {
        if (actionList.Contains(act) == false)
        {
            return;
        }
        actionList.Remove(act);
    }

    public void tick()
    {
        //if (tempAdd.Count > 0)
        //{
        //    for (int i = 0; i < tempAdd.Count; i++)
        //    {
        //        if (Time.realtimeSinceStartup > tempAddTime[i])
        //        {
        //            if (actionList.Contains(tempAdd[i]) == false)
        //            {
        //                actionList.Add(tempAdd[i]);
        //            }
        //            tempAddTime.RemoveAt(i);
        //            tempAdd.RemoveAt(i);
        //        }
        //    }            
        //}
        if (actionList.Count == 0) return;
        temp.Clear();
        temp.AddRange(actionList);
        for (int i = 0; i < temp.Count; i++)
        {
            temp[i]();
        }
    }

}
