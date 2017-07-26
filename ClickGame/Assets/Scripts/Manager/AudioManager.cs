using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    #region singleton
    private static AudioManager _instance = null;
    public static AudioManager Instance {
        get {
            return _instance;
        }
    }
    #endregion


    private void Awake() {
        _instance = this;
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
