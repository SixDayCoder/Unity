using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityEngine.AI;

//描述Defender的防御行为
public class Defend : Action{

    public SharedFloat viewDistance;

    public SharedFloat filedOfViewAngle;

    public SharedFloat speed;

    public SharedFloat angularSpeed;

    public SharedTransform target;

    private NavMeshAgent navMeshAgent;

    public override void OnAwake() {

        navMeshAgent = GetComponent<NavMeshAgent>();

    }

    public override void OnStart() {
        //启用导航组件
        navMeshAgent.enabled = true;
        navMeshAgent.speed = speed.Value;
        navMeshAgent.angularSpeed = angularSpeed.Value;
        navMeshAgent.destination = target.Value.position;
    }

    //如果attacker在视野范围之内就去追,否则认为防御成功
    public override TaskStatus OnUpdate() {

        if (target == null || target.Value == null)
            return TaskStatus.Failure;

        //如果Attacker在视野范围之内
        if (IsInSight()) {
            return TaskStatus.Running;
        }

        return TaskStatus.Success;
    }

    public override void OnEnd() {
        //禁用导航组件
        navMeshAgent.enabled = false;
    }

    private bool IsInSight() {

        Vector3 toTarget = target.Value.position - transform.position;
        float angle = Vector3.Angle(transform.forward, toTarget);

        //如果在视野中 
        if (angle < filedOfViewAngle.Value * 0.5 &&
           toTarget.magnitude < viewDistance.Value) {
            return true;
        }

        return false;   
    }

}
