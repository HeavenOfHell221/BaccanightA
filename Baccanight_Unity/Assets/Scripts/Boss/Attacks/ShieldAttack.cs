using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

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
    [SerializeField] private Light2D[] m_lights;
    [SerializeField] private HealthBoss m_health;

    [Header("Phase 1")]
    [Space(5)]
    [SerializeField] [Range(1f, 10f)] private float m_durationHeal;
    [SerializeField] [Range(0f, 50f)] private float m_healAmount;
    [SerializeField] [Range(0.2f, 2f)] private float m_durationWarning;

    [Header("Phase 2")]
    [Space(5)]
    [SerializeField] [Range(1f, 10f)] private float m_durationHealUpgrade;
    [SerializeField] [Range(0f, 50f)] private float m_healAmountUpgrade;
    [SerializeField] [Range(0.2f, 2f)] private float m_durationWarningUpgrade;

#pragma warning restore 0649
    #endregion

    #region Getters / Setters
    public BossStateEvent CounterAttackEvent { get => m_counterAttackEvent; private set => m_counterAttackEvent = value; }
    #endregion

    private void Start()
    {
        m_collider.isTrigger = true;
        DesactivateObjects();
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
        yield return new WaitForSecondsRealtime(m_durationWarning);

        foreach (var light in m_lights)
        {
            light.enabled = true;
        }

        m_collider.enabled = true;

        if (IsUpgraded)
        {
            EndAttack();
        }

        float duration = m_durationHeal;
        float time;

        while (duration > 0f)
        {
            time = Time.deltaTime;
            duration -= time;

            if (duration < 0f)
            {
                time += duration;
            }

            m_health.ModifyHealth(m_healAmount * time);
            yield return null;
        }

        DesactivateObjects();
        EndAttack();
    }

    [ContextMenu("Upgrade Attack")]
    public override void UpgradeAttack()
    {
        base.UpgradeAttack();
        m_durationHeal = m_durationHealUpgrade;
        m_durationWarning = m_durationWarningUpgrade;
        m_healAmount = m_healAmountUpgrade;
    }

    [ContextMenu("Cancel Attack")]
    public override void CancelAttack()
    {
        StopAllCoroutines();
        base.CancelAttack();
        DesactivateObjects();
    }

    private void DesactivateObjects()
    {
        m_collider.enabled = false;
        m_model.SetActive(false);
        foreach (var light in m_lights)
        {
            light.enabled = false;
        }
    }

    public void HandleArrowHit()
    {
        if (m_collider.enabled)
        {
            CancelAttack();
            CounterAttackEvent.Invoke(BossActionType.CounterAttack);
        }
    }
}
