using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : Node
{
    public override int UpdateNode(EnemyAbstract con)
    {
        for(int i = 0; i < nList.Count; i++)
        {
            state = nList[i].UpdateNode(con);

            switch (state)
            {
                case 1:
                    return 1;
                case 2:
                    return 2;
            }
        }
        return 0;
    }
}
