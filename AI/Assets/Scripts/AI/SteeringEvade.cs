using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringEvade : Steering {


    //目标
    public GameObject target;

    //AI
    private AILocomotion locomotion;

    private void Start() {

        locomotion = GetComponent<AILocomotion>();

    }

    public override Vector3 Force() {

        Vector3 toTarget = target.transform.position - transform.position;

        Vector3 targetVelocity = target.GetComponent<AILocomotion>().velocity;

        //预测时间
        float lookAheadTime = toTarget.magnitude / (locomotion.velocity.magnitude + targetVelocity.magnitude);

        Vector3 targetPos = target.transform.position + targetVelocity * lookAheadTime;

        //计算预期速度,向着远离追逐者的地方
        Vector3 desiredVelocity = (transform.position - targetPos).normalized * locomotion.maxSpeed;

        return desiredVelocity - locomotion.velocity;

    }
}
