using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPlayerAction : ActionGoap
{
    public override bool runAction(BossGOAP boss)
    {
        if (Time.time - boss.lastTimeFired <= 0)
        {
            if(boss.check == false)
            {
                boss.StartCoroutine("GetPlayerDirection");
            }
                boss.anim.SetFloat("speed", 1);
            
            if (Vector3.Distance(boss.transform.position, boss.allWalkLocations[boss.selectedPath][boss.selectedLocation].position) <= 2)
            {
                if (boss.selectedLocation == boss.allWalkLocations[boss.selectedPath].Count - 1)
                {
                    boss.selectedLocation = 0;
                }
                else
                {
                    boss.selectedLocation++;
                }
            }
            if (boss.waypoints != null && boss.waypoints.Count != 0)
            {
                transform.position = Vector3.MoveTowards(boss.transform.position, boss.waypoints[0], boss.speed * Time.deltaTime);
                transform.LookAt(new Vector3(boss.waypoints[0].x, transform.position.y, boss.waypoints[0].z));

                if (this.transform.position == boss.waypoints[0])
                {
                    boss.waypoints.RemoveAt(0);
                }
            }
            return false;
        }
        else
        {
            return true;
        }
    }
}
