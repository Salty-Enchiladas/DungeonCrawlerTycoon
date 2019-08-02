using System.Collections.Generic;
using UnityEngine;
using OuterRimStudios.Utilities;
using System.Linq;
using System.Collections;

public static class CombatManager
{
    static bool extraAttack;

    public static IEnumerator SimulateFight(Team playerTeam, Team enemyTeam)
    {
        CombatLog.ClearLog();
        //Key = Speed stat | Value = List of characters with same Speed stat 
        SortedDictionary<int, List<Character>> combatants = new SortedDictionary<int, List<Character>>(new SpeedComp());
        AddCombatants(ref combatants, playerTeam);
        AddCombatants(ref combatants, enemyTeam);

        //________GENERATE LOOT TABLE INFO_________
        int roundCount = 0;
        for(;;)
        {
            if (!CheckTeamStatus(playerTeam) || !CheckTeamStatus(enemyTeam))
            {
                CombatLog.Log(CheckTeamStatus(playerTeam) ? "Player Team Wins!" : "Enemy Team Winds!");
                yield break;
            }

            roundCount++;
            CombatLog.Log("<b><i><size=50>Round " + roundCount + "</size></i></b>");
            CombatLog.Log("_________________________________________________");
            List<Character> combatOrder = new List<Character>();
            combatOrder = GetCombatOrder(combatants);

            CombatLog.Log("<b>Combat Order</b>");
            foreach (Character c in combatOrder)
                CombatLog.Log(c.characterName);
            CombatLog.Log("------------");
            CombatLog.Log("");

            //Loops through each Character in the combatOrder to take thier turn
            foreach (Character character in combatOrder)
            {
                if (!CheckTeamStatus(playerTeam) || !CheckTeamStatus(enemyTeam))
                    yield break;
                if (character.HealthPercentage <= 0) continue;
                //Check for potion use here

                CombatLog.Log(character.characterName + "'s Turn!");
                CombatLog.Log("Health: " + character.Health.Value);
                CombatLog.Log("");

                if (character.weapons.Count > 0 && character.specialization.actionType == ActionType.Damage) //Attacks with the character's primary weapon
                {
                    //Finds a list of targets at random according to the character's allegiance and how many target's this weapon hits
                    List<Character> targets = character.allegiance == Character.Allegiance.Player ? CollectionUtilities.GetRandomItems(enemyTeam.characters, character.weapons[0].targetCount) : CollectionUtilities.GetRandomItems(playerTeam.characters, character.weapons[0].targetCount);
                    CombatLog.Log("Target(s):");
                    foreach (Character c in targets)
                        CombatLog.Log(c.characterName);
                    CombatLog.Log("");

                    //Deals damage to all targets
                    foreach (Character target in targets)
                        DealDamage(character, character.weapons[0], target, playerTeam, enemyTeam);
                }
                else if (character.weapons.Count > 0 && character.specialization.actionType == ActionType.Healing)  //If the weapon heals this will heal as many targets as the weapon dictates
                {
                    Heal(character, character.weapons[0], playerTeam, enemyTeam);
                }
                else
                {
                    //Finds a list of targets at random according to the character's allegiance and how many target's this weapon hits
                    Character target = character.allegiance == Character.Allegiance.Player ? CollectionUtilities.GetRandomItem(enemyTeam.characters) : CollectionUtilities.GetRandomItem(playerTeam.characters);

                    CombatLog.Log("Target:" + target.characterName);
                    CombatLog.Log("");

                    //Deals damage to target
                    DealDamage(character, Character.unarmedWeapon, target, playerTeam, enemyTeam);
                }

                //if the Character is dual wielding weapons they will try to attack or heal with their offhand
                if (character.weapons.Count > 1 && character.specialization.actionType == ActionType.Damage)
                {
                    List<Character> targets = character.allegiance == Character.Allegiance.Player ? CollectionUtilities.GetRandomItems(enemyTeam.characters, character.weapons[1].targetCount) : CollectionUtilities.GetRandomItems(playerTeam.characters, character.weapons[1].targetCount);

                    CombatLog.Log("Target(s):");
                    foreach (Character c in targets)
                        CombatLog.Log(c.characterName);
                    CombatLog.Log("");

                    foreach (Character target in targets)
                        DealDamage(character, character.weapons[1], target, playerTeam, enemyTeam);
                }
                else if (character.weapons.Count > 1 && character.weapons[1] != null && character.specialization.actionType == ActionType.Healing)
                {
                    Heal(character, character.weapons[1], playerTeam, enemyTeam);
                }
                CombatLog.Log("End of " + character.characterName + "'s Turn.");
                CombatLog.Log("_______________________");

                yield return new WaitForSeconds(.025f);
            }
            yield return new WaitForEndOfFrame();
        }
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
        //Debug.Log("combatants: " + _combatants.Count);
        //cohort refers to the characters that have the same speed value
        foreach(KeyValuePair<int, List<Character>> cohort in _combatants)
        {
            //Check if more than one character with the same speed
            if(cohort.Value.Count > 1)
            {
                //Roll for initiative
                List<Character> cohortList = new List<Character>();

                //Debug.Log("<><><><>");
                foreach (Character c in cohort.Value)
                {
                    cohortList.Add(c);
                    //Debug.Log("Adding " + c.name + " to cohortList.");
                }

                int characterCount = cohort.Value.Count;

                for (int i = characterCount; i > 0; i--)
                {
                    Character selectedChar = CollectionUtilities.GetRandomItem(cohortList);
                    if (selectedChar.HealthPercentage > 0)
                    {
                        //Debug.Log(selectedChar.name + " selected. Removing from cohort list.");

                        cohortList.Remove(selectedChar);
                        //Debug.Log("Remaining In Cohort List");

                        //foreach (Character c in cohort.Value)
                        //    Debug.Log(c.name);
                        combatOrder.Add(selectedChar);
                    }
                }
                //Debug.Log("<><><><>");
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
        //Debug.Log("team size: " + team.characters.Count);
        foreach(Character character in team.characters)
        {
            //Debug.Log(character.name + "'s HP: " + character.Health.Value);
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
        //if (roll <= character.HitChance)
        //{
            float damage = weapon.weaponDamage + character.Damage.Value;
            roll = Random.Range(1, 101);

            //Character crits
            if (roll <= character.CritChance)
            {
                CombatLog.Log("<color=yellow>" + character.characterName + "'s attack was a crit!</color>");
                damage *= Character.criticalDamageModifier;
            }
            //Debug.Log(target.name + " resisted " + target.DamageResistance + " damage.");

            //Apply armor resistance to damage value
            damage -= damage * target.DamageResistance;

            CombatLog.Log(character.characterName + "'s attack hit " + target.characterName + "for <color=red>" + damage + "</color>damage!");

            //Apply damage to target
            target.Health.AddModifier(new StatModifier(-damage, StatModType.Flat));

            //If Target is dead remove from team and combatants
            if (target.Health.Value <= 0)
            {
                CombatLog.Log(target.characterName + " Died!");
                Team team = target.allegiance == Character.Allegiance.Player ? playerTeam : enemyTeam;
                team.characters.Remove(target);
            }
            else
                CombatLog.Log(target.characterName + "'s health is " + target.Health.Value);
        //}
        //else CombatLog.Log(character.characterName + "'s attack missed!");

        roll = Random.Range(1, 101);

        if (!extraAttack && roll <= character.ExtraAttackChance)
        {
            extraAttack = true;
            CombatLog.Log("<color=blue>" + character.characterName + " attacks again!</color>");
            DealDamage(character, weapon, target, playerTeam, enemyTeam);
        }
        else if (extraAttack)
            extraAttack = false;
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

            CombatLog.Log("Target: " + lowestCharacter.characterName);

            //Roll for hit
            int roll = Random.Range(1, 101);

            ////Character hits
            //if (roll <= character.HitChance)
            //{
                float healAmount = weapon.weaponDamage + character.Healing.Value;

                roll = Random.Range(1, 101);

                //Character crits
                if (roll <= character.CritChance)
                {
                    CombatLog.Log("<color=yellow>" + character.characterName + "'s heal was a crit!</color>");
                    healAmount *= Character.criticalDamageModifier;
                }

                //Figures out how much health the character is missing to avoid overhealing
                float missingHealth = lowestCharacter.Health.BaseValue - lowestCharacter.Health.Value;

                CombatLog.Log(character.characterName + " healed " + lowestCharacter.characterName + " for <color=green>" + missingHealth + "</color>");

                if (healAmount > missingHealth)
                    lowestCharacter.Health.AddModifier(new StatModifier(missingHealth, StatModType.Flat));
                else
                    lowestCharacter.Health.AddModifier(new StatModifier(healAmount, StatModType.Flat));
            //}
            //else
            //    CombatLog.Log(character.characterName + "'s heal missed!");

            healedTargets.Add(lowestCharacter);

            if (!extraAttack && roll <= character.ExtraAttackChance)
            {
                extraAttack = true;
                CombatLog.Log("<color=blue>" + character.characterName + " heals again!</color>");
                Heal(character, weapon, playerTeam, enemyTeam);
            }
            else if (extraAttack)
                extraAttack = false;
        }

        //If there were no friendlies healed then the character will deal damage instead using an unarmed weapon (It only uses the Power Stat to determine damage)
        if (healedTargets.Count <= 0)
        {
            Team team = character.allegiance == Character.Allegiance.Player ? enemyTeam : playerTeam;

            if (!CheckTeamStatus(team)) return;
            Character target = CollectionUtilities.GetRandomItem(team.characters);

            Weapon unarmedWeapon = new Weapon();
            unarmedWeapon.targetCount = 1;
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