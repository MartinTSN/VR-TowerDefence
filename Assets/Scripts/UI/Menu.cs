using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
using Valve.VR.InteractionSystem;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public string nextLevel = "Level #";

    public enum MenuStates { Main, HowTo };
    public MenuStates currentstate;

    public GameObject mainMenu;
    public GameObject howToMenu;

    void Awake()
    {
        currentstate = MenuStates.Main;
    }

    void Update()
    {
        switch (currentstate)
        {
            case MenuStates.Main:
                mainMenu.SetActive(true);
                howToMenu.SetActive(false);
                break;
            case MenuStates.HowTo:
                howToMenu.SetActive(true);
                mainMenu.SetActive(false);
                break;
        }
    }

    public void OnMainMenu()
    {
        currentstate = MenuStates.Main;
    }

    public void OnHowTo()
    {
        currentstate = MenuStates.HowTo;
    }

    public void StartGame()
    {
        SceneManager.LoadScene(nextLevel);
    }
}
