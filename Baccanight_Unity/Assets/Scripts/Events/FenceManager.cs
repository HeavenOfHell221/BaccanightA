using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FenceManager : MonoBehaviour
{
    [SerializeField] [Range(0, 2)] private int m_whichLevelIsThis;

    public GameObject m_fence ;
    public GameObject[] m_piedestalList;
    private Piedestal[] m_piedestalScriptList;
    private Fence m_fenceScript;
    public PlayerSucces m_openGrilleSuccess;

    void Start()
    {
        m_piedestalScriptList = new Piedestal[m_piedestalList.Length];
        for (int i = 0; i< m_piedestalList.Length; i++)
        {
            m_piedestalScriptList[i] = m_piedestalList[i].GetComponent<Piedestal>();
        }
        m_fenceScript = m_fence.GetComponent<Fence>();
        
        if (m_openGrilleSuccess.HaveSucces("Grille2" + m_whichLevelIsThis))
        {
            m_fenceScript.OpenFence();
        }
    }

    public void CheckForFence()
    {
        int cmp = 0;
        for(int i = 0; i< m_piedestalList.Length; i++)
        {
            if (m_piedestalScriptList[i].IsUnlocked())
            {
                cmp++;
            }
        }
        if(cmp == m_piedestalList.Length)
        {
            if (!m_openGrilleSuccess.HaveSucces("Grille2" + m_whichLevelIsThis))
            {
                m_openGrilleSuccess.SetSucces("Grille2" + m_whichLevelIsThis, true);
                m_fenceScript.OpenFence();
            }
        }
    }
}
