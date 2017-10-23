using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameGrade
{
    GAME_GRADE_EASY,
    GAME_GRADE_NORMAL,
    GAME_GRADE_HARD
}

public enum ControlType
{
    CONTROL_TYPE_TOUCH,
    CONTROL_TYPE_MOUSE,
    CONTROL_TYPE_KEYBOARD

}

public class GameSetting : MonoBehaviour {

    public static float Volume = 1;
    public static GameGrade Grade = GameGrade.GAME_GRADE_EASY;
    public static ControlType Control = ControlType.CONTROL_TYPE_KEYBOARD;
    public static bool IsFullScreen = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
