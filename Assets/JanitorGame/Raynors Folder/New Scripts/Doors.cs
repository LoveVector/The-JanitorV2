using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Doors : MonoBehaviour
{
    public bool beginningDoor;
    public bool turnedOn;
    // Start is called before the first frame update
    void Start()
    {
        if(beginningDoor == true)
        {
            turnedOn = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && turnedOn == true)
        {
            if (beginningDoor == true)
            {
                LevelManager.Instance.levelStart = true;
                LevelManager.Instance.levelEnd = false;
                Destroy(this.gameObject);
            }
            else if(beginningDoor == false && LevelManager.Instance.levelEnd == true)
            {
                LevelManager.Instance.Save();
                SceneManager.LoadScene(LevelManager.Instance.currentScene + 1);
            }
        }
    }
}
