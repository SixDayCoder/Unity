using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowEnemy : EnemyBase{

    private void Start() {

        moveSpeed = 3.0f;
        groupNumber = 2;

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
