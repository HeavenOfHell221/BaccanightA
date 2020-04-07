using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterAttack : BossAttack
{
    #region Inspector
    [Header("General")]
    [Space(5)]
    [SerializeField] private Collider2D m_collider;
    [SerializeField] private Collider2D m_hitCollider;
    [SerializeField] private LayerMask m_layerCollision;
    [SerializeField] private LayerMask m_layerHit;
    [SerializeField] private Rigidbody2D Rigidbody;
    //[SerializeField] private MinMaxFloat m_distanceToPlayer;

    [Header("Phase 1")]
    [Space(5)]
    [SerializeField] [Range(1, 30)] private int m_speed = 10;
    [SerializeField] [Range(1, 3)] private int m_hitDamage = -1;
    [SerializeField] [Range(0.1f, 3f)] private float m_cooldownBeforeAttack = 1.5f;

    [Header("Phase 2")]
    [Space(5)]
    [SerializeField] [Range(1, 30)] private int m_speedUpgrade = 20;
    [SerializeField] [Range(1, 3)] private int m_hitDamageUpgrade = -1;
    [SerializeField] [Range(0.1f, 3f)] private float m_cooldownBeforeAttackUpgrade = 1.0f;
    #endregion

    private Transform m_player;

    private void Start()
    {

    }

    [ContextMenu("Start Attack")]
    public override void StartAttack()
    {
        base.StartAttack();
        

        StartCoroutine(HandleAttack());
    }

    public void OnEnterPlayer(GameObject player)
    {
        player.GetComponent<Health>().ModifyHealth(m_hitDamage, gameObject);
    }

    protected override IEnumerator HandleAttack()
    {
        yield return new WaitForSeconds(!IsCanceled ? m_cooldownBeforeAttack : 0f);

        Move();

        while (!m_collider.IsTouchingLayers(m_layerCollision))
        {
            yield return null;
        }

        Rigidbody.velocity = Vector2.zero;

        EndAttack();
    }

    private void Move()
    {
        m_player = PlayerManager.Instance.PlayerReference.transform;
        Vector3 direction = (m_player.position - transform.position).normalized;
        Rigidbody.AddForce(direction * m_speed, ForceMode2D.Impulse);
        Rigidbody.velocity = direction * m_speed;
    }


    [ContextMenu("Upgrade Attack")]
    public override void UpgradeAttack()
    {
        base.UpgradeAttack();
        m_speed = m_speedUpgrade;
        m_cooldownBeforeAttack = m_cooldownBeforeAttackUpgrade;
        m_hitDamage = m_hitDamageUpgrade;
    }

    [ContextMenu("Cancel Attack")]
    public override void CancelAttack()
    {
        StopAllCoroutines();
        base.CancelAttack();
    }
}
