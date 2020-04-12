using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fence : MonoBehaviour
{
    [SerializeField] private bool m_isOpen = false;
    [SerializeField] private Collider2D m_collider;

    public void Start()
    {
        m_collider.enabled = !m_isOpen;
    }

    public void OpenFence()
    {
        //m_collider.enabled = false;
        transform.position += Vector3.up * 3;
    }

    public void CloseFence()
    {
        m_collider.enabled = true;
    }
}
