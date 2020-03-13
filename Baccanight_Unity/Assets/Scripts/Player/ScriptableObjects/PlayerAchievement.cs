using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Succes
{
    public KeySucces m_key; // L'ID du succès
    public bool m_isUnlocked; // Est-ce qu'il est unlock
    public bool m_value; // Est-ce qu'il est validé
    public KeySucces[] m_succesToUnlock; // Tabeau des succès à unlock 
}

public enum KeySucces
{
    #region Levels

    Level1,
    Level2,
    LevelBoss,

    #endregion

    #region Fragments

    Fragment1,
    Fragment2,
    Fragment3,
    Fragment4,
    Fragment5,

    #endregion
}

[CreateAssetMenu(fileName = "PlayerAchievement", menuName = "AssetProject/PlayerAchievement")]
public class PlayerAchievement : ScriptableObject
{
    #region Inspector

    [SerializeField]
    private Succes[] m_succes;

    #endregion

    #region Variables
    #endregion

    #region Getters / Setters

    public bool HaveSucces(KeySucces index) => m_succes[(int)index].m_value;
    public bool IsUnlockedSucces(KeySucces index) => m_succes[(int)index].m_isUnlocked;
    public void UnlockSucces(KeySucces index) => m_succes[(int)index].m_isUnlocked = true;

    #endregion

    public void ValidSucces(KeySucces index, bool force = false)
    {
        Succes succes = m_succes[(int)index];

        if(force)
        {
            succes.m_isUnlocked = true;
        }

        if(succes.m_isUnlocked)
        {
            succes.m_value = true;

            foreach(KeySucces key in succes.m_succesToUnlock)
            {
                UnlockSucces(key);
            }
        }
    }

    public void Reset()
    {
        foreach(Succes succes in m_succes)
        {
            succes.m_value = false;
        }
    }
}
