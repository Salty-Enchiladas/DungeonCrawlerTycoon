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

    public int armorSlots = 4;
    public int weaponSlots = 2;

    public Character GenerateCharacter()
    {
        Character character = Instantiate(characterPrefab, canavs);
        Specialization spec = CollectionUtilities.GetRandomItem(specializations);
        character.specialization = spec;
        character.InitializeStats();

        print("Character Spec: " + spec.specName);

        #region EquipArmor
        ArmorTypes[] armorTypes = (ArmorTypes[])Enum.GetValues(typeof(ArmorTypes));
        ArmorTypes[] randomArmorTypes = CollectionUtilities.GetRandomItems(armorTypes, armorSlots);

        for (int i = 0; i < armorSlots; i++)
        {
            int roll = Random.Range(0, 5);

            if(roll == 4) //Recieved Gear
            {
                Armor armor = null;
                if(randomArmorTypes[i] == ArmorTypes.Rings || randomArmorTypes[i] == ArmorTypes.Necklace)
                    armor = itemGenerator.GenerateArmor(ArmorCategories.Accessory, randomArmorTypes[i]);
                else
                    armor = itemGenerator.GenerateArmor(spec, randomArmorTypes[i]);
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
        for (int i = 0; i < weaponSlots; i++)
        {
            int roll = Random.Range(0, 5);
            if (roll == 4) //Recieved Gear
            {
                Weapon weapon = itemGenerator.GenerateWeapon(spec);

                if (weapon.weaponCategory == WeaponCategories.TwoHanded)
                {
                    if(character.primaryWeapon != null || character.secondaryWeapon != null)
                        break;
                    character.primaryWeapon = weapon;
                    break;
                }
                else if(weapon.weaponCategory == WeaponCategories.OffHand)
                {
                    if(character.secondaryWeapon != null)
                        character.secondaryWeapon = weapon;
                }
                else if(weapon.weaponCategory == WeaponCategories.OneHanded)
                {
                    if(character.primaryWeapon != null)
                        character.primaryWeapon = weapon;
                    else if(character.secondaryWeapon != null)
                        character.secondaryWeapon = weapon;
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