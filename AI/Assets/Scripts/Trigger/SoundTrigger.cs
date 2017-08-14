using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTrigger : TriggerLimitedLifeTime {

    private void Start() {

        lifeTime = 3;

        TriggerSystemManager.Instance.RegisterTrigger(this);
    }

    public override void Reaction(Sensor s) {

        if (isTouchingTrigger(s)) {
            s.Notify(this);
        }

    }

    protected override bool isTouchingTrigger(Sensor sensor) {

        GameObject go = sensor.gameObject;

        if(sensor.sensorType == SensorType.Sound) {
            if (Vector3.Distance(transform.position, go.transform.position) < radius)
                return true;
        }

        return false;
    }

    private void OnDrawGizmos() {

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);
    }


}
