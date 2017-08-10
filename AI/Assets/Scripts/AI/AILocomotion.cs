using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AILocomotion : MonoBehaviour {


    //AI角色能达到的最大速度
    public float maxSpeed;

    //AI角色最大速度的平方
    public float sqrMaxSpeed;

    //AI角色能获得的最大的力
    public float maxForce;

    //AI角色的质量 
    public float mass;

    //AI角色的速度
    public Vector3 velocity;

    //AI转向的速度
    public float rotationSpeed;

    //AI角色的加速度
    public Vector3 acceleration;

    //操控力的计算间隔时间,为了达到更高的帧率,操控力不需要每帧更新
    public float computeInterval;

    //是否在一个平面上,如果在一个平面上忽略y值
    public bool isPlanar;

    //计算得到的操控力
    public Vector3 steeringForce;

    //AI角色包含的操控行为列表
    private Steering[] steerings;

    //计时器
    private float timer = 0.0f;

    //AI角色的控制器
    private CharacterController controller;

    //AI的刚体,运动系统 
    private Rigidbody rb;

    //AI每次移动的距离
    private Vector3 moveDistance;


    private void Start() {

        isPlanar = true;
        maxSpeed = 10.0f;
        sqrMaxSpeed = maxSpeed * maxSpeed;
        maxForce = 100.0f;
        rotationSpeed = 0.9f;
        computeInterval = 0.2f;

        velocity = Vector3.zero;
        acceleration = Vector3.zero;
        steeringForce = Vector3.zero;
        moveDistance = Vector3.zero;

        steerings = GetComponents<Steering>();
        controller = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();

    }

    //先fixedupdate->update
    //物理运动相关的操作放到FixedUpdate里边
    private void FixedUpdate() {

        //计算速度
        velocity += acceleration * Time.fixedDeltaTime;

        //限制速度最大值
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

        //计算AI角色的移动距离 
        moveDistance = velocity * Time.fixedDeltaTime;

        //执行Move动作
        DoMove();

        //播放行走动画
        //gameObject.animation.play("walk");
    }

    //每帧计算操作力
    private void Update() {
        if (CanComputeSteeringForce()) {
            DoComputeSteeringForce();
        }
    }

    //执行move操作改变物体的位置
    private void DoMove() {
        //如果要求角色在平面上移动,置y为0
        if (isPlanar) {
            velocity.y = 0;
            moveDistance.y = 0;
        }

        //如果已经为角色添加了控制器,利用控制器移动 
        if (controller) {
            controller.SimpleMove(velocity);
        }
        //如果没控制器也没rb或者有rb但是设定用运动学的方式控制它
        else if (rb == null || rb.isKinematic == true) {
            transform.position += moveDistance;
        }
        //只有rb
        else {
            rb.MovePosition(rb.position + moveDistance);
        }

        //更新朝向,如果速度大于某个阈值(为了防止抖动)
        if (velocity.sqrMagnitude > 0.0001) {
            //通过当前朝向与速度方向的插值,计算新的朝向
            Vector3 newForward = Vector3.Slerp(transform.forward, velocity, rotationSpeed * Time.deltaTime);
            if (isPlanar)
                newForward.y = 0;
            transform.forward = newForward;
        }
    }

    //是否可以计算操纵力
    private bool CanComputeSteeringForce() {
        timer += Time.deltaTime;
        if (timer >= computeInterval) {
            timer = 0;
            return true;
        }
        return false;
    }

    //计算操作力
    private void DoComputeSteeringForce() {
        //求合力
        foreach (Steering s in steerings) {
            if (s.enabled) {
                steeringForce += s.Force() * s.Weight;
            }
        }
        //控制操作力不大于maxforce
        steeringForce = Vector3.ClampMagnitude(steeringForce, maxForce);
        //计算加速度
        acceleration = steeringForce / mass;
    }
}
