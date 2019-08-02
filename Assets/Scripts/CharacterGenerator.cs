using UnityEngine;
using OuterRimStudios.Utilities;
using System.Collections.Generic;

public class CharacterGenerator : MonoBehaviour
{
    public static CharacterGenerator Instance;
    public Character characterPrefab;

    public List<Specialization> specializations;

    public ItemDatabase itemDatabase;
    public ItemGenerator itemGenerator;

    public Transform playerTeamSlot;
    public Transform enemyTeamSlot;

    public int armorSlots = 4;
    public int weaponSlots = 2;

    private void Awake()
    {
        Instance = this;
    }

    public Character GenerateCharacter(Character.Allegiance allegiance, int challengeRating)
    {
        Character character = Instantiate(characterPrefab, allegiance == Character.Allegiance.Player ? playerTeamSlot : enemyTeamSlot);
        character.allegiance = allegiance;
        Specialization spec = CollectionUtilities.GetRandomItem(specializations);
        character.specialization = spec;
        character.InitializeStats();

        List<Equipment> equipment = itemGenerator.GenerateEquipment(challengeRating, character);

        foreach(Equipment item in equipment)
        {

            if (item.EquipmentType == EquipmentType.Armor)
                character.armor.Add((Armor)item);
            else if (item.EquipmentType == EquipmentType.Weapon)
                character.weapons.Add((Weapon)item);

            item.Equip(character);
        }

        return character;
    }
}

//Picks either Player or Enemy
//Decide if Character has armor
//Set the character's stats
//Set Icon and Name and Class