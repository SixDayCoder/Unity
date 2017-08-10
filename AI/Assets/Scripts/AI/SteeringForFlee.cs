using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringForFlee : Steering {

    //目标
    public GameObject target;

    //安全距离
    private float safeDistance = 30;

    //AI
    private AILocomotion locomotion;


    //判断物体是否在危险区域
    private bool IsInDangerousRange() {
        Vector3 pos = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 targetPos = new Vector3(target.transform.position.x, 0, target.transform.position.z);

        if (Vector3.Distance(pos, targetPos) <= safeDistance) {
            return true;
        }
        return false;   
    }

    private void Start() {
        locomotion = GetComponent<AILocomotion>();
    }

    public override Vector3 Force() {

        //预期速度
        Vector3 desiredVelocity;

        if (IsInDangerousRange()) {
            desiredVelocity = (transform.position - target.transform.position).normalized * locomotion.maxSpeed;
            if (locomotion.isPlanar) {
                desiredVelocity.y = 0;
            }
            return desiredVelocity - locomotion.velocity;
        }
        else {
            return Vector3.zero;
        }
      
    }

}
