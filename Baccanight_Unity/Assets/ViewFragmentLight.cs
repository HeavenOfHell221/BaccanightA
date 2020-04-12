using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewFragmentLight : MonoBehaviour
{
    [SerializeField] private PlayerSucces m_playerFragment;
    [SerializeField] private SpriteRenderer m_doorLevel_1;
    [SerializeField] private SpriteRenderer m_doorLevel_2;

    [SerializeField] private Sprite m_light_00;
    [SerializeField] private Sprite m_light_01;
    [SerializeField] private Sprite m_light_10;
    [SerializeField] private Sprite m_light_11;

    private void Start()
    {
        if(m_playerFragment.HaveSucces("Fragment 1") && m_playerFragment.HaveSucces("Fragment 2"))
        {
            m_doorLevel_1.sprite = m_light_11;
        }
        else if(m_playerFragment.HaveSucces("Fragment 1") && !m_playerFragment.HaveSucces("Fragment 2"))
        {
            m_doorLevel_1.sprite = m_light_10;
        }
        else if(!m_playerFragment.HaveSucces("Fragment 1") && m_playerFragment.HaveSucces("Fragment 2"))
        {
            m_doorLevel_1.sprite = m_light_01;
        }
        else
        {
            m_doorLevel_1.sprite = m_light_00;
        }


        if (m_playerFragment.HaveSucces("Fragment 3") && m_playerFragment.HaveSucces("Fragment 4"))
        {
            m_doorLevel_2.sprite = m_light_11;
        }
        else if (m_playerFragment.HaveSucces("Fragment 3") && !m_playerFragment.HaveSucces("Fragment 4"))
        {
            m_doorLevel_2.sprite = m_light_10;
        }
        else if (!m_playerFragment.HaveSucces("Fragment 3") && m_playerFragment.HaveSucces("Fragment 4"))
        {
            m_doorLevel_2.sprite = m_light_01;
        }
        else
        {
            m_doorLevel_2.sprite = m_light_00;
        }
    }
}
