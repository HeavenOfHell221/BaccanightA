using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthBoss : MonoBehaviour
{
    [SerializeField] private float m_maxHealth = 800f;
    [SerializeField] private UnityEvent m_FirstSwitchPhase;
    [SerializeField] private UnityEvent m_SecondSwitchPhase;
    [SerializeField] private UnityEvent m_DeathPhase;

    public float CurrentHealth {get; private set; }
    public bool IsDead { get; private set; } = false;
    public float Ratio => CurrentHealth / m_maxHealth;

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

        if (lastRatio >= 0.5f && Ratio < 0.5f)
        {
            m_FirstSwitchPhase.Invoke();
        }
        else if (lastRatio >= 0.1f && Ratio < 0.1f)
        {
            m_SecondSwitchPhase.Invoke();
        }
        else if(Ratio <= 0f)
        {
            m_DeathPhase.Invoke();
            IsDead = true;
        }
    }


}
