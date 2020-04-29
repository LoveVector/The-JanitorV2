using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAbstract : MonoBehaviour
{
    public int health;
    public int attackDamage;
    public int bulletLayer;

    public float attackRate;
    public float runSpeed;
    public float lastAttack;

    public float speed;

    public RaycastHit hit;

    public bool dead = false;

    public Animator anim;

    public GameObject player;

    public LevelManager level;

    public Rigidbody rb;

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
}
