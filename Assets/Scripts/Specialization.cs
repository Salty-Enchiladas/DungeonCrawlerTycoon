using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Specialization")]
public class Specialization : ScriptableObject
{
    public string specName;
    public List<ArmorCategories> armorCategories;
    public List<WeaponTypes> weaponTypes;
    public ActionType actionType;
}

public enum ActionType
{
    Damage,
    Healing
}
