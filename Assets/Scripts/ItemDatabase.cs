using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDatabase")]
public class ItemDatabase : ScriptableObject
{
    public List<WeaponDatabase> oneHandedWeapons;
    public List<WeaponDatabase> twoHandedWeapons;
    public List<WeaponDatabase> offHandWeapons;

    public List<ArmorDatabase> accessories;
    public List<ArmorDatabase> lightArmor;
    public List<ArmorDatabase> mediumArmor;
    public List<ArmorDatabase> heavyArmor;

    public List<WeaponValues> weaponValues;
    public List<ArmorValues> armorValues;

    public WeaponValues GetWeaponValues(WeaponCategories weaponCategory, RarityType rarityType)
    {
        foreach (WeaponValues weaponValue in weaponValues)
        {
            if (weaponValue.weaponCategory == weaponCategory && weaponValue.rarityType == rarityType)
                return weaponValue;
        }

        return weaponValues[0];
    }

    public WeaponDatabase GetWeaponDatabase(WeaponTypes weaponType)
    {
        foreach(WeaponDatabase database in oneHandedWeapons)
        {
            if(database.weaponType == weaponType)
                return database;
        }

        foreach (WeaponDatabase database in twoHandedWeapons)
        {
            if (database.weaponType == weaponType)
                return database;
        }

        foreach (WeaponDatabase database in offHandWeapons)
        {
            if (database.weaponType == weaponType)
                return database;
        }

        return null;
    }

    public ArmorValues GetArmorValues(ArmorCategories armorCategory, RarityType rarityType)
    {
        foreach(ArmorValues armorValue in armorValues)
        {
            if (armorValue.armorCategory == armorCategory && armorValue.rarityType == rarityType)
                return armorValue;
        }

        return armorValues[0];
    }

    public ArmorDatabase GetArmorDatabase(ArmorCategories armorCategory, ArmorTypes armorType)
    {
        switch(armorCategory)
        {
            case ArmorCategories.Accessory:
                foreach(ArmorDatabase armorDatabase in accessories)
                    if (armorType == armorDatabase.armorTypes)
                        return armorDatabase;
                break;
            case ArmorCategories.Light:
                foreach (ArmorDatabase armorDatabase in lightArmor)
                    if (armorType == armorDatabase.armorTypes)
                        return armorDatabase;
                break;
            case ArmorCategories.Medium:
                foreach (ArmorDatabase armorDatabase in mediumArmor)
                    if (armorType == armorDatabase.armorTypes)
                        return armorDatabase;
                break;
            case ArmorCategories.Heavy:
                foreach (ArmorDatabase armorDatabase in heavyArmor)
                    if (armorType == armorDatabase.armorTypes)
                        return armorDatabase;
                break;
        }

        return null;
    }
}

#region Weapon Stuff
[System.Serializable]
public class WeaponDatabase
{
    public WeaponCategories weaponCategory;
    public WeaponTypes weaponType;
    public List<Sprite> weaponIcons;
}

[System.Serializable]
public class WeaponValues
{
    public WeaponCategories weaponCategory;
    public RarityType rarityType;
    public int targetCount;
    public int minDamage;
    public int maxDamage;
    public int minStats;
    public int maxStats;
}

public enum WeaponCategories
{
    OneHanded,
    TwoHanded,
    OffHand
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
    GreatSword,
    Wands
}
#endregion

#region Armor Stuff
[System.Serializable]
public class ArmorDatabase
{
    public ArmorCategories armorCategory;
    public ArmorTypes armorTypes;
    public List<Sprite> armorIcons;
}

[System.Serializable]
public class ArmorValues
{
    public ArmorCategories armorCategory;
    public RarityType rarityType;
    public int minArmor;
    public int maxArmor;
    public int minStats;
    public int maxStats;
}


public enum ArmorCategories
{
    Accessory,
    Light,
    Medium,
    Heavy
}


public enum ArmorTypes
{
    Belt,
    Boots,
    Bracers,
    Chestplate, 
    Gloves,
    Helmet,
    Legplate,
    Pauldrons,
    Necklace,
    Rings
}
#endregion