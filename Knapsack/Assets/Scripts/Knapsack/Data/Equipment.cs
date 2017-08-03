using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum EEquipmentType {
    Head,//头盔
    Necklace,//项链
    Ring,//指环
    Leg,//护腿
    Belt,//腰带
    Wear,//上衣
}


[System.Serializable]
public class Equipment : Item{

    public EEquipmentType equipmentType;
    public uint strength;
    public uint agility;
    public uint intellect;
    public uint defend;


    public Equipment(string id,        string name,
                     EItemType type, EQuality quality,
                     string des,     uint capacity,
                     uint buy,       uint sell,
                     string icon,    EEquipmentType equimentType,
                     uint strength,  uint agility,
                     uint intellect, uint defend) :

                     base(id, name, type, quality, des, capacity, buy, sell, icon)
    {
        this.equipmentType = equimentType;
        this.strength = strength;
        this.agility = agility;
        this.intellect = intellect;
        this.defend = defend;
    }

}
