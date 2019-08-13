using UnityEngine;
using System.Collections;

public class LoadEvent : MyEvent {

    public const string Progress = "Progress";
    public const string Cancel = "Cancel";
    public const string Complete = "Complete";
    public const string QueueProgress = "QueueProgress";
    public const string QueueComplete = "QueueComplete";

    public const string OpenLoadingPage = "OpenLoadingPage";
    public const string RefreshLoadingPage = "RefreshLoadingPage";
    public const string CloseLoadingPage = "CloseLoadingPage";

    public const string ItemProgress = "ItemProgress";

    public LoadEvent(string type, object data = null):base(type, data)
    {

    }
}
