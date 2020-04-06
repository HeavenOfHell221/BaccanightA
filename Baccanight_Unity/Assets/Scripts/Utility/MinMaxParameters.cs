using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MinMaxInt
{
    public int Min;
    public int Max;

    public int GetRandomValue() => Random.Range(Min, Max + 1);
    public float GetValueFromRatio(float ratio) => Mathf.Lerp(Min, Max, ratio);
}

[System.Serializable]
public class MinMaxFloat
{
    public float Min;
    public float Max;

    public float GetRandomValue() => Random.Range(Min, Max + 1f);
    public float GetValueFromRatio(float ratio) => Mathf.Lerp(Min, Max, ratio);
}

[System.Serializable]
public class MinMaxVector2
{
    public Vector2 Min;
    public Vector2 Max;

    public MinMaxVector2(Vector2 min, Vector2 max)
    {
        Min = min;
        Max = max;
    }

    public Vector2 GetValueFromRatio(float ratio) => Vector2.Lerp(Min, Max, ratio);
}

[System.Serializable]
public class MinMaxVector3
{
    public Vector3 Min;
    public Vector3 Max;

    public Vector3 GetValueFromRatio(float ratio) => Vector2.Lerp(Min, Max, ratio);
}

