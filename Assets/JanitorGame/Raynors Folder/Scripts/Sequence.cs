using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : Node
{

    public override int UpdateNode(EnemyAbstract con)
    {
        for (int i = 0; i < nList.Count; i++)
        {
            state = nList[i].UpdateNode(con);

            switch (state)
            {
                case 0:
                    return 0;
                case 1:
                    return 1;
                case 2:

                    break;
                default:
                    break;
            }
        }
        return 2;

   
    }
}
