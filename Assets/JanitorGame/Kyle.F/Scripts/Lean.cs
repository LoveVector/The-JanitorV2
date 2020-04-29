using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lean : MonoBehaviour
{
    public float speed = 100f;
    public float targetAng = 20f;

    float curAng = 0f;
    Rigidbody rb;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame  
    void Update()
    {

        // lean left
        if (Input.GetKey(KeyCode.Q))
        {
            curAng = Mathf.MoveTowardsAngle(curAng, targetAng, speed * Time.deltaTime);
            Debug.Log("Leaning Left" + curAng);
        }
        // lean right
        else if (Input.GetKey(KeyCode.E))
        {
            curAng = Mathf.MoveTowardsAngle(curAng, -targetAng, speed * Time.deltaTime);
            Debug.Log("Leaning Right" + curAng);
        }
        // reset lean
        else
        {
            curAng = Mathf.MoveTowardsAngle(curAng, 0f, speed * Time.deltaTime);
        }

        rb.transform.rotation = Quaternion.AngleAxis(curAng, Vector3.forward);
    }
}
