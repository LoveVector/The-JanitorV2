using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurrenderCheckNode : Node
{
    public override int UpdateNode(EnemyAbstract con)
    {
        if (con.health <= 25)
        {
            return 2;
        }
        else
        {
            return 0;
        }
    }
}
