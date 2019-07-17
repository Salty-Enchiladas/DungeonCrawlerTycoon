using System.Collections.Generic;
using UnityEngine;
using OuterRimStudios.Utilities;
using System.Linq;
using System.Collections;

public static class CombatManager
{
    public static IEnumerator SimulateFight(Team playerTeam, Team enemyTeam)
    {
        //Key = Speed stat | Value = List of characters with same Speed stat 
        SortedDictionary<int, List<Character>> combatants = new SortedDictionary<int, List<Character>>(new SpeedComp());
        AddCombatants(ref combatants, playerTeam);
        AddCombatants(ref combatants, enemyTeam);

        foreach(KeyValuePair<int, List<Character>> keyValuePair in combatants)
        {
            Debug.Log("-- " + keyValuePair.Key + " --");
            foreach (Character c in keyValuePair.Value)
                Debug.Log(c.name);
        }

        //________GENERATE LOOT TABLE INFO_________

        for(;;)
        {
            Debug.Log("Simulating...");

            if (!CheckTeamStatus(playerTeam) || !CheckTeamStatus(enemyTeam))
            {
                Debug.Log("Player team alive = " + CheckTeamStatus(playerTeam));
                Debug.Log("Enemy team alive = " + CheckTeamStatus(enemyTeam));
                yield break;
            }

            List<Character> combatOrder = new List<Character>();
            combatOrder = GetCombatOrder(combatants);

            Debug.Log("------Combat Order------");
            foreach (Character c in combatOrder)
            {
                Debug.Log(c.name);
            }
            Debug.Log("------------");

            //Loops through each Character in the combatOrder to take thier turn
            foreach (Character character in combatOrder)
            {
                if (character.HealthPercentage <= 0) continue;
                //Check for potion use here

                Debug.Log(character.name + "'s Turn!");

                if(!character.primaryWeapon)
                {
                    Character target = character.allegiance == Character.Allegiance.Player ? CollectionUtilities.GetRandomItem(enemyTeam.characters) : CollectionUtilities.GetRandomItem(playerTeam.characters);
                    Weapon unarmedWeapon = new Weapon();
                    DealDamage(character, unarmedWeapon, target, playerTeam, enemyTeam);
                }
                else if (character.primaryWeapon.actionType == Weapon.ActionType.Damage) //Attacks with the character's primary weapon
                {
                    //Finds a list of targets at random according to the character's allegiance and how many target's this weapon hits
                    List<Character> targets = character.allegiance == Character.Allegiance.Player ? CollectionUtilities.GetRandomItems(enemyTeam.characters, character.primaryWeapon.targetCount) : CollectionUtilities.GetRandomItems(playerTeam.characters, character.primaryWeapon.targetCount);

                    Debug.Log(character.name + " is attacking: ");

                    foreach (Character c in targets)
                        Debug.Log(c.name);

                    //Deals damage to all targets
                    foreach (Character target in targets)
                        DealDamage(character, character.primaryWeapon, target, playerTeam, enemyTeam);
                }
                else if (character.primaryWeapon.actionType == Weapon.ActionType.Heal)  //If the weapon heals this will heal as many targets as the weapon dictates
                {
                    Heal(character, character.primaryWeapon, playerTeam, enemyTeam);
                }

                //if the Character is dual wielding weapons they will try to attack or heal with their offhand
                if (character.secondaryWeapon && character.secondaryWeapon.actionType == Weapon.ActionType.Damage)
                {
                    List<Character> targets = character.allegiance == Character.Allegiance.Player ? CollectionUtilities.GetRandomItems(enemyTeam.characters, character.secondaryWeapon.targetCount) : CollectionUtilities.GetRandomItems(playerTeam.characters, character.secondaryWeapon.targetCount);

                    Debug.Log(character.name + " is attacking: ");

                    foreach (Character c in targets)
                        Debug.Log(c.name);

                    foreach (Character target in targets)
                        DealDamage(character, character.secondaryWeapon, target, playerTeam, enemyTeam);
                }
                else if (character.secondaryWeapon && character.secondaryWeapon.actionType == Weapon.ActionType.Heal)
                {
                    Heal(character, character.secondaryWeapon, playerTeam, enemyTeam);
                }
            }

            yield return new WaitForEndOfFrame();
        }

        //yield return new WaitUntil(() => Simulating(ref combatants, playerTeam, enemyTeam));
    }

    static void AddCombatants(ref SortedDictionary<int, List<Character>> combatants, Team team)
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
    static List<Character> GetCombatOrder(SortedDictionary<int, List<Character>> _combatants)
    {
        List<Character> combatOrder = new List<Character>();
        Debug.Log("combatants: " + _combatants.Count);
        //cohort refers to the characters that have the same speed value
        foreach(KeyValuePair<int, List<Character>> cohort in _combatants)
        {
            //Check if more than one character with the same speed
            if(cohort.Value.Count > 1)
            {
                //Roll for initiative
                List<Character> cohortList = new List<Character>();

                Debug.Log("<><><><>");
                foreach (Character c in cohort.Value)
                {
                    cohortList.Add(c);
                    Debug.Log("Adding " + c.name + " to cohortList.");
                }

                int characterCount = cohort.Value.Count;

                for (int i = characterCount; i > 0; i--)
                {
                    Character selectedChar = CollectionUtilities.GetRandomItem(cohortList);
                    if (selectedChar.HealthPercentage > 0)
                    {
                        Debug.Log(selectedChar.name + " selected. Removing from cohort list.");

                        cohortList.Remove(selectedChar);
                        Debug.Log("Remaining In Cohort List");

                        foreach (Character c in cohort.Value)
                        {
                            Debug.Log(c.name);
                        }
                        combatOrder.Add(selectedChar);
                    }
                }
                Debug.Log("<><><><>");
            }
            else    //Only one person with specific speed stat
            {
                if(cohort.Value[0].HealthPercentage > 0)
                    combatOrder.Add(cohort.Value[0]);
            }
        }

        return combatOrder;
    }

    //Checks if team is still alive
    //return true if alive
    static bool CheckTeamStatus(Team team)
    {
        bool teamAlive = false;
        Debug.Log("team size: " + team.characters.Count);
        foreach(Character character in team.characters)
        {
            Debug.Log(character.name + "'s HP: " + character.HealthPercentage);
            if(character.HealthPercentage > 0)
                teamAlive = true;
        }
        
        return teamAlive;
    }

    static void DealDamage(Character character, Weapon weapon, Character target, Team playerTeam, Team enemyTeam)
    {
        //Roll for hit
        int roll = Random.Range(1, 101);

        //Character hits
        if (roll <= character.HitChance)
        {
            Debug.Log(character.name + "'s attack hit " + target.name + "!");

            float damage = weapon.weaponDamage + character.Damage.Value;
            roll = Random.Range(1, 101);

            //Character crits
            if (roll <= character.CritChance)
            {
                Debug.Log(character.name + "'s attack was a crit!");
                damage *= Character.criticalDamageModifier;
            }
            Debug.Log(target.name + " resisted " + target.DamageResistance + " damage.");

            //Apply armor resistance to damage value
            damage -= target.DamageResistance;
            Debug.Log(character.name + " did " + damage + " damage to " + target.name);

            //Apply damage to target
            target.Health.AddModifier(new StatModifier(-damage, StatModType.Flat));

            //If Target is dead remove from team and combatants
            if (target.Health.Value <= 0)
            {
                Debug.Log(target.name + " Died!");
                Team team = target.allegiance == Character.Allegiance.Player ? playerTeam : enemyTeam;
                team.characters.Remove(target);
            }
            else
                Debug.Log(target.name + "'s health is " + target.Health.Value);
        }
        else Debug.Log(character.name + "'s attack missed!");
    }
    
    static void Heal(Character character, Weapon weapon, Team playerTeam, Team enemyTeam)
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
            if (roll <= character.HitChance)
            {
                float healAmount = weapon.weaponDamage + character.Damage.Value;

                roll = Random.Range(1, 101);

                //Character crits
                if (roll <= character.CritChance)
                    healAmount *= Character.criticalDamageModifier;

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
            DealDamage(character, unarmedWeapon, target, playerTeam, enemyTeam);
        }
    }
}

//Used to sort the list based on Character Speed
public class SpeedComp : IComparer<int>
{
    public int Compare(int a, int b)
    {
        /*  Default return values:
            1   : Greater Than
            0   : Equal
            -1  : Less Than
        */
        //Return the inverse value of the normal compare because we are sorting in descending order
        return -a.CompareTo(b);
    }
}

//Move the readonly static modifiers to Character.cs
//Set the Health and Armor stats at Start in Character