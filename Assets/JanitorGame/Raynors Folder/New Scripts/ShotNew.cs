using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotNew : GunsNew
{
   public ParticleSystem muzzleParticles;

    Quaternion rotation;

    List<Vector3> dir;

    public float angle;
    public int bulletAmount;
    // Start is called before the first frame update
    void Start()
    {
        dir = new List<Vector3>();
        barrelLayer = LayerMask.NameToLayer("Barrel");
        rec = player.GetComponent<CameraLook>();
        move = player.GetComponent<PlayerMovement>();
        enemyLayer = LayerMask.NameToLayer("Enemy");
        anim = GetComponent<Animator>();
        if(LevelManager.Instance.currentScene != 1)
        {
            ammoCap = GameManager.Instance.shottyAmmo;
        }
        else
        {
            ammoCap = 5;
        }
        beginningAmmo = 2;
        ammo = beginningAmmo;
        audioo = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Knife();
        Movement();
        AnimationCheck();
        Fire();

        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }

        LevelManager.Instance.ammoText.text = ammo + " / " + ammoCap;
        LevelManager.Instance.shottyAmmo = ammoCap;
    }

    public override void Fire()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= lastShot && ammo > 0 && !isReloading && !isHolster & !isDraw && !move.sprint)
        {
            audioo.Play();
            lastShot = Time.time + fireRate;
            rec.AddRecoil(0, 2);
            startPoint = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
            ammo--;
            anim.SetTrigger("fire");

           muzzleParticles.Emit(10);

            for (int i = 0; i < bulletAmount; i++)
            {
                Vector3 direction;
                direction.x = Random.Range(-angle, angle);
                direction.y = Random.Range(-angle, angle);
                direction.z = Random.Range(-angle, angle);

                GameObject bullet1 = Instantiate(bullet, bulletLocation.transform.position, bulletLocation.transform.rotation);
                bullet1.transform.Rotate(direction);
                bullet1.GetComponent<Rigidbody>().velocity = bullet1.transform.forward * 500;

                RaycastHit hit;

                Debug.DrawRay(startPoint, bullet1.transform.forward);
                dir.Add(bullet1.transform.forward);

                if (Physics.Raycast(startPoint, bullet1.transform.forward, out hit, range))
                {
                    Debug.Log("haha");
                    if (hit.collider.gameObject.layer == enemyLayer)
                    {
                        if (hit.collider.gameObject.GetComponentInParent<EnemyAbstract>() != null)
                        {
                            EnemyAbstract basic = hit.collider.gameObject.GetComponentInParent<EnemyAbstract>();
                            basic.hit = hit;
                            basic.Damage(damage);
                        }
                        else
                        {
                            BossGOAP basic = hit.collider.gameObject.GetComponentInParent<BossGOAP>();
                            basic.Damage(damage);
                        }
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
}
