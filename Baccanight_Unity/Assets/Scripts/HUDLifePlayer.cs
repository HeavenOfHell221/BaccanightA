using UnityEngine;
using UnityEngine.UI;

public class HUDLifePlayer : MonoBehaviour
{
    #region Inspector
#pragma warning disable 0649

    [SerializeField]
    private PlayerHealth m_playerHealth;

    [SerializeField]
    private Image[] m_uiLifeItems;

    #endregion

    private void Start()
    {
        UpdateLife();
    }

    [ContextMenu("Update Life")]
    public void UpdateLife()
    {
        int life = m_playerHealth.CurrentHealth;
        int i;
        for (i = 0; i < life && i < m_uiLifeItems.Length; i++)
        {
            m_uiLifeItems[i].enabled = true;
        }
        for (; i < m_uiLifeItems.Length; i++)
        {
            m_uiLifeItems[i].enabled = false;
        }
    }
}

