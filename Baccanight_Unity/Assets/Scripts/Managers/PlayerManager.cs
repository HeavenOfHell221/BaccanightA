using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : SingletonBehaviour<PlayerManager>
{
    #region Inspector
#pragma warning disable 0649
    [SerializeField] private PlayerMotion m_playerMotion;
    [SerializeField] private PlayerSound m_playerSound;
    [SerializeField] private PlayerHealth m_playerHealth;
    [SerializeField] private PlayerState m_playerState;
    [SerializeField] private PlayerSucces[] m_playerSucces;
#pragma warning restore 0649
    #endregion

    #region Variables 
    public GameObject PlayerReference { get; set; }
    public InputController PlayerInputController { get; set; }
    public GameObject CameraReference { get; set; }
    #endregion

    public void SetCameraReference(GameObject camera)
    {
        if(CameraReference)
        {
            CameraReference.SetActive(false);
        }

        CameraReference = camera;
        camera.SetActive(true);
    }

    public void ResetPlayerData()
    {
        m_playerHealth.Reset();
        m_playerMotion.Reset();
        m_playerSound.Reset();
        m_playerState.Reset();
        foreach(PlayerSucces playerSucces in m_playerSucces)
        {
            playerSucces.Reset();
        }
    }
}
