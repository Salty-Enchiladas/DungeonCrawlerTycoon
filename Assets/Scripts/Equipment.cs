using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType { Armor, Weapon }

[System.Serializable]
public class Equipment : Item
{
    protected Character character;
    public EquipmentType EquipmentType { get; protected set; }

    public int power;
    public int accuracy;
    public int constitution;
    public int speed;
    public int luck;

    public virtual void Equip(Character _character) { }
    public virtual void UnEquip() { }
}