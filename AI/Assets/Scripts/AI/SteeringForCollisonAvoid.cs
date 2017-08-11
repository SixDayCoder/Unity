using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringForCollisonAvoid : Steering {

    //障碍物
    private GameObject[] colliders;

    //AI
    private AILocomotion locomotion;

    //逃避力的最大力度
    private float maxAvoidenceForce;

    //AI向前看的最大距离
    private readonly float MAX_SEE_HEAD = 5.0f;

    private void Start() {

        maxAvoidenceForce = 30.0f;

        locomotion = GetComponent<AILocomotion>();

        colliders = GameObject.FindGameObjectsWithTag("Obstacle");
        
    }

    public override Vector3 Force() {

        //画出一条线,考查和该摄像相交的物体
        Debug.DrawLine(transform.position, transform.position + locomotion.velocity.normalized * MAX_SEE_HEAD);

        RaycastHit hit;

        //从物体向着速度行进方向发出射线
        if(Physics.Raycast(transform.position, locomotion.velocity.normalized, out hit, MAX_SEE_HEAD)){

            //射线检测成功,那么说明可能和该碰撞体碰撞,提前拐弯 

            Debug.Log(hit.collider.name);

            Vector3 ahead = transform.position + locomotion.velocity.normalized * MAX_SEE_HEAD;
            ahead *= maxAvoidenceForce;

            if (locomotion.isPlanar) {

                ahead.y = 0;

            }

            //将该碰撞体的颜色改为红色,其他的是灰色
            foreach(var c in colliders) {

                if (hit.collider.gameObject == c)

                    c.GetComponent<Renderer>().material.color = Color.red;

                else

                    c.GetComponent<Renderer>().material.color = Color.gray;

            }

            //远离碰撞体的方向 
            return ahead - hit.collider.transform.position;

        }
        //射线检测失败,不会发生碰撞,那么不需要改变原来的运动状态
        else {

            //改变颜色
            foreach(var c in colliders) {
            
                c.GetComponent<Renderer>().material.color = Color.white; 
            
            }

            return Vector3.zero;
        }
    }
}
