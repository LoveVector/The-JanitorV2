using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowParabola : MonoBehaviour
{
    public Rigidbody throwableObj;
    public Transform target;

    public float height;
    public float gravity;

    // Start is called before the first frame update
    void Start()
    {
        throwableObj.useGravity = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {

          Throw();

        }
    }

    void Throw()
    {
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
