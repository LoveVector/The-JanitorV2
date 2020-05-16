using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Rigidbody rb;

    private Vector3 movement;
    Vector3 slideDir;

    public Transform player;

    public LayerMask groundLayer;

    public float speed;
    public float dashT;
    public float dashSp;
    public float jumpH;
    //public float slideSpeed;
    //public float slideTime;
    //public float slideTimeMax;

    bool crouching = false;
    bool isGrounded;
    bool sliding = false;

    int weaponsCount;
    int weaponSelected = 0;

    public bool weaponsEnabled;
    public GameObject[] weapons;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            StartCrouch();
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            StopCrouch();
        }

        if (!isGrounded)
        {
            Debug.Log("Hitting ground");
        }

        WeaponSwitch();

        if (Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(Melee());
        }

        Debug.Log(weaponsEnabled);
    }

    void FixedUpdate()
    {
        Move();

        RaycastHit hit;
        isGrounded = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 1, groundLayer);

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpH, rb.velocity.z);
            Debug.Log("Jumping");
        }

        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartCoroutine(Dash());  
        }
    }

    IEnumerator Dash()
    {
        rb.velocity = movement * dashSp;
        yield return new WaitForSeconds(dashT);
        Debug.Log("Dashing");
    }

    IEnumerator Melee()
    {
        weaponsEnabled = false;
        //animation here
        Debug.Log("Hitting Enemy");
        yield return new WaitForSeconds(2.0f);
        weaponsEnabled = true;
    }

    void Move()
    {
        movement = Quaternion.Euler(0, rb.transform.eulerAngles.y, 0) * new Vector3(Input.GetAxis("Horizontal") * speed, rb.velocity.y, Input.GetAxis("Vertical") * speed);
        rb.velocity = movement;
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            isGrounded = false;
        }
    }

    void StartCrouch()
    {
        //float slideForce = 400;
        transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
        transform.localScale = new Vector3(1, 0.7f, 1);
    }

    void StopCrouch()
    {
        transform.localScale = new Vector3(1, 1, 1);
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
    }

    void WeaponSwitch()
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

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
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
