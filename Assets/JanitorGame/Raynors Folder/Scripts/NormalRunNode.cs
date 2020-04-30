using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalRunNode : Node
{
    public override int UpdateNode(EnemyAbstract con)
    {
        con.speed = con.runSpeed;
        con.anim.SetFloat("Blend", 0);
        return 2;
    }
}