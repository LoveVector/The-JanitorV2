using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallBackupAction : ActionGoap
{
    public override bool runAction(BossGOAP boss)
    {
        LevelManager.Instance.bossWave = true;
        boss.anim.SetTrigger("CallBackup");
        boss.StartCoroutine("CallBackUpAction");
        return true;
    }
}