using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogGameMaster : MonoBehaviour
{
    public GameObject ui;
    public UnityEngine.UI.Text uiScore;

    public static int scoreValue;
    public static bool isGameOver;
    private static bool isGameOverPrev;
    public static float nextPositionDelta = -5;

    public Transform spawner;
    public GameObject terrainPrefab;
    public FrogGamePlayer player;

    public List<int> indexList = new List<int>();

    public static int latestIndexOffset = 4;
    public static int latestIndex0Terrain;
    public static int latestIndex1Terrain;
    public static int latestIndex0;
    public static int latestIndex1;
    public static int latestJumpType;

    public float randomTimeMax = 3;
    public float randomTime = 0;

    public static void AddScore(int score)
    {
        scoreValue += score;
    }

    void CheckGameOver()
    {
        if (isGameOver != isGameOverPrev)
        {
            FireGameOver();
        }

        isGameOverPrev = isGameOver;
    }

    public void FireGameOver()
    {
        isGameOver = true;

        ui.SetActive(true);
    }

    public void Restart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    void Start()
    {
        latestIndex0 = 0;
        latestIndex1 = 0;
        latestIndex0Terrain = 0;
        latestIndex1Terrain = 0;
        latestJumpType = 0;
        scoreValue = 0;
        isGameOver = false;
        isGameOverPrev = false;
        Spawn(true); Spawn(); Spawn(); Spawn(); Spawn();

        GetLatestIndex();
    }

    void RandomMove()
    {
        randomTime -= Time.deltaTime;
        if (randomTime < 0)
        {
            if (FrogGameMemory.memoryFragments.Count > 0)
            {
                randomTime = Random.Range(0.0f, randomTimeMax / (FrogGameMemory.memoryFragments.Count * 2));
            } else
            {
                randomTime = Random.Range(0.0f, randomTimeMax);
            }

            if (FrogGameMemory.CheckMemory(latestIndex0,latestIndex1) == 0)
            {
                Jump();
            } else if (FrogGameMemory.CheckMemory(latestIndex0, latestIndex1) == 1)
            {
                DoubleJump();
            }
        }
    }

    void Jump()
    {
        Spawn();
        player.Jump();
        AddScore(100);
        latestJumpType = 0;
    }

    void DoubleJump()
    {
        Spawn();
        Spawn();
        player.Jump();
        AddScore(200);
        latestJumpType = 1;
    }

    public void GetLatestIndex()
    {
        //for (int i = 0;i < indexList.Count; i++) { //Debug.Log(indexList[i]); }

        if (indexList.Count - latestIndexOffset + 1 >= 0)
        {
            latestIndex0Terrain = indexList.Count - latestIndexOffset;
            latestIndex1Terrain = indexList.Count - latestIndexOffset + 1;

            latestIndex0 = indexList[latestIndex0Terrain];
            latestIndex1 = indexList[latestIndex1Terrain];

            Debug.Log(latestIndex0Terrain + " - " + latestIndex1Terrain);
        }
        else {
            Debug.Log("NOPE");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Time.timeScale == 0)
            {
                Time.timeScale = 1.0f;
            } else
            {
                Time.timeScale = 0;
            }
        }

        if (FrogGamePlayer.CheckGround() && !isGameOver)
        {
            //GetLatestIndex();
            RandomMove();

            if (Input.GetKeyDown(KeyCode.A))
            {
                Jump();
            } else if (Input.GetKeyDown(KeyCode.S))
            {
                DoubleJump();
            }

            uiScore.text = "Score : " + scoreValue.ToString();
        }

        CheckGameOver();
    }

    void Spawn(bool skip = false)
    {
        int isSafe = Random.Range(0, 2);

        if (skip)
        {
            isSafe = 1;
        }
        else
        {
            if (indexList[indexList.Count - 1] == 0 && isSafe == 0)
            {
                isSafe = 1;
            }
        }

        //isSafe = 1;

        GameObject ne = Instantiate(terrainPrefab, spawner.position, Quaternion.identity, spawner);

        if (isSafe == 1)
        {
            ne.GetComponent<FrogGameTerrain>().Initialize(true, indexList.Count);
        } else
        {
            ne.GetComponent<FrogGameTerrain>().Initialize(false, indexList.Count);
        }

        indexList.Add(isSafe);

        RefreshTerrain();
    }

    void RefreshTerrain()
    {
        GameObject[] terrains = GameObject.FindGameObjectsWithTag("Terrain");
        for (int i = 0; i < terrains.Length; i++)
        {
            GameObject ne = terrains[i];
            ne.GetComponent<FrogGameTerrain>().NextStep();
        }
    }
}
