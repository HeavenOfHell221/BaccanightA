using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthBoss : MonoBehaviour
{
    #region Inspector
#pragma warning disable 0649
    [Header("Health")]
    [Space(5)]
    [SerializeField] [Range(0, 2000)] private float m_maxHealth = 1000f;
    [SerializeField] [Range(0, 2000)] private float m_currentHealth = 1000f;

    [Header("Events")]
    [Space(5)]
    [SerializeField] private BossStateEvent m_FirstSwitchPhase;
    [SerializeField] private UnityEvent m_SecondSwitchPhase;
    [SerializeField] private UnityEvent m_DeathPhase;
    [SerializeField] private UnityEvent m_UpgradeAttacks;
#pragma warning restore 0649
    #endregion

    public float CurrentHealth {get => m_currentHealth; private set => m_currentHealth = value; }
    public bool IsDead { get; private set; } = false;
    public float Ratio => CurrentHealth / m_maxHealth;

    private bool m_isEnraging = false;

    private void Start()
    {
        CurrentHealth = m_maxHealth;
    }

    public void ModifyHealth(float deltaHealth)
    {
        if (IsDead)
        {
            return;
        }

        float lastRatio = Ratio;

        CurrentHealth = Mathf.Clamp(CurrentHealth += deltaHealth, 0f, m_maxHealth);

        if (lastRatio >= 0.5f && Ratio < 0.5f && !m_isEnraging)
        {
            m_FirstSwitchPhase.Invoke(BossActionType.Enraging);
            m_UpgradeAttacks.Invoke();
            m_isEnraging = true;
        }
        else if (lastRatio >= 0.1f && Ratio < 0.1f)
        {
            m_SecondSwitchPhase.Invoke();
            m_UpgradeAttacks.Invoke();
        }
        else if (Ratio <= 0f)
        {
            m_DeathPhase.Invoke();
            IsDead = true;
        }
    }
}
