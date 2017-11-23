using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Snake : MonoBehaviour {
    public Grid grid;
    public int startLength;
    public float startSpeed;
    public Text pointsText;
    public Text gameOverText;
    public float timeBetweenNodeDeaths;
    private float currentSpeed;
    private float timeUntilSnakeMoves = 1f;
    private enum Direction {up, down, left, right }
    private Queue<Direction> nextDirections;
    public int CurrentLength { get { return snakeNodes.Length; } }
    private bool snakeSpawned;
    private bool snakeDestroyed;
    private int startNodeX = 4;
    private int startNodeZ = 8;
    public static SingleNode[] snakeNodes;
    //this node is a place holder for the upgrade node position once upgraded
    public static SingleNode trailingSnakeNode;

    public static SingleNode upgradeNode;
    public UIController ui;


    private void Start()
    {
        snakeSpawned = false;
        snakeDestroyed = false;
        currentSpeed = startSpeed;
        timeUntilSnakeMoves = 1 / currentSpeed;
        GameManager.upgradesGathered = 0;
        pointsText.text = GameManager.upgradesGathered.ToString();


    }

    void Update()
    {
        if (!GameManager.gameStarted)
            return;
        if (GameManager.gameIsOver && snakeDestroyed)        
            return;

        
        if (GameManager.gameIsOver && !snakeDestroyed)
        {
            snakeDestroyed = true;
            StartCoroutine(DestroySnakeGameOver());
            return;
        }
        if (!snakeSpawned)
            SpawnSnake();
        //inputs
        var directionToCheck = nextDirections.Count - 1;
        if (Input.GetKeyDown("w") || Input.GetKeyDown("up"))
        {
            if (nextDirections.ToArray()[directionToCheck] != Direction.down)
            {
                nextDirections.Enqueue(Direction.up);
            }
        }
        if (Input.GetKeyDown("s") || Input.GetKeyDown("down"))
        {
            if (nextDirections.ToArray()[directionToCheck] != Direction.up)
                nextDirections.Enqueue(Direction.down);
        }
        if (Input.GetKeyDown("d") || Input.GetKeyDown("right"))
        {
            if (nextDirections.ToArray()[directionToCheck] != Direction.left)
                nextDirections.Enqueue(Direction.right);
        }
        if (Input.GetKeyDown("a") || Input.GetKeyDown("left"))
        {
            if (nextDirections.ToArray()[directionToCheck] != Direction.right)
                nextDirections.Enqueue(Direction.left);
        }

        //Movement timer
        if (timeUntilSnakeMoves <= 0f)
        {
            MoveSnake();
            timeUntilSnakeMoves = 1 / currentSpeed;
        }
        timeUntilSnakeMoves -= Time.deltaTime;
    }

    public void SpawnSnake()
    {
        Debug.Log("spawning snake");
        nextDirections = new Queue<Direction>();
        snakeNodes = new SingleNode[startLength];
        nextDirections.Enqueue(Direction.right);
        snakeSpawned = true;
        for (int i = 0; i < startLength; i++)
        {
            snakeNodes[i] = new SingleNode(Grid.Panel.front, startNodeX - i, startNodeZ);
            grid.ColorSnake(snakeNodes[i]);
            //Debug.Log(i+": snake node : " + snakeNodes[i].ToString());
        }
        trailingSnakeNode = new SingleNode(Grid.Panel.front, startNodeX - startLength, startNodeZ);
        //Debug.Log("trailing snake node : " + trailingSnakeNode.ToString());

        upgradeNode = grid.SpawnNewUpgradeNode();
    }

    void MoveSnake()
    {
        if (GameManager.LockSnake)
            return;
        SingleNode[] tempSnakeNodes = new SingleNode[CurrentLength];
        if (nextDirections.Count > 1)
            nextDirections.Dequeue();

        switch (nextDirections.Peek())
        {            
            case Direction.right:
                if (snakeNodes[0].x + 1 >= 16)
                {
                    tempSnakeNodes[0] = new SingleNode(grid.GetRightPanel(snakeNodes[0].panel), 0, snakeNodes[0].z);                    
                    CameraController.ShiftViewRight();
                    break;
                }
                else
                {
                    tempSnakeNodes[0] = new SingleNode(snakeNodes[0].panel, snakeNodes[0].x + 1, snakeNodes[0].z);
                    break;
                }
            case Direction.left:
                if (snakeNodes[0].x - 1 <= -1)
                {
                    tempSnakeNodes[0] = new SingleNode(grid.GetLeftPanel(snakeNodes[0].panel), 16, snakeNodes[0].z);
                    CameraController.ShiftViewLeft();
                    break;
                }
                else
                {
                    tempSnakeNodes[0] = new SingleNode(snakeNodes[0].panel, snakeNodes[0].x - 1, snakeNodes[0].z);
                    break;
                }
            case Direction.up:
                if (snakeNodes[0].z + 1 >= 16)
                {
                    ui.ShowMenu(UIController.UI.gameOverMenu);
                    gameOverText.text = pointsText.text;
                    Debug.Log("Game over top");
                    return;
                }
                tempSnakeNodes[0] = new SingleNode(snakeNodes[0].panel, snakeNodes[0].x, snakeNodes[0].z + 1);
                break;
            case Direction.down:
                if (snakeNodes[0].z - 1 <= -1)
                {
                    ui.ShowMenu(UIController.UI.gameOverMenu);
                    gameOverText.text = pointsText.text;
                    Debug.Log("Game over bottom");
                    return;
                }
                tempSnakeNodes[0] = new SingleNode(snakeNodes[0].panel, snakeNodes[0].x, snakeNodes[0].z - 1);
                break;
        }
        //Debug.Log("0: coloring: " + tempSnakeNodes[0].ToString());
        grid.ColorSnake(tempSnakeNodes[0]);
        if (tempSnakeNodes[0].IsEqual(upgradeNode))
        {
            Debug.Log("upgrading");
            UpgradeSnake();
            return;
        }
        for(int i = 0; i < Grid.deadNodes.Length; i++)
        {
            if (tempSnakeNodes[0].IsEqual(Grid.deadNodes[i]))
            {
                ui.ShowMenu(UIController.UI.gameOverMenu);
                gameOverText.text = pointsText.text;
                Debug.Log("Game over dead node hit");
                return;
            }
        }

        for (int i = 1; i < CurrentLength; i++)
        {

            tempSnakeNodes[i] = null;
            tempSnakeNodes[i] = snakeNodes[i - 1];
            if (tempSnakeNodes[0].IsEqual(tempSnakeNodes[i]))
            {
                ui.ShowMenu(UIController.UI.gameOverMenu);
                gameOverText.text = pointsText.text;
                Debug.Log("Game over hit self");
                string debugText = string.Format("(|| snake nodes: ");
                for (int j = 0; j < tempSnakeNodes.Length; j++)
                {
                    debugText += string.Format("||{0}:({1},{2})||", tempSnakeNodes[j].panel.ToString(), tempSnakeNodes[j].x, tempSnakeNodes[j].z);
                }
                Debug.Log(debugText);

                return;
            }
            //Debug.Log(i+":coloring: " + tempSnakeNodes[i].ToString());
            grid.ColorSnake(tempSnakeNodes[i]);
        }
        trailingSnakeNode = snakeNodes[CurrentLength - 1];
        grid.ColorNormal(trailingSnakeNode);
        //Debug.Log("uncoloring: " + trailingSnakeNode.ToString());
        snakeNodes = tempSnakeNodes;


    }

    void UpgradeSnake()
    {
        var tempSnakeNodes = new SingleNode[CurrentLength + 1];
        tempSnakeNodes[0] = upgradeNode;
        grid.ColorSnake(upgradeNode);
        for (int i = 1; i < tempSnakeNodes.Length; i++)
        {
            tempSnakeNodes[i] = null;
            tempSnakeNodes[i] = snakeNodes[i - 1];
        }
        //grid.ColorSnake(trailingSnakeNode);

        snakeNodes = tempSnakeNodes;
        currentSpeed += 0.1f;
        upgradeNode = grid.SpawnNewUpgradeNode();
        GameManager.wavesSurvied++;
        pointsText.text = GameManager.wavesSurvied.ToString();
    }

    IEnumerator DestroySnakeGameOver()
    {
        for(int i = 0; i < snakeNodes.Length; i++)
        {
            if (i == snakeNodes.Length - 1)
                break;//invisible snake node to remember position

            
            var nodeToDestroy = snakeNodes[i];
            grid.DestroyNodeGameOver(nodeToDestroy);
            yield return new WaitForSeconds(timeBetweenNodeDeaths);
        }
    }

    public void ResetSnake()
    {
        nextDirections = new Queue<Direction>();
        snakeNodes = new SingleNode[startLength];
        nextDirections.Enqueue(Direction.right);
        snakeSpawned = false;
        snakeDestroyed = false;

    }
}

