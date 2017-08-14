using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringFollowPath : Steering {

    //路径点标识
    public GameObject[] wayPoints = new GameObject[5];

    //AI
    private AILocomotion locomotion;

    //当前节点的序号
    private int currentNode = 0;

    //当前的target
    private Transform currentTarget;

    //当和路径点的距离小于该值时,认为已经到达,开始寻找一下点
    private float arriveDistance;

    //当和目标距离小于该值时,开始减速 
    private float slowDownDistance;

    private void Start() {

        locomotion = GetComponent<AILocomotion>();

        arriveDistance = 1.0f;

        slowDownDistance = 1.0f;

        currentNode = 0;

        currentTarget = wayPoints[currentNode].GetComponent<Transform>();
    }

    public override Vector3 Force() {

        Vector3 toTarget = currentTarget.position - transform.position;

        if (locomotion.isPlanar) {

            toTarget.y = 0;

        }

        float distance = toTarget.magnitude;

        //如果是路径点的最后一个节点
        if(currentNode == wayPoints.Length - 1) {

            //如果到了减速的距离,减速
            if(distance < slowDownDistance) {
                //预期速度
                Vector3 desiredVelocity = toTarget - locomotion.velocity;
                //操控力
                return desiredVelocity - locomotion.velocity;
            }
            //否则,seek行为
            else {
                //预期速度
                Vector3 desiredVelocity = toTarget.normalized * locomotion.maxSpeed;
                //操控力
                return desiredVelocity - locomotion.velocity;
            }
        }
        else {
            //如果到达了当前路径点,那么准备变换到下一个路径点 
            if(distance < arriveDistance) {

                //切换target目标
                currentNode++;

                currentTarget = wayPoints[currentNode].transform;

            }

            //计算预期速度和操控力
            Vector3 desiredVelocity = toTarget.normalized * locomotion.maxSpeed;

            return desiredVelocity - locomotion.velocity;

        }
  
    }

}
