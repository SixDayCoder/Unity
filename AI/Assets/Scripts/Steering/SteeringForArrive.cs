using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringForArrive : Steering {

    //目标
    public GameObject target;

    //减速半径
    private float slowDownDistance;

    //AI
    private AILocomotion locomotion;

    private void Start() {

        locomotion = GetComponent<AILocomotion>();

        slowDownDistance = 5.0f;
    }

    public override Vector3 Force() {

        //预期速度
        Vector3 desiredVelocity;
        //目标位置向量
        Vector3 toTarget = target.transform.position - transform.position;

        if (locomotion.isPlanar) {

            toTarget.y = 0;

        }

        //计算到目标点的位置
        float dist = toTarget.magnitude;

        //减速的快慢,假设希望减速2s
        float decelerationTime = 2.0f;

        if(dist < slowDownDistance) {

            //给定预期的减速度,计算到达目标位置所需的速度 
            float speed = dist / decelerationTime; // v = s/t

            speed = Mathf.Min(speed, locomotion.maxSpeed);

            desiredVelocity = toTarget.normalized * speed;

            return desiredVelocity - locomotion.velocity;

        }
        else {
            //这部分和seek一样
            //计算预期速度
            desiredVelocity = toTarget.normalized * locomotion.maxSpeed;

            //返回操控力向量,即预期速度和当前速度的差
            return desiredVelocity - locomotion.velocity;

        }

    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(target.transform.position, slowDownDistance);
    }
}
