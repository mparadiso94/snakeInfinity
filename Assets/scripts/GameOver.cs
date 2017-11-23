using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour {
    public Text roundsText;
    public Grid nodes;
    public UIController ui;
    public GameManager gm;

    private void OnEnable()
    {
        int total = ((GameManager.wavesSurvied * 3) + (GameManager.upgradesGathered * 2));
        roundsText.text = string.Format("({0}*3)+({1}*2)={2}", GameManager.wavesSurvied, GameManager.upgradesGathered, total);
    }

    public void Retry()
    {
        nodes.ResetGrid();
    }

    public void Menu()
    {

        ui.ShowMenu(UIController.UI.mainMenu);
        gm.DestroyGrid();
    }

}
