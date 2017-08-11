using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringForCohesion : Steering {

    //AI角色的雷达
    private Radar radar;

    //AI
    private AILocomotion locomotion;

    //邻居的数量
    private int neighborCount;

    private void Start() {

        radar = GetComponent<Radar>();

        locomotion = GetComponent<AILocomotion>();

    }

    //如果Radar的detectRadius很大,那么会倾向于聚集到一个地方,否则倾向于聚集到多个地方
    public override Vector3 Force() {

        //操纵力
        Vector3 steeringForce = Vector3.zero;

        //AI角色所有邻居的重心 
        Vector3 centerMass = Vector3.zero;

        //清零邻居数量
        neighborCount = 0;

        foreach (var obj in radar.GetNeighbors()) {

            //如果不是当前游戏物体
            if ((obj != null) && (obj != this.gameObject)) {

                centerMass += obj.transform.position;

                neighborCount++;

            }

            if(neighborCount > 0) {

                //计算平均值
                centerMass /= neighborCount;

                //计算预期速度
                Vector3 desiredVelocity = (centerMass - transform.position).normalized * locomotion.maxSpeed;

                steeringForce = desiredVelocity - locomotion.velocity;
            }

        }

        return steeringForce;

    }
}
