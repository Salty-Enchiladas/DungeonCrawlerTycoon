using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public List<Weapons> weapons;
    public List<Armors> armors;
}

[System.Serializable]
public class Weapons
{
    public WeaponTypes weaponType;
    public List<Sprite> weaponIcons;
}

[System.Serializable]
public class Armors
{
    public ArmorTypes armorTypes;
    public List<Sprite> armorIcons;
}

public enum WeaponTypes
{
    Axe,
    Bow,
    Crossbow,
    Dagger,
    FistWeapon,
    Focus,
    Gun,
    Mace,
    Scythe,
    Shield,
    Spear,
    Staff,
    Sword,
    Tome,
    TwoHandedAxe,
    TwoHandedMace,
    TwoHandedSword
}
public enum ArmorTypes
{
    LightBelt,
    LightBoots,
    LightBracers,
    LightChectplate,
    LightCloak,
    LightGloves,
    LightHelmet,
    LightLegplate,
    LightShoulderpads,

    MediumBelt,
    MediumBoots,
    MediumBracers,
    MediumChectplate,
    MediumCloak,
    MediumGloves,
    MediumHelmet,
    MediumLegplate,
    MediumShoulderpads,

    HeavyBelt,
    HeavyBoots,
    HeavyBracers,
    HeavyChectplate,
    HeavyGloves,
    HeavyHelmet,
    HeavyLegplate,
    HeavyShoulderpads,

    Necklace,
    Rings
}
