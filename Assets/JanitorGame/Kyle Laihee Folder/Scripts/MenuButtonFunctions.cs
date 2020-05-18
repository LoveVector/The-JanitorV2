using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtonFunctions : MonoBehaviour
{
    private Animation animatePlay;
    public MainMenuCamera menuSc;
    public GameObject optionsPanel;
    public Transform creditsView;
    private void Start()
    {
        animatePlay = GetComponent<Animation>();
    }

    public void PlayButton()
    {
        animatePlay.Play();
    }

    public void OptionsButton()
    {
        optionsPanel.SetActive(true);
    }

    public void NoOptionsButtonPress()
    {
        optionsPanel.SetActive(false);
    }

    public void CreditsButton()
    {
        menuSc.viewIndex = 5;
    }

    public void CreditsBack()
    {
        menuSc.viewIndex = 0;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
