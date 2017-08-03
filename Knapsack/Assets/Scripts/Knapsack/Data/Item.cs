using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public enum EItemType {
    Consume,
    Weapon,
    Equipment,
    Material
}


[System.Serializable]
public enum EQuality {
    Normal,//普通
    Rare,//稀有
    Lengendary,//传说
    Epic,//史诗
}

/// <summary>
/// 物品基类
/// </summary>

[System.Serializable]
public class Item {

    public string id;
    public string name;
    public EItemType itemtype;
    public EQuality quality;
    public uint capacity;
    public string description;
    public uint buyprice;
    public uint sellprice;
    public string icon;

    public Item() {

    }

    public Item(string id, string name, EItemType type, EQuality quality, string des, uint capacity, uint buy, uint sell, string icon) {
        this.id = id;
        this.name = name;
        this.itemtype = type;
        this.quality = quality;
        this.description = des;
        this.capacity = capacity;
        this.buyprice = buy;
        this.sellprice = sell;
        this.icon = icon;
    }

}
