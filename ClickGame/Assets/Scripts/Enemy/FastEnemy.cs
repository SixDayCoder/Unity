using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastEnemy : EnemyBase{

    private void Start() {
        moveSpeed = 6.0f;
        groupNumber = 3;
    }

    private void Update() {
        Move();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.transform.tag == TagManager.Player) {
            Death();
        }
    }

}
