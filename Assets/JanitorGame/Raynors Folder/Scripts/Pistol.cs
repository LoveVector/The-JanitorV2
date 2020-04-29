using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Guns
{
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
        AnimationCheck();
        Fire();
        if (ammo == 0 || Input.GetKeyDown(KeyCode.R) && isReloading != true)
        {
            Reload();
        }
    }

    public override void Reload()
    {
      Debug.Log("Random");
      anim.SetTrigger("reload");
      ammo = beginningAmmo;
    }
}
