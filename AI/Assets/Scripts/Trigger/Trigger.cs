using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//所有触发器的基类
public class Trigger : MonoBehaviour {

   
    //触发器的位置
    protected Vector3 position;

    //触发器的半径
    protected float radius;

    //当前触发器是否需要被移除
    public bool toBeRemoved;

    //检测感知器sensor是否在触发器的范围内,即触发器是否能被sensor感知到
    protected virtual bool isTouchingTrigger(Sensor sensor) {
        return false;
    }

    //更新触发器的内部状态,例如声音触发器的剩余有效时间等属性
    public virtual void UpdateStatus() {

    }

    public virtual void Reaction(Sensor s) {

    }

}
