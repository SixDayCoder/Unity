using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Consume : Item{

    public uint HP {
        get; set;
    }

    public uint MP {
        get; set;
    }

    public Consume(string id,      string name,
                   EItemType type, EQuality quality,
                   string des,     uint capacity,
                   uint buy,       uint sell,
                   string icon,    uint hp,
                   uint mp) :
                   base(id, name, type, quality, des, capacity, buy, sell, icon)
    {
        this.HP = hp;
        this.MP = mp;
    }

}
