using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Succes
{
    public string Key;
    public bool Value;

    public Succes(string key, bool value)
    {
        Key = key;
        Value = value;
    }
}

[CreateAssetMenu(fileName = "PlayerSucces", menuName = "AssetProject/PlayerSucces")]
public class PlayerSucces : ScriptableObject
{
    [SerializeField]
    private Succes[] m_playerSuccesInit;

    public Dictionary<string, Succes> m_playerSucces;

    public void Reset()
    {
        m_playerSucces = new Dictionary<string, Succes>();

        foreach(Succes s in m_playerSuccesInit)
        {
            Succes newSucces = new Succes(s.Key, s.Value);
            m_playerSucces.Add(newSucces.Key, newSucces);
        }
    }

    public void SetSucces(string key, bool value)
    {
        m_playerSucces[key].Value = value;
    }

    public bool HaveSucces(string key)
    {
        return m_playerSucces[key].Value;
    }

    public bool HaveAllSucces()
    {
        var values = m_playerSucces.Values;
        foreach(Succes succes in values)
        {
            if(!succes.Value)
            {
                return false;
            }
        }
        return true;
    }
}