using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField]
    private Vector2 m_arrowSpeed;

    [SerializeField]
    private Rigidbody2D m_rigidbody;

    [SerializeField]
    private PlayerMotion m_playerMotion;

    private void OnEnable()
    {
        Flip();
        ApplySpeed();
    }

    private void Flip()
    {
        if (m_playerMotion.FlipSprite == -1)
        {
            m_arrowSpeed.x *= -1;
            gameObject.transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void ApplySpeed()
    {
        m_arrowSpeed.y = Random.Range(-0.5f, 0.5f);

        m_rigidbody.velocity = m_arrowSpeed;

        float angle = Mathf.Atan2(m_rigidbody.velocity.y, m_rigidbody.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        StartCoroutine(Disable(10f));
    }

    public void OnEnter(GameObject target)
    {
        m_rigidbody.velocity = Vector2.zero;
        StartCoroutine(Disable(2f));
    }

    private IEnumerator Disable(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
        StopAllCoroutines();
    }
}
