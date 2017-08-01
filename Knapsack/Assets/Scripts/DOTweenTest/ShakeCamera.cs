using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ShakeCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
        transform.DOShakePosition(1.0f);		
	}

}
