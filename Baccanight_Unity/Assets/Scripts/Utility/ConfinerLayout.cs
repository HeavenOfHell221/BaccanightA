using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class ConfinerLayout : MonoBehaviour
{

    #region Inspector
    [SerializeField]
    private int m_ID = -1;
    #endregion

    #region Variables
    private static GameObject m_totalConfiner;
    private static Dictionary<int, bool> m_IDDict = new Dictionary<int, bool>(); 
    #endregion


    void Start()
	{
        if(m_ID == -1)
        {
            Debug.LogError("ConfinerLayout ID ERROR. (-1)");
        }
        else
        {
            if(m_IDDict.ContainsKey(m_ID))
            {
                Destroy(gameObject);
            }
            else
            {
                m_IDDict.Add(m_ID, true);
                StartCoroutine(GetTotalConfiner());
            }            
        }
	}

	IEnumerator GetTotalConfiner()
	{
		while (m_totalConfiner == null)
		{
			m_totalConfiner = GameObject.Find("TotalConfiner");
			yield return null;
		}
		gameObject.transform.SetParent(m_totalConfiner.transform);
	}

    
}
