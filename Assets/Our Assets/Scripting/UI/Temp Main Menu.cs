using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TempMainMenu : MonoBehaviour
{
    //Menu Game Objects
    public GameObject mainMenu;

    public GameObject explanationMenu;


    private void Start()
    {
        ReturnMainMenu();
    }

    public void StartGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void Explanation()
    {
        mainMenu.SetActive(false);
        explanationMenu.SetActive(true);
    }

    public void ReturnMainMenu()
    {
        mainMenu.SetActive(true);
        explanationMenu.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
