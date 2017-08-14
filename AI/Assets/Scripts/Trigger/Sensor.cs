using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SensorType {
    Sight,
    Sound,
    Health
}


//所有感知器的基类,包括视觉感知器,听觉感知器,数值感知器等
public class Sensor : MonoBehaviour {

    public SensorType sensorType;

    //感知到了Trigger,采取相应的行为
    public virtual void Notify(Trigger t) {

    }
}
