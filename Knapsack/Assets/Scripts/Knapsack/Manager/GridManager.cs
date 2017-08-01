using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GridManager : MonoBehaviour {

    private Item item = null;//当前格子存放的item的信息
    private Image itemUI;//item icon ui
    private Text itemNumber;//item number ui



    private void Start() {
        foreach(var image in gameObject.GetComponentsInChildren<Image>(true)) {
            if (image.name == "ItemUI")
                itemUI = image.GetComponent<Image>();
        }
        itemNumber = gameObject.GetComponentInChildren<Text>();
    }

    public bool IsEmpty() {
        return item == null ? true : false;
    }

    public void IncNumber(uint amount) {

        if (!IsEmpty()) {
            if(item.Capacity + amount <= 99) {
                item.Capacity += amount;
                //提示该物品数量已达到上限
            }
        }
        else {
            //LogError
        }
    }

    public void AddItemData(Item item) {
        if (IsEmpty()) {
            this.item = item; 
        }
        else {
            //LogError
        }
    }

    public void RemoveItemData() {
        if(!IsEmpty())
            item = null;
        else {
            //LogError
        }
    }

    //UseItem?
    
}
