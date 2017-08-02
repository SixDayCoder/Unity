using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnapsackManager : MonoBehaviour {

    // Use this for initialization

    public class Test{
        public int ID {
            get; set;
        }
        public string Des {
            get; set;
        }

        public int id;
        public string des;

    }

    private string json = @"{
    ""ID""        	    :	""W_Axe001"",
	""Name""      	    :	""战斧"",
	""ItemType""  	    :	""Weapon"",
	""Quality""		    :	""Rare"",
	""Capacity""        :	1,
	""Description""	    :	""只有英勇的武士才可以使用的武器"",
	""BuyPrice""        :	1500,
	""SellPrice""	    :	750,
	""Icon""	        :	""Icons/Weapon/W_Axe001"",
	""Strength""	    :	10,
	""Agility""		    :	5,
	""Intelligence""	:	1,
	""Damage""		    :	100
    }";

	void Start () {
        /*
       Weapon weapon =  JsonUtility.FromJson(json, typeof(Weapon)) as Weapon;
        Debug.Log(weapon.ID);
        Debug.Log(weapon.Name);
        Debug.Log(weapon.ItemType);
        Debug.Log(weapon.Quality);
        Debug.Log(weapon.Capacity);
        Debug.Log(weapon.Description);
        Debug.Log(weapon.BuyPrice);
        Debug.Log(weapon.SellPrice);
        Debug.Log(weapon.Icon);
        Debug.Log(weapon.Strength);
        Debug.Log(weapon.Agility);
        Debug.Log(weapon.Intelligence);
        Debug.Log(weapon.Damage);*/

	}
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space)) {
            Knapsack.Instance.AddItem(new Item());
        }
	}
}
