using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootPlayerAction : ActionGoap
{
    public override bool runAction(BossGOAP boss)
    {
            boss.anim.SetFloat("speed", 0);
            boss.anim.SetTrigger("Shoot");
            boss.gun.SetActive(true);
            boss.StartCoroutine("ShootPlayer");
            return true;
    }
}