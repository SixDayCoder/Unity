using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalEnemy : EnemyBase{

    private void Start() {
        moveSpeed = 5.0f;
        groupNumber = 1;
    }

    private void Update() {
        Move();       
    }


    private new void Death() {
        EnemySpawner.Instance.EnemyDead();
        ScoreManager.Instance.AddScore(groupNumber);
        gameObject.SetActive(false);
        Destroy(gameObject, 1.0f);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.transform.tag == TagManager.Player) {
            Death();
        }
    }



}
