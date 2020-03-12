using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    [SerializeField]
    private KeySucces m_OpenCondition;

    [SerializeField]
    private PlayerAchievement m_PlayerAchievement;

    private BoxCollider2D m_DoorCollider;

    [SerializeField]
    private bool AllwaysOpen;

    void Start()
    {
        m_DoorCollider = gameObject.GetComponent<BoxCollider2D>();
        if (m_PlayerAchievement.HaveSucces(m_OpenCondition) || AllwaysOpen)
        {
            OpenDoor();
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
}
