using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Guns : MonoBehaviour
{
    public int damage;
    protected int ammo;
    public int ammoCap;
    public int beginningAmmo;

    public float range;
    public float fireRate;
    protected float lastShot = 0;

    protected bool isReloading = false;
    protected bool isHolster = false;

    protected Animator anim;

    public GameObject firePoint;
    public GameObject bullet;

    protected BulletScript bulletScript;

    protected int enemyLayer;
    public Camera cam;

    public virtual void Fire()
    { 
        Vector3 startPoint = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));

        if (Input.GetMouseButtonDown(0) && Time.time >= lastShot && ammo > 0)
        {
            ammo--;
            anim.SetTrigger("Fire");
            lastShot = Time.time + fireRate;

          // GameObject newBull = Instantiate(bullet, firePoint.transform.position, firePoint.transform.rotation);
          // bulletScript = newBull.GetComponent<BulletScript>();
          // bulletScript.damage = damage;

            RaycastHit hit;

            if(Physics.Raycast(startPoint, cam.transform.forward, out hit, range))
            {
                if (hit.collider.gameObject.layer == enemyLayer)
                {
                    Debug.Log("Enemydet");
                    EnemyAbstract basic = hit.collider.gameObject.GetComponentInParent<EnemyAbstract>();
                    basic.hit = hit;
                    basic.Damage(damage);
                }
                else
                {
                  if(hit.rigidbody != null)
                  {
                      hit.rigidbody.AddForce(-hit.normal * 10);
                  }
                }
            }
          // if (Physics.Raycast(startPoint, cam.transform.forward, out hit, range))
          // {
          //     Debug.Log("Hitpoint");
          //     bulletScript.target = hit.point;
          //     bulletScript.hit = hit;
          // }
          // else
          // {
          //     bulletScript.target = startPoint + cam.transform.forward * range; 
          // }
        }
    }

    public virtual void Reload()
    {
        if (ammo == 0 && ammoCap > 0)
        {
            if(ammoCap >= beginningAmmo)
            {
                ammo = beginningAmmo;
                ammoCap -= beginningAmmo;
            }
            else
            {
                ammo = ammoCap;
                ammoCap -= ammoCap;
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
    }
}