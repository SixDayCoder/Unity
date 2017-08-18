using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

public class IsHavingFlag : Conditional {

    private Attacker attacker;

    public override void OnAwake() {
        attacker = GetComponent<Attacker>();
    }

    public override TaskStatus OnUpdate() {
        if (attacker.isHandleFlag == true)
            return TaskStatus.Success;
        return TaskStatus.Failure;
    }
}
