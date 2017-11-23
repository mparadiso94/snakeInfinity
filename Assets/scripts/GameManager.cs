using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static bool gameIsOver;
    public static bool gameStarted;
    public Grid nodes;
    public static bool LockSnake;
    public static int wavesSurvied;
    public static int upgradesGathered;

    // Update is called once per frame
    void Update () {
        if (gameIsOver)
            return;
        if (Input.GetKeyDown("e"))
        {
            EndGame();
        }
	}

    private void Start()
    {
        gameIsOver = false;
        gameStarted = false;
        LockSnake = false;
    }

    private void EndGame()
    {
        gameIsOver = true;
        Debug.Log("Game over");
    }

    public void DestroyGrid()
    {
        StartCoroutine(nodes.DestroyGrid());
    }
}
