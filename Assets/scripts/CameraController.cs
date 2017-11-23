using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public float panSpeed = 30f;
    public float panBorderThickness = 10f;
    public float scrollSpeed = 5f;
    public float minY = 10f;
    public float maxY = 80f;
    public Vector3 playingPositionFront;
    private Vector3 playingPositionBack;
    private Vector3 playingPositionLeft;
    private Vector3 playingPositionRight;
    public static Quaternion playingRotation = Quaternion.Euler(65f, 0f, 0f);
    public UIController uiController;
    public Snake snake;
    public Transform lookAtPoint;
    public static Grid.Panel currentlyViewingPanel;
    public static bool panelToViewChanging;
    private float distanceFromLookAtPointToCamera;
    private void Start()
    {
        transform.position = playingPositionFront;
        transform.LookAt(lookAtPoint);
        transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        currentlyViewingPanel = Grid.Panel.front;
        panelToViewChanging = false;
        distanceFromLookAtPointToCamera = playingPositionFront.y - lookAtPoint.position.y; 
        playingPositionBack = new Vector3(lookAtPoint.position.x, lookAtPoint.position.y - distanceFromLookAtPointToCamera, lookAtPoint.position.z);
        playingPositionLeft = new Vector3(lookAtPoint.position.x - distanceFromLookAtPointToCamera, lookAtPoint.position.y, lookAtPoint.position.z);
        playingPositionRight = new Vector3(lookAtPoint.position.x + distanceFromLookAtPointToCamera, lookAtPoint.position.y, lookAtPoint.position.z);
    }

    public static void ShiftViewLeft ()
    {
        Grid.Panel next = Grid.Panel.front;
        switch (currentlyViewingPanel)
        {           
            case Grid.Panel.front:
                next = Grid.Panel.left;
                break;
            case Grid.Panel.left:
                next = Grid.Panel.back;
                break;
            case Grid.Panel.back:
                next = Grid.Panel.right;
                break;
            case Grid.Panel.right:
                next = Grid.Panel.front;
                break;
        }
        if (currentlyViewingPanel != Snake.snakeNodes[0].panel)
            return;
        currentlyViewingPanel = next;
        Time.timeScale = 0f;
        panelToViewChanging = true;
    }

    public static void ShiftViewRight()
    {
        Grid.Panel next = Grid.Panel.front;
        switch (currentlyViewingPanel)
        {
            case Grid.Panel.front:
                next = Grid.Panel.right;
                break;
            case Grid.Panel.right:
                next = Grid.Panel.back;
                break;
            case Grid.Panel.back:
                next = Grid.Panel.left;
                break;
            case Grid.Panel.left:
                next = Grid.Panel.front;
                break;
        }
        if (currentlyViewingPanel != Snake.snakeNodes[0].panel)
            return;
        currentlyViewingPanel = next;
        Time.timeScale = 0f;
        panelToViewChanging = true;
    }

    void Update () {
        if(GameManager.gameIsOver)
        {
            this.enabled = false;
            return;
        }
        /*
        if (Input.GetKeyDown("j"))
            ShiftViewLeft();
        if (Input.GetKeyDown("l"))
            ShiftViewRight();        
            */
        if (panelToViewChanging)
        {
            MoveCameraToNewPosition();
            return;
        }
    }

    public void MoveCameraToNewPosition()
    {
        Vector3 dir = new Vector3();
        switch (currentlyViewingPanel)
        {
            case (Grid.Panel.front):
                dir = playingPositionFront - transform.position;
                break;
            case (Grid.Panel.back):
                dir = playingPositionBack - transform.position;
                break;
            case (Grid.Panel.left):
                dir = playingPositionLeft - transform.position;
                break;
            case (Grid.Panel.right):
                dir = playingPositionRight - transform.position;
                break;
        }

        float distanceThisFrame = panSpeed * Time.unscaledDeltaTime;


        if (dir.magnitude <= distanceThisFrame)
        {
            //turn off sweeping motion
            Time.timeScale = 1f;
            panelToViewChanging = false;
            return;
        }
        transform.LookAt(lookAtPoint, new Vector3(0f,0f,90f));
        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        //transform.rotation.SetLookRotation(lookAtPoint.position);
        

    }
}
