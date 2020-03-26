using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{
    #region Inspector

    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float m_timeLerp;

    [SerializeField]
    [Min(0.1f)]
    private float m_timeWaitBeforeNext = 0.1f;

    [SerializeField]
    [Range(0f, 360f)]
    private float m_angleMax = 45f;

    [SerializeField]
    [Range(0.01f, 10f)]
    private float m_deltaAngleBeforeNext = 0.1f;
    #endregion

    private float m_lastRotationZ = 0f;

    private void Start()
    {
        StartCoroutine(NextRotate());
    }

    private IEnumerator NextRotate()
    {
        float angle = Random.Range(-m_angleMax, m_angleMax);
        yield return StartCoroutine(RotateObject(angle));
        StartCoroutine(NextRotate());
    }

    public IEnumerator RotateObject(float angleZ)
    {
        float actualRotation = m_lastRotationZ;

        while (Mathf.Abs(actualRotation - angleZ) > m_deltaAngleBeforeNext)
        {
            actualRotation = Mathf.Lerp(actualRotation, angleZ, m_timeLerp);
            Quaternion rotation = Quaternion.Euler(0f, 0f, actualRotation);
            transform.localRotation = rotation;
            m_lastRotationZ = actualRotation;
            yield return null;
        }
        yield return new WaitForSeconds(m_timeWaitBeforeNext);
    }
}
