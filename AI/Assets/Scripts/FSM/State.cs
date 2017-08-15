using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class State<EntityType> {

    //key : Transiton序号  value : 状态实体
    public Dictionary<int, State<EntityType>> transitionMap = new Dictionary<int, State<EntityType>>();

    public void AddTransition(int transition, State<EntityType> state) {
        if (transitionMap.ContainsKey(transition)) {
            //FSM中一个转换只能对应一个新的状态
            Debug.LogWarning("FSM ERROR : transition is already inside the map");
            return;
        }
        else {
            //加入字典
            transitionMap.Add(transition, state);
        }
    }

    public void DeleteTransition(int transition) {
        if (!transitionMap.ContainsKey(transition)) {
            //不存在要删除的转换
            Debug.LogWarning("FSM ERROR : transition is not inside the map");
            return;
        }
        else {
            //从字典中删除
            transitionMap.Remove(transition);
        }
    }

    public State<EntityType> NextState(int transition) {
        if (!transitionMap.ContainsKey(transition)) {
            //不存在下一个变换
            Debug.LogWarning("FSM ERROR : transition is not inside the map");
            return null;
        }
        else {
            return transitionMap[transition];
        }

    }

    public virtual void Enter(EntityType entity) {}

    public virtual void Execute(EntityType entity) {}

    public virtual void Exit(EntityType entiy) {}

   


}
