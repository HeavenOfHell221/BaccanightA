using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SuccesLevels
{
    public KeyLevels Key;
    public bool Value;
}

public enum KeyLevels
{
   FinishLevel1,
   FinishLevel2,
   FinishBoss1,
   CanOpenLevel1,
   CanOpenLevel2,
   CanOpenBoss1,
}

[CreateAssetMenu(fileName = "Levels", menuName = "AssetProject/Succes/Levels")]
public class PlayerSuccesLevels : ScriptableObject
{
    [SerializeField]
    private SuccesLevels[] m_playerSuccesLevelsInit;

    private Dictionary<KeyLevels, SuccesLevels> m_playerSuccesLevels;

    public void Reset()
    {
        m_playerSuccesLevels = new Dictionary<KeyLevels, SuccesLevels>();

        foreach (var s in m_playerSuccesLevelsInit)
        {
            m_playerSuccesLevels.Add(s.Key, s);
        }
    }

    public void SetSucces(KeyLevels key, bool value)
    {
        m_playerSuccesLevels[key].Value = value;
    }

    public bool HaveSucces(KeyLevels key)
    {
        return m_playerSuccesLevels[key].Value;
    }
}