using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class ConfinerLayout : MonoBehaviour
{
    #region Variables
    private static GameObject m_totalConfiner;
    private static GameObject m_currentConfiner;
    #endregion

    private void Start()
    {
        StartCoroutine(GetTotalConfiner());
    }

    IEnumerator GetTotalConfiner()
	{
		while (m_totalConfiner == null)
		{
			m_totalConfiner = GameObject.Find("TotalConfiner");
			yield return null;
		}

        if(m_currentConfiner)
        {
            Destroy(m_currentConfiner);
        }

		gameObject.transform.SetParent(m_totalConfiner.transform);
        m_currentConfiner = gameObject;
	}

    
}
