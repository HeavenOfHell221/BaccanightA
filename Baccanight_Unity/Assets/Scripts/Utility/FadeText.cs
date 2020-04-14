using System.Collections;
using TMPro;
using UnityEngine;

public class FadeText : MonoBehaviour
{
    [SerializeField]
    private Collider2D m_collider;

    [SerializeField]
    private LayerMask Layers;

    [SerializeField]
    private TextMeshProUGUI m_textMeshPro;

    [SerializeField]
    private float m_fadeSpeed = 0.05f;

    private void Start()
    {
        m_collider.isTrigger = true;
        FadeOut(true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (enabled)
        {
            if (((1 << other.gameObject.layer) & Layers) != 0)
            {
                FadeIn();
            }
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (enabled)
        {
            if (((1 << other.gameObject.layer) & Layers) != 0)
            {
                FadeOut();
            }
        }
    }

    public void FadeIn()
    {
        StopAllCoroutines();
        StartCoroutine(_FadeIn());
    }

    public void FadeOut(bool force = false)
    {
        StopAllCoroutines();
        StartCoroutine(_FadeOut(force));
    }

    private IEnumerator _FadeIn()
    {
        while (m_textMeshPro.color.a < 1f)
        {
            Color color = m_textMeshPro.color;
            color.a = Mathf.Clamp(color.a += m_fadeSpeed, 0f, 1f);
            m_textMeshPro.color = color;
            yield return null;
        }
    }

    private IEnumerator _FadeOut(bool force)
    {
        if (force)
        {
            Color color = m_textMeshPro.color;
            color.a = 0f;
            m_textMeshPro.color = color;
        }

        while (m_textMeshPro.color.a > 0f && !force)
        {
            Color color = m_textMeshPro.color;
            color.a = Mathf.Clamp(color.a -= m_fadeSpeed, 0f, 1f);
            m_textMeshPro.color = color;
            yield return null;
        }
    }
}
