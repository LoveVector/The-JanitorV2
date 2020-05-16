using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TofuMelee : TofuMeleeEnemyAbstract
{
    enum state { chasing, attacking, dead, leaderfollow}
    state enemyState;

    public GameObject model;

    List<Vector3> waypoints;

    bool move = false;

    public bool leader;

    bool deadForce;

    PlayerHealth pHealth;

    // Start is called before the first frame update
    void Start()
    {
        waypoints = new List<Vector3>();

        enemyState = state.chasing;

        anim = GetComponent<Animator>();

        rb = model.GetComponent<Rigidbody>();

        deadForce = false;

        pHealth = FindObjectOfType<PlayerHealth>();

        //StartCoroutine("GetPlayerDirection");

        if(player == null)
        {
            Debug.Log("player null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        HealthCheck();
        getNeighbors();
        if (dead != true)
        {
            if (Vector3.Distance(transform.position, player.transform.position) >= 1.5)
            {
                if (neighbors.Length > 3)
                {
                    enemyState = state.leaderfollow;
                }
                else
                    enemyState = state.chasing;
            }
            else
            {
                enemyState = state.attacking;
            }
        }
        else
        {
            enemyState = state.dead;
        }

        switch (enemyState)
        {
            case state.chasing:
                Chasing();
                break;
            case state.leaderfollow:
                LeaderFollow();
                break;
            case state.attacking:
                Attacking();
                break;
            case state.dead:
                anim.enabled = false;
                if (deadForce == false)
                {
                    rb.AddForce(-hit.normal * 500);
                    level.DeadEnemy();
                    deadForce = true;
                    Destroy(this.gameObject, 5f);
                }
                break;
            default:
                break;
        }
    }

    void HealthCheck()
    {
        if(health <= 0)
        {
            dead = true;
        }
        else
        {
            dead = false;
        }
    }

    void Chasing()
    {
        //if (move == true)
        //{
        //    //if (waypoints != null)
        //    //{
        //        //transform.position = Vector3.MoveTowards(this.transform.position, waypoints[0], speed * Time.deltaTime);
        //        //transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
        //
        //        //if (this.transform.position == waypoints[0])
        //        //{
        //            //waypoints.RemoveAt(0);
        //            //if (waypoints.Count == 0)
        //            //{
        //                //waypoints = Pathfinding.Instance.PathFind(this.transform.position, player.transform.position);
        //            //}
        //        //}
        //    //}
        //}

        this.transform.position = Vector3.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
        TurnToTarget(player.transform.position);

    }

    void LeaderFollow()
    {
        this.transform.position = Vector3.MoveTowards(this.transform.position, targetposition, speed * Time.deltaTime);
        TurnToTarget(player.transform.position);
        separation();
    }

    void Attacking()
    {
        if (Time.time - lastAttack >= attackRate)
        {
            lastAttack = Time.time + attackRate;
            anim.SetTrigger("Attack");
            pHealth.playerHealth -= attackDamage;
            Debug.Log("Attacking");
        }
    }

    IEnumerator GetPlayerDirection()
    {
        yield return new WaitForSeconds(Random.Range(0.1f, 0.6f));
        move = true;
        waypoints = Pathfinding.Instance.PathFind(this.transform.position, targetposition);
        StartCoroutine("GetPlayerDirection");
    }
}