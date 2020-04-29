using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitch : MonoBehaviour
{
    public GameObject[] weapons;

    int weaponsCount;
    int weaponSelected = 0;

    public bool weaponsEnabled;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            if (weaponsEnabled == false)
            {
                weapons[i].SetActive(false);
            }
            else
            {
                if (weaponSelected == i)
                {
                    weapons[i].SetActive(true);
                }
                else
                {
                    weapons[i].SetActive(false);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            weaponSelected = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            weaponSelected = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            weaponSelected = 2;
        }

        if(Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            weaponSelected++;

            if (weaponSelected >= weapons.Length)
            {
                weaponSelected = 0;
            }
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            weaponSelected--;

            if (weaponSelected < weapons.Length - weapons.Length)
            {
                weaponSelected = weapons.Length - 1; 
            }
        }
    }
}
