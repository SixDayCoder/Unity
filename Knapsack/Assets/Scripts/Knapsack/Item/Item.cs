using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum EItemType {
    Consume,
    Weapon,
    Equipment,
    Material
}

public enum EQuality {
    Normal,//普通
    Rare,//稀有
    Lengendary,//传说
    Epic,//史诗
}

/// <summary>
/// 物品基类
/// </summary>
public class Item {

    public uint ID{
        get; set;
    }

    public string Name {
        get; set;
    }

    public EItemType ItemType {
        get; set;
    }

    public EQuality Quality {
        get; set;
    }
    
    public uint Capacity {
        get; set;
    }

    public string Description {
        get; set;
    }

    public uint BuyPrice {
        get; set;
    }

    public uint SellPrice {
        get; set;
    }

    public string Icon {
        get; set;
    }


    public Item() {

    }

    public Item(uint id, string name, EItemType type, EQuality quality, string des, uint capacity, uint buy, uint sell, string icon) {
        this.ID = id;
        this.Name = name;
        this.ItemType = type;
        this.Quality = quality;
        this.Description = des;
        this.Capacity = capacity;
        this.BuyPrice = buy;
        this.SellPrice = sell;
        this.Icon = icon;
    }


}
