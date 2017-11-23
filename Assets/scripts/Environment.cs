using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour {

    public Transform rightPanel; 
    public Transform leftPanel;
    public Transform frontPanel;
    public Transform backPanel;
    public GameObject test;

    // Use this for initialization
    void Start () {
        rightPanel.gameObject.SetActive(false);
        leftPanel.gameObject.SetActive(false);
        frontPanel.gameObject.SetActive(false);
        backPanel.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update () {
		
	}
    public void SpawnFrontPanel()
    {
        Debug.Log("spawning front panel");
        frontPanel.gameObject.SetActive(true);
    }
    public void SpawnBackPanel()
    {
        backPanel.gameObject.SetActive(true);
    }
    public void SpawnLeftPanel()
    {
        leftPanel.gameObject.SetActive(true);
    }
    public void SpawnRightPanel()
    {
        rightPanel.gameObject.SetActive(true);
    }

}
