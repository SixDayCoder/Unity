using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EEquipmentType {
    Head,//头盔
    Necklace,//项链
    Ring,//指环
    Leg,//护腿
    Belt,//腰带
    Wear,//上衣
}

public class Equipment : Item{
    
    public EEquipmentType EquipmentType {
        get; set;
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

    public uint Defend {
        get; set;
    }



    public Equipment(string id,        string name,
                     EItemType type, EQuality quality,
                     string des,     uint capacity,
                     uint buy,       uint sell,
                     string icon,    EEquipmentType equimentType,
                     uint strength,  uint agility,
                     uint intellect, uint defend) :

                     base(id, name, type, quality, des, capacity, buy, sell, icon)
    {
        this.EquipmentType = equimentType;
        this.Strength = strength;
        this.Agility = agility;
        this.Defend = defend;
    }

}
