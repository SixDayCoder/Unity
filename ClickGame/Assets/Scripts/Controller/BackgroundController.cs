using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BackgroundController : MonoBehaviour {


    private float singleWidth;//单个背景板的长度
    private float totalWidth;//背景板的总长度
    private Vector3 startPosition;//背景板的初始位置

    /* 思路：每块背景板移动时,必然移动totalWidth个长度,那么他们的位置,必然是
     * 初始位置
     * 初始位置 + 1 * totalWidth
     * 初始位置 + n * totalWidth
     * 所以只需记录下玩家移动的距离dist,使用dist / totalWidth四舍五入得到最靠近玩家的n
     * 使用该值对bg进行位置的变换
     */

    private void Start() {
        singleWidth = 10.0f;
        totalWidth = singleWidth * 3;
        Debug.Log(Mathf.RoundToInt(15.5f / totalWidth));
        startPosition = transform.position;
    }

    private void Update() {
        float distance = Camera.main.transform.position.x - startPosition.x;
        int n = Mathf.FloorToInt(distance / totalWidth + 0.5f);
        Debug.Log(string.Format("n : {0} , distance : {1}", n, distance));
        Vector3 position = startPosition;
        position.x += + n * totalWidth;
        transform.position = position;
    }

}
