using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TofuMeleeEnemyAbstract : MonoBehaviour
{
    public int health;
    public int attackDamage;
    public int bulletLayer;

    public float attackRate;
    public float runSpeed;
    public float lastAttack;
    public float MAX_VELOCITY = 3;
    public float MAX_FORCE = 5.4f;
    float rotSpeed = 2.0f;
    float MAX_SEPARATION = 1.0f;
    float SEPARATION_RADIUS = 2.5f;

    public Vector3 desired;
    public Vector3 steering;
    public Vector3 velocity;

    public float speed;

    public RaycastHit hit;

    public bool dead = false;

    public Animator anim;

    public GameObject player;

    public LevelManager level;

    public Rigidbody rb;

    public TofuMeleeEnemyAbstract[] neighbors;

    public Vector3 targetposition;

   
    void Start()
    {
        bulletLayer = LayerMask.NameToLayer("PlayerBullet"); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void Damage(int damage)
    {
        Debug.Log("Damaged");
        health -= damage;
    }

    protected void getNeighbors()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Enemy");

        neighbors = new TofuMeleeEnemyAbstract[objs.Length];

        for (int i = 0; i < objs.Length; i++)
        {
            neighbors[i] = objs[i].GetComponent<TofuMeleeEnemyAbstract>();
        }
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

    protected void TurnToTarget(Vector3 t)
    {
        Quaternion tarRot = Quaternion.LookRotation(t - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, tarRot, rotSpeed * Time.deltaTime);
    }

    protected Vector3 separation()
    {
        Vector3 force = Vector3.zero;
        int neighborCount = 0;
        for (int i = 0; i < neighbors.Length; i++)
        {
            TofuMeleeEnemyAbstract b = neighbors[i];
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
}
