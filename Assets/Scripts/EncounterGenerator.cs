using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using UnityEditor;
using OuterRimStudios.Utilities;

public class EncounterGenerator : MonoBehaviour
{
    public CharacterGenerator characterGenerator;
    public int maxTeamSize;

    Team playerTeam;
    Team enemyTeam;

    List<Character> playerTeamChars = new List<Character>();
    List<Character> enemyTeamChars = new List<Character>();

    Coroutine combat;

    public void GenerateEncounter(int challengeRating)
    {
        if (combat != null)
        {
            StopCoroutine(combat);
            ClearConsole();
        }

        if (playerTeam != null)
        {
            foreach (Character character in playerTeamChars)
            {
                if (character)
                {
                    for (int i = 0; i < character.armor.Count; i++)
                        Destroy(character.armor[i].inventoryItem.gameObject);

                    for (int i = 0; i < character.weapons.Count; i++)
                        Destroy(character.weapons[i].inventoryItem.gameObject);

                    Destroy(character.gameObject);
                }
            }
        }

        if (enemyTeam != null)
        {
            foreach (Character c in enemyTeamChars)
            {
                if (c)
                {
                    for (int i = 0; i < c.armor.Count; i++)
                        Destroy(c.armor[i].inventoryItem.gameObject);

                    for (int i = 0; i < c.weapons.Count; i++)
                        Destroy(c.weapons[i].inventoryItem.gameObject);

                    Destroy(c.gameObject);
                }
            }
        }

        //What do we do if a character is CR1? Characters start with 5 stats so they can not be 1-5 stats

        if(challengeRating == 1)
        {
            Debug.Log("Player StatPoints: " + 5);
            Character player = characterGenerator.GenerateCharacter(Character.Allegiance.Player, 5);
            player.characterName = "Player";
            playerTeam.characters.Add(player);
            playerTeamChars.Add(player);

            Debug.Log("Enemy StatPoints: " + 5);
            Character enemy = characterGenerator.GenerateCharacter(Character.Allegiance.Enemy, 5);
            enemy.characterName = "Enemy";
            enemyTeam.characters.Add(enemy);
            enemyTeamChars.Add(enemy);
        }
        else if(challengeRating > 1)
        {
            int crMaxStats = challengeRating * 5; // *5 because the max of each Challenge Rating increases by 5.
            int crStatPoints = Random.Range(crMaxStats - 4, crMaxStats); //-4 because the range of a challenge rating is 4.

            int maxDistributives = challengeRating > maxTeamSize ? maxTeamSize : challengeRating;
            List<int> playerStatPoints = MathUtilities.GetRandomDistribution(crStatPoints, maxDistributives, 0, 20, 20);

            int teamSize = playerStatPoints.Count;

            playerTeam = new Team();
            for (int i = 0; i < teamSize; i++)
            {
                Debug.Log("Player" + (i + 1) + " StatPoints: " + playerStatPoints[i]);
                Character player = characterGenerator.GenerateCharacter(Character.Allegiance.Player, playerStatPoints[i]);
                player.characterName = "Player" + (i + 1);
                playerTeam.characters.Add(player);
                playerTeamChars.Add(player);
            }

            List<int> enemyStatPoints = MathUtilities.GetRandomDistribution(crStatPoints, maxDistributives, 0, 20, 20);
            int enemyTeamSize = enemyStatPoints.Count;

            enemyTeam = new Team();
            for (int i = 0; i < enemyTeamSize; i++)
            {
                Debug.Log("Enemy" + (i + 1) + " StatPoints: " + enemyStatPoints[i]);
                Character enemy = characterGenerator.GenerateCharacter(Character.Allegiance.Enemy, enemyStatPoints[i]);
                enemy.characterName = "Enemy" + (i + 1);
                enemyTeam.characters.Add(enemy);
                enemyTeamChars.Add(enemy);
            }
        }

        combat = StartCoroutine(CombatManager.SimulateFight(playerTeam, enemyTeam));
    }

    static void ClearConsole()
    {
        var assembly = Assembly.GetAssembly(typeof(SceneView));
        var type = assembly.GetType("UnityEditor.LogEntries");
        var method = type.GetMethod("Clear");
        method.Invoke(new object(), null);
    }
}