using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public GameObject BallPrefab;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.A)) {
            Instantiate(BallPrefab, new Vector3(20, 6, 0), Quaternion.identity);
        }
    }

}
