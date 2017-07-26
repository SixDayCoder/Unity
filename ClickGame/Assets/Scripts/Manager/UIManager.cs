using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

    #region singleton
    private UIManager() {
    }
    private static UIManager _instance = null;
    public  static UIManager Instance {
        get {
            return _instance;
        }
    }
    #endregion

    private void Awake() {
        _instance = this;
    }

    public void ShowScorePanel(int score) {

    }


    
}
