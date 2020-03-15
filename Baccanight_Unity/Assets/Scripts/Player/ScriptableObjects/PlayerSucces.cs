using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Succes
{
    public string Key;
    public bool Value;
}

[CreateAssetMenu(fileName = "PlayerSucces", menuName = "AssetProject/PlayerSucces")]
public class PlayerSucces : ScriptableObject
{
    [SerializeField]
    private Succes[] m_playerSuccesInit;

    private Dictionary<string, Succes> m_playerSucces;

    public void Reset()
    {
        m_playerSucces = new Dictionary<string, Succes>();

        foreach(var s in m_playerSuccesInit)
        {
            m_playerSucces.Add(s.Key, s);
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
}