using UnityEngine;
using System.Collections;

public class UIData<T> : Singleton<T>, IUIData
    where T : IInstance, IUIData, new()
{

}

public interface IUIData
{

}
