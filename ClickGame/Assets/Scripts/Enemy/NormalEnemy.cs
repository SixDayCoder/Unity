using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalEnemy : EnemyBase{

    private void Start() {
        moveSpeed = 4.0f;
        groupNumber = 1;
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
