using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consume : Item{

    public uint hp;
    public uint mp;

    public Consume(string id,      string name,
                   EItemType type, EQuality quality,
                   string des,     uint capacity,
                   uint buy,       uint sell,
                   string icon,    uint hp,
                   uint mp) :
                   base(id, name, type, quality, des, capacity, buy, sell, icon)
    {
        this.hp = hp;
        this.mp = mp;
    }

}
