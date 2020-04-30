using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingAction : ActionGoap
{
    public override bool runAction(BossGOAP boss)
    {
        if (boss.currentHealth >= boss.maxHealth || boss.damageTaken >= 100)
        {
            Debug.Log("damaged");
            if (boss.currentHealth >= boss.maxHealth)
            {
                boss.currentHealth = boss.maxHealth;
            }
            boss.StartCoroutine(boss.Teleport(boss.allWalkLocations[boss.selectedPath][boss.selectedPath].position, 1f));
            boss.worldState.states.Clear();
            boss.worldState.states.Add("canMove");
            return true;
        }
        else
        {
            Debug.Log(boss.damageTaken);
            Debug.Log("Healing");
            boss.Damage(-50f * Time.deltaTime);
            return false;
        }
    }
}
