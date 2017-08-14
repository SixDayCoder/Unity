using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightSensor : Sensor {

    //定义该AI角色的视域
    public float fieldOfView = 45;

    //定义该AI角色能看到的最远的距离
    public float viewDistance;

    private void Start() {
        sensorType = SensorType.Sight;
        //注册感知器
        TriggerSystemManager.Instance.RegisterSensor(this);
    }

    public override void Notify(Trigger t) {

        Debug.Log("I can see : " + t.gameObject.name);

        Debug.DrawLine(transform.position, t.transform.position, Color.red);

        //move to 

    }

    private void OnDrawGizmos() {

        float fieldOfViewInRadius = fieldOfView * Mathf.PI / 180.0f;

        Vector3 frontRayPoint = transform.position + (transform.forward * viewDistance);

        Vector3 leftRayPoint = transform.TransformPoint(new Vector3( viewDistance * Mathf.Sin(fieldOfViewInRadius), 0, viewDistance * Mathf.Cos(fieldOfViewInRadius)));

        Vector3 rightRayPont = transform.TransformPoint(new Vector3(-viewDistance * Mathf.Sin(fieldOfViewInRadius), 0, viewDistance * Mathf.Cos(fieldOfViewInRadius)));

        Debug.DrawLine(transform.position, frontRayPoint, Color.green);

        Debug.DrawLine(transform.position, leftRayPoint, Color.green);

        Debug.DrawLine(transform.position, rightRayPont, Color.green);
    }
}
