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

    private static int chanceIncrease = 20;

    public void SetStatUI(Equipment equipment, InventoryItem _inventoryItem)
    {
        equipment.inventoryItem = _inventoryItem;
        _inventoryItem.itemDescription.Power.gameObject.SetActive(false);
        _inventoryItem.itemDescription.Accuracy.gameObject.SetActive(false);
        _inventoryItem.itemDescription.Constituion.gameObject.SetActive(false);
        _inventoryItem.itemDescription.Speed.gameObject.SetActive(false);
        _inventoryItem.itemDescription.Luck.gameObject.SetActive(false);
        
        Rarity rarity = rarities.GetRarity(equipment.rarity);

        _inventoryItem.itemDescription.itemName.color = rarity.color;
        _inventoryItem.itemDescription.itemInfo.color = rarity.color;

        if(equipment.power > 0)
        {
            _inventoryItem.itemDescription.Power.gameObject.SetActive(true);
            _inventoryItem.itemDescription.Power.text = equipment.power + " Power";
        }

        if(equipment.accuracy > 0)
        {
            _inventoryItem.itemDescription.Accuracy.gameObject.SetActive(true);
            _inventoryItem.itemDescription.Accuracy.text = equipment.accuracy + " Accuracy";
        }

        if(equipment.constitution > 0)
        {
            _inventoryItem.itemDescription.Constituion.gameObject.SetActive(true);
            _inventoryItem.itemDescription.Constituion.text = equipment.constitution + " Constitution";
        }

        if(equipment.speed > 0)
        {
            _inventoryItem.itemDescription.Speed.gameObject.SetActive(true);
            _inventoryItem.itemDescription.Speed.text = equipment.speed + " Speed";
        }

        if(equipment.luck > 0)
        {
            _inventoryItem.itemDescription.Luck.gameObject.SetActive(true);
            _inventoryItem.itemDescription.Luck.text = equipment.luck + " Luck";
        }
    }

    public Equipment ApplyStats(Equipment equipment, int statCount)
    {
        int pow = 0;
        int acc = 0;
        int con = 0;
        int spd = 0;
        int lck = 0;

        Rarity rarity = rarities.GetRarity(equipment.rarity);

        List<int> tempStats = new List<int>() { 1, 2, 3, 4, 5};
        List<int> stats = CollectionUtilities.GetRandomItems(tempStats, rarity.statCount);

        if (stats.Count > 0)
        {
            for (int i = 0; i < statCount; i++)
            {
                int stat = CollectionUtilities.GetRandomItem(stats);

                switch (stat)
                {
                    case 1:
                        pow++;
                        equipment.power = pow;
                        break;
                    case 2:
                        acc++;
                        equipment.accuracy = acc;
                        break;
                    case 3:
                        con++;
                        equipment.constitution = con;
                        break;
                    case 4:
                        spd++;
                        equipment.speed = spd;
                        break;
                    case 5:
                        lck++;
                        equipment.luck = lck;
                        break;
                }
            }
        }

        return equipment;
    }

    public List<Equipment> GenerateEquipment(int challengeRating, Character character)
    {
        if (challengeRating == 1) return new List<Equipment>(0);
        int statPointMax = (challengeRating * 5) - 5;
        int statPoints = Random.Range(statPointMax - 4, statPointMax);

        int gearSlotMin = 1;
        if (statPoints > 20)
            gearSlotMin = (int)Math.Ceiling(statPoints / 20.0);
        int gearSlots = Random.Range(gearSlotMin, 7);

        int weaponCount;
        int armorCount;

        if (gearSlots > CharacterGenerator.Instance.armorSlots)
        {
            weaponCount = gearSlots - CharacterGenerator.Instance.armorSlots;
            armorCount = gearSlots - weaponCount;
        }
        else
        {
            weaponCount = Random.Range(0, Mathf.Clamp(gearSlots, 0, 2) + 1);
            armorCount = gearSlots - weaponCount;
        }

        int _statMax = statPoints / gearSlots;
        List<Stat> stats = new List<Stat>();

        #region GenerateEquipment

        ArmorTypes[] armorTypes = (ArmorTypes[])Enum.GetValues(typeof(ArmorTypes));
        ArmorTypes[] randomArmorTypes = CollectionUtilities.GetRandomItems(armorTypes, armorCount);

        List<Armor> armorGenerated = new List<Armor>();
        for (int i = 0; i < armorCount; i++)
        {
            int gearStartStats = Random.Range(0, _statMax);
            armorGenerated.Add(GenerateArmor(character.specialization, randomArmorTypes[i], gearStartStats));
            statPoints -= gearStartStats;
            stats.Add(new Stat() { statValue = gearStartStats, statMax = 20, rollChance = (100 - (int)((float)(gearStartStats / 20) * 100)) });
        }

        #region Weapons
        List<Weapon> weaponsGenerated = new List<Weapon>();
        if (weaponCount > 0)
        {
            List<WeaponCategories> weaponCategories = new List<WeaponCategories>();

            foreach (WeaponTypes weaponType in character.specialization.weaponTypes)
            {
                WeaponCategories category = (WeaponCategories)itemDatabase.GetWeaponCategory(weaponType);
                if (!weaponCategories.Contains(category))
                    weaponCategories.Add(category);
            }

            if (weaponCount == 2)
            {
                if (weaponCategories.Contains(WeaponCategories.TwoHanded))
                    weaponCategories.Remove(WeaponCategories.TwoHanded);
            }

            WeaponCategories weaponOne = CollectionUtilities.GetRandomItem(weaponCategories);
            WeaponCategories? weaponTwo = null;

            switch (weaponOne)
            {
                case WeaponCategories.TwoHanded:
                    break;
                case WeaponCategories.OneHanded:
                    if (weaponCategories.Contains(WeaponCategories.TwoHanded))
                        weaponCategories.Remove(WeaponCategories.TwoHanded);
                    weaponTwo = CollectionUtilities.GetRandomItem(weaponCategories);
                    break;
                case WeaponCategories.OffHand:
                    if (weaponCategories.Contains(WeaponCategories.TwoHanded))
                        weaponCategories.Remove(WeaponCategories.TwoHanded);

                    if (weaponCategories.Contains(WeaponCategories.OffHand))
                        weaponCategories.Remove(WeaponCategories.OffHand);
                    
                    weaponTwo = CollectionUtilities.GetRandomItem(weaponCategories);
                    break;
            }


            List<WeaponDatabase> weaponOneDatabases = itemDatabase.GetWeaponDatabases(character.specialization.weaponTypes, weaponOne);
            Debug.Log("Weapon Count: " + weaponCount + " --  Weapon: " + weaponOne + " -- WeaponDatabase: " + weaponOneDatabases.Count);
            WeaponDatabase randomWeaponOneDatabase = CollectionUtilities.GetRandomItem(weaponOneDatabases);

            List<WeaponDatabase> weaponTwoDatabases = new List<WeaponDatabase>();
            WeaponDatabase randomWeaponTwoDatabase = null;

            if (weaponTwo != null)
            {
                weaponTwoDatabases = itemDatabase.GetWeaponDatabases(character.specialization.weaponTypes, weaponOne);
                randomWeaponTwoDatabase = CollectionUtilities.GetRandomItem(weaponTwoDatabases);
            }
            
            for (int i = 0; i < weaponCount; i++)
            {
                Weapon weapon;// = GenerateWeapon(spec, i == 0 ? randomWeaponOneDatabase.weaponType : randomWeaponTwoDatabase.weaponType, statPoints);
                              //check for one handed weapon
                              //if _statMax is over 10. set it to 10, add extra back into statpoints
                              //set Stat.statMax to 10
                              //if not one handed, set Stat.statMax to 20

                if ((i == 0 ? randomWeaponOneDatabase.weaponCategory : randomWeaponTwoDatabase.weaponCategory) == WeaponCategories.OneHanded)
                {
                    int gearStartStats = _statMax == 0 ? 0 : Random.Range(0, _statMax / 2);
                    weapon = GenerateWeapon(character.specialization, i == 0 ? randomWeaponOneDatabase.weaponType : randomWeaponTwoDatabase.weaponType, gearStartStats);
                    statPoints -= gearStartStats;
                    stats.Add(new Stat() { statValue = gearStartStats, statMax = 10, rollChance = (100 - (int)((float)(gearStartStats / 10) * 100)) });
                }
                else
                {
                    int gearStartStats = Random.Range(0, _statMax);
                    weapon = GenerateWeapon(character.specialization, i == 0 ? randomWeaponOneDatabase.weaponType : randomWeaponTwoDatabase.weaponType, gearStartStats);
                    statPoints -= gearStartStats;
                    stats.Add(new Stat() { statValue = gearStartStats, statMax = 20, rollChance = (100 - (int)((float)(gearStartStats / 20) * 100)) });
                }
                weaponsGenerated.Add(weapon);
            }
        }
        #endregion
        #endregion

        StartCoroutine(AllocateStats(statPoints, stats));
        List<Equipment> equipment = new List<Equipment>();

        int statIndex = 0;
        foreach (Armor armor in armorGenerated)
        {
            equipment.Add(ApplyStats(armor, stats[statIndex].statValue));

            InventoryItem _inventoryItem = Instantiate(inventoryItem, character.armorSlots[statIndex]);

            ArmorDatabase armorDatabase = itemDatabase.GetArmorDatabase(armor.armorCategory, armor.armorType);
            ArmorValues armorValues = itemDatabase.GetArmorValues(armorDatabase.armorCategory, armor.rarity);

            string itemType = " " + armorDatabase;
            itemType = itemType.Replace('_', ' ');
            _inventoryItem.itemDescription.itemName.text = itemType;
            _inventoryItem.itemDescription.itemInfo.text = armor.rarity.ToString() + " " + armorDatabase.armorTypes;
            _inventoryItem.icon.sprite = CollectionUtilities.GetRandomItem(armorDatabase.armorIcons);

            if (armorValues.minArmor != 0)
            {
                _inventoryItem.itemDescription.extraStat.gameObject.SetActive(true);
                _inventoryItem.itemDescription.extraStat.text = armor.armorValue + " Armor";
            }
            else
                _inventoryItem.itemDescription.extraStat.gameObject.SetActive(false);

            SetStatUI(armor, _inventoryItem);
            statIndex++;
        }

        for (int i = 0; i < weaponsGenerated.Count; i++)
        {
            Weapon weapon = weaponsGenerated[i];

            equipment.Add(ApplyStats(weapon, stats[statIndex].statValue));

            Transform slot = character.primaryWeaponSlot;

            if(i == 0)
            {
                if(weapon.weaponCategory == WeaponCategories.OffHand)
                    slot = character.secondaryWeaponSlot;
            }
            else
            {
                switch (weapon.weaponCategory)
                {
                    case WeaponCategories.OneHanded:
                    case WeaponCategories.OffHand:
                        slot = character.secondaryWeaponSlot;
                        break;
                }
            }

            InventoryItem _inventoryItem = Instantiate(inventoryItem, slot);

            WeaponDatabase weaponDatabase = itemDatabase.GetWeaponDatabase(weapon.weaponType);
            WeaponValues weaponValues = itemDatabase.GetWeaponValues(weaponDatabase.weaponCategory, weapon.rarity);

            _inventoryItem.itemDescription.itemName.text = " " + weaponDatabase.weaponType;
            _inventoryItem.itemDescription.itemInfo.text = weapon.rarity.ToString() + " " + weaponDatabase.weaponType;
            _inventoryItem.icon.sprite = CollectionUtilities.GetRandomItem(weaponDatabase.weaponIcons);

            if (weaponValues.minDamage != 0)
            {
                _inventoryItem.itemDescription.extraStat.gameObject.SetActive(true);
                _inventoryItem.itemDescription.extraStat.text = weapon.weaponDamage + " Damage";
            }
            else
                _inventoryItem.itemDescription.extraStat.gameObject.SetActive(false);

            SetStatUI(weapon, _inventoryItem);
            statIndex++;
        }

        return equipment;
    }

    IEnumerator AllocateStats(int statPoints, List<Stat> stats)
    {
        int iterations = 0;
        for (; ; )
        {
            if (statPoints == 0) break;
            for (int slot = 0; slot < stats.Count; slot++)
            {
                if (statPoints == 0) break;
                iterations++;
                if (stats[slot].rollChance == 0) continue;
                int roll = Random.Range(0, 101);
                if (roll <= stats[slot].rollChance)
                {
                    stats[slot].statValue += 1;
                    stats[slot].rollChance = (100 - (int)((float)(stats[slot].statValue / stats[slot].statMax) * 100));
                    statPoints--;
                }
                else
                    stats[slot].rollChance += chanceIncrease;
            }
        }
        yield break;
    }

    #region Weapon Methods
    public Weapon GenerateWeapon(WeaponCategories weaponCategory, Transform slot)
    {
        InventoryItem _inventoryItem = Instantiate(inventoryItem, slot);

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

        int statCount = Random.Range(weaponValues.minStats, weaponValues.maxStats);
        Weapon _weapon = (Weapon)ApplyStats(weapon, statCount);
        SetStatUI(_weapon, _inventoryItem);
        return _weapon;
    }

    public Weapon GenerateWeapon(Specialization charSpec, Transform slot)
    {
        InventoryItem _inventoryItem = Instantiate(inventoryItem, slot);

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


        int statCount = Random.Range(weaponValues.minStats, weaponValues.maxStats);
        Weapon _weapon = (Weapon)ApplyStats(weapon, statCount);
        SetStatUI(_weapon, _inventoryItem);
        return _weapon;
    }

    public Weapon GenerateWeapon(WeaponTypes weaponType, Transform slot)
    {
        InventoryItem _inventoryItem = Instantiate(inventoryItem, slot);
        WeaponDatabase weaponDatabase = itemDatabase.GetWeaponDatabase(weaponType);

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

        int statCount = Random.Range(weaponValues.minStats, weaponValues.maxStats);
        Weapon _weapon = (Weapon)ApplyStats(weapon, statCount);
        SetStatUI(_weapon, _inventoryItem);
        return _weapon;
    }

    public Weapon GenerateWeapon(Specialization charSpec, WeaponTypes weaponType, int statCount)
    {
        WeaponDatabase weaponDatabase = itemDatabase.GetWeaponDatabase(weaponType);

        Weapon weapon = new Weapon();

        WeaponValues weaponValues = itemDatabase.GetWeaponValues(weaponDatabase.weaponCategory, statCount);

        weapon.rarity = weaponValues.rarityType;
        weapon.weaponCategory = weaponDatabase.weaponCategory;
        weapon.weaponType = weaponType;
        
        weapon.itemName = weaponDatabase.weaponType.ToString();
        weapon.targetCount = weaponValues.targetCount;
        int actionTypeRoll = Random.Range(0, Enum.GetNames(typeof(Weapon.ActionType)).Length);
        weapon.actionType = (Weapon.ActionType)actionTypeRoll;

        if (weaponValues.minDamage != 0)
        {
            int damage = Random.Range(weaponValues.minDamage, weaponValues.maxDamage + 1);
            weapon.weaponDamage = damage;
        }

        return weapon;
    }

    #endregion

    #region Armor Methods
    public Armor GenerateArmor(ArmorCategories armorCategory, ArmorTypes armorType, Transform slot)
    {
        InventoryItem _inventoryItem = Instantiate(inventoryItem, slot);
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

        int statCount = Random.Range(armorValues.minStats, armorValues.maxStats);
        Armor _armor = (Armor)ApplyStats(armor, statCount);
        SetStatUI(_armor, _inventoryItem);
        return _armor;
    }

    public Armor GenerateArmor(Specialization charSpec, ArmorTypes armorType, Transform slot)
    {
        InventoryItem _inventoryItem = Instantiate(inventoryItem, slot);

        ArmorCategories selectedCategory = CollectionUtilities.GetRandomItem(charSpec.armorCategories, new List<ArmorCategories>(){ArmorCategories.Accessory});
        //Debug.Log("Category: " + selectedCategory + " | Type: " + armorType);
        ArmorDatabase armorDatabase = itemDatabase.GetArmorDatabase(selectedCategory, armorType);
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

        int statCount = Random.Range(armorValues.minStats, armorValues.maxStats);
        Armor _armor = (Armor)ApplyStats(armor, statCount);
        SetStatUI(_armor, _inventoryItem);
        return _armor;
    }

    public Armor GenerateArmor(Specialization spec, ArmorTypes armorType, int statCount)
    {
        ArmorCategories selectedCategory = CollectionUtilities.GetRandomItem(spec.armorCategories, new List<ArmorCategories>() { ArmorCategories.Accessory });

        if (armorType == ArmorTypes.Rings || armorType == ArmorTypes.Necklace)
            selectedCategory = ArmorCategories.Accessory;
        
        ArmorDatabase armorDatabase = itemDatabase.GetArmorDatabase(selectedCategory, armorType);
        
        Armor armor = new Armor();
        armor.armorCategory = selectedCategory;
        armor.armorType = armorType;
        ArmorValues armorValues = itemDatabase.GetArmorValues(armorDatabase.armorCategory, statCount);

        armor.rarity = armorValues.rarityType;
        string itemType = " " + armorDatabase.armorTypes;
        itemType = itemType.Replace('_', ' ');
        armor.itemName = itemType;

        if (armorValues.minArmor != 0)
        {
            int armorValue = Random.Range(armorValues.minArmor, armorValues.maxArmor + 1);
            armor.armorValue = armorValue;
        }

        return armor;
    }

    #endregion
}

public class Stat
{
    public int statValue;
    public int statMax;
    public int rollChance;
}