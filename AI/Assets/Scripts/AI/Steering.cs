using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Steering : MonoBehaviour {

    //每个操作力的权重
    public float Weight = 1;

    //计算操作力的方法,由派生类实现
    public virtual Vector3 Force() {
        return Vector3.zero;
    }
}
