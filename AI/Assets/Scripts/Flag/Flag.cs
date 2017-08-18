using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour {

    private Attacker owner = null;

    private void OnTriggerEnter(Collider other) {

        if(other.GetComponent<Collider>().gameObject.tag == "Attacker") {

            Attacker newOwner = other.gameObject.GetComponent<Attacker>();
            newOwner.isHandleFlag = true;

            //之前有人
            if (owner != null) {
                //夺走旗帜
                owner.isHandleFlag = false;
                owner = newOwner;
                transform.parent = owner.transform;
            }
            else {
                owner = newOwner;
                transform.parent = owner.transform;
            }
        }
    }
}
