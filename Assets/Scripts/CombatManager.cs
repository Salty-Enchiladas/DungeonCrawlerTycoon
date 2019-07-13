using System.Collections.Generic;
using UnityEngine;
using OuterRimStudios.Utilities;
using System.Linq;

public static class CombatManager
{
    static readonly int baseHitChance = 60;
    static readonly int maximumArmor = 100;

    static readonly float accuracyHitChanceRatio = 2.0f;
    static readonly float criticalHitChanceRatio = 1.0f;

    static readonly float criticalDamageModifier = 2.0f;
    static readonly float damageModifier = 1.0f;
    static readonly float armorModifier = 1.0f;

    static readonly Weapon unarmedWeapon;

    public static bool SimulateFight(Team playerTeam, Team enemyTeam)
    {
        //Key = Speed stat | Value = List of characters with same Speed stat 
        Dictionary<int, List<Character>> combatants = new Dictionary<int, List<Character>>();
        AddCombatants(ref combatants, playerTeam);
        AddCombatants(ref combatants, enemyTeam);

        foreach(KeyValuePair<int, List<Character>> keyValuePair in combatants)
        {
            Debug.Log("------------");
            Debug.Log("--> " + keyValuePair.Key + " <--");

            foreach (Character _character in keyValuePair.Value)
                Debug.Log(_character.name);
            Debug.Log("------------");
        }
        
        //________GENERATE LOOT TABLE INFO_________

        while(CheckTeamStatus(playerTeam))
        {
            if(!CheckTeamStatus(enemyTeam))
                return true;
                
            List<Character> combatOrder = new List<Character>();
            combatOrder = GetCombatOrder(combatants);

            //Loops through each Character in the combatOrder to take thier turn
            foreach(Character character in combatOrder)
            {
                if (character.Health.Value <= 0) continue;
                //Check for potion use here

                //Attacks with the character's primary weapon
                if(character.primaryWeapon.actionType == Weapon.ActionType.Damage)
                {
                    //Finds a list of targets at random according to the character's allegiance and how many target's this weapon hits
                    List<Character> targets = character.allegiance == Character.Allegiance.Player ? CollectionUtilities.GetRandomItems(enemyTeam.characters, character.primaryWeapon.targetCount) : CollectionUtilities.GetRandomItems(playerTeam.characters, character.primaryWeapon.targetCount);

                    //Deals damage to all targets
                    foreach (Character target in targets)
                        DealDamage(ref combatants, character, character.primaryWeapon, target, playerTeam, enemyTeam);
                }
                else if (character.primaryWeapon.actionType == Weapon.ActionType.Heal)  //If the weapon heals this will heal as many targets as the weapon dictates
                {
                    Heal(ref combatants, character, character.primaryWeapon, playerTeam, enemyTeam);
                }

                //if the Character is dual wielding weapons they will try to attack or heal with their offhand
                if(character.secondaryWeapon && character.secondaryWeapon.actionType == Weapon.ActionType.Damage)
                {
                    List<Character> targets = character.allegiance == Character.Allegiance.Player ? CollectionUtilities.GetRandomItems(enemyTeam.characters, character.secondaryWeapon.targetCount) : CollectionUtilities.GetRandomItems(playerTeam.characters, character.secondaryWeapon.targetCount);

                    foreach (Character target in targets)
                        DealDamage(ref combatants, character, character.secondaryWeapon, target, playerTeam, enemyTeam);
                }
                else if (character.secondaryWeapon && character.secondaryWeapon.actionType == Weapon.ActionType.Heal)
                {
                    Heal(ref combatants, character, character.secondaryWeapon, playerTeam, enemyTeam);
                }
            }
        }

        return false;
    }

    static void AddCombatants(ref Dictionary<int, List<Character>> combatants, Team team)
    {
        //This organizes the combtants list so that all characters with similar speeds share the same key
        foreach (Character character in team.characters)
        {
            if (combatants.ContainsKey((int)character.Speed.Value))
                combatants[(int)character.Speed.Value].Add(character);
            else
                combatants.Add((int)character.Speed.Value, new List<Character>() { character });
        }
    }

    //Sorts combat order based on speed
    static List<Character> GetCombatOrder(Dictionary<int, List<Character>> _combatants)
    {
        List<Character> combatOrder = new List<Character>();
        //cohort refers to the characters that have the same speed value
        foreach(KeyValuePair<int, List<Character>> cohort in _combatants)
        {
            //Check if more than one character with the same speed
            if(cohort.Value.Count > 1)
            {
                //Roll for initiative
                int characterCount = cohort.Value.Count;
                for (int i = characterCount; i > 0; i--)
                {
                    Character selectedChar = CollectionUtilities.GetRandomItem(cohort.Value);
                    cohort.Value.Remove(selectedChar);
                    combatOrder.Add(selectedChar);
                }
            }
            else    //Only one person with specific speed stat
                combatOrder.Add(cohort.Value[0]);
        }

        return combatOrder;
    }

    //Checks if team is still alive
    //return true if alive
    static bool CheckTeamStatus(Team team)
    {
        bool teamAlive = false;
        foreach(Character character in team.characters)
        {
            if(character.Health.Value > 0)
                teamAlive = true;
        }
        
        return teamAlive;
    }

    static void DealDamage(ref Dictionary<int, List<Character>> combatants, Character character, Weapon weapon, Character target, Team playerTeam, Team enemyTeam)
    {
        //Roll for hit
        int roll = Random.Range(1, 101);

        //Character hits
        if (roll <= baseHitChance + ((int)character.Accuracy.Value * accuracyHitChanceRatio))
        {
            float damage = weapon.weaponDamage + ((int)character.Power.Value * damageModifier);
            roll = Random.Range(1, 101);

            //Character crits
            if (roll <= (int)character.Accuracy.Value * criticalHitChanceRatio)
                damage *= criticalDamageModifier;

            //Apply armor resistance to damage value
            damage -= (target.DamageResistance.Value + (target.Constitution.Value * armorModifier)) / maximumArmor;
            //Apply damage to target
            target.Health.AddModifier(new StatModifier(-damage, StatModType.Flat));

            //If Target is dead remove from team and combatants
            if (target.Health.Value <= 0)
            {
                Team team = target.allegiance == Character.Allegiance.Player ? playerTeam : enemyTeam;
                team.characters.Remove(target);
                combatants[(int)target.Speed.Value].Remove(target);

                //If there is no one else with the same Speed value in the combatants list, remove this value from the dictionary
                if (combatants[(int)target.Speed.Value].Count <= 0)
                    combatants.Remove((int)target.Speed.Value);
            }
        }
    }
    
    static void Heal(ref Dictionary<int, List<Character>> combatants, Character character, Weapon weapon, Team playerTeam, Team enemyTeam)
    {
        Team friendlyTeam = character.allegiance == Character.Allegiance.Player ? playerTeam : enemyTeam;

        List<Character> healedTargets = new List<Character>();

        for (int i = 0; i < weapon.targetCount; i++)
        {
            Character lowestCharacter = null;
            foreach (Character friendly in friendlyTeam.characters)
            {
                if (lowestCharacter == null)
                    lowestCharacter = friendly;
                else if (friendly.HealthPercentage < lowestCharacter.HealthPercentage && !healedTargets.Contains(friendly))
                    lowestCharacter = friendly;
            }

            if (lowestCharacter.HealthPercentage >= 1) continue;

            //Roll for hit
            int roll = Random.Range(1, 101);

            //Character hits
            if (roll <= baseHitChance + ((int)character.Accuracy.Value * accuracyHitChanceRatio))
            {
                float healAmount = weapon.weaponDamage + ((int)character.Power.Value * damageModifier);

                roll = Random.Range(1, 101);

                //Character crits
                if (roll <= (int)character.Accuracy.Value * criticalHitChanceRatio)
                    healAmount *= criticalDamageModifier;


                //Figures out how much health the character is missing to avoid overhealing
                float missingHealth = lowestCharacter.Health.BaseValue - lowestCharacter.Health.BaseValue;

                if (healAmount > missingHealth)
                    lowestCharacter.Health.AddModifier(new StatModifier(missingHealth, StatModType.Flat));
                else
                    lowestCharacter.Health.AddModifier(new StatModifier(healAmount, StatModType.Flat));
            }

            healedTargets.Add(lowestCharacter);
        }

        //If there were no friendlies healed then the character will deal damage instead using an unarmed weapon (It only uses the Power Stat to determine damage)
        if (healedTargets.Count <= 0)
        {
            Character target = character.allegiance == Character.Allegiance.Player ? CollectionUtilities.GetRandomItem(enemyTeam.characters) : CollectionUtilities.GetRandomItem(playerTeam.characters);

            Weapon unarmedWeapon = new Weapon();
            DealDamage(ref combatants, character, unarmedWeapon, target, playerTeam, enemyTeam);
        }
    }
}

//Press play and hit fight to see the error
//Move the readonly static modifiers to Character.cs
//Set the Health and Armor stats at Start in Character