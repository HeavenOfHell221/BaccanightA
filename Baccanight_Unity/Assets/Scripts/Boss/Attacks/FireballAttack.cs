using System.Collections;
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
    [SerializeField] [Range(1, 10)] private int m_numberPerBurst = 4;
    [SerializeField] [Range(0.1f, 3f)] private float m_cooldownBetweenBurst = 1.5f;
    [SerializeField] [Range(0.05f, 0.5f)] private float m_cooldownBetweenFireball = 0.4f;
    [SerializeField] [Range(2, 10)] private int m_numberTotalBurst = 4;
    [SerializeField] [Range(5f, 20f)] private float m_speedFireball;

    [Header("Phase 2")]
    [Space(5)]
    [SerializeField] [Range(1, 10)] private int m_numberPerBurstUpgrade = 6;
    [SerializeField] [Range(0.1f, 3f)] private float m_cooldownBetweenBurstUpgrade = 1;
    [SerializeField] [Range(0.05f, 0.5f)] private float m_cooldownBetweenFireballUpgrade = 0.3f;
    [SerializeField] [Range(2, 10)] private int m_numberTotalBurstUpgrade = 5;
    [SerializeField] [Range(5f, 20f)] private float m_speedFireballUpgrade;

#pragma warning restore 0649
    #endregion

    private Transform m_player;
    private int m_numberBurstRemaining;

    private void Start()
    {
        m_collider.isTrigger = true;
    }

    [ContextMenu("Start Attack")]
    public override void StartAttack()
    {
        base.StartAttack();
        m_player = PlayerManager.Instance.PlayerReference.transform;
        m_numberBurstRemaining = m_numberTotalBurst;

        StartCoroutine(HandleAttack());
    }

    protected override IEnumerator HandleAttack()
    {
        for (int i = 0; i < m_numberPerBurst; i++)
        {
            StartCoroutine(SpawnFireball());
            yield return new WaitForSecondsRealtime(m_cooldownBetweenFireball);
        }

        yield return new WaitForSecondsRealtime(!IsCanceled ? m_cooldownBetweenBurst : 0f);

        if (m_numberBurstRemaining > 1)
        {
            m_numberBurstRemaining--;
            StartCoroutine(HandleAttack());
        }
        else
        {
            yield return new WaitForSecondsRealtime(1f);
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

        GameObject fireball = ObjectPooler.Instance.SpawnFromPool(m_fireball, spawnPosition, Quaternion.identity, transform);
        fireball.GetComponent<TargetFireball>().Speed = m_speedFireball;
    }

    [ContextMenu("Upgrade Attack")]
    public override void UpgradeAttack()
    {
        base.UpgradeAttack();
        m_numberPerBurst = m_numberPerBurstUpgrade;
        m_numberTotalBurst = m_numberTotalBurstUpgrade;
        m_cooldownBetweenFireball = m_cooldownBetweenFireballUpgrade;
        m_cooldownBetweenBurst = m_cooldownBetweenBurstUpgrade;
        m_speedFireball = m_speedFireballUpgrade;
    }

    [ContextMenu("Cancel Attack")]
    public override void CancelAttack()
    {
        StopAllCoroutines();
        base.CancelAttack();
    }
}
