using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOIDS : MonoBehaviour
{
    public float MAX_FORCE = 5.4f;
    public float MAX_VELOCITY = 3;

    // Leader sight evasion
    public float LEADER_BEHIND_DIST = 3;
    // Separation
    float MAX_SEPARATION = 1.0f;
    float SEPARATION_RADIUS = 2.5f;

    public GameObject Target;

    float rotSpeed = 2.0f;

    public Vector3 velocity;
    public Vector3 desired;
    public Vector3 ahead;
    public Vector3 behind;
    public Vector3 steering;

    public bool leader;

    public BOIDS[] neighbors;

    protected Rigidbody rbody;

    protected void getNeighbors()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Enemy");

        neighbors = new BOIDS[objs.Length];

        for (int i = 0; i < objs.Length; i++)
        {
            neighbors[i] = objs[i].GetComponent<BOIDS>();
        }
    }

    protected Vector3 seek(Vector3 target)
    {
        Vector3 force = Vector3.zero;
        desired = target - transform.position;
        desired.Normalize();
        desired *= MAX_VELOCITY;
        force = desired - velocity;
        return force;
    }

    protected Vector3 flee(Vector3 target)
    {
        Vector3 force = Vector3.zero;
        desired = transform.position - target;
        desired.Normalize();
        desired *= MAX_VELOCITY;
        force = desired - velocity;
        return force;
    }

    protected Vector3 separation()
    {

        Vector3 force = Vector3.zero;
        int neighborCount = 0;
        for (int i = 0; i < neighbors.Length; i++)
        {
            BOIDS b = neighbors[i];
            if (b != this && distance(b.transform.position, this.transform.position) <= SEPARATION_RADIUS)
            {
                force.x += b.transform.position.x - this.transform.position.x;
                force.z += b.transform.position.z - this.transform.position.z;
                neighborCount++;
            }
        }

        if (neighborCount != 0)
        {
            force.x /= neighborCount;
            force.z /= neighborCount;
            force *= -1;
        }

        force.Normalize();
        force *= MAX_SEPARATION;
        return force;

    }

     protected float distance(Vector3 a, Vector3 b)
    {
        return Mathf.Sqrt((a.x - b.x) * (a.x - b.x) + (a.z - b.z) * (a.z - b.z));
    }

    protected Vector3 arrive(Vector3 target, float slowingRadius)
    {
        Vector3 force = Vector3.zero;
        float distance;

        desired = target - transform.position;

        distance = desired.magnitude;
        desired.Normalize();

        if (distance <= slowingRadius)
        {
            desired *= MAX_VELOCITY * (distance / slowingRadius);
        }
        else
        {
            desired *= MAX_VELOCITY;
        }

        force = desired - velocity;

        return force;
    }

    protected Vector3 evade(BOIDS target)
    {
        Vector3 distance = target.transform.position - transform.position;
        float updatesNeeded = distance.magnitude / MAX_VELOCITY;
        Vector3 tv = new Vector3(target.velocity.x, target.velocity.y, target.velocity.z);
        tv *= updatesNeeded;
        Vector3 targetFuturePosition = new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z);
        targetFuturePosition += tv;
        return flee(targetFuturePosition);
    }

    protected Vector3 followLeader(BOIDS leader)
    {
        Vector3 tv = new Vector3(leader.velocity.x, leader.velocity.y, leader.velocity.z);
        Vector3 force = Vector3.zero;
        tv.Normalize();
        tv *= LEADER_BEHIND_DIST;
        ahead = leader.transform.position + tv;
        tv *= -1;
        behind = leader.transform.position + tv;

        if (distance(leader.transform.position, this.transform.position) <= LEADER_BEHIND_DIST)
        {
            force += (evade(leader));
            force *= 0.5f;

        }
        else
        {
            force += (arrive(behind, LEADER_BEHIND_DIST));
        }

        force += separation();

        //Vector3 Offset from leader = x * transform.Right + y * transform.Forward

        //Offset + position of leader = target position
        Debug.DrawLine(leader.transform.position, behind);
        return force;
    }

    protected void TurnToTarget(Vector3 t)
    {
        Quaternion tarRot = Quaternion.LookRotation(t - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, tarRot, rotSpeed * Time.deltaTime);
    }
}
