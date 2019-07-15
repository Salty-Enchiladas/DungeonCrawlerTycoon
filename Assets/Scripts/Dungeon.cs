using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon : PointOfInterest
{
    public Team playerTeam;
    public Team enemyTeam;

    public void SimulateFight()
    {
        StartCoroutine(CombatManager.SimulateFight(playerTeam, enemyTeam));
    }
}
