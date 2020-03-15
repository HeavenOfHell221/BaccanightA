using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    #region Inspector
    [SerializeField]
    private PlayerSuccesLevels m_playerSuccesLevels;
    [SerializeField]
    private KeyLevels m_succesForOpenDoor;
    #endregion

    #region Variables
    private BoxCollider2D m_DoorCollider;
    #endregion

    [SerializeField]
    private bool m_allwaysOpen;

    void Start() 
    {
        m_DoorCollider = gameObject.GetComponent<BoxCollider2D>();
        if (m_allwaysOpen || m_playerSuccesLevels.HaveSucces(m_succesForOpenDoor))
        {
            OpenDoor();
        }
        else
        {
            CloseDoor();
        }
    }

    public void OpenDoor()
    {
        m_DoorCollider.enabled = true;
        //Affichage de porte Ouverte
    }

    public void CloseDoor()
    {
        m_DoorCollider.enabled = false;
        //Affichage de porte fermée
    }

    public void PlayerEnter()
    {
        PlayerManager.Instance.PlayerinputController.OnInteract.AddListener(LevelManager.Instance.OnInteract);
    }

    public void PlayerExit()
    {
        PlayerManager.Instance.PlayerinputController.OnInteract.RemoveListener(LevelManager.Instance.OnInteract);
    }
}
