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
            //Debug.Log("Statue");
            obj.transform.position = transform.position + Vector3.up;
            if (obj == m_statueExpected)
            {
                m_isUnlocked = true;
                m_fenceManager.GetComponent<FenceManager>().CheckForFence();

                
            }
            obj.layer = LayerMask.NameToLayer("Props"); // Pour que le joueur le traverse et ne puisse pas le repousser
            obj.GetComponentInChildren<EventScriptTrigger>().enabled = false; // Pour que l'animation du joueur qui pousse ne s'active pas
        }
    }

    public bool IsUnlocked()
    {
        return m_isUnlocked;
    }
}
