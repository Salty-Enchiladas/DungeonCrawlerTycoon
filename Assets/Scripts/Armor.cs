using UnityEngine;

[System.Serializable]
public class Armor : Equipment
{
    [Space, Header("Armor Values")]
    public ArmorTypes armorType;
    public ArmorCategories armorCategory;
    public int armorValue;

    public Armor()
    {
        EquipmentType = EquipmentType.Armor;
    }

    public override void Equip(Character _character)
    {
        character = _character;

        character.ArmorRating.AddModifier(new StatModifier(armorValue, StatModType.Flat, this));
        character.Power.AddModifier(new StatModifier(power, StatModType.Flat, this));
        character.Zeal.AddModifier(new StatModifier(zeal, StatModType.Flat, this));
        character.Constitution.AddModifier(new StatModifier(constitution, StatModType.Flat, this));
        character.Speed.AddModifier(new StatModifier(speed, StatModType.Flat, this));
        character.Luck.AddModifier(new StatModifier(luck, StatModType.Flat, this));
    }

    public override void UnEquip()
    {
        character.ArmorRating.RemoveAllModifiersFromSource(this);
        character.Power.RemoveAllModifiersFromSource(this);
        character.Zeal.RemoveAllModifiersFromSource(this);
        character.Constitution.RemoveAllModifiersFromSource(this);
        character.Speed.RemoveAllModifiersFromSource(this);
        character.Luck.RemoveAllModifiersFromSource(this);
        character = null;
    }
}