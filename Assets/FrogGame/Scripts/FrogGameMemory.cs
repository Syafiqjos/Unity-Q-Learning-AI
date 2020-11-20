using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryFragment
{
    //public int input0 = 0;
    //public int input1 = 0;
    public bool[] input;

    public int output = 0;
    
    public bool result = false;

    public MemoryFragment(bool[] input, int output, bool result)
    {
        this.input = input;
        this.output = output;
        this.result = result;
    }

    public override string ToString()
    {
        string x = "Based On my experience (";
        for (int i = 0;i < input.Length; i++)
        {
            x += input[i].ToString();
            if (i < input.Length - 1)
            {
                x += ", ";
            }
        }

        x += ")";

        return  x + " : " + output.ToString() + " is resulting in " + result.ToString();
    }
}


public static class FrogGameMemory
{
    private static int InputCount = 3;

    public static List<MemoryFragment> memoryFragments;

    static FrogGameMemory()
    {
        memoryFragments = new List<MemoryFragment>();
    }

    public static int GetInputCount()
    {
        return InputCount;
    }

    public static int CheckMemory(bool[] input)
    {
        if (memoryFragments != null)
        {
            return SummarizeMemory(input);
        }

        Debug.Log("Because I'm confused, I jumped randomly 1");
        return Random.Range(0, GetInputCount());
    }

    public static void AddMemory(bool[] input, int output, bool result)
    {
        MemoryFragment mf = new MemoryFragment(input, output, result);
        if (SearchEqualMemory(mf) == false)
        {
            memoryFragments.Add(mf);
            Debug.Log("Okay, I got new memory : " + memoryFragments[memoryFragments.Count - 1]);
        }
        else
        {
            Debug.Log("Memory is exists, if I recall it again : " +
                mf); //Useless Memory
        }

        Debug.Log("REASONGING SIZE " + memoryFragments.Count);
        for (int k = 0; k < memoryFragments.Count; k++)
        {
            Debug.Log("REASONING : " + memoryFragments[k]);
        }
    }

    public static int SummarizeMemory(bool[] input)
    {
        MemoryFragment mfnew = new MemoryFragment(input, -1, false);
        foreach (MemoryFragment mf in memoryFragments)
        {
            int reason = ReasoningMemory(mf, mfnew);
            if (reason == -1)
            {

            } else
            {
                return reason;
            }
        }
        Debug.Log("Because I'm confused, I jumped randomly 2");
        return Random.Range(0, GetInputCount());
    }

    public static int ReasoningMemory(MemoryFragment experience, MemoryFragment moment)
    {
        if (CheckInput(experience,moment)){
            if (experience.result) //jika benar lakukan aja
            {
                Debug.Log("I know where i must jumped to, because I experienced it : " + experience);
                return experience.output;
            } else //jika pengalman salah, harus diperbaiki, setidaknya satu langkah kedepan
            {
                //return experience.output == 1 ? 0 : 1; //Abaikan ini
                //experience.output = (1 + experience.output) % experience.input.Length;
                //return experience.output;

                Debug.Log("Hmm.. I ever fall but i don't know where i must go");

                //Mencari yang benar
                bool[] isExists = new bool[GetInputCount()];
                for (int i = 0;i < memoryFragments.Count; i++)
                {
                    if (CheckInput(memoryFragments[i], experience))//Ada yang sama
                    {
                        isExists[experience.output] = true;
                        if (memoryFragments[i].result)
                        {
                            Debug.Log("Ahh I know it");
                            return memoryFragments[i].output;
                        }
                    }
                }

                Debug.Log("Maybe i can guess the jumped i never do");

                //Tidak ada yang benar, mencari yang belum pernah diambil
                for (int i = 0;i < isExists.Length; i++)
                {
                    if (!isExists[i])
                    {
                        return i; //Return output yang belum pernah diambil
                    }
                }

                Debug.Log("Well, idk. Just let me die, so I experienced more.");
                //Sama sekali nggak ada
                return experience.output = (1 + experience.output) % GetInputCount();
            }
        }
        Debug.Log("Memory not found, I must think further");
        return -1;
    }

    public static bool SearchEqualMemory(MemoryFragment input)
    {
        for (int i = 0; i < memoryFragments.Count; i++)
        {
            if (CheckInput(memoryFragments[i], input) && input.output == memoryFragments[i].output)
            {
                return true;
            }
        }
        return false;
    }

    public static bool CheckInput(MemoryFragment exp, MemoryFragment mom)
    {
        bool[] experience = exp.input;
        bool[] moment = mom.input;

        for (int j = 0; j < experience.Length; j++)
        {
            if (experience[j] != moment[j])
            {
                //Debug.Log("NOT EQUAL");
                return false; //Not Equal
            }
        }
        Debug.Log("EQUAL" + exp + mom);
        return true; //Equal
    }
}
