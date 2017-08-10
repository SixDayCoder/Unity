using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : MonoBehaviour {

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

    private void Start() {

        maxSpeed = 10.0f;
        sqrMaxSpeed = maxSpeed * maxSpeed;
        maxForce = 100.0f;
        velocity = Vector3.zero;
        rotationSpeed = 0.9f;
        computeInterval = 0.2f;
        acceleration = Vector3.zero;
        isPlanar = true;
        steeringForce = Vector3.zero;

        steerings = GetComponents<Steering>();

        if(steerings != null) {
            Debug.Log(steerings.Length);
            Debug.Log(steerings[0].tag);
        }
    }

    private void Update() {
        if (CanComputeSteeringForce()) {

            //求合力
            foreach(Steering s in steerings) {
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

    //可以计算操纵力
    private bool CanComputeSteeringForce() {
        timer += Time.deltaTime;
        if(timer >= computeInterval) {
            timer = 0;
            return true;
        }
        return false;
    }
}
