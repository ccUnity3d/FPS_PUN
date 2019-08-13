using UnityEngine;
using System.Collections;

public interface IController {

    IPage getPanel { get; }
    void SetData(GameObject goClone);
    void ready();
    void awake();
    void sleep();
}
