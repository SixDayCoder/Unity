using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class KnapsackManager : MonoBehaviour {

    // Use this for initialization
    /*
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
    */

	void Start () {
        StartCoroutine(DownLoadJson("http://localhost/json/weapon1.json"));
        StartCoroutine(DownLoadJson("http://localhost/json/weapon2.json"));
	}

    IEnumerator DownLoadJson(string url) {
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.Send();

        if (!request.isError) {
            string json = request.downloadHandler.text;
            Weapon w = JsonUtility.FromJson<Weapon>(json);
            Debug.Log(w.description);
            Knapsack.Instance.AddItem(w);
        }
        else {
            //LogError
            yield return null;
        }
    }
}
