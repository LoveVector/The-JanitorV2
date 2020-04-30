using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackNode : Node
{
    public override int UpdateNode(EnemyAbstract con)
    {
        if (Time.time - con.lastAttack >= con.attackRate)
        {
            con.lastAttack = Time.time + con.attackRate;
            int attackType = Random.Range(0, 2);
            con.anim.SetFloat("AttackBlend", attackType);
            con.anim.SetTrigger("Attack");
            return 2;
        }
        else
        {
            return 0;
        }
    }
}
