using UnityEngine;

public class PlayerGetWings : MonoBehaviour
{
    [SerializeField] private PlayerMotion m_playerMotion;
    [SerializeField] private GameObject m_Wings;
    [SerializeField] private GameObject m_Bow;

    public void Awake()
    {
        if(m_playerMotion.UseWings)
        {
            m_Wings.SetActive(false);
            m_Bow.SetActive(false);
        }
    }

    public void OnPlayerEnter(GameObject player)
    {
        m_playerMotion.UseWings = true;
        m_Wings.SetActive(false);
        m_Bow.SetActive(false);
    }
}
