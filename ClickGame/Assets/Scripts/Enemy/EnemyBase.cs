using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyBase : MonoBehaviour {

    [SerializeField] protected float moveSpeed = 5.0f; //该组敌人的移动速度 
    [SerializeField] protected int groupNumber = 1;//该组敌人的数量
    [SerializeField] protected AudioClip hitClip = null;//被击中时播放的音效

    protected  void Death() {
        EnemySpawner.Instance.EnemyDead();
        ScoreManager.Instance.AddScore(groupNumber);
        gameObject.SetActive(false);
        Destroy(gameObject, 0.3f);
    }

    protected void Move() {//移动
        transform.Translate(-Vector3.right * moveSpeed * Time.deltaTime);
    }
}
