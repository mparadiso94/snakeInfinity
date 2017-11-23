using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour {
    public enum UI { mainMenu, noMenu, pauseMenu,gameOverMenu, gameUI }
    public static UI currentUI;
    public Transform mainMenu;
    public Transform pauseMenu;
    public Transform gameOverMenu;
    public Transform gameUI;
    public static bool cameraSweepingIn = false;
    public Grid nodes;

    // Use this for initialization
    public void Start() {
        ShowMenu(UI.mainMenu);
    }

    // Update is called once per frame
    public void Update() {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))        
            Toggle();  
    }

    public void Toggle()
    {
        if (currentUI == UI.pauseMenu)
        {
            Time.timeScale = 1f;
            ShowMenu(UI.gameUI);
        }
        else
        {
            Time.timeScale = 0f;
            ShowMenu(UI.pauseMenu);
        }
    }

    public void StartGame()
    {
        ShowMenu(UI.noMenu);
        StartCoroutine(nodes.SpawnGrid());
    }






    public void ShowMenu(UI menu)
    {
        switch(menu)
        {
            case UI.gameOverMenu:
                GameManager.gameIsOver = true;
                currentUI = UI.gameOverMenu;
                mainMenu.gameObject.SetActive(false);
                pauseMenu.gameObject.SetActive(false);
                gameOverMenu.gameObject.SetActive(true);
                gameUI.gameObject.SetActive(false);
                break;
            case UI.gameUI:
                currentUI = UI.gameUI;
                mainMenu.gameObject.SetActive(false);
                pauseMenu.gameObject.SetActive(false);
                gameOverMenu.gameObject.SetActive(false);
                gameUI.gameObject.SetActive(true);
                break;
            case UI.mainMenu:
                GameManager.gameIsOver = false;
                GameManager.gameStarted = false;
                currentUI = UI.mainMenu;
                mainMenu.gameObject.SetActive(true);
                pauseMenu.gameObject.SetActive(false);
                gameOverMenu.gameObject.SetActive(false);
                gameUI.gameObject.SetActive(false);
                break;
            case UI.pauseMenu:
                currentUI = UI.pauseMenu;
                mainMenu.gameObject.SetActive(false);
                pauseMenu.gameObject.SetActive(true);
                gameOverMenu.gameObject.SetActive(false);
                gameUI.gameObject.SetActive(false);
                break;
            case UI.noMenu:
                currentUI = UI.noMenu;
                mainMenu.gameObject.SetActive(false);
                pauseMenu.gameObject.SetActive(false);
                gameOverMenu.gameObject.SetActive(false);
                gameUI.gameObject.SetActive(false);
                break;
        }
    }

    public void Retry()
    {
        Toggle();
    }

    public void MenuButton()
    {
        Toggle();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
