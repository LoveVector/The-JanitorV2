using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasePlayer : Node
{
    public override int UpdateNode(EnemyAbstract con)
    {
        if (Vector3.Distance(con.transform.position, con.player.transform.position) >= 2)
        {
            Vector3 distance = (con.player.transform.position - con.transform.position).normalized;
            con.transform.LookAt(new Vector3(con.player.transform.position.x, con.transform.position.y, con.player.transform.position.z));
            //distance.y = 0;
            con.transform.position += distance * con.speed * Time.deltaTime;
            return 1;
        }
        else
        {
            return 2;
        }
    }
}
