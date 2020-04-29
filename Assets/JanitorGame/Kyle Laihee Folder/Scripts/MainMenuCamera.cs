using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public static class DefineLerp
{
    public static void LerpTransform(this Transform thisObject, Transform thatObject, float t)
    {
        thisObject.position = Vector3.Lerp(thisObject.position, thatObject.position, t);
        thisObject.rotation = Quaternion.Lerp(thisObject.rotation, thatObject.rotation, t);
        thisObject.localScale = Vector3.Lerp(thisObject.localScale, thatObject.localScale, t);
    }
}
public class MainMenuCamera : MonoBehaviour
{
    //public Transform playCamera1;
    //public Transform playCamera2;
    public Transform[] cameraOptions;
    public TextMeshPro[] menuTexts;
    public float transitionSpeed;
    private Transform currentView;
    TextMeshPro currentText;
    [SerializeField]
    int viewIndex = 0;
    [SerializeField]
    int colorIndex = 0;

    void Start()
    {
        currentView = cameraOptions[viewIndex];
        currentText = menuTexts[colorIndex];
    }

    void Update()
    {
        currentView = cameraOptions[viewIndex];
        currentText = menuTexts[colorIndex];


        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
             if(viewIndex > 0)
             {
                menuTexts[colorIndex].color = Color.white;
                colorIndex--;
                viewIndex--;
             }
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if(viewIndex < cameraOptions.Length - 1)
            {
                menuTexts[colorIndex].color = Color.white;
                colorIndex++;
                viewIndex++;
            }
        }

        menuTexts[colorIndex].color = Color.red;
    }

    void LateUpdate()
    {
        transform.LerpTransform(currentView.transform, Time.deltaTime);
    }

}
