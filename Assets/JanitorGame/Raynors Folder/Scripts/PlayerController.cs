using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Camera cam;
    public GameObject hand;

    public Rigidbody throwableObj;
    public Transform target;
    public float height;
    public float gravity;

    Animator handAnim;
    Weapon myWeapon;

    float attackTimer;
    // Start is called before the first frame update
    void Start()
    {
        handAnim = hand.GetComponentInChildren<Animator>();
        myWeapon = hand.GetComponentInChildren<Weapon>();
        throwableObj.useGravity = false;
    }

    // Update is called once per frame
    void Update()
    {
        AttackMelee();
        if(Input.GetKeyDown(KeyCode.R))
        {
            Throw();
        }

    }

    void AttackRayCast()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, myWeapon.attackRange))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                Debug.Log("EnemyHitWithMelee");
            }
        }

    }

    void AttackMelee()
    {
        attackTimer += Time.deltaTime;
        if(Input.GetMouseButtonDown(0) && attackTimer >= myWeapon.attackCoolDown)
        {
            handAnim.Play("AttackMeleeHammer");
        }
        if(Input.GetMouseButtonUp(0) && attackTimer >= myWeapon.attackCoolDown)
        {
            attackTimer = 0f;
            AttackRayCast();
        }
    }

    void Throw()
    {
        Debug.Log("ObjectThrown");
        Physics.gravity = Vector3.up * gravity;
        throwableObj.useGravity = true;
        throwableObj.velocity = CalculateLaunchVelocity();
    }

    Vector3 CalculateLaunchVelocity()
    {
        float displacementY = target.position.y - throwableObj.position.y;
        Vector3 displacementXZ = new Vector3(target.position.x - throwableObj.position.x, 0, target.position.z - throwableObj.position.z);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * height);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * height / gravity) + Mathf.Sqrt(2 * (displacementY - height) / gravity));

        return velocityXZ + velocityY;
    }
}
