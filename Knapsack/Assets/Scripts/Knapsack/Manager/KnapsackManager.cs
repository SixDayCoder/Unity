using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnapsackManager {

    #region singleton
    private KnapsackManager() {}
    private KnapsackManager _instance = null;
    public KnapsackManager Instance {
        get {
            if (_instance == null)
                _instance = new KnapsackManager();
            return _instance;
        }
    }
    #endregion

    private GameObject FindFirstEmptyGrid() {
        return new GameObject();
    }

    public void AddItem() {
        
    }

    public void RemoveItem() {

    }

    public void LoadItems() {//从数据库中获取json文件并解析

    }

}
