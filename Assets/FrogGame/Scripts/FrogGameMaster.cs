using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FrogGameMaster : MonoBehaviour
{
    public GameObject ui;
    public Text uiScore;
    public Text uiTime;
    public Text uiRetry;

    public static int scoreValue;
    public static float timeValue;
    public static int retryValue;
    public static bool isGameOver;
    private static bool isGameOverPrev;
    public static float nextPositionDelta = -5;

    public Transform spawner;
    public GameObject terrainPrefab;
    public FrogGamePlayer player;

    public List<bool> indexList = new List<bool>();

    public static int latestIndexOffset = 4;

    public static int[] latestIndexTerrain;
    public static bool[] latestIndex;

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

        Restart();
    }

    public void Restart()
    {
        retryValue++;
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    void Start()
    {
        latestIndexTerrain = new int[FrogGameQLearning.GetInputCount()];
        latestIndex = new bool[FrogGameQLearning.GetInputCount()];

        latestIndexOffset = FrogGameQLearning.GetInputCount();
        //latestIndexOffset = 8;
        //latestIndexOffset = FrogGameQLearning.NUMBER_OF_ACTIONS;

        latestJumpType = 0;
        scoreValue = 0;
        isGameOver = false;
        isGameOverPrev = false;
        //

        uiRetry.text = "Retry : " + retryValue.ToString() + " (s)";

        for (int i = 0; i < FrogGameQLearning.GetInputCount() + 10; i++)
        {
            Spawn(true);
        }

        GetLatestIndex();
    }

    void RandomMove()
    {
        randomTime -= Time.deltaTime;
        if (randomTime < 0)
        {
            /*
            if (FrogQLearning.memoryFragments.Count > 0)
            {
                randomTime = Random.Range(0.0f, randomTimeMax / (FrogQLearning.memoryFragments.Count * 2));
            } else
            {
                randomTime = Random.Range(0.0f, randomTimeMax);
            }
            */

            Jump(FrogGameQLearning.CheckMemory(latestIndex));
        }
    }

    void Jump(int far)
    {
        for (int i = 0; i <= far; i++)
        {
            Spawn();
        }
        player.Jump();
        AddScore(100 * (far + 1));
        latestJumpType = far;
    }

    public void GetLatestIndex()
    {
        //for (int i = 0;i < indexList.Count; i++) { //Debug.Log(indexList[i]); }

        if (indexList.Count - latestIndexOffset + 1 >= 0)
        {
            for (int i = 0;i < latestIndexTerrain.Length;i++)
            {
                latestIndexTerrain[i] = indexList.Count - latestIndexOffset + i;
                latestIndex[i] = indexList[latestIndexTerrain[i]];
            }

            //Debug.Log(latestIndex0Terrain + " - " + latestIndex1Terrain);
        }
        else {
            Debug.Log("NOPE");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Time.timeScale *= 2;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Time.timeScale *= 0.5f;
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }

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
                //Jump();
            } else if (Input.GetKeyDown(KeyCode.S))
            {
                //DoubleJump();
            }

            uiScore.text = "Score : " + scoreValue.ToString();
        }

        timeValue += Time.deltaTime;
        uiTime.text = "Time : " + ((int)timeValue / 60).ToString("D2") + ":" + (((int)timeValue) % 60).ToString("D2");

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
            if (isSafe == 0)
            {
                int count = 0;
                for (int i = 1; i < FrogGameQLearning.GetInputCount(); i++)
                {
                    if (indexList[indexList.Count - i] == false)
                    {
                        count++;
                    }
                }

                if (count >= FrogGameQLearning.GetInputCount() - 1)
                {
                    isSafe = 1;
                }
            }
        }

        //isSafe = 1;

        GameObject ne = Instantiate(terrainPrefab, spawner.position, Quaternion.identity, spawner);

        if (isSafe == 1)
        {
            ne.GetComponent<FrogGameTerrain>().Initialize(true, indexList.Count);
            indexList.Add(true);
        } else
        {
            ne.GetComponent<FrogGameTerrain>().Initialize(false, indexList.Count);
            indexList.Add(false);
        }

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
