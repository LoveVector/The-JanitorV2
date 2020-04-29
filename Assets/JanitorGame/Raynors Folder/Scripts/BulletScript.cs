using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float despawnTime;

    public Vector3 direction;

    public int damage;

    int enemyLayer;

    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        enemyLayer = LayerMask.NameToLayer("Enemy");
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(this.gameObject, 1f);
    }

   // private void OnTriggerEnter(Collider other)
   // {   
   //     Debug.Log("Coll");
   //     if(other.gameObject.layer == enemyLayer)
   //     {
   //         EnemyAbstract basic = other.gameObject.GetComponentInParent<EnemyAbstract>();
   //         //other.GetComponent<Rigidbody>().AddForce(10f);
   //         basic.Damage(damage);
   //     }
   //         Destroy(this.gameObject);
   // }
}
