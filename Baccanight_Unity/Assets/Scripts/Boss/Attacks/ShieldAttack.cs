using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShieldAttack : BossAttack
{
    #region Inspector
#pragma warning disable 0649
    [Header("Events")]
    [Space(5)]
    [SerializeField] private BossStateEvent m_counterAttackEvent;

    [Header("Attributes")]
    [Space(5)]
    [SerializeField] private Collider2D m_collider;
    [SerializeField] private GameObject m_model;
    [SerializeField] private HealthBoss m_health;

    [Header("Phase 1")]
    [Space(5)]
    [SerializeField] [Range(1f, 10f)] private float m_durationAttack;
    [SerializeField] [Range(0f, 50f)] private float m_healAmount;

    [Header("Phase 2")]
    [Space(5)]
    [SerializeField] [Range(1f, 10f)] private float m_durationAttackUpgrade;
    [SerializeField] [Range(0f, 50f)] private float m_healAmountUpgrade;

#pragma warning restore 0649
    #endregion

    #region Getters / Setters
    public BossStateEvent CounterAttackEvent { get => m_counterAttackEvent; private set => m_counterAttackEvent = value; }
    #endregion

    private void Start()
    {
        m_collider.isTrigger = true;
        m_collider.enabled = false;
        m_model.SetActive(false);
    }

    [ContextMenu("Start Attack")]
    public override void StartAttack()
    {
        base.StartAttack();
        
        m_model.SetActive(true);
        StartCoroutine(HandleAttack());
    }

    protected override IEnumerator HandleAttack()
    {
        yield return new WaitForSeconds(1f);

        if (IsUpgraded)
        {
            EndAttack();
        }

        m_collider.enabled = true;

        float duration = m_durationAttack;
        float time;

        while(duration > 0f)
        {
            time = Time.deltaTime;
            duration -= time;

            if(duration < 0f)
            {
                time += duration;
            }

            m_health.ModifyHealth(m_healAmount * time);
            yield return null;
        }

        m_collider.enabled = false;
        m_model.SetActive(false);
        EndAttack();
    }

    [ContextMenu("Upgrade Attack")]
    public override void UpgradeAttack()
    {
        base.UpgradeAttack();
    }

    [ContextMenu("Cancel Attack")]
    public override void CancelAttack()
    {
        StopAllCoroutines();
        base.CancelAttack();
        m_collider.enabled = false;
        m_model.SetActive(false);
    }
}
