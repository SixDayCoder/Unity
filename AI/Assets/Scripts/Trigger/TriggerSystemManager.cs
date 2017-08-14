using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//注册感知器和触发器,并在他们过期时移除
public class TriggerSystemManager : MonoBehaviour {

    #region singelton
    private TriggerSystemManager() {}
    static public TriggerSystemManager Instance = null;
    #endregion

    //当前感知器列表
    private List<Sensor> currentSensors = new List<Sensor>();

    //记录当前时刻被移除的感知器,例如感知体死亡时或者时间过期
    private List<Sensor> sensorsToRemove = new List<Sensor>();

    //当前触发器列表
    private List<Trigger> currentTriggers = new List<Trigger>();

    //记录当前时刻需要被移除的触发器
    private List<Trigger> triggesToRemove = new List<Trigger>();

    private void UpdateTriggers() {

        foreach (var t in currentTriggers) {
            //在foreach中不能删除或者插入元素
            if (t.toBeRemoved) {
                triggesToRemove.Add(t);
            }
            else {
                t.UpdateStatus();
            }
        }

        foreach (var t in triggesToRemove)
            currentTriggers.Remove(t);
    }

    private void UpdateSensors() {

        foreach(var s in currentSensors) {
            //如果感知器对应的感知体没有被销毁
            if(s.gameObject != null) {
                
                foreach(var t in currentTriggers) {
                    //检测s是否在t的作用范围之内,并作出相应的相应
                    t.Reaction(s);
                }

            }
            //感知器的感知体死亡,需要移除该感知器
            else {
                sensorsToRemove.Add(s);
            }

        }

        //清除感知体
        foreach (var s in sensorsToRemove)
            currentSensors.Remove(s);

    }

    public void RegisterSensor(Sensor s) {

        Debug.Log("Register Sensor " + s.name);

        currentSensors.Add(s);

    }

    public void RegisterTrigger(Trigger t) {

        Debug.Log("Register Trigger " + t.name);

        currentTriggers.Add(t);

    }


    private void Awake() {

        Instance = this;    
        
    }

    private void LateUpdate() {

        //更新所有触发器内部状态
        UpdateSensors();

        //迭代感知器和触发器,作出相应的行为
        UpdateTriggers();

    }
}
