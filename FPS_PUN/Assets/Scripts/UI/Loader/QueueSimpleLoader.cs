
using System;
using System.Collections.Generic;

public class QueueSimpleLoader : MyEventDispatcher {

    public object bringData;
    public List<QueueItem> loaderQueue = new List<QueueItem>();
    private int count = 0;
    private float temptotal;
    private MainPageUIController mainPageUIController
    {
        get {
            return MainPageUIController.Instance;
        }
    }
    public int getCount
    {
        get
        {
            return count;
        }
    }
    public QueueSimpleLoader(object data = null)
    {
        this.bringData = data;
        SimpleLoader.StaticEventDispatcher.addEventListener(LoadEvent.Cancel, CancelQueueLoad);
    }

    private void CancelQueueLoad(MyEvent obj)
    {
        mainPageUIController.dispatchEvent(new LoadEvent(LoadEvent.CloseLoadingPage));
        ClearAllListioner();
        loaderQueue.Clear();
    }

    private void ClearAllListioner()
    {
        this.ClearListioner();
        for (int i = 0; i < loaderQueue.Count; i++)
        {
            loaderQueue[i].ClearListioner();
        }
    }

    public float progress
    {
        get {
            if (count == 0) return 1;
            temptotal = 0;
            for (int i = 0; i < loaderQueue.Count; i++)
            {
                temptotal += loaderQueue[i].progress;
            }
            return temptotal / count;
        }
    }

    /// <summary>
    /// 项进度
    /// </summary>
    public float itemProgress
    {
        get
        {
            if (count == 0) return 1;
            temptotal = 0;
            for (int i = 0; i < loaderQueue.Count; i++)
            {
                if (loaderQueue[i].progress == 1)
                {
                    temptotal += 1;
                }
            }
            return temptotal / count;
        }
    }

    public void AddQueueItem(SimpleLoader loader)
    {
        if(loader==null) return;
        QueueItem item = new QueueItem(loader);
        item.addEventListener(LoadEvent.Progress, ItemProgress);
        item.addEventListener(LoadEvent.Complete, ItemComplete);
        loaderQueue.Add(item);
        count = loaderQueue.Count;
    }

    private void ItemComplete(MyEvent obj)
    {
        QueueItem item = obj.data as QueueItem;
        item.removeEventListener(LoadEvent.Complete, ItemComplete);
        this.dispatchEvent(new LoadEvent(LoadEvent.ItemProgress, new object[] { item.loader, itemProgress }));
    }

    private void ItemProgress(MyEvent obj)
    {
        this.dispatchEvent(new LoadEvent(LoadEvent.QueueProgress, progress));
        if (progress == 1)
        {
            this.dispatchEvent(new LoadEvent(LoadEvent.QueueComplete, this));
        }
    }

    //public void AddQueueItem(SimpleLoader loader)
    //{
    //    loaderQueue.Add(loader);
    //}

    public void Stop()
    {
        ClearAllListioner();
        for (int i = 0; i < loaderQueue.Count; i++)
        {
            loaderQueue[i].Stop();
        }
    }

    public class QueueItem : MyEventDispatcher
    {
        public float progress;
        public SimpleLoader loader;
        public SimpleLoadedState state
        {
            get {
                if (loader == null) return SimpleLoadedState.None;
                return loader.state;
            }
        }

        public QueueItem(SimpleLoader loader)
        {
            this.loader = loader;
            loader.addEventListener(LoadEvent.Complete, LoadComplete);
            loader.addEventListener(LoadEvent.Progress, LoadProgress);
        }

        private void LoadProgress(MyEvent obj)
        {
            progress = (float)obj.data;
            this.dispatchEvent(new LoadEvent(LoadEvent.Progress, loader.progress));
        }

        private void LoadComplete(MyEvent obj)
        {
            loader.removeEventListener(LoadEvent.Complete, LoadComplete);
            loader.removeEventListener(LoadEvent.Progress, LoadProgress);
            this.dispatchEvent(new LoadEvent(LoadEvent.Complete, this));
            progress = 1;
            this.dispatchEvent(new LoadEvent(LoadEvent.Progress, progress));
        }

        public void Stop()
        {
            loader.canceled = true;
            loader.removeEventListener(LoadEvent.Complete, LoadComplete);
            loader.removeEventListener(LoadEvent.Progress, LoadProgress);
            SimpleLoader.RemoveLoader(loader);
        }
    }
    
}
