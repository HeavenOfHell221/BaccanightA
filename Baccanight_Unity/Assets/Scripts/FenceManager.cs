using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FenceManager : MonoBehaviour
{
    public GameObject m_fence ;
    public GameObject[] m_piedestalList;
    private Piedestal[] m_piedestalScriptList;
    private Fence m_fenceScript;

    void Start()
    {
        m_piedestalScriptList = new Piedestal[m_piedestalList.Length];
        for (int i = 0; i< m_piedestalList.Length; i++)
        {
            m_piedestalScriptList[i] = m_piedestalList[i].GetComponent<Piedestal>();
        }
        m_fenceScript = m_fence.GetComponent<Fence>();
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
            m_fenceScript.OpenFence();
        }
    }
}
