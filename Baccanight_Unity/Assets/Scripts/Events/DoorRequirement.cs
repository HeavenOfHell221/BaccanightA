using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorRequirement : MonoBehaviour
{
    #region Inspector
#pragma warning disable 0649
    [SerializeField]
    private PlayerSucces m_playerSucces;
    [SerializeField]
    private string m_succesForOpenDoor;
    [SerializeField]
    private bool m_allwaysOpen;
#pragma warning restore 0649
    #endregion

    #region Variables
    private BoxCollider2D m_DoorCollider;
    #endregion

    
    void Start() 
    {
        m_DoorCollider = gameObject.GetComponent<BoxCollider2D>();
        StartCoroutine(TestDoor());
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
        PlayerManager.Instance.PlayerInputController.OnInteract.AddListener(LevelManager.Instance.OnInteract);
    }

    public void PlayerExit()
    {
        PlayerManager.Instance.PlayerInputController.OnInteract.RemoveListener(LevelManager.Instance.OnInteract);
    }

    private IEnumerator TestDoor()
    {
        yield return new WaitForSeconds(0.5f);

        if (m_allwaysOpen || m_playerSucces.HaveSucces(m_succesForOpenDoor))
        {
            OpenDoor();
        }
        else
        {
            StartCoroutine(TestDoor());
        }
        
    }
}
