    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveAfterTime : MonoBehaviour
{
    public float m_timeBeforeSetActive = 1f;
    public bool m_active = false;

    private void OnEnable()
    {
        Invoke("SetActiveObject", m_timeBeforeSetActive);
    }

    public void SetActiveObject()
    {
        gameObject.SetActive(m_active);
    }
}
