using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
    float rotX;
    float rotY;
    public float RotSpeed = 6f;
    public GameObject camera;

    float sideRecoil = 0;
    float upRecoil = 0;

    float headsp = 1;
    float stepcount;
    float xamount = 1;
    float yamount = 1;
    Vector3 lastPos;
    float h = 0.9f;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        CameraRotate();
    }

    void CameraRotate()
    {
        rotX += sideRecoil + Input.GetAxis("Mouse X") * RotSpeed;
        rotY += upRecoil + Input.GetAxis("Mouse Y") * RotSpeed;
        rotY = Mathf.Clamp(rotY, -90f, 90f);

        upRecoil -= 30 * Time.deltaTime;
        sideRecoil -= 30 * Time.deltaTime;
        
        if(upRecoil <= 0)
        {
            upRecoil = 0;
        }
        if(sideRecoil <= 0)
        {
            sideRecoil = 0;
        }
        camera.transform.localRotation = Quaternion.Euler(-rotY, 0f, 0f);
        transform.rotation = Quaternion.Euler(0f, rotX, 0f);
    }

    public void AddRecoil(float side, float up)
    {
        sideRecoil = side;
        upRecoil = up;
    }
}
