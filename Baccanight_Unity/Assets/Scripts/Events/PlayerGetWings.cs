using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGetWings : MonoBehaviour
{
    [SerializeField] private PlayerMotion m_playerMotion;
    [SerializeField] private GameObject m_Wings;
    [SerializeField] private GameObject m_Bow;

    public void OnPlayerEnter(GameObject player)
    {
        m_playerMotion.UseWings = true;
        m_Wings.SetActive(false);
        m_Bow.SetActive(false);
    }
}
