using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastThenSlowEnemy : EnemyBase {

    private float minMoveSpeed = 2.0f;
    private float maxMoveSpeed = 6.0f;

    protected new void Move() {
        moveSpeed = Mathf.Clamp(moveSpeed, minMoveSpeed, moveSpeed);
        transform.Translate(-Vector3.right * moveSpeed * Time.deltaTime);
    }

    private void Start() {
        groupNumber = 4;
        moveSpeed = maxMoveSpeed;
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
