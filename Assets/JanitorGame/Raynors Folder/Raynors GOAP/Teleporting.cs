using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporting : ActionGoap
{
    public override bool runAction(BossGOAP boss)
    {
        boss.anim.SetFloat("speed", 0);
        boss.anim.SetTrigger("Teleport");

        if (boss.selectedPath == boss.allWalkLocations.Count - 1)
        {
            boss.selectedPath = 0;
        }
        else
        {
            boss.selectedPath++;
        }
        boss.StartCoroutine(boss.Teleport(boss.allWalkLocations[boss.selectedPath][boss.selectedPath].position, 1f));
        boss.worldState.states.Clear();
        boss.worldState.states.Add("canMove");
        return true;
    }
}
