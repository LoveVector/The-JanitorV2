using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PressSpaceToPlay : MonoBehaviour
{
    private Animation animatePlay;

    private void Start()
    {
        animatePlay = GetComponent<Animation>();
    }
    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animatePlay.Play();
        }
    }

    /*public static void ChangeScene(string sceneName)
    {
        LoadSceneMode = 1;
    }*/
}
