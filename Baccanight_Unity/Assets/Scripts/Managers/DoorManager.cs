using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    #region Inspector
    [SerializeField]
    private PlayerSucces m_playerSucces;
    [SerializeField]
    private string m_succesForOpenDoor;
    [SerializeField]
    private bool m_allwaysOpen;
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
        PlayerManager.Instance.PlayerinputController.OnInteract.AddListener(LevelManager.Instance.OnInteract);
    }

    public void PlayerExit()
    {
        PlayerManager.Instance.PlayerinputController.OnInteract.RemoveListener(LevelManager.Instance.OnInteract);
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
