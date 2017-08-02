using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Weapon : Item {


    public Weapon(string id,      string name, 
                  EItemType type, EQuality quality, 
                  string des,     uint capacity, 
                  uint buy,       uint sell, 
                  string icon,    uint strength, 
                  uint agility,   uint intelligence, 
                  uint damage) :
                  base(id, name, type, quality, des, capacity, buy, sell, icon)
    {
        this.Strength = strength;
        this.Agility = agility;
        this.Intelligence = intelligence;
        this.Damage = damage;
    }

    public uint Strength {
        get; set;
    }
    public uint Agility {
        get; set;
    }
    public uint Intelligence {
        get; set;
    }
    public uint Damage {
        get; set;
    }

}
