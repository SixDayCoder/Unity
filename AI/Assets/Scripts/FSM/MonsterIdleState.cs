using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterIdleState : State<Monster> {

    public override void Enter(Monster entity) {
        Debug.Log("I am in idle state");
    }

    public override void Execute(Monster entity) {
        base.Execute(entity);
    }

    public override void Exit(Monster entiy) {
        base.Exit(entiy);
    }

}
