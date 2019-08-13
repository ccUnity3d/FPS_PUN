using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Singleton本身功能是给其他类型创建单例  他自己不一定是单例 
/// where T : IInstance, new() 表示T必须是单例和具有new（）构造方法
/// : IInstance表示自己也必须是单例
/// </summary>
/// <typeparam name="T"></typeparam>
public class Singleton<T> : MyEventDispatcher, IInstance where T : IInstance, new()
{
    private static T instance;

    public static T Instance
    {
        get {
            if (instance == null)
            {
                instance = new T();
                instance.OnInstance();
            }
            return instance;
        }
    }

    public virtual void OnInstance()    
    {
        
    }
}

public interface IInstance {
    /// <summary>
    /// 使用前 初始化数据
    /// </summary>
    void OnInstance();
}

