using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllowJump : MonoBehaviour
{
    [SerializeField] private PlayerMotion m_playerMotion;
    [SerializeField] private bool m_allowTheJump = false;

    private void Start()
    {
        m_playerMotion.CanJump = m_allowTheJump;
    }
}
    