using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionGoap : MonoBehaviour
{
   public StatesCollection outcomeStates;
   public StatesCollection requiredStates;
    // Start is called before the first frame update
    public virtual bool runAction(BossGOAP boss)
    {
        return false;
    }
}
