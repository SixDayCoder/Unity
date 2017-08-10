using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringForSeek : Steering {

    //要寻找的物体
    public GameObject target;

    //预期速度
    private Vector3 desiredVelocity;

    //AI角色
    private AILocomotion locomotion;

    private void Start() {
        locomotion = GetComponent<AILocomotion>();
    }

    public override Vector3 Force() {
        //计算预期速度
        desiredVelocity = (target.transform.position - transform.position).normalized * locomotion.maxSpeed;

        if (locomotion.isPlanar) {
            desiredVelocity.y = 0;
        }

        //返回操控力向量,即预期速度和当前速度的差
        return (desiredVelocity - locomotion.velocity);
    }

}
