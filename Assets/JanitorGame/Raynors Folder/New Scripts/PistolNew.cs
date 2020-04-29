using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolNew : GunsNew
{
    public ParticleSystem muzzleParticles;
    public ParticleSystem sparkParticles;
    void Start()
    {
        barrelLayer = LayerMask.NameToLayer("Barrel");
        rec = player.GetComponent<CameraLook>();
        move = player.GetComponent<PlayerMovement>();
        enemyLayer = LayerMask.NameToLayer("Enemy");
        anim = GetComponent<Animator>();
        ammo = beginningAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        Knife();
        Movement();
        AnimationCheck();
        if (ammo == 0 || Input.GetKeyDown(KeyCode.R) && isReloading != true)
        {
            Reload();
        }
        Fire();
    }

    public override void Reload()
    {
        Debug.Log("Random");
        anim.SetTrigger("reload");
        ammo = beginningAmmo;
    }

    public override void Fire()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= lastShot && ammo > 0 && !isReloading && !isHolster & !isDraw && !move.sprint)
        {
            rec.AddRecoil(0, 0.5f);
            startPoint = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));

            ammo--;
            anim.SetTrigger("fire");
            muzzleParticles.Emit(Random.Range(2, 4));
            sparkParticles.Emit(Random.Range(1, 10));

           GameObject bullet1 = Instantiate(bullet, bulletLocation.transform.position, bulletLocation.transform.rotation);
            bullet1.GetComponent<Rigidbody>().velocity = bullet1.transform.forward * 500;

            lastShot = Time.time + fireRate;

            RaycastHit hit;

            if (Physics.Raycast(startPoint, cam.transform.forward, out hit, range))
            {
                if (hit.collider.gameObject.layer == enemyLayer)
                {
                    EnemyAbstract basic = hit.collider.gameObject.GetComponentInParent<EnemyAbstract>();
                    basic.hit = hit;
                    basic.Damage(damage);
                }
                else if (hit.collider.gameObject.layer == barrelLayer)
                {
                    
                    hit.collider.gameObject.GetComponent<Barrel>().StartCoroutine("Explodo");
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
}
