using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarBoss : MonoBehaviour
{
    [SerializeField] private Image m_redBar;
    [SerializeField] private Image m_yellowBar;
    [SerializeField] private float m_redDeltaFillAmount;
    [SerializeField] private float m_YellowDeltaFillAmount;
    [SerializeField] private float m_timeBeforeUpdateYellowBar;

    private float m_actualRatio = 1f;
    private float m_lastUpdateLife = 0f;

    private bool m_updateYellowBar = false;
    private float m_redBarrFillAmountBeforeUpdateYellowBar;

    // Start is called before the first frame update
    void Awake()
    {
        GetComponentInParent<HealthBoss>().OnHealthPctChanged += HandleHealthChanged;
        m_redBar.fillAmount = 1f;
        m_yellowBar.fillAmount = 1f;
    }

    private void Update()
    {
        m_redBar.fillAmount = Mathf.Lerp(m_redBar.fillAmount, m_actualRatio , m_redDeltaFillAmount * Time.deltaTime);

        if(m_lastUpdateLife + m_timeBeforeUpdateYellowBar < Time.time)
        {
            m_updateYellowBar = true;
            m_redBarrFillAmountBeforeUpdateYellowBar = m_redBar.fillAmount;
        }

        if(m_updateYellowBar)
        {
            m_yellowBar.fillAmount = Mathf.Lerp(m_yellowBar.fillAmount, m_actualRatio, m_redDeltaFillAmount * Time.deltaTime);
            if(m_yellowBar.fillAmount <= m_redBarrFillAmountBeforeUpdateYellowBar)
            {
                m_updateYellowBar = false;
            }
        }

        if(m_yellowBar.fillAmount < m_redBar.fillAmount)
        {
            m_yellowBar.fillAmount = m_redBar.fillAmount;
        }
    }

    private void HandleHealthChanged(float ratio)
    {
        /* StopAllCoroutines();

         StartCoroutine(ChangeToPct(pct));*/

        m_actualRatio = ratio;
        m_lastUpdateLife = Time.time;
    }

    private IEnumerator ChangeToPct(float pct)
    {
        m_redBar.fillAmount = pct;

        yield return new WaitForSeconds(0.5f);
        m_yellowBar.fillAmount = pct;

        yield return null;
    }
}
