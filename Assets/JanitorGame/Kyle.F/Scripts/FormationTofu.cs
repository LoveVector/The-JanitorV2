using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationTofu : MonoBehaviour
{

    TofuMeleeEnemyAbstract Tofus;
    public Transform leader;

    // Start is called before the first frame update
    void Start()
    {
        Tofus = FindObjectOfType<TofuMeleeEnemyAbstract>();
    }

    // Update is called once per frame
    void Update()
    {
            getTargetPosition();
    }

    void getTargetPosition()
    {
        leader = Tofus.neighbors[0].transform;
        Tofus.neighbors[0].targetposition = Tofus.player.transform.position;
        for (int i = 1; i < Tofus.neighbors.Length; i++)
        {
            Tofus.neighbors[i].targetposition = leader.position + (1 + i) / 2 * -leader.forward;
            if (i % 2 == 0)
                Tofus.neighbors[i].targetposition += leader.right;
            else
                Tofus.neighbors[i].targetposition += -leader.right;
        }
    }
}
