using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnapsackManager : MonoBehaviour {

    // Use this for initialization


    private Weapon weapon;

    private string json = @"{
    ""id""        	    :	""W_Axe001"",
	""name""      	    :	""战斧"",
	""itemtype""  	    :	1,
	""quality""		    :	1,
	""capacity""        :	1,
	""description""	    :	""只有英勇的武士才可以使用的武器"",
	""buyprice""        :	1500,
	""sellprice""	    :	750,
	""icon""	        :	""Icons/Weapon/W_Axe001"",
	""strength""	    :	10,
	""agility""		    :	5,
	""intellect""	    :	1,
	""damage""		    :	100
    }";

	void Start () {
        weapon =  JsonUtility.FromJson(json, typeof(Weapon)) as Weapon;
        //Debug.Log(weapon.id);
        //Debug.Log(weapon.name);
        //Debug.Log(weapon.itemtype);
        //Debug.Log(weapon.quality);
        //Debug.Log(weapon.intellect);
	}
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space)) {
            Knapsack.Instance.AddItem(weapon);
        }
	}
}
