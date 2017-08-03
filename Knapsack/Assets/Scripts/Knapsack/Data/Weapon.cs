using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[System.Serializable]
public class Weapon : Item {


    public Weapon(string id,      string name, 
                  EItemType type, EQuality quality, 
                  string des,     uint capacity, 
                  uint buy,       uint sell, 
                  string icon,    uint strength, 
                  uint agility,   uint intellect, 
                  uint damage) :
                  base(id, name, type, quality, des, capacity, buy, sell, icon)
    {
        this.strength = strength;
        this.agility = agility;
        this.intellect = intellect;
        this.damage = damage;
    }

    public uint strength;
    public uint agility;
    public uint intellect;
    public uint damage;



}
