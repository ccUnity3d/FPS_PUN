using UnityEngine;

public interface IPage {

    GameObject skin { get; set; }

    void SetData(Object arg1);
    string GetPrefabPath();
}
