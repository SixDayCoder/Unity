using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;

public class CTFGameManager : MonoBehaviour {

    #region singleton
    private CTFGameManager() {}
    private static CTFGameManager _instance;
    public static CTFGameManager Instance {
        get {
            return _instance;
        }
    }
    #endregion 


    //持有旗帜的bt
    private List<BehaviorTree> withoutFlagBT = new List<BehaviorTree>();
    //不持有旗帜的bt
    private  List<BehaviorTree> withFlagBT = new List<BehaviorTree>();

    private void Awake() {
        _instance = this;
    }

    private void Start() {

        BehaviorTree[] bts = FindObjectsOfType<BehaviorTree>();

        foreach(var bt in bts) {

            if(bt.Group == 1) {
                withoutFlagBT.Add(bt);
            }
            else if(bt.Group == 2) {
                withFlagBT.Add(bt);
            }

        }

    }

    //捡起旗帜
    public void TakeFlag() {

        foreach(var bt in withoutFlagBT) {
            //判断行为树是否启用,如果启用那么禁用他
            if (BehaviorManager.instance.IsBehaviorEnabled(bt)) {
                bt.DisableBehavior();//禁用自身
            }
        }

        foreach (var bt in withFlagBT) {
            //判断行为树是否禁用,如果禁用那么启用他
            if (!BehaviorManager.instance.IsBehaviorEnabled(bt)) {
                bt.EnableBehavior();//启用自身
            }
        }

    }

    //扔下旗帜
    public void DropFlag() {
        foreach (var bt in withFlagBT) {
            //判断行为树是否启用,如果启用那么禁用他
            if (BehaviorManager.instance.IsBehaviorEnabled(bt)) {
                bt.DisableBehavior();//禁用自身
            }
        }

        foreach (var bt in withoutFlagBT) {
            //判断行为树是否禁用,如果禁用那么启用他
            if (!BehaviorManager.instance.IsBehaviorEnabled(bt)) {
                bt.EnableBehavior();//启用自身
            }
        }
    }
}
