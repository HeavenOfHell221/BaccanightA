using Cinemachine;
using System.Collections;
using UnityEngine;

public class ShakeCamera : MonoBehaviour
{
    private CinemachineVirtualCamera m_VirtualCamera;
    private CinemachineBasicMultiChannelPerlin m_noise;

    private bool m_shakeTrigger = false;
    private float m_currentDuration;
    private float m_currentAmplitude;
    private float m_currentFrequency;

    public bool IsShake { get; private set; } = false;

    private void Start()
    {
        m_VirtualCamera = GetComponent<CinemachineVirtualCamera>();
        m_noise = m_VirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        m_noise.ReSeed();
    }

    public void Shake(float amplitude = 1f, float frequency = 1f, float duration = 0.2f)
    {
        m_shakeTrigger = true;

        m_currentAmplitude = amplitude;
        m_currentFrequency = frequency;
        m_currentDuration = duration;

        if (!IsShake)
        {
            StartCoroutine(_ProcessShake(amplitude, frequency, duration));
        }
    }

    private IEnumerator _ProcessShake(float amplitude, float frequency, float duration)
    {
        m_shakeTrigger = false;
        IsShake = true;

        while (!m_noise)
        {
            m_VirtualCamera = GetComponent<CinemachineVirtualCamera>();
            if (m_VirtualCamera)
            {
                m_noise = m_VirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            }

            yield return null;
        }

        Noise(amplitude, frequency);

        yield return new WaitForSecondsRealtime(duration);

        if (m_shakeTrigger)
        {
            StartCoroutine(_ProcessShake(m_currentAmplitude, m_currentFrequency, m_currentDuration));
        }
        else
        {
            StopAllCoroutines();
            Noise(0f, 0f);
            IsShake = false;
        }
    }

    private void Noise(float amplitudeGain, float frequencyGain)
    {
        m_noise.m_AmplitudeGain = amplitudeGain;
        m_noise.m_FrequencyGain = frequencyGain;
    }
}
