using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Wave_Spawner : MonoBehaviour {

    public Transform enemyPrefab;
    public float[] timeBetweenWaves;
    public int[] numberOfNodesToDestroy;
    public int WaveIndexMax
    {
        get
        {
            if(timeBetweenWaves.Length != numberOfNodesToDestroy.Length)
            {
                Debug.Log("timeBetweenWaves and numberOfNodesToDestroy array lengths are not equal. they must be equal. Go fix that shit");
            }
            return timeBetweenWaves.Length - 1;
        }
    }
    private float countdown;
    private int waveIndex = 0;
    public float dyingAnimationLength;
    public Grid grid;
    public static bool spawningWave;

    public Text waveCountdownText;
    private void Start()
    {
        spawningWave = false;
        countdown = timeBetweenWaves[0];
    }

    private void Update()
    {
        if (!GameManager.gameStarted)
            return;
        if (spawningWave)
        {
            waveCountdownText.text = "Wave Incoming";
            return;
        }

        if (countdown <= 0f)
        {
            SpawnWave();
            countdown = timeBetweenWaves[waveIndex];
            return;
        }
        countdown -= Time.deltaTime;
        countdown = Mathf.Clamp(countdown,0f,Mathf.Infinity);

        waveCountdownText.text = string.Format("{0:00.00}",countdown);
    }

    void SpawnWave ()
    {
        Debug.Log("Wave Incoming...");
        if (waveIndex != WaveIndexMax)
        {
            waveIndex++;
        }

        GameManager.wavesSurvied++;
        SingleNode[] nodesToDestroy = grid.GetNodesToDestroy(numberOfNodesToDestroy[waveIndex]);
        StartCoroutine(grid.SpawnDyingNodes(nodesToDestroy, dyingAnimationLength));                

    }

}
