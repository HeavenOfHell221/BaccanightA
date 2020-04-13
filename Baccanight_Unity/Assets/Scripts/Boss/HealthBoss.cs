using System;
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

    [Header("Attributes")]
    [Space(5)]
    [SerializeField] [Range(0.5f, 5f)] private float m_invincibleDuration;
    [SerializeField] [Range(0, -3)] private int m_damageCollision;

    [Header("Events")]
    [Space(5)]
    [SerializeField] private BossStateEvent m_FirstSwitchPhase;
    [SerializeField] private UnityEvent m_SecondSwitchPhase;
    [SerializeField] private BossStateEvent m_DeathPhase;
    [SerializeField] private UnityEvent m_UpgradeSpeedBetweenTwoAttacks;
#pragma warning restore 0649
    #endregion

    public float CurrentHealth {get => m_currentHealth; private set => m_currentHealth = value; }
    public bool IsDead { get; private set; } = false;
    public float Ratio => CurrentHealth / m_maxHealth;
    public bool IsInvincible { get; set; } = false;

    private bool m_isEnraging = false;

    public event Action<float> OnHealthPctChanged = delegate { };

    private void Awake()
    {
        CurrentHealth = m_maxHealth;
        IsInvincible = true;
    }

    public void ModifyHealth(float deltaHealth)
    {
        if (IsInvincible || IsDead)
        {
            return;
        }

        float lastRatio = Ratio;

        CurrentHealth = Mathf.Clamp(CurrentHealth += deltaHealth, 0f, m_maxHealth);

        OnHealthPctChanged(Ratio);

        if (lastRatio >= 0.5f && Ratio < 0.5f && !m_isEnraging)
        {
            StartCoroutine(InvincibleFrame());
            m_isEnraging = true;
            m_FirstSwitchPhase.Invoke(BossActionType.Enraging);   
        }
        else if (lastRatio >= 0.25f && Ratio < 0.25f)
        {
            m_UpgradeSpeedBetweenTwoAttacks.Invoke();
            m_SecondSwitchPhase.Invoke();
        }
        else if (Ratio <= 0f)
        {
            StartCoroutine(_Death());
            m_DeathPhase.Invoke(BossActionType.Dying);
            IsDead = true;     
        }
    }

    private IEnumerator InvincibleFrame()
    {
        IsInvincible = true;
        yield return new WaitForSecondsRealtime(m_invincibleDuration);
        IsInvincible = false;
    }

    public void OnEnterPlayer(GameObject other)
    {
        other.GetComponent<Health>().ModifyHealth(m_damageCollision, gameObject);       
    }

    private IEnumerator _Death()
    {
        yield return new WaitForSeconds(2f);
        LevelManager.Instance.ChangeScene(0, 2);
    }
}
