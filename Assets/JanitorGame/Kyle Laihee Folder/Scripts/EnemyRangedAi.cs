using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangedAi : MonoBehaviour
{
    public float speed;
    public float stoppingDistance;
    public float retreatDistance;

    private float shootRate;
    public float startShootRate;

    public GameObject bulletPrefab;
    public Transform player;
    public Transform gun;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        shootRate = startShootRate;
    }

    // Update is called once per frame
    void Update()
    {
        EnemyMovement();
    }

    void EnemyMovement()
    {
        transform.LookAt(2 * transform.position - player.position); // look towards player

        if (Vector3.Distance(transform.position, player.position) > stoppingDistance) // Stop to shoot
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
        else if (Vector3.Distance(transform.position, player.position) < stoppingDistance && Vector3.Distance
            (transform.position, player.position) > retreatDistance) // Close enough, SHoot
        {
            transform.position = this.transform.position;
            EnemyShooting();
        }
        else if (Vector3.Distance(transform.position, player.position) < retreatDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, -speed * Time.deltaTime);
        }
    }

    void EnemyShooting()
    {
        if(shootRate <= 0)
        {
            Instantiate(bulletPrefab, gun.position, Quaternion.identity);
            shootRate = startShootRate;
        }
        else
        {
            shootRate -= Time.deltaTime;
        }
    }
}
