using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fence : MonoBehaviour
{
    [SerializeField] private bool m_isOpen = false;
    [SerializeField] private Collider2D m_collider;
    private Vector3 m_endMarker;

    public void Start()
    {

        m_collider.enabled = !m_isOpen;
    }

    public void OpenFence()
    {
        //m_collider.enabled = false;
        m_endMarker = transform.position + Vector3.up * 3;
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        transform.position = Vector3.Lerp(transform.position, m_endMarker, 0.05f);
        if (transform.position != m_endMarker)
        {
            StartCoroutine(Move());
        }
        else yield return null;
    }

    public void CloseFence()
    {
        m_collider.enabled = true;
    }
}
