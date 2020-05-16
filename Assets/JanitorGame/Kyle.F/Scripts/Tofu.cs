using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tofu : MonoBehaviour
{
    public Vector3 targetposition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = Vector3.MoveTowards(transform.position, targetposition, 8 * Time.deltaTime);
    }
}
