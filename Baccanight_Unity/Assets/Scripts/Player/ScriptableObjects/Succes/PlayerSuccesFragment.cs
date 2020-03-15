using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SuccesFragment
{
    public KeyFragment Key;
    public bool Value;
}

public enum KeyFragment
{
    Fragment1,
    Fragment2,
    Fragment3,
    Fragment4,
}

[CreateAssetMenu(fileName = "Fragment", menuName = "AssetProject/Succes/Fragment")]
public class PlayerSuccesFragment : ScriptableObject
{
    [SerializeField]
    private SuccesFragment[] m_playerSuccesFragmentInit;

    private Dictionary<KeyFragment, SuccesFragment> m_playerSuccesFragment;

    public void Reset()
    {
        m_playerSuccesFragment = new Dictionary<KeyFragment, SuccesFragment>();

        foreach(var s in m_playerSuccesFragmentInit)
        {
            m_playerSuccesFragment.Add(s.Key, s);
        }
    }

    public void SetSucces(KeyFragment key, bool value)
    {
        m_playerSuccesFragment[key].Value = value;
    }

    public bool HaveSucces(KeyFragment key)
    {
        return m_playerSuccesFragment[key].Value;
    }
}