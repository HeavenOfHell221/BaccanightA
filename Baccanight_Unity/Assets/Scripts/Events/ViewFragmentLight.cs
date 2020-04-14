using UnityEngine;

public class ViewFragmentLight : MonoBehaviour
{
    [SerializeField] private PlayerSucces m_playerFragment;
    [SerializeField] private PlayerSucces m_playerOpenLevel;
    [SerializeField] private SpriteRenderer m_doorLevel_1;
    [SerializeField] private SpriteRenderer m_doorLevel_2;

    [SerializeField] private Sprite m_light_00;
    [SerializeField] private Sprite m_light_01;
    [SerializeField] private Sprite m_light_10;
    [SerializeField] private Sprite m_light_11;

    [SerializeField] private GameObject m_fragment_1;
    [SerializeField] private GameObject m_fragment_2;
    [SerializeField] private GameObject m_fragment_3;
    [SerializeField] private GameObject m_fragment_4;

    private void Start()
    {
        m_fragment_1.SetActive(false);
        m_fragment_2.SetActive(false);
        m_fragment_3.SetActive(false);
        m_fragment_4.SetActive(false);

        if (m_playerFragment.HaveSucces("Fragment 1") && m_playerFragment.HaveSucces("Fragment 2"))
        {
            m_doorLevel_1.sprite = m_light_11;
            m_playerOpenLevel.SetSucces("Level 2.0", true);
            m_fragment_2.SetActive(true);
            m_fragment_1.SetActive(true);
        }
        else if (m_playerFragment.HaveSucces("Fragment 1") && !m_playerFragment.HaveSucces("Fragment 2"))
        {
            m_doorLevel_1.sprite = m_light_10;
            m_fragment_1.SetActive(true);
        }
        else if (!m_playerFragment.HaveSucces("Fragment 1") && m_playerFragment.HaveSucces("Fragment 2"))
        {
            m_doorLevel_1.sprite = m_light_01;
            m_fragment_2.SetActive(true);
        }
        else
        {
            m_doorLevel_1.sprite = m_light_00;
        }


        if (m_playerFragment.HaveSucces("Fragment 3") && m_playerFragment.HaveSucces("Fragment 4"))
        {
            m_doorLevel_2.sprite = m_light_11;
            m_fragment_3.SetActive(true);
            m_fragment_4.SetActive(true);
        }
        else if (m_playerFragment.HaveSucces("Fragment 3") && !m_playerFragment.HaveSucces("Fragment 4"))
        {
            m_doorLevel_2.sprite = m_light_10;
            m_fragment_3.SetActive(true);
        }
        else if (!m_playerFragment.HaveSucces("Fragment 3") && m_playerFragment.HaveSucces("Fragment 4"))
        {
            m_doorLevel_2.sprite = m_light_01;
            m_fragment_4.SetActive(true);
        }
        else
        {
            m_doorLevel_2.sprite = m_light_00;
        }

        if (m_playerFragment.HaveAllSucces())
        {
            m_playerOpenLevel.SetSucces("Level 3.0", true);
        }
    }
}
