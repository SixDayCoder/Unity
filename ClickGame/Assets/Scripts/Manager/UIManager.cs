using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

    private UIManager() {}
    static private UIManager _instance = null;
    static public UIManager Instance {
        get {
            if (_instance == null) {
                _instance = new UIManager();
                return _instance;
            }
            return _instance;
        }
    }

    public void ShowScorePanel(int score) {

    }


    
}
