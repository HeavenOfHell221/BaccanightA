using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeObject : MonoBehaviour
{
    #region Inspector
    [SerializeField]
    private float m_maxValue = 1f;

    [SerializeField]
    private float m_speedLerp = 0.05f;

    [SerializeField]
    private float m_timeWaitBeforeStartCoroutine = 0.1f;

    [SerializeField]
    private float m_distanceMinBeforeNext = 0.1f;
    #endregion

    private void Start()
    {
        StartCoroutine(Shake());
    }

    private IEnumerator Shake()
    {
        Vector3 originalPos = transform.localPosition;
        float x = Random.Range(-m_maxValue, m_maxValue);
        float y = Random.Range(-m_maxValue, m_maxValue);
        Vector3 destinationPos = new Vector3(x, y, 0f);
        float t = 0.0f;

        while (Vector3.Distance(transform.localPosition, destinationPos) > m_distanceMinBeforeNext)
        { 
            transform.localPosition = Vector3.Lerp(originalPos, destinationPos, t);
            t += m_speedLerp * Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(m_timeWaitBeforeStartCoroutine);

        StartCoroutine(Shake());
    }
        
}
