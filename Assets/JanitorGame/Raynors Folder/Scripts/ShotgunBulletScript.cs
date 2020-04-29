using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunBulletScript : MonoBehaviour
{
    public float speed;
    public float despawnTime;

    public Vector3 target;
    Vector3 movement;

    public bool right;

    int bulletLayer;

    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        bulletLayer = LayerMask.NameToLayer("Bullet");
        rb = GetComponent<Rigidbody>();
        if(right == true)
        {
            target += Vector3.right;
        }
        else
        {
            target += Vector3.left;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 norm = (target - transform.position).normalized;
        Vector3 distance = (target - transform.position);
        if (distance.magnitude <= norm.magnitude)
        {
            Destroy(this.gameObject);
        }
        else
        {
            rb.velocity = (target - transform.position).normalized * speed * Time.deltaTime;
        }
    }
}
