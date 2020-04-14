using System.Collections;
using TMPro;
using UnityEngine;

public class PaintingDialog : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_textDialog;

    [SerializeField]
    private SpriteRenderer m_bubbleSpriteDialog;

    [SerializeField]
    private TextMeshProUGUI m_textName;

    [SerializeField]
    private SpriteRenderer m_bubbleSpriteName;

    [SerializeField]
    private float m_deltaAlphaBubble = 0.05f;

    [SerializeField]
    private float m_deltaAlphaText = 0.03f;

    private Color m_color;

    private void Start()
    {
        m_color = m_textDialog.color;
        m_color.a = 0;
        m_textDialog.color = m_color;

        m_color = m_bubbleSpriteDialog.color;
        m_color.a = 0;
        m_bubbleSpriteDialog.color = m_color;

        m_color = m_textName.color;
        m_color.a = 0;
        m_textName.color = m_color;

        m_color = m_bubbleSpriteName.color;
        m_color.a = 0;
        m_bubbleSpriteName.color = m_color;
    }

    public void EnterPlayer()
    {
        StopAllCoroutines();
        StartCoroutine(AddAlpha());
    }

    public void ExitPlayer()
    {
        StopAllCoroutines();
        StartCoroutine(RemoveAlpha());
    }

    private IEnumerator AddAlpha()
    {
        while (m_textDialog.color.a < 1 || m_bubbleSpriteDialog.color.a < 1
            || m_textName.color.a < 1 || m_bubbleSpriteName.color.a < 1)
        {
            m_color = m_textDialog.color;
            m_color.a = Mathf.Clamp(m_color.a += m_deltaAlphaText, 0f, 1f);
            m_textDialog.color = m_color;

            m_color = m_bubbleSpriteDialog.color;
            m_color.a = Mathf.Clamp(m_color.a += m_deltaAlphaBubble, 0f, 1f);
            m_bubbleSpriteDialog.color = m_color;

            m_color = m_textName.color;
            m_color.a = Mathf.Clamp(m_color.a += m_deltaAlphaText, 0f, 1f);
            m_textName.color = m_color;

            m_color = m_bubbleSpriteName.color;
            m_color.a = Mathf.Clamp(m_color.a += m_deltaAlphaBubble, 0f, 1f);
            m_bubbleSpriteName.color = m_color;
            yield return null;
        }
    }

    private IEnumerator RemoveAlpha()
    {
        while (m_textDialog.color.a > 0 || m_bubbleSpriteDialog.color.a > 0
            || m_textName.color.a > 0 || m_bubbleSpriteName.color.a > 0)
        {
            m_color = m_textDialog.color;
            m_color.a = Mathf.Clamp(m_color.a -= m_deltaAlphaBubble, 0f, 1f);
            m_textDialog.color = m_color;

            m_color = m_bubbleSpriteDialog.color;
            m_color.a = Mathf.Clamp(m_color.a -= m_deltaAlphaText, 0f, 1f);
            m_bubbleSpriteDialog.color = m_color;

            m_color = m_textName.color;
            m_color.a = Mathf.Clamp(m_color.a -= m_deltaAlphaBubble, 0f, 1f);
            m_textName.color = m_color;

            m_color = m_bubbleSpriteName.color;
            m_color.a = Mathf.Clamp(m_color.a -= m_deltaAlphaText, 0f, 1f);
            m_bubbleSpriteName.color = m_color;
            yield return null;
        }
    }
}
