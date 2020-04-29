using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InjuredRunNode : Node
{
    public override int UpdateNode(EnemyAbstract con)
    {
        if (con.health <= 50)
        {
            //con.speed = con.injuredSpeed;
            con.anim.SetFloat("Blend", 1);
            return 2;
        }
        else
        {
            return 0;
        }
    }
}