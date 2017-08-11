using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringForSeparation : Steering{

    //可接受的距离
    private float comfortDistance = 2.0f;

    //AI角色的雷达
    private Radar radar;

    //AI
    private AILocomotion locomotion;

    private void Start() {

        radar = GetComponent<Radar>();

        locomotion = GetComponent<AILocomotion>();

    }

    public override Vector3 Force() {

        Vector3 steeringForce = Vector3.zero;

        foreach(var obj in radar.GetNeighbors()) {

            //如果不是当前游戏物体
            if( (obj != null) && (obj != this.gameObject) ){

                Vector3 toNeighbor = obj.transform.position - transform.position;

                if (locomotion.isPlanar)

                    toNeighbor.y = 0;

                float dist = toNeighbor.magnitude;

                //计算和该邻居之间的排斥力,排斥力和距离成反比,距离越近力度越大 
                Vector3 force = (locomotion.velocity - toNeighbor).normalized / dist;

                //如果距离太小,力度加大
                if (dist < comfortDistance)
                    force += force;

                //计算出合力
                steeringForce += force;
            }

        }

        return steeringForce;

    }

}
