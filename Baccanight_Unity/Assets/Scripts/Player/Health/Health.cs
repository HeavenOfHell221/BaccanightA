using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private PlayerHealth m_health;
    [SerializeField] private PlayerState m_state;
    [SerializeField] private float m_timeTimeScaleNull;
    [SerializeField] private float m_timeInvincibleFrame;
    [SerializeField] private LifeEvent m_onDamaged;
    [SerializeField] private LifeEvent m_onHealed;

    public bool IsInvincible { get => m_health.IsInvincible; }
    public float TimeScaleNull { get => m_timeTimeScaleNull; }

    public void ModifyHealth(int deltaLife, GameObject source)
    {

        //Vector3 direction = (transform.position - source.transform.position).normalized;
        //Debug.DrawRay(transform.position, direction * 100f, Color.blue, 10f, false);


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

        if (!m_health.IsInvincible)
        {
            StartCoroutine(InvincibleFrame());
        }
    }

    private IEnumerator InvincibleFrame()
    {
        PlayerManager.Instance.ShakeCamera.Shake(2f, 1f, TimeScaleNull);
        m_health.IsInvincible = true;
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(m_timeTimeScaleNull);
        Time.timeScale = 1f;
        yield return new WaitForSecondsRealtime(m_timeInvincibleFrame - m_timeTimeScaleNull);
        m_health.IsInvincible = false;
    }
}