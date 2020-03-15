using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : SingletonBehaviour<PlayerManager>
{
    #region Inspector
    public GameObject PlayerReference;
    public CameraController CameraReference;
    public InputController PlayerinputController;

    [SerializeField] private PlayerMotion m_playerMotion;
    [SerializeField] private PlayerSound m_playerSound;
    [SerializeField] private PlayerHealth m_playerHealth;
    [SerializeField] private PlayerState m_playerState;
    [SerializeField] private PlayerSuccesLevels m_playerSuccesLevels;
    [SerializeField] private PlayerSuccesFragment m_playerSuccesFragment;
    #endregion

    #region Variables
    
    #endregion

    public void ResetPlayerData()
    {        
        m_playerHealth.Reset();
        m_playerMotion.Reset();
        m_playerSound.Reset();
        m_playerState.Reset();
        m_playerSuccesFragment.Reset();
        m_playerSuccesLevels.Reset();
    }
}
