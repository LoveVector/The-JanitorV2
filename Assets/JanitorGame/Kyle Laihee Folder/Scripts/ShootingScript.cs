using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingScript : MonoBehaviour
{
    public float speed;
    public float bulletLifeSpan;
    public float updatePlayerPos;
    Vector3 generalPlayerPosition;

    public const float generalPositionRadius = 2f;

    private Transform player;

    private Vector3 target;

    // ------------- Seek ----------------
    private static Vector3 velocity;
    private Vector3 desiredVelocity;
    private Vector3 steering;

    public float maxSpeed;
    public float maxForce;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        //generalPlayerPosition = Random.insideUnitSphere * generalPositionRadius + player.position;
        target = new Vector3(player.position.x, player.position.y, player.position.z);
    }

    void Update()
    {
        //transform.position = Vector3.MoveTowards(transform.position, generalPlayerPosition, speed * Time.deltaTime);   
        SeekVelocity();

        if (bulletLifeSpan <= 0)
        {
            DestroyBullet();
        }
        else
        {
            bulletLifeSpan -= Time.deltaTime;
        }

        if (transform.position.x == target.x && transform.position.y == target.y)
        {
            DestroyBullet();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DestroyBullet();
        }
    }

    void SeekVelocity()
    {
        desiredVelocity = target - transform.position;
        steering = desiredVelocity - velocity;
        steering = Vector3.ClampMagnitude(steering, maxForce);

        velocity = Vector3.ClampMagnitude(velocity + steering, maxSpeed);
        transform.position += velocity * speed * Time.deltaTime;
        transform.forward = velocity;
    }

    void DestroyBullet()
    {
        Destroy(gameObject);
    }
}
