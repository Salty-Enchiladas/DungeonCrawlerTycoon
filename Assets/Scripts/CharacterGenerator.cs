using System;
using UnityEngine;
using OuterRimStudios.Utilities;
using Random = UnityEngine.Random;

public class CharacterGenerator : MonoBehaviour
{
    public Transform canavs;
    public Character characterPrefab;

    public ItemDatabase itemDatabase;
    public ItemGenerator itemGenerator;

    public int armorSlots = 4;
    public int weaponSlots = 2;
    
    private void Start() {
        GenerateCharacter();
    }

    public void GenerateCharacter()
    {
        Character character = Instantiate(characterPrefab, canavs);

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
                {
                    armor = itemGenerator.GenerateArmor(ArmorCategories.Accessory, randomArmorTypes[i]);
                }
                else
                {
                    roll = Random.Range(0, 3);

                    switch(roll)
                    {
                        case 0:
                            armor = itemGenerator.GenerateArmor(ArmorCategories.Light, randomArmorTypes[i]);
                            break;
                        case 1:
                            armor = itemGenerator.GenerateArmor(ArmorCategories.Medium, randomArmorTypes[i]);
                            break;
                        case 2:
                            armor = itemGenerator.GenerateArmor(ArmorCategories.Heavy, randomArmorTypes[i]);
                            break;
                    }
                }
                //Equip gear

                if(armor != null)
                {
                    print(armor.itemName);

                    character.armor.Add(armor);
                    armor.Equip(character);
                }
                //Sam and Hector were here
            }
        }
        #endregion
        #region EquipWeapon
        WeaponCategories[] weaponTypes = (WeaponCategories[])Enum.GetValues(typeof(WeaponCategories));
        WeaponCategories[] randomWeaponTypes = CollectionUtilities.GetRandomItems(weaponTypes, weaponSlots);

        for (int i = 0; i < weaponSlots; i++)
        {
            int roll = Random.Range(0, 5);

            if (roll == 4) //Recieved Gear
            {
                Weapon weapon = null;
                if (randomWeaponTypes[i] == WeaponCategories.TwoHanded)
                {
                    if(character.primaryWeapon != null || character.secondaryWeapon != null)
                        break;
                    weapon = itemGenerator.GenerateWeapon(randomWeaponTypes[i]);
                    character.primaryWeapon = weapon;
                    weapon.Equip(character);
                    break;
                }
                else if(randomWeaponTypes[i] == WeaponCategories.OffHand)
                {
                    if(character.secondaryWeapon != null)
                    {
                        weapon = itemGenerator.GenerateWeapon(randomWeaponTypes[i]);
                        character.secondaryWeapon = weapon;
                        weapon.Equip(character);
                    }
                }
                else if(randomWeaponTypes[i] == WeaponCategories.OneHanded)
                {
                    if(character.primaryWeapon != null)
                    {
                        weapon = itemGenerator.GenerateWeapon(randomWeaponTypes[i]);
                        character.primaryWeapon = weapon;
                        weapon.Equip(character);
                    }
                    else if(character.secondaryWeapon != null)
                    {
                        weapon = itemGenerator.GenerateWeapon(randomWeaponTypes[i]);
                        character.secondaryWeapon = weapon;
                        weapon.Equip(character);
                    }
                }
            }
        }
        #endregion
    }
}

//Picks either Player or Enemy
//Decide if Character has armor
//Set the character's stats
//Set Icon and Name and Class