using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayController : MonoBehaviour {

    private float moveSpeed;


	// Use this for initialization
	void Start () {
        moveSpeed = 2.0f;
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
	}
}
