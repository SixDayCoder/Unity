using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager{

    private ScoreManager() {}
    static private ScoreManager _instance = null;
    static public ScoreManager Instance {
        get {
            if(_instance == null) {
                _instance = new ScoreManager();
                return _instance;
            }
            return _instance;
        }
    }


    private int score = 0;


    public void AddScore(int newscore) {
        score += newscore;
        UIManager.Instance.ShowScorePanel(score);
    }

}
