using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;

    public GameObject view;

    private Vector3 movement;
    Vector3 slideDir;

    public Transform player;

    CapsuleCollider cap;

    public LayerMask groundLayer;

    public float speed;
    public float maxRunSpeed;
    public float maxWalkSpeed;
    public float crouchSpeed;
    public float dashT;
    public float dashSp;
    public float jumpH;

    bool isCrouching = false;
    bool isGrounded;
    bool sliding = false;

    int weaponSelected = 0;
    int changedTo;
    float animLength;

    public bool weaponsEnabled = true;
    public bool sprint = false;
    public GameObject[] weapons;

    public Vector3 crouchCameraLocation;
    public Vector3 normalCameraLocation;
    Vector3 camPosition;
    // Start is called before the first frame update
    void Start()
    {
        speed = maxWalkSpeed;
        cap = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();
        ChangeWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        view.transform.localPosition = Vector3.Lerp(view.transform.localPosition, camPosition, 5 * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }

        RaycastHit hit;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isGrounded = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 1, groundLayer);
            if (isGrounded)
            {
                rb.velocity = new Vector3(rb.velocity.x, jumpH, rb.velocity.z);
                Debug.Log("Jumping");
            }
        }
        if (weaponsEnabled == true)
        {
            WeaponSwitch();
        }
    }

    void FixedUpdate()
    {
        Move();

        RaycastHit hit;
        isGrounded = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 1, groundLayer);
    }

    IEnumerator Dash()
    {
        rb.velocity = movement * dashSp;
        yield return new WaitForSeconds(dashT);
        Debug.Log("Dashing");
    }

    void Move()
    {
        if (Input.GetAxis("Horizontal") > 0 || Input.GetAxis("Horizontal") < 0 || Input.GetAxis("Vertical") > 0 || Input.GetAxis("Vertical") < 0)
        {
            if (isCrouching == false)
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    sprint = true;
                    speed += 0.2f;
                    if (speed >= maxRunSpeed)
                    {
                        speed = maxRunSpeed;
                    }
                }
                else
                {
                    sprint = false;
                    speed -= 0.2f;
                    if (speed <= maxWalkSpeed)
                    {
                        speed = maxWalkSpeed;
                    }
                }
            }
            else
            {
                sprint = false;
                speed = crouchSpeed;
            }
            
        }
        else
        {
            speed -= 0.2f;
            if(speed <= 0)
            {
                speed = 0;
            }
        }
        movement = Quaternion.Euler(0, rb.transform.eulerAngles.y, 0) * new Vector3(Input.GetAxis("Horizontal") * speed, rb.velocity.y, Input.GetAxis("Vertical") * speed);
        rb.velocity = movement;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = false;
        }
    }

    void Crouch()
    {
        if (isCrouching == false && isGrounded == true)
        {
            cap.center = new Vector3(0, -0.3f, 0);
            cap.height = 1.3f;
            camPosition = crouchCameraLocation;
            isCrouching = true;
        }
        else if(isCrouching == true && isGrounded == true)
        {
            cap.center = new Vector3(0, 0, 0);
            cap.height = 2f;
            camPosition = normalCameraLocation;
            isCrouching = false;
        }
    }

    void WeaponSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (weapons.Length >= 1)
            {
                if (weaponSelected != 0)
                {
                    StopCoroutine("Change");
                    GunsNew gun = weapons[weaponSelected].GetComponent<GunsNew>();
                    gun.anim.SetTrigger("holster");
                    gun.isHolster = true;
                    animLength = gun.holLength;
                    changedTo = 0; ;
                    StartCoroutine("Change");
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (weapons.Length >= 2)
            {
                if (weaponSelected != 1)
                {
                    StopCoroutine("Change");
                    GunsNew gun = weapons[weaponSelected].GetComponent<GunsNew>();
                    gun.anim.SetTrigger("holster");
                    gun.isHolster = true;
                    animLength = gun.holLength;
                    changedTo = 1; ;
                    StartCoroutine("Change");
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (weapons.Length >= 3)
            {
                if (weaponSelected != 2)
                {
                    StopCoroutine("Change");
                    GunsNew gun = weapons[weaponSelected].GetComponent<GunsNew>();
                    gun.anim.SetTrigger("holster");
                    gun.isHolster = true;
                    animLength = gun.holLength;
                    changedTo = 2; ;
                    StartCoroutine("Change");
                }
            }
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            weaponSelected++;

            if (weaponSelected >= weapons.Length)
            {
                weaponSelected = 0;
            }
            ChangeWeapon();
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            weaponSelected--;

            if (weaponSelected < weapons.Length - weapons.Length)
            {
                weaponSelected = weapons.Length - 1;
            }
            ChangeWeapon();
        }
    }

    public void ChangeWeapon()
    {
       for (int i = 0; i < weapons.Length; i++)
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

    public void Holster()
    {
        if (weaponsEnabled == false)
        {
            GunsNew gun = weapons[weaponSelected].GetComponent<GunsNew>();
            gun.anim.SetBool("isHolster", false);
            weaponsEnabled = true;
        }
        else
        {
            GunsNew gun = weapons[weaponSelected].GetComponent<GunsNew>();
            gun.anim.SetBool("isHolster", true);
            weaponsEnabled = false;
        }
    }

    IEnumerator Change()
    {
        yield return new WaitForSeconds(animLength);
        weaponSelected = changedTo;
        ChangeWeapon();
    }

    public void AmmoRefill(int amount, string weaponName)
    {
        if(weaponName == "shotgun")
        {
            weapons[2].GetComponent<GunsNew>().ammoCap += amount;
        }
        else if(weaponName == "ak")
        {
            weapons[1].GetComponent<GunsNew>().ammo += amount;
        }
    }
}
