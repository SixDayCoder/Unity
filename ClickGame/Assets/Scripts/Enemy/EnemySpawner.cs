using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EnemyType {
    Normal,//正常速度的敌人
    Slow,//缓行的敌人
    Fast,//快速的敌人
    FastThenSlow//先加速后缓行的敌人
}

public class EnemySpawner : MonoBehaviour {


    #region singleton
    private EnemySpawner() {}
    private static EnemySpawner _instance = null;
    public static EnemySpawner Instance {
        get {
            return _instance;
        }
    }
    #endregion

    #region public serialized, prefab
    public GameObject normalPrefab;
    public GameObject slowPrefab;
    public GameObject fastPrefab;
    public GameObject fastThenSlowPrefab;
    #endregion 

    private Vector3 offset;//玩家和敌人之间的距离
    private GameObject player;//根据当前玩家的位置来决定spwanPosition


    private float readyTime;//游戏刚开始时留给玩家的适应时间
    private bool isReady;//是否准备好生成敌人
    private int currentEnemyNum;//当前画面中敌人的数量
    private int maxEnemyNum;//画面中最大的敌人数量

    //计时器,用于设定生成怪物的时间
    private Clock clock = new Clock();


    private void Awake() {
        _instance = this;
    }


    private void Start() {

        currentEnemyNum = 0;
        maxEnemyNum = 3;
        readyTime = 2;
        isReady = false;

        player = GameObject.FindGameObjectWithTag("Player");//获取player
        offset = new Vector3(15, 0, 0);//初始化偏移量 
        clock.SetMaxtime(1.5f);//设定生成怪物的计时器的计时间隔为1.5
        StartCoroutine(StartSpwanEnemy());
    }

    private void Update() {

        if ( IsReady() && currentEnemyNum < maxEnemyNum) {

            //生成敌人

            int type = Random.Range(0, 3);

            type = 0;

            GameObject prefab = null;
            switch (type) {
                case 0: prefab = normalPrefab;break;
                case 1: prefab = slowPrefab; break;
                case 2: prefab = fastPrefab; break;
                case 3: prefab = fastThenSlowPrefab; break;
                default: break;
            }

            Instantiate(prefab, player.transform.position + offset, Quaternion.identity);
            currentEnemyNum++;
            if(currentEnemyNum == maxEnemyNum)
                isReady = false;
        }


    }

    //使用协程延迟第一波怪物生成的时间
    IEnumerator StartSpwanEnemy() {
        yield return new WaitForSeconds(readyTime);
        SetReady(true);
    }

    //判定是否准备好生成怪物
    private bool IsReady() {
        if (isReady && clock.IsTickComplete())
            return true;
        return false;
    }


    //怪物死亡后设置为true,玩家死亡后设置为false,画面中没有怪物的时候设置为true
    public void SetReady(bool flag) {
        isReady = flag;
    }

    //Enemy死亡时触发该事件
    public void EnemyDead() {
        if(currentEnemyNum > 0) { 
            currentEnemyNum--;
            isReady = true;
        }
    }
}
