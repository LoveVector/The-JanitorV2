using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathNode : Node
{
    public override int UpdateNode(EnemyAbstract con)
    {
        if (con.dead == false)
        {
            Debug.Log("Dead");
            con.dead = true;
            con.anim.SetTrigger("Dead");
            return 2;
        }
        else
        {
            return 2;
        }
    }
}