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

        int maxDistributives = challengeRating > maxTeamSize ? maxTeamSize : challengeRating;
        List<int> challengeRatings = MathUtilities.GetRandomDistribution(challengeRating, maxDistributives, 1,20, 20);

        int teamSize = challengeRatings.Count;

        playerTeam = new Team();
        for (int i = 0; i < teamSize; i++)
        {
            Character player = characterGenerator.GenerateCharacter(Character.Allegiance.Player, challengeRatings[i]);
            player.characterName = "Player" + (i + 1);
            
            playerTeam.characters.Add(player);
            playerTeamChars.Add(player);
        }

        List<int> enemyChallengeRatings = MathUtilities.GetRandomDistribution(challengeRating, maxDistributives, 1, 20, 20);
        int enemyTeamSize = enemyChallengeRatings.Count;

        enemyTeam = new Team();
        for (int i = 0; i < enemyTeamSize; i++)
        {
            Character enemy = characterGenerator.GenerateCharacter(Character.Allegiance.Enemy, enemyChallengeRatings[i]);
            enemy.characterName = "Enemy" + (i + 1);
            enemyTeam.characters.Add(enemy);
            enemyTeamChars.Add(enemy);
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