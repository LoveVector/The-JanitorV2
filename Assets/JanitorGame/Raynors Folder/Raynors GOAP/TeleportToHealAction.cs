using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportToHealAction : ActionGoap
{
    public override bool runAction(BossGOAP boss)
    {
        boss.anim.SetFloat("speed", 0);
        boss.anim.SetTrigger("Teleport");

        boss.StartCoroutine(boss.Teleport(boss.healPosition.position, 1f));
        return true;
    }
}
