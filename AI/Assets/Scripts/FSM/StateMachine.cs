using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<EntityType> {

    private EntityType entity;
    public StateMachine(EntityType entity){
        this.entity = entity;
    }

    #region current, previous, global state
        
    private State<EntityType> currentState;

    public void SetCurrentState(State<EntityType> state) {
        currentState = state;
    }

    public State<EntityType> GetCurrentState(State<EntityType> state) {
        return currentState;
    }

    private State<EntityType> previousState;

    public void SetPreviousState(State<EntityType> state) {
        previousState = state;
    }

    public State<EntityType> GetPreviousState(State<EntityType> state) {
        return previousState;
    }

    private State<EntityType> globalState;

    public void SetGlobalState(State<EntityType> state) {
        globalState = state;
    }

    public State<EntityType> GetGlobalState(State<EntityType> state) {
        return globalState;
    }

    #endregion

    public void ChangeState(int transition) {

        if(currentState == null) {
            //错误的变换
            Debug.LogWarning("FSM ERROR : current state is a null pointer");
            return;
        }
        else {
            //退出当前状态
            currentState.Exit(entity);
            previousState = currentState;
            //获取下一个状态
            currentState = currentState.NextState(transition);
            //进入状态
            currentState.Enter(entity);
            //直接执行?
            currentState.Execute(entity);
        }

    }
    
}
