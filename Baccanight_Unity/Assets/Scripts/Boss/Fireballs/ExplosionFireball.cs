using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionFireball : Fireball
{
    [Header("Range Direction X")]
    [Space(5)]
    [SerializeField] [Range(-3f, 0f)] private float m_MinDirectionX = -0.75f;
    [SerializeField] [Range(-3f, 0f)] private float m_MaxDirectionX = 0f;

    [Header("Range Direction Y")]
    [Space(5)]
    [SerializeField] [Range(0f, 3f)] private float m_MinDirectionY = 0.25f;
    [SerializeField] [Range(0f, 3f)] private float m_MaxDirectionY = 1.5f;

    protected override void Start()
    {
        StartCoroutine(_Move());
    }

    private IEnumerator _Move()
    {
        yield return new WaitForSeconds(m_timeBeforeMove);
        Move();
    }

    protected override void Move()
    {
        float directionX = Random.Range(m_MinDirectionX, m_MaxDirectionX);
        float directionY = Random.Range(m_MinDirectionY, m_MaxDirectionY);
        Vector3 direction = new Vector3(directionX, directionY, 0f);
        Rigidbody.AddForce(direction * Speed, ForceMode2D.Impulse);
    }
}
