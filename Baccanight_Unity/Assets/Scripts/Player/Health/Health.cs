using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] private PlayerHealth m_health;
    [SerializeField] private PlayerState m_state;
    [SerializeField] private float m_timeTimeScaleNull;
    [SerializeField] private float m_timeInvincibleFrame;
    [SerializeField] private LifeEvent m_onDamaged;
    [SerializeField] private LifeEvent m_onHealed;
    [SerializeField] private UnityEvent m_onRespawn;

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

        HandleDeath(deltaLife);
    }

    public void HandleDeath(int deltaLife)
    {
        if (m_health.CurrentHealth <= 0)
        {
            m_health.IsDead = true;
            m_state.State = GamePlayerState.inDie;
            StartCoroutine(PlayerRespawn());
        }

        if (!m_health.IsInvincible && deltaLife < 0)
        {
            StartCoroutine(InvincibleFrame(deltaLife));
        }
    }

    private IEnumerator InvincibleFrame(int deltaLife)
    {
        deltaLife *= -1;
        float timeScaleNull = TimeScaleNull * deltaLife;

        PlayerManager.Instance.ShakeCamera.Shake(2f * deltaLife, 1f, timeScaleNull);
        m_health.IsInvincible = true;
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(timeScaleNull);
        Time.timeScale = 1f;
        yield return new WaitForSecondsRealtime(m_timeInvincibleFrame - timeScaleNull);
        m_health.IsInvincible = false;
    }

    private IEnumerator PlayerRespawn()
    {
        yield return StartCoroutine(LevelManager.Instance.ReloadScene());
        m_health.Reset();
        m_onRespawn.Invoke();
    }
}