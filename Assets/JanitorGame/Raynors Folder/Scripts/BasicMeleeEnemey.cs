using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicMeleeEnemey : EnemyAbstract
{
    public GameObject weapon;
    enum state { chasing, attacking, dead}
    state enemyState;

    public GameObject model;

    List<Vector3> waypoints;

    bool check = false;

    bool deadForce;

    NavMeshAgent agent;

    public bool useAstar;
    // Start is called before the first frame update
    void Start()
    {
        enemyState = state.chasing;

        anim = GetComponent<Animator>();

        rb = model.GetComponent<Rigidbody>();

        deadForce = false;

        if (useAstar == false)
        {
            agent = GetComponent<NavMeshAgent>();
            agent.destination = player.transform.position;
        }
        else
        {
            StartCoroutine("GetPlayerDirection");
        }
    }

    // Update is called once per frame
    void Update()
    {
        HealthCheck();
        if (dead != true)
        {
            if (Vector3.Distance(transform.position, player.transform.position) >= 3)
            {
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
            case state.attacking:
                Attacking();
                break;
            case state.dead:
                anim.enabled = false;
                if (deadForce == false)
                {
                    rb.AddForce(-hit.normal * 500);
                    LevelManager.Instance.DeadEnemy();
                    deadForce = true;
                    if (!useAstar)
                    {
                        agent.velocity = Vector3.zero;
                    }
                    Destroy(this.gameObject, 5f);
                    Destroy(weapon);
                }
                break;
            default:
                break;
        }

        if (useAstar == false)
        {
            agent.SetDestination(player.transform.position);
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
        if (useAstar)
        {
            if (check == true)
            {
                StartCoroutine("GetPlayerDirection");
            }
            if (waypoints != null)
            {
                transform.position = Vector3.MoveTowards(this.transform.position, waypoints[0], speed * Time.deltaTime);
                transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));

                if (this.transform.position == waypoints[0])
                {
                    waypoints.RemoveAt(0);
                    if (waypoints.Count == 0)
                    {
                        waypoints = LevelManager.Instance.pathfinding.PathFind(this.transform.position, player.transform.position, LevelManager.Instance.grid);
                    }
                }
            }
        }
        else
        {
            agent.isStopped = false;
            transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
        }
    }

    void Attacking()
    {
        if (Time.time - lastAttack >= attackRate)
        {
            lastAttack = Time.time + attackRate;
            int attackType = Random.Range(0, 2);
            anim.SetFloat("AttackBlend", attackType);
            Debug.Log("attack");
            anim.SetTrigger("Attack");
        }
        agent.isStopped = true;
    }

    IEnumerator GetPlayerDirection()
    {
        check = false;
        yield return new WaitForSeconds(Random.Range(0.3f, 1f));
        check = true;
        waypoints = LevelManager.Instance.pathfinding.PathFind(this.transform.position, player.transform.position, LevelManager.Instance.grid);
    }
}