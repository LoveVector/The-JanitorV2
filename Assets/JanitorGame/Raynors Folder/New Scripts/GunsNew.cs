using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GunsNew : MonoBehaviour
{
    public int damage;
    protected int ammo;
    public int ammoCap;
    public int beginningAmmo;

    public float range;
    public float fireRate;
    protected float lastShot = 0;

    public float meleeRate;
    float lastMeleeTime;

    public float holLength;

    protected bool isReloading = false;
    public bool isHolster = false;
    protected bool isDraw = false;

    public Animator anim;

    public Transform bulletLocation;

    public GameObject firePoint;
    public GameObject bullet;
    public GameObject player;

    protected PlayerMovement move;
    protected CameraLook rec;

    protected int enemyLayer;
    protected int barrelLayer;
    public Camera cam;

    protected Vector3 startPoint;

    public virtual void Fire()
    {
        startPoint = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));

        Debug.DrawRay(startPoint, cam.transform.forward, Color.red);

        if (Input.GetMouseButtonDown(0) && Time.time >= lastShot && ammo > 0)
        {
            ammo--;
            anim.SetTrigger("Fire");
            lastShot = Time.time + fireRate;

            RaycastHit hit;

            if (Physics.Raycast(startPoint, cam.transform.forward, out hit, range))
            {
                Debug.Log("haha");
                if (hit.collider.gameObject.layer == enemyLayer)
                {
                    EnemyAbstract basic = hit.collider.gameObject.GetComponentInParent<EnemyAbstract>();
                    basic.hit = hit;
                    basic.Damage(damage);
                }
                else
                {
                    if (hit.rigidbody != null)
                    {
                        hit.rigidbody.AddForce(-hit.normal * 5);
                    }
                }
            }
        }
    }

    protected void Movement()
    {
        anim.SetFloat("speed", move.speed);
    }

    protected void Knife()
    {
        if (Time.time - lastMeleeTime >= meleeRate)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                int i = Random.Range(0, 2);
                anim.SetFloat("knifeFloat", i);
                anim.SetTrigger("knife");
                lastMeleeTime = Time.time;
            }
        }
    }

    public virtual void Reload()
    {
        if (ammo != beginningAmmo)
        {
            if (ammo == 0 && ammoCap > 0)
            {
                if (ammoCap >= beginningAmmo)
                {
                    ammo = beginningAmmo;
                    ammoCap -= beginningAmmo;
                }
                else
                {
                    ammo = ammoCap;
                    ammoCap -= ammoCap;
                }
                anim.SetTrigger("reload");
            }
            else if (ammo > 0 && ammoCap > 0)
            {
                int leftAmmo = beginningAmmo - ammo;
                if (ammoCap >= leftAmmo)
                {
                    ammo = ammo + leftAmmo;
                    ammoCap -= leftAmmo;
                }
                else
                {
                    ammo += ammoCap;
                    ammoCap -= ammoCap;
                }
                anim.SetTrigger("reload");
            }
        }
    }

    protected void AnimationCheck()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Reload"))
        {
            isReloading = true;
        }
        else
        {
            isReloading = false;
        }

        //Check if inspecting weapon
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Holster"))
        {
            isHolster = true;
        }
        else
        {
            isHolster = false;
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Take out"))
        {
            isDraw = true;
        }
        else
        {
            isDraw = false;
        }
    }
}
