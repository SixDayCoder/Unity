using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class Ball : MonoBehaviour {


    private Rigidbody rb = null;

	void Start () {
        rb = GetComponent<Rigidbody>();
        rb.velocity = new Vector3(-15.0f, 15.0f, 0.0f);//合成矢量是一个左上角,再与重力合成,抛物线
        StartCoroutine(Death());
	}

    IEnumerator Death() {
        yield return new WaitForSeconds(5.0f);
        DestroyImmediate(gameObject);
     }


}
