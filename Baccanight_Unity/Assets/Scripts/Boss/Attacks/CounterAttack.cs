using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterAttack : BossAttack
{
    #region Inspector
    [Header("General")]
    [Space(5)]
    [SerializeField] private Collider2D m_collider;
    [SerializeField] private Rigidbody2D m_rigidbody;
    [SerializeField] private HealthBoss m_health;

    [Header("Phase 1")]
    [Space(5)]
    [SerializeField] [Range(1, 40)] private int m_speed = 10;
    [SerializeField] [Range(0.1f, 3f)] private float m_cooldownBeforeAttack = 1.5f;

    [Header("Phase 2")]
    [Space(5)]
    [SerializeField] [Range(1, 40)] private int m_speedUpgrade = 20;
    [SerializeField] [Range(0.1f, 3f)] private float m_cooldownBeforeAttackUpgrade = 1.0f;
    #endregion

    private Transform m_player;

    private void Start()
    {
        m_collider.enabled = false; // Pour pas trigger les évents
                                    // on active le collider seulement pendant l'attaque.
        m_player = PlayerManager.Instance.PlayerReference.transform;
    }

    [ContextMenu("Start Attack")]
    public override void StartAttack()
    {
        base.StartAttack();
        StartCoroutine(HandleAttack());
    }

    // Quand le collider entre dans le stage
    // On arrête l'attaque. Dans EndAttack il y a la désactivation du collier et la vitesse du rigibody à zero.
    public void OnEnterStage()
    {     
        EndAttack();       
    }

    protected override IEnumerator HandleAttack()
    {
        yield return new WaitForSeconds(m_cooldownBeforeAttack);

        m_health.IsInvincible = true; // Pour rendre la contre-attaque plus drole, le joueur peut pas lui faire de dégâts ? 
                                      // A voir, tu peux retirer si tu trouves ça trop.

        StartCoroutine(MoveTowardPlayer());
    }

    private IEnumerator MoveTowardPlayer()
    {    
        Vector3 direction = (m_player.position - transform.position).normalized;
        m_rigidbody.velocity = direction * m_speed;
        yield return new WaitForSeconds(0.1f);
        m_collider.enabled = true;
    }

    protected override void EndAttack()
    {
        ResetAttributes();
        base.EndAttack();
    }

    [ContextMenu("Upgrade Attack")]
    public override void UpgradeAttack()
    {
        base.UpgradeAttack();
        m_speed = m_speedUpgrade;
        m_cooldownBeforeAttack = m_cooldownBeforeAttackUpgrade;
    }

    [ContextMenu("Cancel Attack")]
    public override void CancelAttack()
    {
        StopAllCoroutines();
        ResetAttributes();
        base.CancelAttack();
    }

    private void ResetAttributes()
    {
        m_rigidbody.velocity = Vector2.zero;
        m_collider.enabled = false;
        m_health.IsInvincible = false;
    }
}
