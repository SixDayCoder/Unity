using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacker : MonoBehaviour {

    //是否持有旗帜
    public bool isHandleFlag = false;

    public Transform startPoint;

    private void OnTriggerEnter(Collider other) {
        //被抓到了
        if(other.tag == "Defender") {
            CatchByDefender();
        }
    }

    private void CatchByDefender() {

        //如果持有旗帜,丢下旗帜
        if(isHandleFlag == true) {

            isHandleFlag = false;

            CTFGameManager.Instance.DropFlag();

            if(transform.childCount > 0) {
                //旗帜一定是0号子孙
                Transform flag = transform.GetChild(0);
                flag.gameObject.GetComponent<Flag>().owner = null;//无人持旗帜
                flag.parent = null;//取下旗帜
                flag.position = transform.position;//设置位置

                ResetToStart();//复位
            }

        }

    }

    private void ResetToStart() {
        transform.position = startPoint.position;
        transform.rotation = startPoint.rotation;
    }

}
