using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickAmmo : MonoBehaviour
{
    public bool shotgun;

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (shotgun == true)
                {
                    other.gameObject.GetComponent<PlayerMovement>().AmmoRefill(5, "shotgun");
                }
                else
                {
                    other.gameObject.GetComponent<PlayerMovement>().AmmoRefill(100, "ak");
                }
                Destroy(this.gameObject);
            }
        }
    }
}
