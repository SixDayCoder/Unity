using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CharacterController))]
public class AILocomotion : Vehicle {

    //AI角色的控制器
    private CharacterController controller;

    //AI的刚体,运动系统 
    private Rigidbody rb;

    //AI每次移动的距离
    private Vector3 moveDistance;


    private void Start() {

        controller = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        moveDistance = Vector3.zero;

        //初始化基类的数据

    }


    //物理运动相关的操作放到FixedUpdate里边
    private void FixedUpdate() {

        //计算速度
        Velocity += Acceleration * Time.fixedDeltaTime;

        //限制速度最大值
        if(Velocity.sqrMagnitude > SqrMaxSpeed) {
            Velocity = Velocity.normalized * MaxSpeed;
        }

        //计算AI角色的移动距离 
        moveDistance = Velocity * Time.fixedDeltaTime;

        //如果要求角色在平面上移动,置y为0
        if (IsPlanar) {
            Velocity.y = 0;
            moveDistance.y = 0;
        }

        //如果已经为角色添加了控制器,利用控制器移动 
        if (controller) {
            controller.SimpleMove(Velocity);
        }
        //如果没控制器也没rb或者有rb但是设定用运动学的方式控制它
        else if(rb == null || rb.isKinematic == true) {
            transform.position += moveDistance;
        }
        //只有rb
        else {
            rb.MovePosition(rb.position + moveDistance);
        }

        //更新朝向,如果速度大于某个阈值(为了防止抖动)
        if(Velocity.sqrMagnitude > 0.0001) {
            //通过当前朝向与速度方向的插值,计算新的朝向
            Vector3 newForward = Vector3.Slerp(transform.forward, Velocity, Damping * Time.deltaTime);
            if (IsPlanar)
                newForward.y = 0;
            transform.forward = newForward;
        }

        //播放行走动画
        //gameObject.animation.play("walk");
    }
}
