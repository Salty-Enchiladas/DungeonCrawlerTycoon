using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon : PointOfInterest
{
    public Team playerTeam;
    public Team enemyTeam;

    public void SimulateFight()
    {
        if(CombatManager.SimulateFight(playerTeam, enemyTeam))
        {
            print("Win!");
        }
        else
        {
            print("Lose!");
        }
    }
}
