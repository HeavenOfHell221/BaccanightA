using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MinMaxInt
{
    public int Min;
    public int Max;

    public MinMaxInt(int min, int max)
    {
        Min = min;
        Max = max;
    }

    public int GetRandomValue()
    {
        return Random.Range(Min, Max + 1);
    }
}
