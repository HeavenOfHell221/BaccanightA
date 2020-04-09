using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piedestal : MonoBehaviour
{
    public GameObject m_statueExpected;
    public GameObject m_fenceManager;
    private bool m_isUnlocked = false;
    public void CollisionEnter(GameObject obj)
    {
        if(obj.tag == "Statue")
        {
            //Debug.Log("Statue !");
            if (obj == m_statueExpected)
            {
                m_isUnlocked = true;
                m_fenceManager.GetComponent<FenceManager>().CheckForFence();
            }
        }
    }

    public bool IsUnlocked()
    {
        return m_isUnlocked;
    }
}
