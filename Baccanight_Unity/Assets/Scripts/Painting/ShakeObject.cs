using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ShakeObject : MonoBehaviour
{
    [SerializeField] [Range(0f, 0.5f)] private float m_duration = 0.2f;
    [SerializeField] [Range(0f, 2f)] private float m_amplitude = 1f;
    [SerializeField] [Range(0f, 2f)] private float m_frenquency = 1f;
    [SerializeField] private CinemachineVirtualCamera vcam = null;

    private CinemachineBasicMultiChannelPerlin noise;
    private bool m_shakeTrigger = false;
    private bool m_shakeCurrently = false;

    private void Start()
    {
        noise = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    [ContextMenu("Shake")]
    public void Shake()
    {
        m_shakeTrigger = true;

        if (!m_shakeCurrently)
        {
            StartCoroutine(_ProcessShake());
        }
    }

    private IEnumerator _ProcessShake()
    {
        m_shakeTrigger = false;
        m_shakeCurrently = true;

        Noise(m_amplitude, m_frenquency);

        yield return new WaitForSeconds(m_duration);

        if(m_shakeTrigger)
        {
            StartCoroutine(_ProcessShake());     
        }
        else
        {
            Noise(0, 0);
            m_shakeCurrently = false;
        }     
    }

    private void Noise(float amplitudeGain, float frequencyGain)
    {
        noise.m_AmplitudeGain = amplitudeGain;
        noise.m_FrequencyGain = frequencyGain;
    }
}
