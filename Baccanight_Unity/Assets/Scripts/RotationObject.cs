using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationObject : MonoBehaviour
{
    #region Inspector
    [Header("Shake rotation")]
    [SerializeField]
    private bool m_useRandomAngle = false;

    [SerializeField]
    [Range(0.01f, 1.0f)]
    private float m_timeLerp;

    [SerializeField]
    [Min(0.01f)]
    private float m_timeWaitBeforeNext = 0.1f;

    [SerializeField]
    [Range(0f, 360f)]
    private float m_angleMax = 45f;

    [SerializeField]
    [Range(0.01f, 20f)]
    private float m_deltaAngleBeforeNext = 0.1f;

    [Header("Normal rotation")]
    [SerializeField]
    [Range(-6f, 6f)]
    private float m_speedRotation = 1f;
    #endregion

    private float m_lastRotationZ = 0f;

    private void Start()
    {
        if (m_useRandomAngle)
        {
            StartCoroutine(NextRandomRotation());
        }
        else
        {
            StartCoroutine(NextRotation());
        }
    }

    private IEnumerator NextRandomRotation()
    {
        float angle = Random.Range(-m_angleMax, m_angleMax);
        yield return StartCoroutine(RotateObjectAtAngle(angle));
        StartCoroutine(NextRandomRotation());
    }

    private IEnumerator NextRotation()
    {
        yield return StartCoroutine(RotateObject());
        StartCoroutine(NextRotation());
    }

    private IEnumerator RotateObjectAtAngle(float angleZ)
    {
        float actualRotation = m_lastRotationZ;
        Quaternion rotation;

        while (Mathf.Abs(actualRotation - angleZ) > m_deltaAngleBeforeNext)
        {
            actualRotation = Mathf.Lerp(actualRotation, angleZ, m_timeLerp);
            rotation = Quaternion.Euler(0f, 0f, actualRotation);
            transform.localRotation = rotation;
            m_lastRotationZ = actualRotation;
            yield return null;
        }
        yield return new WaitForSeconds(m_timeWaitBeforeNext);
    }

    private IEnumerator RotateObject()
    {
        float actualRotation;
        Quaternion rotation;

        while (true)
        {
            actualRotation = (m_lastRotationZ + m_speedRotation) % 360f;
            rotation = Quaternion.Euler(0f, 0f, actualRotation);
            transform.localRotation = rotation;
            m_lastRotationZ = actualRotation;
            yield return null;
        }
    }
}
