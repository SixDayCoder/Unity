using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//用来检测AI的邻居
public class Radar : MonoBehaviour {

    //计时器,每过0.2s检测一次邻居
    private float timer = 0;

    //邻居列表
    private List<GameObject> neighbors;

    //检测半径
    private float detectRadius = 3.0f;

    //设置检测哪一层的游戏对象
    private LayerMask layerCheck; 

    private bool CanUpdateNegiborsList() {

        timer += Time.deltaTime;

        if(timer >= 0.2f) {
            timer = 0;
            return true;
        }

        return false;
    }

    private void UpdateNeiborsList() {
        //清空原有的邻居列表
        neighbors.Clear();

        Collider[] colliders = Physics.OverlapSphere(transform.position, detectRadius, layerCheck);

        foreach(var c in colliders) {

            //同类必定也是AI智能体
            if (c.GetComponent<AILocomotion>()) {

                //Debug.Log(string.Format("Name is : {0}, Neighbor is : {1}, Pos is : {2}", gameObject.name, c.gameObject.name, c.transform.position));
                neighbors.Add(c.gameObject);

            }

        }

    }

    private void Start() {

        neighbors = new List<GameObject>();

        layerCheck = LayerMask.GetMask("Robot");

    }

    
    private void Update() {
        if (CanUpdateNegiborsList())
            UpdateNeiborsList();
    }

    public List<GameObject> GetNeighbors() {

        return neighbors;

    }
}
