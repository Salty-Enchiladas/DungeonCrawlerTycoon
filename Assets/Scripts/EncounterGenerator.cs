using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using UnityEditor;

public class EncounterGenerator : MonoBehaviour
{
    public CharacterGenerator characterGenerator;
    public int teamSize = 2;

    Team playerTeam;
    Team enemyTeam;

    Coroutine combat;

    public void GenerateEncounter()
    {
        if(combat != null)
        {
            StopCoroutine(combat);
            ClearConsole();
        }

        if(playerTeam != null)
        {
            foreach (Character c in playerTeam.characters)
            {
                for (int i = 0; i < c.armor.Count; i++)
                    Destroy(c.armor[i].inventoryItem.gameObject);

                if (c.primaryWeapon != null && c.primaryWeapon.inventoryItem)
                    Destroy(c.primaryWeapon.inventoryItem.gameObject);

                if (c.secondaryWeapon != null && c.secondaryWeapon.inventoryItem)
                    Destroy(c.secondaryWeapon.inventoryItem.gameObject);

                Destroy(c.gameObject);
            }
        }

        if(enemyTeam != null)
        {
            foreach (Character c in enemyTeam.characters)
            {
                for (int i = 0; i < c.armor.Count; i++)
                    Destroy(c.armor[i].inventoryItem.gameObject);

                if (c.primaryWeapon != null && c.primaryWeapon.inventoryItem)
                    Destroy(c.primaryWeapon.inventoryItem.gameObject);

                if (c.secondaryWeapon != null && c.secondaryWeapon.inventoryItem)
                    Destroy(c.secondaryWeapon.inventoryItem.gameObject);

                Destroy(c.gameObject);
            }
        }

        playerTeam = new Team();
        for(int i = 0; i < teamSize; i++)
        {
            Character player = characterGenerator.GenerateCharacter();
            player.name = "Player" + i;
            Debug.Log(player.name + ": " + player.Power.Value + " | " + player.Accuracy.Value + " | " + player.Constitution.Value + " | " + player.Speed.Value + " | " + player.Luck.Value);
            player.allegiance = Character.Allegiance.Player;
            playerTeam.characters.Add(player);
        }

        enemyTeam = new Team();
        for (int i = 0; i < teamSize; i++)
        {
            Character enemy = characterGenerator.GenerateCharacter();
            enemy.name = "Enemy" + i;
            Debug.Log(enemy.name + ": " + enemy.Power.Value + " | " + enemy.Accuracy.Value + " | " + enemy.Constitution.Value + " | " + enemy.Speed.Value + " | " + enemy.Luck.Value);
            enemy.allegiance = Character.Allegiance.Enemy;
            enemyTeam.characters.Add(enemy);
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