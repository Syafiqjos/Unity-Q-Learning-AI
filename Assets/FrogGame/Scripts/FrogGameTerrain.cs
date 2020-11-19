using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogGameTerrain : MonoBehaviour
{
    public int index = 0;

    public bool isInitialized = false;
    public Vector2 targetPos;

    public bool isSafeTerrain = true;
    public GameObject minion;

    public void NextStep()
    {
        targetPos += new Vector2(FrogGameMaster.nextPositionDelta,0);
    }

    void Start()
    {
        if (!isInitialized)
        {
            targetPos = transform.position;
            isInitialized = true;
        }
    }

    public void Initialize(bool safe, int index)
    {
        this.index = index;

        targetPos = transform.position;
        isInitialized = true;
        isSafeTerrain = safe;

        if (isSafeTerrain)
        {
            GetComponent<SpriteRenderer>().color = Color.white;
        } else
        {
            GetComponent<SpriteRenderer>().color = Color.red;
        }
    }

    void Update()
    {
        Move();
        CheckDeath();

        CheckCursor();
    }

    void CheckCursor()
    {
        minion.SetActive(FrogGameMaster.latestIndex0Terrain == index || FrogGameMaster.latestIndex1Terrain == index);
    }

    void Move()
    {
        if (targetPos != null)
        {
            transform.position = Vector2.Lerp(transform.position, targetPos, Time.deltaTime * 8);
        }
    }

    void CheckDeath()
    {
        if (transform.position.x < -20)
        {
            Destroy(gameObject);
        }
    }
}
