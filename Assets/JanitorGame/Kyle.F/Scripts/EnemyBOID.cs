using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBOID : BOIDS
{
    public GameObject Leader;
    public GameObject[] Neighbours;

    void Start()
    {
        rbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        getNeighbors();
        GetAllNeighbours();

        steering *= 0;

        if (leader)
        {
            steering = steering + arrive(Target.transform.position, 3);
            TurnToTarget(Target.transform.position);
        }
        else
        {
           if (Leader)
           {
                BOIDS l = Leader.GetComponent<BOIDS>(); ;
                //steering = steering + followLeader(l);
                //Formation(l);
                steering = steering + Formation(l);
                TurnToTarget(l.transform.position);
           }
        }

        Vector3.ClampMagnitude(steering, MAX_FORCE);
        steering *= (1 / rbody.mass);
        velocity = velocity + steering;
        rbody.velocity = velocity;
    }

    Vector3 Formation(BOIDS leader)
    {
        int counter = -1;
        int zoffset = -1;
        float xoffset = 0.25f;

        Vector3 tv = new Vector3(leader.velocity.x, leader.velocity.y, leader.velocity.z);
        Vector3 force = Vector3.zero;
        tv.Normalize();
        tv *= LEADER_BEHIND_DIST;
        Vector3 ahead = leader.transform.position + tv;
        tv *= -1;
        Vector3 behind = leader.transform.position + tv;

        Vector3 targetposition = behind;

        float perline = 2;

        float startz = targetposition.z;

        if (distance(leader.transform.position, this.transform.position) <= LEADER_BEHIND_DIST)
        {
            force += (evade(leader));
            force *= 0.5f;

        }
        else
        {
            for (int i = 0; i < neighbors.Length; i++)
            {
                counter++;
                zoffset++;

                if (zoffset > 1)
                {
                    zoffset = 1;
                }

                targetposition = new Vector3(targetposition.x, targetposition.y, targetposition.z + (zoffset * 2.0f));

                //targetposition.z = targetposition.z + (zoffset * 2.0f);

                if (counter == Mathf.Floor(perline))
                {
                    counter = 0;
                    targetposition.z = startz; //-= 1 + 0.25f;
                    targetposition.x -= 1 + xoffset; //startx; 
                }

               // Neighbours[i].transform.position = Vector3.MoveTowards(Neighbours[i].transform.position, targetposition, 2);
                                                                   //steering = steering + arrive(behind, 5);
                                                                   force += arrive(targetposition, LEADER_BEHIND_DIST);
            }
        }
        Debug.Log(targetposition);
        Debug.DrawLine(leader.transform.position, targetposition, Color.red);
        Debug.DrawLine(leader.transform.position, behind, Color.yellow);

        return force;
    }

    void GetAllNeighbours()
    {
        GameObject[] ob = GameObject.FindGameObjectsWithTag("Enemy");

        Neighbours = new GameObject[ob.Length];

        for (int i = 0; i < ob.Length; i++)
        {
            Neighbours[i] = ob[i];
        }

        //Leader = Neighbours[0];
    }

    Vector3 FollowLeader(BOIDS leader)
    {
        Vector3 tv = new Vector3(leader.velocity.x, leader.velocity.y, leader.velocity.z);
        Vector3 force = Vector3.zero;
        tv.Normalize();
        tv *= LEADER_BEHIND_DIST;
        Vector3 ahead = leader.transform.position + tv;
        tv *= -1;
        Vector3 behind = leader.transform.position + tv;

        int counter = -1;
        int zoffset = -1;
        float xoffset = 0.25f;

        Vector3 targetposition = behind;

        float perline = 2;

        float startz = targetposition.z;

        if (distance(leader.transform.position, this.transform.position) <= LEADER_BEHIND_DIST)
        {
            force += (evade(leader));
            force *= 0.5f;

        }
        else
        {
            //force += (arrive(behind, LEADER_BEHIND_DIST));
            for (int i = 0; i < neighbors.Length; i++)
            {
                counter++;
                zoffset++;

                if (zoffset > 1)
                {
                    zoffset = 1;
                }

                //targetpositon = new Vector3(targetpositon.x + (xoffset * 2.0f), targetpositon.y, targetpositon.z);

                targetposition.z = targetposition.z + (zoffset * 2.0f);

                if (counter == Mathf.Floor(perline))
                {
                    counter = 0;
                    targetposition.z = startz; //-= 1 + 0.25f;
                    targetposition.x -= 1 + xoffset; //startx; 
                }

                Debug.Log(targetposition);
                force += arrive(targetposition, LEADER_BEHIND_DIST);
            }
        }

        force += separation();

        //Vector3 Offset from leader = x * transform.Right + y * transform.Forward

        //Offset + position of leader = target position
        //  Vector3.ClampMagnitude(velocity, (!leader ? MAX_VELOCITY * (0.3f + Random.Range(0.1f, 0.5f)) : MAX_VELOCITY));

        return force;
    }
}
