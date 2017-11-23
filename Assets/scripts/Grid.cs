using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

    public Material frontPanelMaterial;
    public Material backPanelMaterial;
    public Material leftPanelMaterial;
    public Material rightPanelMaterial;
    public Material upgradeMaterial;
    public Material snakeMaterial;
    public Material deadNodeRevealedMaterial;
    public float timeBetweenNodeSpawns;
    public GameObject gameOverEffect;
    public UIController ui;
    public Snake snake;

    public Transform nodePrefab;
    Dictionary<string, Transform> gridDictionary;

    public enum Panel { front, left, right, back }


    public static SingleNode[] deadNodes;
    public static int gridSize = 16;
    private Renderer rend;
    private float distanceBetweenNodes = 5f;

    public static float backPanelY;
    public static float leftPanelX;
    public static float rightPanelX;
    public Animation dyingAnimation;
 
    private void Start()
    {
        gridDictionary = new Dictionary<string, Transform>();
        deadNodes = new SingleNode[0];
        leftPanelX = -distanceBetweenNodes;
        rightPanelX = distanceBetweenNodes * gridSize;
        backPanelY = -(distanceBetweenNodes * gridSize) - distanceBetweenNodes;
    }

    public void ColorSnake(SingleNode node)
    {
        var rend = GetNode(node).GetComponent<Renderer>();
        rend.material = snakeMaterial;
    }
    public void ColorUpgrade(SingleNode node)
    {
        var rend = GetNode(node).GetComponent<Renderer>();
        rend.material = upgradeMaterial;
    }
    public void ColorNormal(SingleNode node)
    {
        var rend = GetNode(node).GetComponent<Renderer>();
        switch (node.panel)
        {
            case Panel.front:
                rend.material = frontPanelMaterial;
                break;
            case Panel.back:
                rend.material = backPanelMaterial;
                break;
            case Panel.left:
                rend.material = leftPanelMaterial;
                break;
            case Panel.right:
                rend.material = rightPanelMaterial;
                break;
        }
    }

    public IEnumerator SpawnGrid()
    {
        Debug.Log("Spawning grid");
        //front panel
        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {              

                var gridPiece = Instantiate(nodePrefab, new Vector3(distanceBetweenNodes * x , 0f, distanceBetweenNodes * z), Quaternion.identity);
                gridPiece.transform.parent = this.transform;
                this.gridDictionary.Add(string.Format("front{0},{1}", x.ToString(), z.ToString()), gridPiece);
                //TODO only do this once
                gridPiece.GetComponent<Renderer>().material = frontPanelMaterial;

            }
            yield return new WaitForSeconds(timeBetweenNodeSpawns);
        }
        //left panel
        for (int y = 0; y < gridSize; y++)
        {
            for (int z = 0; z < gridSize; z++)
            {

                var gridPiece = Instantiate(nodePrefab, new Vector3(-distanceBetweenNodes, -(distanceBetweenNodes * y) -distanceBetweenNodes, distanceBetweenNodes * z), Quaternion.identity);
                gridPiece.transform.parent = this.transform;
                this.gridDictionary.Add(string.Format("left{0},{1}", (gridSize - 1 - y).ToString(), z.ToString()), gridPiece);
                //TODO only do this once
                gridPiece.GetComponent<Renderer>().material = leftPanelMaterial;
            }
            yield return new WaitForSeconds(timeBetweenNodeSpawns);
        }
        //right panel
        for (int y = 0; y < gridSize; y++)
        {
            for (int z = 0; z < gridSize; z++)
            {

                var gridPiece = Instantiate(nodePrefab, new Vector3(distanceBetweenNodes * gridSize, -(distanceBetweenNodes * y) - distanceBetweenNodes, distanceBetweenNodes * z), Quaternion.identity);
                gridPiece.transform.parent = this.transform;
                this.gridDictionary.Add(string.Format("right{0},{1}", y.ToString(), z.ToString()), gridPiece);
                //TODO only do this once
                gridPiece.GetComponent<Renderer>().material = rightPanelMaterial;

            }
            yield return new WaitForSeconds(timeBetweenNodeSpawns);
        }
        //back panel
        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {

                var gridPiece = Instantiate(nodePrefab, new Vector3(distanceBetweenNodes * x, -(distanceBetweenNodes*gridSize) - distanceBetweenNodes, distanceBetweenNodes * z), Quaternion.identity);
                gridPiece.transform.parent = this.transform;
                this.gridDictionary.Add(string.Format("back{0},{1}", (gridSize - 1 - x).ToString(), z.ToString()), gridPiece);
                //TODO only do this once
                gridPiece.GetComponent<Renderer>().material = backPanelMaterial;

            }
            yield return new WaitForSeconds(timeBetweenNodeSpawns);
        }
        GameManager.gameStarted = true;
        ui.ShowMenu(UIController.UI.gameUI);
    }

    public void ResetGrid()
    {
        Debug.Log("Spawning grid");
        //front panel
        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {

                var gridPiece = Instantiate(nodePrefab, new Vector3(distanceBetweenNodes * x, 0f, distanceBetweenNodes * z), Quaternion.identity);
                gridPiece.transform.parent = this.transform;
                this.gridDictionary.Add(string.Format("front{0},{1}", x.ToString(), z.ToString()), gridPiece); 
            }
        }
        //left panel
        for (int y = 0; y < gridSize; y++)
        {
            for (int z = 0; z < gridSize; z++)
            {

                var gridPiece = Instantiate(nodePrefab, new Vector3(-distanceBetweenNodes, -(distanceBetweenNodes * y) - distanceBetweenNodes, distanceBetweenNodes * z), Quaternion.identity);
                gridPiece.transform.parent = this.transform;
                this.gridDictionary.Add(string.Format("left{0},{1}", (gridSize - 1 - y).ToString(), z.ToString()), gridPiece);
                //TODO only do this once
                gridPiece.GetComponent<Renderer>().material = leftPanelMaterial;
            }
        }
        //right panel
        for (int y = 0; y < gridSize; y++)
        {
            for (int z = 0; z < gridSize; z++)
            {

                var gridPiece = Instantiate(nodePrefab, new Vector3(distanceBetweenNodes * gridSize, -(distanceBetweenNodes * y) - distanceBetweenNodes, distanceBetweenNodes * z), Quaternion.identity);
                gridPiece.transform.parent = this.transform;
                this.gridDictionary.Add(string.Format("right{0},{1}", y.ToString(), z.ToString()), gridPiece);
                gridPiece.GetComponent<Renderer>().material = rightPanelMaterial;
            }
        }
        //back panel
        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {

                var gridPiece = Instantiate(nodePrefab, new Vector3(distanceBetweenNodes * x, -(distanceBetweenNodes * gridSize) - distanceBetweenNodes, distanceBetweenNodes * z), Quaternion.identity);
                gridPiece.transform.parent = this.transform;
                this.gridDictionary.Add(string.Format("back{0},{1}", (gridSize - 1 - x).ToString(), z.ToString()), gridPiece);
                gridPiece.GetComponent<Renderer>().material = backPanelMaterial;
            }
        }
        GameManager.gameStarted = true;
    }

    public IEnumerator DestroyGrid()
    {
        Debug.Log("Destroying grid");
        int i = 0;
        foreach (var node in gridDictionary.Values)
        {
            Destroy(node.gameObject);
            GameObject effectInstance = (GameObject)Instantiate(gameOverEffect, node.transform.position, node.transform.rotation);
            Destroy(effectInstance, 2f);
            if (i == 5)
            {
                i = 0;
                yield return new WaitForSeconds(0.0001f);

            }
            else {
                i++;
            }

        }
        gridDictionary.Clear();
        snake.ResetSnake();
        ui.ShowMenu(UIController.UI.mainMenu);
    }

    public SingleNode GetRandomDeadNode()
    {
        var randyMcgee = new System.Random();
        int randomx = randyMcgee.Next(0, 16);
        int randomz = randyMcgee.Next(0, 16);

        int randomPanelInt = randyMcgee.Next(0, 4);
        Panel randomPanel = Panel.front;
        //Setting random next dead side
        switch (randomPanelInt)
        {
            case 0:
                randomPanel = Panel.front;
                break;
            case 1:
                randomPanel = Panel.left;
                break;
            case 2:
                randomPanel = Panel.right;
                break;
            case 3:
                randomPanel = Panel.back;
                break;
        }

        SingleNode randomNode = new SingleNode(randomPanel, randomx, randomz);
        //check against current upgrade node
        if (randomNode.IsEqual(Snake.upgradeNode))
        {
            return GetRandomDeadNode();
        }
        //check around snakeNode head
        if (randomNode.IsEqual(new SingleNode(Snake.snakeNodes[0].panel, Snake.snakeNodes[0].x + 1, Snake.snakeNodes[0].z)) ||
            randomNode.IsEqual(new SingleNode(Snake.snakeNodes[0].panel, Snake.snakeNodes[0].x - 1, Snake.snakeNodes[0].z)) ||
            randomNode.IsEqual(new SingleNode(Snake.snakeNodes[0].panel, Snake.snakeNodes[0].x, Snake.snakeNodes[0].z + 1)) ||
            randomNode.IsEqual(new SingleNode(Snake.snakeNodes[0].panel, Snake.snakeNodes[0].x, Snake.snakeNodes[0].z - 1)))
        {
            return GetRandomDeadNode();
        }
        //check against current snake nodes
        for (int snakeNodeIndex = 0; snakeNodeIndex < Snake.snakeNodes.Length; snakeNodeIndex++)
        {
            if (randomNode.IsEqual(Snake.snakeNodes[snakeNodeIndex]))
            {
                return GetRandomDeadNode();
            }
        }
        //check against current dead nodes
        for (int deadNodeIndex = 0; deadNodeIndex < deadNodes.Length; deadNodeIndex++)
        {
            if (randomNode.IsEqual(deadNodes[deadNodeIndex]))
            {
                return GetRandomDeadNode();
            }
        }


        SingleNode[] tempDeadNodes = new SingleNode[deadNodes.Length + 1];

        for (int i = 0; i < deadNodes.Length; i++)
        {
            tempDeadNodes[i] = deadNodes[i];
        }
        tempDeadNodes[tempDeadNodes.Length - 1] = randomNode;
        deadNodes = tempDeadNodes;
        return randomNode;
    }

    public SingleNode SpawnNewUpgradeNode()
    {
        var randyMcgee = new System.Random();
        int randomx = randyMcgee.Next(0, 16);
        int randomz = randyMcgee.Next(0, 16);

        int randomPanelInt = randyMcgee.Next(0, 4);
        Panel randomPanel = Panel.front;
        //Setting random next dead side
        switch (randomPanelInt)
        {
            case 0:
                randomPanel = Panel.front;
                break;
            case 1:
                randomPanel = Panel.left;
                break;
            case 2:
                randomPanel = Panel.right;
                break;
            case 3:
                randomPanel = Panel.back;
                break;
        }

        SingleNode randomNode = new SingleNode(randomPanel, randomx, randomz);

        //check against current snake nodes
        for (int snakeNodeIndex = 0; snakeNodeIndex < Snake.snakeNodes.Length; snakeNodeIndex++)
        {
            if (randomNode.IsEqual(Snake.snakeNodes[snakeNodeIndex]))
            {
                return SpawnNewUpgradeNode();
            }
        }
        //check against current dead nodes
        for (int deadNodeIndex = 0; deadNodeIndex < deadNodes.Length; deadNodeIndex++)
        {
            if (randomNode.IsEqual(deadNodes[deadNodeIndex]))
            {
                return SpawnNewUpgradeNode();
            }
        }
        ColorUpgrade(randomNode);
        return randomNode;
    }

    public void DisableDeadNode(SingleNode deadNode)
    {
        GetNode(deadNode).SetActive(false);
    }

    public GameObject GetNode(SingleNode node)
    {
        node.x = Mathf.Clamp(node.x, 0, 15);
        node.z = Mathf.Clamp(node.z, 0, 15);
        return gridDictionary[string.Format("{2}{0},{1}", node.x.ToString(), node.z.ToString(), node.panel.ToString())].gameObject;
    }

    public void DestroyNodeGameOver(SingleNode nodeToGet)
    {
        GameObject node = GetNode(nodeToGet);
        node.SetActive(false);
        GameObject effectInstance = (GameObject)Instantiate(gameOverEffect, node.transform.position, node.transform.rotation);
        Destroy(effectInstance, 2f);
    }
    
    public void Update()
    {
        if(GameManager.gameIsOver)
        {
            RevealDeadBlocks();
            return;
        }
    }

    public void RevealDeadBlocks()
    {
        for (int i = 0; i < deadNodes.Length; i++)
        {
            GameObject node = GetNode(deadNodes[i]);
            node.SetActive(true);
            var rend = node.GetComponent<Renderer>();
            rend.material = deadNodeRevealedMaterial;
        }

    }

    public Panel GetRightPanel(Panel panel)
    {
        switch (panel)
        {
            case Grid.Panel.front:
                panel = Grid.Panel.right;
                break;
            case Grid.Panel.right:
                panel = Grid.Panel.back;
                break;
            case Grid.Panel.back:
                panel = Grid.Panel.left;
                break;
            case Grid.Panel.left:
                panel = Grid.Panel.front;
                break;
        }
        return panel;
    }

    public Panel GetLeftPanel(Panel panel)
    {
        switch (panel)
        {
            case Grid.Panel.front:
                panel = Grid.Panel.left;
                break;
            case Grid.Panel.left:
                panel = Grid.Panel.back;
                break;
            case Grid.Panel.back:
                panel = Grid.Panel.right;
                break;
            case Grid.Panel.right:
                panel = Grid.Panel.front;
                break;
        }
        return panel;
    }

    public SingleNode[] GetNodesToDestroy(int numberOfNodesToDestroy)
    {
        SingleNode[] nodesToDestroy = new SingleNode[numberOfNodesToDestroy];
        for (int nodeIndex = 0; nodeIndex < numberOfNodesToDestroy; nodeIndex++)
        {
            nodesToDestroy[nodeIndex] = GetRandomDeadNode();
        }
        return nodesToDestroy;
    }

    public IEnumerator SpawnDyingNodes(SingleNode[] nodesToDestroy, float dyingAnimationLength)
    {
        for (int taskNumber = 0; taskNumber < 2; taskNumber++)
        {
            if (taskNumber == 0)
            { 
                for (int nodeToAnimate = 0; nodeToAnimate < nodesToDestroy.Length; nodeToAnimate++)
                {
                    Animator animator = GetNode(nodesToDestroy[nodeToAnimate]).GetComponent<Animator>();
                    animator.enabled = true;
                }
                GameManager.LockSnake = true;
                Wave_Spawner.spawningWave = true;

            }

            if (taskNumber == 1)
            {
                for (int nodeToAnimate = 0; nodeToAnimate < nodesToDestroy.Length; nodeToAnimate++)
                {
                    GetNode(nodesToDestroy[nodeToAnimate]).SetActive(false);
                }
                GameManager.LockSnake = false;
                Wave_Spawner.spawningWave = false;

            }

            yield return new WaitForSeconds(dyingAnimationLength);
        }
    }
}

public class SingleNode
{
    public Grid.Panel panel;
    public int x;
    public int z;

    public SingleNode(Grid.Panel _panel, int _x, int _z)
    {
        this.panel = _panel;
        this.x = _x;
        this.z = _z;
    }

    public bool IsEqual(SingleNode otherNode)
    {
        if(x == otherNode.x && z == otherNode.z && panel == otherNode.panel)
        {
            return true;
        }
        return false;
    }

    public override string ToString()
    {
        return string.Format("{0}:({1},{2})",panel,x,z);
    }
}
