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
        foreach(int num in stats)
            print("stat slot: " + num);

        int statCount = Random.Range(statMin, statMax + 1);
        print("statcount: " + stats.Count);
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


        WeaponValues weaponValues = itemDatabase.GetWeaponValues(weaponDatabase.weaponCategory, rarity.rarity);
        _inventoryItem.icon.sprite = CollectionUtilities.GetRandomItem(weaponDatabase.weaponIcons);
        weapon.itemName = weaponDatabase.weaponType.ToString();
        if (weaponValues.minDamage != 0)
        {
            _inventoryItem.itemDescription.extraStat.gameObject.SetActive(true);
            _inventoryItem.itemDescription.extraStat.text = Random.Range(weaponValues.minDamage, weaponValues.maxDamage + 1) + " Damage";
        }
        else
            _inventoryItem.itemDescription.extraStat.gameObject.SetActive(false);

        _inventoryItem.itemDescription.itemName.text = " " + weaponDatabase.weaponType;
        _inventoryItem.itemDescription.itemInfo.text = rarity.rarity.ToString() + " " + weaponDatabase.weaponType;
        return (Weapon)GenerateEquipment(weapon, _inventoryItem, weaponValues.minStats, weaponValues.maxStats);
    }

    public Armor GenerateArmor(ArmorCategories armorCategory, ArmorTypes armorType)
    {
        InventoryItem _inventoryItem = Instantiate(inventoryItem, canvas);

        ArmorDatabase armorDatabase = itemDatabase.GetArmorDatabase(armorCategory, armorType);

        Rarity rarity = rarities.GetRandomRarity();

        Armor armor = new Armor();
        armor.rarity = rarity.rarity;

        print("Trying to get: " + armorDatabase.armorCategory + " : " + rarity.rarity);
        ArmorValues armorValues = itemDatabase.GetArmorValues(armorDatabase.armorCategory, rarity.rarity);

        string itemType = " " + armorDatabase.armorTypes;
        itemType = itemType.Replace('_', ' ');
        _inventoryItem.itemDescription.itemName.text = itemType;
        _inventoryItem.itemDescription.itemInfo.text = rarity.rarity.ToString() + " " + armorDatabase.armorTypes;
        _inventoryItem.icon.sprite = CollectionUtilities.GetRandomItem(armorDatabase.armorIcons);
        armor.itemName = itemType;

        if (armorValues.minArmor != 0)
        {
            _inventoryItem.itemDescription.extraStat.gameObject.SetActive(true);
            _inventoryItem.itemDescription.extraStat.text = Random.Range(armorValues.minArmor, armorValues.maxArmor + 1) + " Armor";
        }
        else
            _inventoryItem.itemDescription.extraStat.gameObject.SetActive(false);

        return (Armor)GenerateEquipment(armor, _inventoryItem, armorValues.minStats, armorValues.maxStats);
    }
}
