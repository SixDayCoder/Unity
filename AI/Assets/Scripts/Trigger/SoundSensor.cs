using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSensor : Sensor {

    //感知体的听力范围,这里并没有实际用处
    public float hearingDistance = 30.0f;

    private void Start() {

        sensorType = SensorType.Sound;

        TriggerSystemManager.Instance.RegisterSensor(this);

    }

    public override void Notify(Trigger t) {

        Debug.Log("I can hear : " + t.gameObject.name);

        //move to
    }


}
