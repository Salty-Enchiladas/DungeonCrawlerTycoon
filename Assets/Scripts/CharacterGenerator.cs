using System;
using UnityEngine;
using OuterRimStudios.Utilities;
using Random = UnityEngine.Random;
using System.Collections.Generic;

public class CharacterGenerator : MonoBehaviour
{
    public Transform canavs;
    public Character characterPrefab;

    public List<Specialization> specializations;

    public ItemDatabase itemDatabase;
    public ItemGenerator itemGenerator;

    public Transform playerTeamSlot;
    public Transform enemyTeamSlot;

    public int armorChance = 1;
    public int weaponChance = 1;

    public int armorSlots = 4;
    public int weaponSlots = 2;

    public Character GenerateCharacter(Character.Allegiance allegiance)
    {
        Character character = Instantiate(characterPrefab, allegiance == Character.Allegiance.Player ? playerTeamSlot : enemyTeamSlot);
        character.allegiance = allegiance;
        Specialization spec = CollectionUtilities.GetRandomItem(specializations);
        character.specialization = spec;
        character.InitializeStats();

        #region EquipArmor
        ArmorTypes[] armorTypes = (ArmorTypes[])Enum.GetValues(typeof(ArmorTypes));
        ArmorTypes[] randomArmorTypes = CollectionUtilities.GetRandomItems(armorTypes, armorSlots);

        for (int i = 0; i < armorSlots; i++)
        {
            int roll = Random.Range(0, armorChance);

            if(roll == 0) //Recieved Gear
            {
                Armor armor = null;
                if(randomArmorTypes[i] == ArmorTypes.Rings || randomArmorTypes[i] == ArmorTypes.Necklace)
                    armor = itemGenerator.GenerateArmor(ArmorCategories.Accessory, randomArmorTypes[i], character.armorSlots[i]);
                else
                    armor = itemGenerator.GenerateArmor(spec, randomArmorTypes[i], character.armorSlots[i]);
                //Equip gear

                if(armor != null)
                {
                    character.armor.Add(armor);
                    armor.Equip(character);
                }
                //Sam and Hector were here
            }
        }
        #endregion
        #region EquipWeapon

        List<WeaponTypes> randomWeaponTypes = CollectionUtilities.GetRandomItems(spec.weaponTypes, weaponSlots);

        for (int i = 0; i < weaponSlots; i++)
        {
            int roll = Random.Range(0, weaponChance);
            if (roll == 0) //Recieved Gear
            {
                Weapon weapon = null;
                WeaponCategories weaponCategory = (WeaponCategories)itemDatabase.GetWeaponCategory(randomWeaponTypes[i]);
                if (weaponCategory == WeaponCategories.TwoHanded)
                {
                    if(character.primaryWeapon != null || character.secondaryWeapon != null)
                        break;
                    weapon = itemGenerator.GenerateWeapon(randomWeaponTypes[i], character.primaryWeaponSlot);
                    character.primaryWeapon = weapon;
                    break;
                }
                else if(weaponCategory == WeaponCategories.OffHand)
                {
                    if(character.secondaryWeapon != null)
                    {
                        weapon = itemGenerator.GenerateWeapon(randomWeaponTypes[i], character.secondaryWeaponSlot);
                        character.secondaryWeapon = weapon;
                    }
                }
                else if(weaponCategory == WeaponCategories.OneHanded)
                {
                    if(character.primaryWeapon != null)
                    {
                        weapon = itemGenerator.GenerateWeapon(randomWeaponTypes[i], character.primaryWeaponSlot);
                        character.primaryWeapon = weapon;
                    }
                    else if(character.secondaryWeapon != null)
                    {
                        weapon = itemGenerator.GenerateWeapon(randomWeaponTypes[i], character.secondaryWeaponSlot);
                        character.secondaryWeapon = weapon;
                    }
                }

                weapon.Equip(character);
            }
        }
        #endregion

        return character;
    }
}

//Picks either Player or Enemy
//Decide if Character has armor
//Set the character's stats
//Set Icon and Name and Class