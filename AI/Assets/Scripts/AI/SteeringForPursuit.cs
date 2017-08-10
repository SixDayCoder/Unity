using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringForPursuit : Steering {

    //目标
    public GameObject target;

    //AI
    private AILocomotion locomotion;


    private void Start() {

        locomotion = GetComponent<AILocomotion>();

    }

    public override Vector3 Force() {

        //预期速度
        Vector3 desiredVelocity;
        //目标位置向量
        Vector3 toTarget = target.transform.position - transform.position;
        //计算追逐者的前向和逃避者的前向的夹角
        float relativeAngle = Vector3.Dot(transform.forward, target.transform.forward);

        //如果夹角为正,且追逐者基本面对着逃避者,直接向逃避者当前的位置前进
        //我们认为逃避者的反向和AI角色的夹角在20度以内就认为追逐者面对着逃避者,cos(20度) = 0.94
        if( (Vector3.Dot(transform.forward, toTarget) > 0) &&
             relativeAngle > 0.94) {

            //计算预期速度
            desiredVelocity = toTarget.normalized * locomotion.maxSpeed;

            return desiredVelocity - locomotion.velocity;
        }
        //否则,计算预测时间,直接到预测地点拦截逃避者,预测时间正比于追逐者和逃避着的距离,反比于追逐者和逃避者的速度的和
        else {

            //目标的行进速度
            Vector3 targetVelocity = target.GetComponent<AILocomotion>().velocity;

            float lookAheadTime = toTarget.magnitude / (locomotion.velocity.magnitude + targetVelocity.magnitude);

            Vector3 targetPos = target.transform.position + targetVelocity * lookAheadTime;

            desiredVelocity = (targetPos - transform.position).normalized * locomotion.maxSpeed;

            return desiredVelocity - locomotion.velocity;
        }

    }

}
