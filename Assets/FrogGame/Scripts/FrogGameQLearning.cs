using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class FrogGameQLearning
{
    private static int InputCount; //Replaced with NUMBER_OF_ACTIONS anyway...
    public const int NUMBER_OF_ACTIONS = 8; //Change this instead

    private const float REWARD = 10;
    private const float LEARNING_RATE = 0.1f;
    private const float DISCOUNT = 0.8f;

    //State
    //- State of next block(s)
    //Action
    //Jump 0 or Jump 1
    private static Dictionary<int, List<float>> QTable; //State, <Action jump 0, Action jump 1>

    static FrogGameQLearning()
    {
        QTable = new Dictionary<int, List<float>>();
        InputCount = NUMBER_OF_ACTIONS;
    }

    public static int GetInputCount()
    {
        return InputCount;
    }

    public static int CheckMemory(bool[] input)
    {
        int action = GetAction(GetState(input));
        return action;
    }

    public static void AddMemory(bool[] input, int output, bool result)
    {
        int currentState = GetState(input);

        float futureReward = (output + 1) * REWARD;
        int currentAction = output;
        float currentQ = QTable[currentState][currentAction];

        if (!result)
        {
            futureReward = -(NUMBER_OF_ACTIONS - output) * REWARD;
        }

        float newQ = CountNewQValue(currentState, currentQ, LEARNING_RATE, DISCOUNT, futureReward);
        UpdateQValue(currentState, currentAction, newQ);
    }

    public static float CountNewQValue(int currentState, float currentQ, float learningRate, float discountFactor, float reward)
    {
        float newQ = currentQ + learningRate * (reward + discountFactor * GetExpectedFutureQ(currentState) - currentQ);
        return newQ;
    }

    public static float GetExpectedFutureQ(int state)
    {
        float maxim = QTable[state][0];
        int maxid = 0;
        for (int i = 0; i < NUMBER_OF_ACTIONS; i++)
        {
            if (QTable[state][i] > maxim)
            {
                maxid = i;
                maxim = QTable[state][i];
            }
        }
        return maxim;
    }

    public static int GetState(bool[] input)
    {
        int bitmask = 0;
        for (int i = 0; i < input.Length; i++)
        {
            bitmask |= (input[i] ? 1 : 0);
            bitmask <<= 1;
        }

        return bitmask;
    }

    public static int GetAction(int state)
    {
        if (QTable.ContainsKey(state) && QTable[state] != null)
        {
            float maxim = QTable[state][0];
            int maxid = 0;
            for (int i = 0;i < NUMBER_OF_ACTIONS; i++)
            {
                if (QTable[state][i] > maxim)
                {
                    maxid = i;
                    maxim = QTable[state][i];
                }
            }
            return maxid;
        } 
        else
        {
            QTable[state] = new List<float>();
            for (int i = 0; i < NUMBER_OF_ACTIONS; i++)
            {
                QTable[state].Add(0);
            }
        }

        Debug.Log("I'm so confused, so I'll jump randomly");
        return Random.Range(0, NUMBER_OF_ACTIONS);
    }

    public static void UpdateQValue(int state, int action, float newQ)
    {
        QTable[state][action] = newQ;
    }
}
