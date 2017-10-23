using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour {

    public float jumpSpeed = 5.0f;


    private Rigidbody rb = null;
    private bool isGround = true;

    private void Start() {
        rb = GetComponent<Rigidbody>();
        Debug.Log(rb.transform.name);
    }
    private void Update() {
        
        if (isGround && Input.GetKeyDown(KeyCode.Space)) {
            isGround = false;
            rb.velocity = Vector3.up * jumpSpeed;
            Debug.Log(rb.velocity);
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if(collision.transform.tag == "Ground") {
            isGround = true;
        }
    }

}
