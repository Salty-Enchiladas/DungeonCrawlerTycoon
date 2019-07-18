﻿using UnityEngine;

[System.Serializable]
public class Weapon : Equipment
{
    public enum ActionType { Damage, Heal}
    [Space, Header("Weapon Values")]
    public ActionType actionType;

    public int weaponDamage;
    public int targetCount;

    public override void Equip(Character _character)
    {
        character = _character;

        character.ArmorRating.AddModifier(new StatModifier(armorValue, StatModType.Flat, this));
        character.Power.AddModifier(new StatModifier(power, StatModType.Flat, this));
        character.Accuracy.AddModifier(new StatModifier(accuracy, StatModType.Flat, this));
        character.Constitution.AddModifier(new StatModifier(constitution, StatModType.Flat, this));
        character.Speed.AddModifier(new StatModifier(speed, StatModType.Flat, this));
        character.Luck.AddModifier(new StatModifier(luck, StatModType.Flat, this));
    }

    public override void UnEquip()
    {
        character.ArmorRating.RemoveAllModifiersFromSource(this);
        character.Power.RemoveAllModifiersFromSource(this);
        character.Accuracy.RemoveAllModifiersFromSource(this);
        character.Constitution.RemoveAllModifiersFromSource(this);
        character.Speed.RemoveAllModifiersFromSource(this);
        character.Luck.RemoveAllModifiersFromSource(this);
        character = null;
    }
}