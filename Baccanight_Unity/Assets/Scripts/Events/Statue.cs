using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Statue : MonoBehaviour
{
    [SerializeField]
    private PlayerMotion m_playerMotion;

    public void OnEnterPlayer()
    {
        m_playerMotion.IsPushObject = true;
    }

    public void OnExitPlayer()
    {
        m_playerMotion.IsPushObject = false;
    }
}
