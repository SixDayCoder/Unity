using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : MonoBehaviour {

    //AI角色能达到的最大速度
    public float MaxSpeed;

    //AI角色最大速度的平方
    public float SqrMaxSpeed;

    //AI角色能获得的最大的力
    public float MaxForce;

    //AI角色的质量 
    public float Mass;

    //AI角色的速度
    public Vector3 Velocity;

    //AI转向的速度
    public float Damping;

    //AI角色的加速度
    public Vector3 Acceleration;

    //操控力的计算间隔时间,为了达到更高的帧率,操控力不需要每帧更新
    public float ComputeInterval;

    //是否在一个平面上,如果在一个平面上忽略y值
    public bool IsPlanar;

    //计算得到的操控力
    public Vector3 SteeringForce;

    //AI角色包含的操控行为列表
    private Steering[] steerings;

    //计时器
    private float timer = 0.0f;

    private void Start() {

        MaxSpeed = 10.0f;
        SqrMaxSpeed = MaxSpeed * MaxSpeed;
        MaxForce = 100.0f;
        Velocity = Vector3.zero;
        Damping = 0.9f;
        ComputeInterval = 0.2f;
        Acceleration = Vector3.zero;
        IsPlanar = true;
        SteeringForce = Vector3.zero;

        steerings = GetComponents<Steering>();
    }

    private void Update() {
        if (CanComputeSteeringForce()) {

            //求合力
            foreach(var s in steerings) {
                if (s.enabled) {
                    SteeringForce += s.Force() * s.Weight;
                }
            }

            //控制操作力不大于maxforce
            SteeringForce = Vector3.ClampMagnitude(SteeringForce, MaxForce);
            //计算加速度
            Acceleration = SteeringForce / Mass;
        }
    }

    //可以计算操纵力
    private bool CanComputeSteeringForce() {
        timer += Time.deltaTime;
        if(timer >= ComputeInterval) {
            timer = 0;
            return true;
        }
        return false;
    }
}
