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
        // Lerp de la barre rouge
        m_redBar.fillAmount = Mathf.Lerp(m_redBar.fillAmount, m_actualRatio, m_redDeltaFillAmount * Time.unscaledDeltaTime);

        // Si le joueur n'a pas touché le boss pendant un temps
        // On commence à update la barre jaune
        if (m_lastUpdateLife + m_timeBeforeUpdateYellowBar < Time.unscaledTime)
        {
            m_updateYellowBar = true;
            m_redBarrFillAmountBeforeUpdateYellowBar = m_redBar.fillAmount;
        }

        // Si on doit update la barre jaune      
        if (m_updateYellowBar)
        {
            // On le lerp
            m_yellowBar.fillAmount = Mathf.Lerp(m_yellowBar.fillAmount, m_actualRatio, m_redDeltaFillAmount * Time.unscaledDeltaTime);

            // Si la barre jaune a atteint la barre rouge, on arrête de la baisser
            if (m_yellowBar.fillAmount <= m_redBarrFillAmountBeforeUpdateYellowBar)
            {
                m_updateYellowBar = false;
            }
        }

        // Si on a trop baisser la barre jaune, on la remonte jusqu'a la barre rouge
        // Ou si le boss régénère des points de vie
        if (m_yellowBar.fillAmount < m_redBar.fillAmount)
        {
            m_yellowBar.fillAmount = m_redBar.fillAmount;
        }
    }

    private void HandleHealthChanged(float ratio)
    {
        m_actualRatio = ratio;
        m_lastUpdateLife = Time.unscaledTime;
    }
}
