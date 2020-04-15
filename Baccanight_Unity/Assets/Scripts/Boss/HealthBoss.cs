using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class HealthBoss : MonoBehaviour
{
    #region Inspector
#pragma warning disable 0649
    [SerializeField] private HealthBarBoss m_healthBar;

    [Header("Health")]
    [Space(5)]
    [SerializeField] [Range(0, 2000)] private float m_maxHealth = 1000f;
    [SerializeField] [Range(0, 2000)] private float m_currentHealth = 1000f;
    [SerializeField] [Range(0f, 1f)] private float m_ratioEnraging = 0.5f;
    [SerializeField] [Range(0f, 1f)] private float m_ratioThirdPhase = 0.2f;

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

    public float CurrentHealth { get => m_currentHealth; private set => m_currentHealth = value; }
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

        if (lastRatio >= m_ratioEnraging && Ratio < m_ratioEnraging && !m_isEnraging)
        {
            StartCoroutine(InvincibleFrame());
            m_isEnraging = true;
            m_FirstSwitchPhase.Invoke(BossActionType.Enraging);
        }
        else if (lastRatio >= m_ratioThirdPhase && Ratio < m_ratioThirdPhase)
        {
            m_UpgradeSpeedBetweenTwoAttacks.Invoke();
            m_SecondSwitchPhase.Invoke();
        }
        else if (Ratio <= 0f)
        {
            StartCoroutine(_Death(transform.parent.gameObject));
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

    private IEnumerator _Death(GameObject boss)
    {
        m_healthBar.enabled = false;
        gameObject.transform.SetParent(null);
        Destroy(boss);
        yield return new WaitForSecondsRealtime(3f);
        SceneManager.LoadScene(0, LoadSceneMode.Single);
        Destroy(gameObject, 3f);
    }
}
