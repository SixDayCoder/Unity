using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringForWander : Steering {

    //AI
    private AILocomotion locomotion;

    //徘徊圆圈的半径
    private float wanderRadius;

    //徘徊距离,物体和徘徊圆圈中点的距离
    private float wanderDistance;

    //每帧随机小位移的值
    private float wanderJitter;

    //在wander圈上虚拟出的物体的位置
    private Vector3 circleTarget;

    private void Start() {
       
        locomotion = GetComponent<AILocomotion>();

        //徘徊距离越大,物体旋转的角度越小
        wanderDistance = 2.0f;

        wanderRadius = 1.0f;

        wanderJitter = 1.0f;

        //给定一个圆上位置的初始值,这里我把它放到了距离AI物体最远的圆上的点
        circleTarget = transform.forward.normalized * (wanderDistance + wanderRadius);
    }

    public override Vector3 Force() {

        //计算随机位移
        Vector3 displacement = new Vector3(Random.value * wanderJitter, Random.value * wanderJitter, Random.value * wanderJitter);

        if (locomotion.isPlanar) {

            displacement.y = 0;

        }

        //1.让wander圈上的点随机位移
        circleTarget += displacement;

        //2.由于移动后可能会超出wander圈,把新的位置投影到圈上 ? 为什么这样可以
        circleTarget = circleTarget.normalized * wanderRadius;

        //3.现在计算的值是相对于AI角色和AI角色的前进方向的，需要转换为世界坐标 
        Vector3 targetWorld = locomotion.GetComponent<Transform>().TransformPoint(circleTarget);

        //计算预期速度
        Vector3 desiredVelocity = (targetWorld - transform.position).normalized * locomotion.maxSpeed;

        return (desiredVelocity - locomotion.velocity);

    }

    private void OnDrawGizmos() {

        Gizmos.DrawWireSphere(transform.forward.normalized * wanderDistance, wanderRadius);

    }
}
