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
    public ItemDatabase itemDatabase;
    public Rarities rarities;

    [Space]
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemInfo;
    public TextMeshProUGUI extraStat;
    public TextMeshProUGUI target;

    [Space]
    public TextMeshProUGUI Power;
    public TextMeshProUGUI Accuracy;
    public TextMeshProUGUI Constituion;
    public TextMeshProUGUI Speed;
    public TextMeshProUGUI Luck;

    public Image icon;

    public void GenerateItem()
    {
        int pow = 0;
        int acc = 0;
        int con = 0;
        int spd = 0;
        int lck = 0;

        Power.gameObject.SetActive(false);
        Accuracy.gameObject.SetActive(false);
        Constituion.gameObject.SetActive(false);
        Speed.gameObject.SetActive(false);
        Luck.gameObject.SetActive(false);


        Rarity rarity = rarities.GetRandomRarity();

        itemName.color = rarity.color;
        itemInfo.color = rarity.color;

        int roll = Random.Range(0, 101);

        string itemType;
        int statMin;
        int statMax;

        if (roll <= 45)
        {
            int typeRoll = Random.Range(0, Enum.GetNames(typeof(WeaponCategories)).Length);
            WeaponDatabase weaponDatabase = new WeaponDatabase();
            if(typeRoll == 0)
                weaponDatabase = itemDatabase.oneHandedWeapons[Random.Range(0, itemDatabase.oneHandedWeapons.Count)];
            else if (typeRoll == 1)
                weaponDatabase = itemDatabase.twoHandedWeapons[Random.Range(0, itemDatabase.twoHandedWeapons.Count)];
            else
                weaponDatabase = itemDatabase.offHandWeapons[Random.Range(0, itemDatabase.offHandWeapons.Count)];

            WeaponValues weaponValues = itemDatabase.GetWeaponValues(weaponDatabase.weaponCategory, rarity.rarity);
            itemType = " " + weaponDatabase.weaponType;
            icon.sprite = CollectionUtilities.GetRandomItem(weaponDatabase.weaponIcons);

            if(weaponValues.minDamage != 0)
            {
                extraStat.gameObject.SetActive(true);
                extraStat.text = Random.Range(weaponValues.minDamage, weaponValues.maxDamage + 1) + " Damage";
            }
            else
                extraStat.gameObject.SetActive(false);


            statMin = weaponValues.minStats;
            statMax = weaponValues.maxStats;
        }
        else
        {
            int typeRoll = Random.Range(0, Enum.GetNames(typeof(ArmorCategories)).Length);
            ArmorDatabase armorDatabase = new ArmorDatabase();
            if (typeRoll == 0)
                armorDatabase = itemDatabase.accessories[Random.Range(0, itemDatabase.accessories.Count)];
            else if (typeRoll == 1)
                armorDatabase = itemDatabase.lightArmor[Random.Range(0, itemDatabase.lightArmor.Count)];
            else if (typeRoll == 2)
                armorDatabase = itemDatabase.mediumArmor[Random.Range(0, itemDatabase.mediumArmor.Count)];
            else
                armorDatabase = itemDatabase.heavyArmor[Random.Range(0, itemDatabase.heavyArmor.Count)];

            ArmorValues armorValues = itemDatabase.GetArmorValues(armorDatabase.armorCategory, rarity.rarity);
            itemType = " " + armorDatabase.armorTypes;
            itemType = itemType.Replace('_', ' ');
            icon.sprite = CollectionUtilities.GetRandomItem(armorDatabase.armorIcons);

            if (armorValues.minArmor != 0)
            {
                extraStat.gameObject.SetActive(true);
                extraStat.text = Random.Range(armorValues.minArmor, armorValues.maxArmor + 1) + " Armor";
            }
            else
                extraStat.gameObject.SetActive(false);

            statMin = armorValues.minStats;
            statMax = armorValues.maxStats;
        }

        itemName.text = itemType;
        itemInfo.text = rarity.rarity.ToString() + itemType;

        List<int> tempStats = new List<int>() { 1, 2, 3, 4, 5 };
        List<int> stats = CollectionUtilities.GetRandomItems(tempStats, rarity.statCount);

        print(rarity.rarity.ToString());

        int statCount = Random.Range(statMin, statMax + 1);
        for (int i = 0; i < statCount; i++)
        {
            int stat = CollectionUtilities.GetRandomItem(stats);

            switch (stat)
            {
                case 1:
                    pow++;

                    Power.gameObject.SetActive(true);
                    Power.text = pow + " Power";
                    break;
                case 2:
                    acc++;

                    Accuracy.gameObject.SetActive(true);
                    Accuracy.text = acc + " Accuracy";
                    break;
                case 3:
                    con++;

                    Constituion.gameObject.SetActive(true);
                    Constituion.text = con + " Constituion";
                    break;
                case 4:
                    spd++;

                    Speed.gameObject.SetActive(true);
                    Speed.text = spd + " Speed";
                    break;
                case 5:
                    lck++;

                    Luck.gameObject.SetActive(true);
                    Luck.text = lck + " Luck";
                    break;
            }
        }

        if (rarity.rarity == RarityType.Artifact)
            Debug.LogError("FOUND IT");
    }

    void SamGeneration()
    {
        int pow = 0;
        int acc = 0;
        int con = 0;
        int spd = 0;
        int lck = 0;

        Power.gameObject.SetActive(false);
        Accuracy.gameObject.SetActive(false);
        Constituion.gameObject.SetActive(false);
        Speed.gameObject.SetActive(false);
        Luck.gameObject.SetActive(false);

        itemName.text = "Helm of Wrath";
        itemInfo.text = "Rare Heavy Helmet";

        itemName.color = rarities.GetRarityColor(RarityType.Rare);
        itemInfo.color = rarities.GetRarityColor(RarityType.Rare);

        extraStat.text = "24 Armor";
        target.gameObject.SetActive(false);

        for (int i = 0; i < 8; i++)
        {
            int stat = Random.Range(1, 6);

            switch (stat)
            {
                case 1:
                    pow++;

                    Power.gameObject.SetActive(true);
                    Power.text = pow + " Power";
                    break;
                case 2:
                    acc++;

                    Accuracy.gameObject.SetActive(true);
                    Accuracy.text = acc + " Accuracy";
                    break;
                case 3:
                    con++;

                    Constituion.gameObject.SetActive(true);
                    Constituion.text = con + " Constituion";
                    break;
                case 4:
                    spd++;

                    Speed.gameObject.SetActive(true);
                    Speed.text = spd + " Speed";
                    break;
                case 5:
                    lck++;

                    Luck.gameObject.SetActive(true);
                    Luck.text = lck + " Luck";
                    break;
            }
        }
    }
}
