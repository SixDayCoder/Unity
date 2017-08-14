using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightTrigger : Trigger {


    private void Start() {
        //注册触发器
        TriggerSystemManager.Instance.RegisterTrigger(this);
    }

    //带有SightTrigger的物体很可能是可移动的,因此要不停的更新触发器的位置
    public override void UpdateStatus() {
        position = transform.position;
    }


    public override void Reaction(Sensor s) {

        //如果感知器能够感知到该触发器,那么向感知器发送消息,感知器作出相应的决策
        if (isTouchingTrigger(s)) {
            s.Notify(this);
        }

    }

    protected override bool isTouchingTrigger(Sensor sensor) {

        GameObject go = sensor.gameObject;

        if(sensor.sensorType == SensorType.Sight) {
            //从sensor指向触发器的射线
            Vector3 rayDirection = transform.position - go.transform.position;
            rayDirection.y = 0;

            //判断感知体的前向和触发器的方向是否在视域范围之内
            if( (Vector3.Angle(rayDirection, go.transform.forward)) < 
                (sensor as SightSensor).fieldOfView){

                RaycastHit hit;
                if(Physics.Raycast(go.transform.position, rayDirection, 
                                   out hit, 
                                   (sensor as SightSensor).viewDistance)) {

                    if (hit.collider.gameObject == this.gameObject)
                        return true;

                }

            }

        }

        return false;
    }

}
