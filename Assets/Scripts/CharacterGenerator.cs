using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OuterRimStudios.Utilities;
using Random = UnityEngine.Random;

public class CharacterGenerator : MonoBehaviour
{
    public Transform canavs;
    public Character characterPrefab;

    public ItemDatabase itemDatabase;
    public ItemGenerator itemGenerator;
    
    private void Start() {
        GenerateCharacter();
    }

    public void GenerateCharacter()
    {
        Character character = Instantiate(characterPrefab);

        ArmorTypes[] armorTypes = (ArmorTypes[])Enum.GetValues(typeof(ArmorTypes));
        ArmorTypes[] randomArmorTypes = CollectionUtilities.GetRandomItems(armorTypes, 4);

        for (int i = 0; i < 4; i++)
        {
            int roll = Random.Range(0, 5);

            if(roll == 4) //Recieved Gear
            {
                if(randomArmorTypes[i] == ArmorTypes.Rings || randomArmorTypes[i] == ArmorTypes.Necklace)
                {
                    itemGenerator.GenerateArmor(ArmorCategories.Accessory, randomArmorTypes[i]);
                }
                else
                {
                    roll = Random.Range(0, 3);

                    switch(roll)
                    {
                        case 0:
                            itemGenerator.GenerateArmor(ArmorCategories.Light, randomArmorTypes[i]);
                            break;
                        case 1:
                            itemGenerator.GenerateArmor(ArmorCategories.Medium, randomArmorTypes[i]);
                            break;
                        case 2:
                            itemGenerator.GenerateArmor(ArmorCategories.Heavy, randomArmorTypes[i]);
                            break;
                    }
                }
                //Equip gear

            }
        }
    }
}

//Picks either Player or Enemy
//Decide if Character has armor
//Set the character's stats
//Set Icon and Name and Class