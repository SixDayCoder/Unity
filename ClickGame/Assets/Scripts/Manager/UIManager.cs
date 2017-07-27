using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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


    private Text scoreText = null;

    private void Awake() {
        _instance = this;
        scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
    }

    public void ShowScorePanel(int score) {
        scoreText.text = "Score : " + score;
    }


    
}
