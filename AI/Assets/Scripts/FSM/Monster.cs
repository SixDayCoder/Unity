using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Monster : MonoBehaviour {

    public enum Transition : int {
        SeePlayer,
        InAttackRange,
        PlayerDead
    }

    private StateMachine<Monster> stateMachine;
    private CharacterController controller;

    private void Awake() {

        controller = GetComponent<CharacterController>();
        stateMachine = new StateMachine<Monster>(this);

        MonsterIdleState idle = new MonsterIdleState();
        MonsterAttackState attack = new MonsterAttackState();
        MonsterMoveState move = new MonsterMoveState();
        MonsterDeadState dead = new MonsterDeadState();


        idle.AddTransition((int)Transition.SeePlayer, move);
        move.AddTransition((int)Transition.InAttackRange, attack);
        attack.AddTransition((int)Transition.PlayerDead, idle);

        stateMachine.SetCurrentState(idle);
        stateMachine.SetGlobalState(dead);
    }

    private void Update() {
        
    }

}


/*
    全局状态    dead
    状态        转移           状态
    idle      发现敌人         move
    move     进入攻击范围      attack
    attack   敌人死亡          idle
*/