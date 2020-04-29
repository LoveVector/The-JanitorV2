using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Submachine : Guns
{
    float recoilY = 0;
    // Start is called before the first frame update
    void Start()
    {
        enemyLayer = LayerMask.NameToLayer("Enemy");
        anim = GetComponent<Animator>();
        ammo = beginningAmmo;

    }

    // Update is called once per frame
    void Update()
    {
        Fire();
    }

    public override void Fire()
    {
        Vector3 startPoint = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));

        if(Input.GetMouseButton(0) && ammo > 0)
        {
            anim.SetBool("Fire1", true);
            if (Time.time >= lastShot)
            {
                ammo--;
                lastShot = Time.time + fireRate;

                GameObject newBull = Instantiate(bullet, firePoint.transform.position, firePoint.transform.rotation);
                bulletScript = newBull.GetComponent<BulletScript>();
                bulletScript.damage = damage;

                RaycastHit hit;
                if (Physics.Raycast(startPoint, cam.transform.forward, out hit, range))
                {
                   // bulletScript.target = hit.point;
                   // bulletScript.hit = hit;
                }
                else
                {
                   // bulletScript.target = startPoint + cam.transform.forward * range;
                }
                recoilY += 0.4f;
            }
        }
        else
        {
            recoilY = 0;
            anim.SetBool("Fire1", false);
        }
    }
}
