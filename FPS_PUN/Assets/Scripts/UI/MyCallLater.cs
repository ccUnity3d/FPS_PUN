using System;
using System.Collections.Generic;
using UnityEngine;

public class MyCallLater
{
    private static MyCallLater instance = new MyCallLater();

    private List<HandleTask> list = new List<HandleTask>();
    private List<HandleTask> copylist = new List<HandleTask>();

    public MyCallLater()
    {

    }

    public static bool Add(Action handler, float deleTime = 0)
    {
        return instance.add(deleTime, handler);
    }

    public static bool Add(float deleTime, Action handler)
    {
        return instance.add(deleTime, handler);
    }
    public static bool Add(Action<object> handler, float deleTime, object data)
    {
        return instance.add(deleTime, handler, data);
    }

    public static bool Add(float deleTime, Action<object> handler, object data)
    {
        return instance.add(deleTime, handler, data);
    }

    public static void Remove(Action handler)
    {
        instance.remove(handler);
    }
    private void remove(Action handle)
    {
        HandleTask task;
        if (tryGetTask(handle, out task))
        {
            list.Remove(task);
        }
    }
    private void remove(Action<object> handle)
    {
        HandleTask task;
        if (tryGetTask(handle, out task))
        {
            list.Remove(task);
        }
    }

    private bool tryGetTask(Action handle, out HandleTask task)
    {
        task = null;
        if (handle == null) return false;
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].Equle(handle))
            {
                task = list[i];
                return true;
            }
        }
        return false;
    }
    
    private bool tryGetTask(Action<object> handle, out HandleTask task)
    {
        task = null;
        if (handle == null) return false;
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].Equle(handle))
            {
                task = list[i];
                return true;
            }
        }
        return false;
    }

    private bool add(float deletime, Action handle)
    {
        HandleTask task;
        if (tryGetTask(handle, out task))
        {
            //Debug.LogWarning(handle.Method.Name + " false");
            return false;
        }
        //Debug.LogWarning(handle.Method.Name + " true");
        task = new ActionTask(handle, deletime);
        list.Add(task);
        if (list.Count == 1)
        {
            MyTickManager.Add(render);
        }
        return true;
    }

    private bool add(float deletime, Action<object> handle, object data)
    {
        HandleTask task;
        if (tryGetTask(handle, out task))
        {
            return false;
        }
        task = new ObjActionTask(handle, deletime, data);
        list.Add(task);
        if (list.Count == 1)
        {
            MyTickManager.Add(render);
        }
        return true;
    }

    private void render()
    {
        copylist.Clear();
        copylist.InsertRange(0, list);
        float time = Time.time;
        for (int i = 0; i < copylist.Count; i++)
        {
            HandleTask task = copylist[i];
            if (task.calltime == time)
            {
                continue;
            }
            float runTime = task.calltime + task.deletime;
            if (time < runTime)
            {
                continue;
            }
            task.Do();
            //copylist.Remove(task);
            //i--;
            list.Remove(task);

        }
        if (copylist.Count == 0)
        {
            MyTickManager.Remove(render);
        }
    }

    abstract class HandleTask
    {
        /// <summary>
        /// 调用时间
        /// </summary>
        public float calltime;
        /// <summary>
        /// 延迟执行时间
        /// </summary>
        public float deletime;

        public HandleTask(float deletime)
        {
            calltime = Time.time;
            this.deletime = deletime;
        }

        public abstract void Do();

        public abstract bool Equle(object handle);

    }

    class ActionTask : HandleTask
    {
        public Action handle;

        public ActionTask(Action handle, float deletime = 0) : base(deletime)
        {
            this.handle = handle;
        }

        public override void Do()
        {
            handle();
            //Debug.LogWarning("Remove " + handle.Method.Name);
        }

        public override bool Equle(object handle)
        {
            return (this.handle) == (Delegate)handle;
        }
    }

    class ObjActionTask : HandleTask
    {
        public Action<object> handle;
        private object data; 

        public ObjActionTask(Action<object> handle, float deletime = 0, object data = null) : base(deletime)
        {
            this.handle = handle;
            this.data = data;
        }

        public override void Do()
        {
            handle(data);
        }

        public override bool Equle(object handle)
        {
            return (this.handle) == (Delegate)handle;
        }
    }

}
