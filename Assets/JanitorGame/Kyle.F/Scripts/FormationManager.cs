using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationManager : MonoBehaviour
{
    //public BasicMeleeEnemey[] Tofus;
    public List<Tofu> Tofus;
    public Transform leader;

    // Start is called before the first frame update
    void Start()
    {
        Tofus = new List<Tofu>(FindObjectsOfType<Tofu>());
    }

    // Update is called once per frame
    void Update()
    {
       // getNeighbors();

        if (Tofus.Count > 3)
        {
            getTargetPosition();
        }
        
    }

    void getNeighbors()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Enemy");

        //Tofus = new BasicMeleeEnemey[objs.Length];

        for (int i = 0; i < objs.Length; i++)
        {
            //Tofus[i] = objs[i].GetComponent<BasicMeleeEnemey>();
        }
    }

    void getTargetPosition()
    {
       leader = Tofus[0].transform;

       for (int i = 1; i < Tofus.Count; i++)
       {
           Tofus[i].targetposition = leader.position + (1 + i) / 2 * -leader.forward;
           if (i % 2 == 0)
               Tofus[i].targetposition += leader.right;
           else
               Tofus[i].targetposition += -leader.right;
       }
    }
}
