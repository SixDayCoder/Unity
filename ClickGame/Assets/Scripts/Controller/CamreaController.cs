using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamreaController : MonoBehaviour {

    // Use this for initialization

    private GameObject player;//玩家
    private Vector3 offset;//摄像机和玩家的偏移量

	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        offset = transform.position - player.transform.position; 
	}


    private void AdjustPosition() {
        transform.position = new Vector3(player.transform.position.x + offset.x, transform.position.y, transform.position.z);
    }

	// Update is called once per frame
	void Update () {
        AdjustPosition();
        
	}
}
