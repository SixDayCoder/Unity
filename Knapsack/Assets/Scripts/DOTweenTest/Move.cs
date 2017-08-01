using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Move : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //使用true表示相对坐标
        transform.DOMoveX(5, 10.0f).From(true);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
