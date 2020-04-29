using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    int playerLayer;
    int enemyLayer;
    int barrelLayer;

    public float explosionForce;
    public float explosiveRadius;

    public Transform particleLocation;

    public GameObject destroyedBarrel;

    public GameObject explosion;

    // Start is called before the first frame update
    void Start()
    {
        playerLayer = LayerMask.NameToLayer("Player");
        enemyLayer = LayerMask.NameToLayer("Enemy");
        barrelLayer = LayerMask.NameToLayer("Barrel");
    }

    // Update is called once per frame
    void Update()
    {

    }

     void Explode()
    {
        Instantiate(destroyedBarrel, transform.position, transform.rotation);
        Instantiate(explosion, particleLocation.position, transform.rotation);

        Collider[] colliders = Physics.OverlapSphere(transform.position, explosiveRadius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            //Add force to nearby rigidbodies
            if (rb != null)
                rb.AddExplosionForce(explosionForce, transform.position, explosiveRadius);

            
            if (hit.gameObject.layer == barrelLayer)
           {
               hit.gameObject.GetComponent<Barrel>().StartCoroutine("Explodo");
            }

            //If the explosion hit the tag "Target"
            if (hit.gameObject.layer == enemyLayer)
            {
                hit.gameObject.GetComponentInParent<EnemyAbstract>().Damage(100);
            }

        }
            Destroy(gameObject);
    }

    public IEnumerator Explodo()
    {
        yield return new WaitForSeconds(0.1f);
        Explode();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, explosiveRadius);
    }
}