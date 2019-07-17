using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase Instance;

    public List<Weapons> weapons;
    public List<Armors> armors;

    private void Awake()
    {
        Instance = this;
    }
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
    GreatAxe,
    Maul,
    GreatSword
}
public enum ArmorTypes
{
    Light_Belt,
    Light_Boots,
    Light_Bracers,
    Light_Chectplate,
    Light_Cloak,
    Light_Gloves,
    Light_Helmet,
    Light_Legplate,
    Light_Shoulderpads,

    Medium_Belt,
    Medium_Boots,
    Medium_Bracers,
    Medium_Chectplate,
    Medium_Cloak,
    Medium_Gloves,
    Medium_Helmet,
    Medium_Legplate,
    Medium_Shoulderpads,

    Heavy_Belt,
    Heavy_Boots,
    Heavy_Bracers,
    Heavy_Chectplate,
    Heavy_Gloves,
    Heavy_Helmet,
    Heavy_Legplate,
    Heavy_Shoulderpads,

    Necklace,
    Rings
}
