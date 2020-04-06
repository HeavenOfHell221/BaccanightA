using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballAttack : BossAttack
{
    #region Inspector
#pragma warning disable 0649
    [Header("General")]
    [Space(5)]
    [SerializeField] private Collider2D m_collider;
    [SerializeField] private GameObject m_fireball;
    [SerializeField] private MinMaxFloat m_distanceToPlayer; 

    [Header("Phase 1")]
    [Space(5)]
    [SerializeField] [Range(1, 10)] private int m_numberPerSalve1;
    [SerializeField] [Range(0.1f, 3f)] private float m_cooldownBetweenSalve1;
    [SerializeField] [Range(0.05f, 0.5f)] private float m_cooldownBetweenFireball1;
    [SerializeField] [Range(2, 10)] private int m_numberTotalSalve1;

    [Header("Phase 2")]
    [Space(5)]
    [SerializeField] [Range(1, 10)] private int m_numberPerSalve2;
    [SerializeField] [Range(0.1f, 3f)] private float m_cooldownBetweenSalve2;
    [SerializeField] [Range(0.05f, 0.5f)] private float m_cooldownBetweenFireball2;
    [SerializeField] [Range(2, 10)] private int m_numberTotalSalve2;

#pragma warning restore 0649
    #endregion

    private Transform m_player;
    private int m_numberSalveRemaining;
    
    private int m_numberPerSalve;
    private float m_cooldown;
    private int m_numberTotalSalve;
    private float m_cooldownBetweenFireball;


    private void Start()
    {
        m_collider.isTrigger = true;
        m_numberPerSalve = m_numberPerSalve1;
        m_numberTotalSalve = m_numberTotalSalve1;
        m_cooldown = m_cooldownBetweenSalve1;
        m_cooldownBetweenFireball = m_cooldownBetweenFireball1;
    }

    [ContextMenu("Start Attack")]
    public override void StartAttack()
    {
        base.StartAttack();
        m_player = PlayerManager.Instance.PlayerReference.transform;
        m_numberSalveRemaining = m_numberTotalSalve;

        StartCoroutine(HandleAttack());
    }

    protected override IEnumerator HandleAttack()
    {
        for (int i = 0; i < m_numberPerSalve; i++)
        {
            StartCoroutine(SpawnFireball());
            yield return new WaitForSeconds(m_cooldownBetweenFireball);
        }

        yield return new WaitForSeconds(m_cooldown);

        if (m_numberSalveRemaining > 1)
        {
            m_numberSalveRemaining--;
            StartCoroutine(HandleAttack());
        }
        else
        {
            EndAttack();
        }
    }

    private IEnumerator SpawnFireball()
    {
        Vector3 center = m_collider.bounds.center;
        Vector3 extents = m_collider.bounds.extents;
        Vector3 spawnPosition;
        float dist;
        do
        {
            yield return null;
            spawnPosition = new Vector3(
            Random.Range((center.x - extents.x), (center.x + extents.x)),
            Random.Range((center.y - extents.y), (center.y + extents.y)),
            0f);
            dist = Vector2.Distance(spawnPosition, m_player.position);

        } while (dist < m_distanceToPlayer.Min || dist > m_distanceToPlayer.Max);

        ObjectPooler.Instance.SpawnFromPool(m_fireball, spawnPosition);    
    }

    [ContextMenu("Upgrade Attack")]
    public override void UpgradeAttack()
    {
        base.UpgradeAttack();
        m_numberPerSalve = m_numberPerSalve2;
        m_numberTotalSalve = m_numberTotalSalve2;
        m_cooldownBetweenFireball = m_cooldownBetweenFireball2;
        m_cooldown = m_cooldownBetweenSalve2;
    }
}
