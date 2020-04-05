using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballAttack : BossAttack
{
    #region Inspector
#pragma warning disable 0649
    [Header("General")][Space(5)]
    [SerializeField] private Collider2D m_collider;
    [SerializeField] private GameObject m_fireball;
    [SerializeField] [Min(3f)] private float m_DistanceMinPlayer = 10f;

    [Header("Continuously")][Space(5)]
    [SerializeField] private bool m_isContinuously = false;
    [SerializeField] [Range(0.1f, 2f)] private float m_cooldown;

    [Header("Salve")][Space(5)]
    [SerializeField] private bool m_isSalve = false;
    [SerializeField] private MinMaxInt m_numberPerSalve;
    
#pragma warning restore 0649
    #endregion

    private Transform m_player;
    private int m_numberFireball = 1;

    [ContextMenu("Handle Fire Ball")]
    public override void StartAttack()
    {
        m_player = PlayerManager.Instance.PlayerReference.transform;
        IsStarted = true;

        if(m_isSalve && m_isContinuously)
        {
            m_numberFireball = m_numberPerSalve.GetRandomValue();
            for (int i = 0; i < m_numberFireball; i++)
            {
                StartCoroutine(HandleAttack());
            }
        }
        else if(m_isContinuously && !m_isSalve)
        {
            StartCoroutine(HandleAttack());
        }
        else if(m_isSalve && !m_isContinuously)
        {
            m_numberFireball = m_numberPerSalve.GetRandomValue();
            for (int i = 0; i < m_numberFireball; i++)
            {
                StartCoroutine(HandleAttack());
            }
        }
        else
        {
            StartCoroutine(HandleAttack());
        }
    }

    public override IEnumerator HandleAttack()
    {
        InProgress = true;
        Vector3 center = m_collider.bounds.center;
        Vector3 extents = m_collider.bounds.extents;
        Vector3 spawnPosition;
        do
        {
            yield return null;
            spawnPosition = new Vector3(
            Random.Range((center.x - extents.x), (center.x + extents.x)),
            Random.Range((center.y - extents.y), (center.y + extents.y)),
            0f);
        } while (Vector2.Distance(spawnPosition, m_player.position) < m_DistanceMinPlayer);

        ObjectPooler.Instance.SpawnFromPool(m_fireball, spawnPosition);

        InProgress = false;

        if(m_isContinuously)
        {
            yield return new WaitForSeconds(m_cooldown);
            StartCoroutine(HandleAttack());
        }
    }

    public override void EndAttack()
    {
        IsFinish = true;
    }
}
