using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private PlayerHealth m_health;
    [SerializeField] private PlayerState m_state;
    [SerializeField] private float m_timeInvincibleFrame;
    [SerializeField] private LifeEvent m_onDamaged;
    [SerializeField] private LifeEvent m_onHealed;

    [ContextMenu("1 damage")]
    public void Damage()
    {
        ModifyHealth(-6, null);
    }

    public void ModifyHealth(int deltaLife, GameObject source)
    {
        if (m_health.IsInvincible || m_health.IsDead)
        {
            return;
        }

        m_health.CurrentHealth = Mathf.Clamp(m_health.CurrentHealth += deltaLife, 0, m_health.MaxHealth);

        if (deltaLife < 0)
        {
            m_onDamaged.Invoke(deltaLife, source);       
        }
        else if(deltaLife > 0)
        {
            m_onHealed.Invoke(deltaLife, source);
        }

        HandleDeath();
    }

    public void HandleDeath()
    {
        if (m_health.CurrentHealth <= 0)
        {
            m_health.IsDead = true;
            m_state.State = GamePlayerState.inDie;
        }
        else
        {
            StartCoroutine(InvincibleFrame());
        }
    }

    private IEnumerator InvincibleFrame()
    {
        m_health.IsInvincible = true;
        yield return new WaitForSecondsRealtime(m_timeInvincibleFrame);
        m_health.IsInvincible = false;
    }
}