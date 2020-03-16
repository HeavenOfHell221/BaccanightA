using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetrieveAllFragments : MonoBehaviour
{
    [SerializeField]
    private PlayerSucces m_succesFragment;

    [SerializeField]
    private PlayerSucces m_succesFinishLevel;

    [SerializeField]
    private PlayerSucces m_succesOpenLevel;

    [SerializeField]
    private string m_levelIDFinish;

    [SerializeField]
    private string m_LevelIDOpenDoor;

    private FragmentRetrieve[] m_fragments;

    private void Start()
    {
        m_fragments = FindObjectsOfType<FragmentRetrieve>();
        StartCoroutine(RetrieveFragments());
    }

    private IEnumerator RetrieveFragments()
    {
        bool test = true;

        foreach(var fragment in m_fragments)
        {
            if(!m_succesFragment.HaveSucces(fragment.FramentID))
            {
                test = false;
            }

        }

        if(test)
        {
            m_succesFinishLevel.SetSucces(m_levelIDFinish, true);
            m_succesOpenLevel.SetSucces(m_LevelIDOpenDoor, true);
        }

        yield return new WaitForSeconds(0.5f);
        StartCoroutine(RetrieveFragments());
    }
}
