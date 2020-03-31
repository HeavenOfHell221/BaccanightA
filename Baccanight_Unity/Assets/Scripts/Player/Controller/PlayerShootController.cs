using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootController : MonoBehaviour
{
    [SerializeField]
    private GameObject m_arrow;

    [SerializeField]
    private float m_cooldown = 0.5f;

    [SerializeField]
    private Transform m_fire;

    private float m_cooldownCounter;
    private float m_lastShoot = 0;

    public void OnShoot()
    {
        if (m_arrow && (Time.time - m_lastShoot) > m_cooldown)
        {
            m_lastShoot = Time.time;
            ObjectPooler.Instance.SpawnFromPool(m_arrow, m_fire.position, new Quaternion());
        }
    }
}
