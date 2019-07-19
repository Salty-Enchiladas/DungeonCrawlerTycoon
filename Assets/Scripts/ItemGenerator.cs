using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using TMPro;
using OuterRimStudios.Utilities;

public class ItemGenerator : MonoBehaviour
{
    public Transform canvas;
    public InventoryItem inventoryItem;

    public ItemDatabase itemDatabase;
    public Rarities rarities;

    public Equipment GenerateEquipment(Equipment equipment, InventoryItem _inventoryItem, int statMin, int statMax)
    {
        int pow = 0;
        int acc = 0;
        int con = 0;
        int spd = 0;
        int lck = 0;

        equipment.inventoryItem = _inventoryItem;
        _inventoryItem.itemDescription.Power.gameObject.SetActive(false);
        _inventoryItem.itemDescription.Accuracy.gameObject.SetActive(false);
        _inventoryItem.itemDescription.Constituion.gameObject.SetActive(false);
        _inventoryItem.itemDescription.Speed.gameObject.SetActive(false);
        _inventoryItem.itemDescription.Luck.gameObject.SetActive(false);

        Rarity rarity = rarities.GetRarity(equipment.rarity);

        _inventoryItem.itemDescription.itemName.color = rarity.color;
        _inventoryItem.itemDescription.itemInfo.color = rarity.color;

        List<int> tempStats = new List<int>() { 1, 2, 3, 4, 5 };
        List<int> stats = CollectionUtilities.GetRandomItems(tempStats, rarity.statCount);

        int statCount = Random.Range(statMin, statMax + 1);
        if(stats.Count > 0)
        {
            for (int i = 0; i < statCount; i++)
            {
                int stat = CollectionUtilities.GetRandomItem(stats);

                switch (stat)
                {
                    case 1:
                        pow++;

                        _inventoryItem.itemDescription.Power.gameObject.SetActive(true);
                        _inventoryItem.itemDescription.Power.text = pow + " Power";
                        equipment.power = pow;
                        break;
                    case 2:
                        acc++;

                        _inventoryItem.itemDescription.Accuracy.gameObject.SetActive(true);
                        _inventoryItem.itemDescription.Accuracy.text = acc + " Accuracy";
                        equipment.accuracy = acc;
                        break;
                    case 3:
                        con++;

                        _inventoryItem.itemDescription.Constituion.gameObject.SetActive(true);
                        _inventoryItem.itemDescription.Constituion.text = con + " Constitution";
                        equipment.constitution = con;
                        break;
                    case 4:
                        spd++;

                        _inventoryItem.itemDescription.Speed.gameObject.SetActive(true);
                        _inventoryItem.itemDescription.Speed.text = spd + " Speed";
                        equipment.speed = spd;
                        break;
                    case 5:
                        lck++;

                        _inventoryItem.itemDescription.Luck.gameObject.SetActive(true);
                        _inventoryItem.itemDescription.Luck.text = lck + " Luck";
                        equipment.luck = lck;
                        break;
                }
            }
        }

        return equipment;
    }

#region Weapon Methods
    public Weapon GenerateWeapon(WeaponCategories weaponCategory)
    {
        InventoryItem _inventoryItem = Instantiate(inventoryItem, canvas);

        WeaponDatabase weaponDatabase = new WeaponDatabase();
        switch (weaponCategory)
        {
            case WeaponCategories.OneHanded:
                weaponDatabase = itemDatabase.oneHandedWeapons[Random.Range(0, itemDatabase.oneHandedWeapons.Count)];
                break;
            case WeaponCategories.TwoHanded:
                weaponDatabase = itemDatabase.twoHandedWeapons[Random.Range(0, itemDatabase.twoHandedWeapons.Count)];
                break;
            case WeaponCategories.OffHand:
                weaponDatabase = itemDatabase.offHandWeapons[Random.Range(0, itemDatabase.offHandWeapons.Count)];
                break;
        }

        Rarity rarity = rarities.GetRandomRarity();

        Weapon weapon = new Weapon();
        weapon.rarity = rarity.rarity;
        weapon.weaponCategory = weaponDatabase.weaponCategory;

        WeaponValues weaponValues = itemDatabase.GetWeaponValues(weaponDatabase.weaponCategory, rarity.rarity);
        _inventoryItem.icon.sprite = CollectionUtilities.GetRandomItem(weaponDatabase.weaponIcons);
        weapon.itemName = weaponDatabase.weaponType.ToString();
        weapon.targetCount = weaponValues.targetCount;

        if (weaponValues.minDamage != 0)
        {
            int damage = Random.Range(weaponValues.minDamage, weaponValues.maxDamage + 1);
            _inventoryItem.itemDescription.extraStat.gameObject.SetActive(true);
            _inventoryItem.itemDescription.extraStat.text = damage + " Damage";
            weapon.weaponDamage = damage;
        }
        else
            _inventoryItem.itemDescription.extraStat.gameObject.SetActive(false);

        _inventoryItem.itemDescription.itemName.text = " " + weaponDatabase.weaponType;
        _inventoryItem.itemDescription.itemInfo.text = rarity.rarity.ToString() + " " + weaponDatabase.weaponType;
        return (Weapon)GenerateEquipment(weapon, _inventoryItem, weaponValues.minStats, weaponValues.maxStats);
    }

    public Weapon GenerateWeapon(Specialization charSpec)
    {
        InventoryItem _inventoryItem = Instantiate(inventoryItem, canvas);

        WeaponTypes selectedType = CollectionUtilities.GetRandomItem(charSpec.weaponTypes);
        WeaponDatabase weaponDatabase = itemDatabase.GetWeaponDatabase(selectedType);

        Rarity rarity = rarities.GetRandomRarity();

        Weapon weapon = new Weapon();
        weapon.rarity = rarity.rarity;
        weapon.weaponCategory = weaponDatabase.weaponCategory;

        WeaponValues weaponValues = itemDatabase.GetWeaponValues(weaponDatabase.weaponCategory, rarity.rarity);
        _inventoryItem.icon.sprite = CollectionUtilities.GetRandomItem(weaponDatabase.weaponIcons);
        weapon.itemName = weaponDatabase.weaponType.ToString();
        weapon.targetCount = weaponValues.targetCount;
        int actionTypeRoll = Random.Range(0, Enum.GetNames(typeof(Weapon.ActionType)).Length);
        weapon.actionType = (Weapon.ActionType)actionTypeRoll;

        if (weaponValues.minDamage != 0)
        {
            int damage = Random.Range(weaponValues.minDamage, weaponValues.maxDamage + 1);
            _inventoryItem.itemDescription.extraStat.gameObject.SetActive(true);
            _inventoryItem.itemDescription.extraStat.text = damage + " Damage";
            weapon.weaponDamage = damage;
        }
        else
            _inventoryItem.itemDescription.extraStat.gameObject.SetActive(false);

        _inventoryItem.itemDescription.itemName.text = " " + weaponDatabase.weaponType;
        _inventoryItem.itemDescription.itemInfo.text = rarity.rarity.ToString() + " " + weaponDatabase.weaponType;
        return (Weapon)GenerateEquipment(weapon, _inventoryItem, weaponValues.minStats, weaponValues.maxStats);
    }
    #endregion

    #region Armor Methods
    public Armor GenerateArmor(ArmorCategories armorCategory, ArmorTypes armorType)
    {
        InventoryItem _inventoryItem = Instantiate(inventoryItem, canvas);
        ArmorDatabase armorDatabase = itemDatabase.GetArmorDatabase(armorCategory, armorType);
        Rarity rarity = rarities.GetRandomRarity();

        Armor armor = new Armor();
        armor.rarity = rarity.rarity;

        ArmorValues armorValues = itemDatabase.GetArmorValues(armorDatabase.armorCategory, rarity.rarity);

        string itemType = " " + armorDatabase.armorTypes;
        itemType = itemType.Replace('_', ' ');
        _inventoryItem.itemDescription.itemName.text = itemType;
        _inventoryItem.itemDescription.itemInfo.text = rarity.rarity.ToString() + " " + armorDatabase.armorTypes;
        _inventoryItem.icon.sprite = CollectionUtilities.GetRandomItem(armorDatabase.armorIcons);
        armor.itemName = itemType;

        if (armorValues.minArmor != 0)
        {
            int armorValue = Random.Range(armorValues.minArmor, armorValues.maxArmor + 1);
            _inventoryItem.itemDescription.extraStat.gameObject.SetActive(true);
            _inventoryItem.itemDescription.extraStat.text = armorValue + " Armor";
            armor.armorValue = armorValue;
        }
        else
            _inventoryItem.itemDescription.extraStat.gameObject.SetActive(false);

        return (Armor)GenerateEquipment(armor, _inventoryItem, armorValues.minStats, armorValues.maxStats);
    }

    public Armor GenerateArmor(Specialization charSpec, ArmorTypes armorType)
    {
        InventoryItem _inventoryItem = Instantiate(inventoryItem, canvas);

        ArmorCategories selectedCategory = CollectionUtilities.GetRandomItem(charSpec.armorCategories);
        ArmorDatabase armorDatabase = itemDatabase.GetArmorDatabase(selectedCategory, armorType);

        Rarity rarity = rarities.GetRandomRarity();

        Armor armor = new Armor();
        armor.rarity = rarity.rarity;

        //null reference
        ArmorValues armorValues = itemDatabase.GetArmorValues(armorDatabase.armorCategory, rarity.rarity);

        string itemType = " " + armorDatabase.armorTypes;
        itemType = itemType.Replace('_', ' ');
        _inventoryItem.itemDescription.itemName.text = itemType;
        _inventoryItem.itemDescription.itemInfo.text = rarity.rarity.ToString() + " " + armorDatabase.armorTypes;
        _inventoryItem.icon.sprite = CollectionUtilities.GetRandomItem(armorDatabase.armorIcons);
        armor.itemName = itemType;

        if (armorValues.minArmor != 0)
        {
            int armorValue = Random.Range(armorValues.minArmor, armorValues.maxArmor + 1);
            _inventoryItem.itemDescription.extraStat.gameObject.SetActive(true);
            _inventoryItem.itemDescription.extraStat.text = armorValue + " Armor";
            armor.armorValue = armorValue;
        }
        else
            _inventoryItem.itemDescription.extraStat.gameObject.SetActive(false);

        return (Armor)GenerateEquipment(armor, _inventoryItem, armorValues.minStats, armorValues.maxStats);
    }

    #endregion
}