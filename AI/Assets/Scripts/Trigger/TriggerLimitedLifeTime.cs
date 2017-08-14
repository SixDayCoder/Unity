using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//所有具有特定生命周期的触发器的基类
public class TriggerLimitedLifeTime : Trigger {

    //该触发器持续的时间
    protected int lifeTime;


    public override void UpdateStatus() {
        lifeTime--; 
        if(lifeTime <= 0) {
            toBeRemoved = true;
        }

    }
}
