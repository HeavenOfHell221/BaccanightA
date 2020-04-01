using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    #region Inspector
#pragma warning disable 0649
    [SerializeField]
    private Vector2 m_arrowSpeed;

    [SerializeField]
    private Rigidbody2D m_rigidbody;

    [SerializeField]
    private PlayerMotion m_playerMotion;
#pragma warning restore 0649
    #endregion

    private Vector2 m_speed;

    private void Start()
    {
        ResetTransform();
        Flip();
        ApplySpeed();
    }

    private void OnEnable()
    {
        ResetTransform();
        Flip();
        ApplySpeed();
    }

    private void Flip()
    {
        if (m_playerMotion.FlipSprite == -1)
        {
            m_speed.x *= -1;
            gameObject.transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void ApplySpeed()
    {
        m_speed.y = Random.Range(-0.25f, 0.25f);

        m_rigidbody.velocity = m_speed;

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

    private void ResetTransform()
    {
        m_speed = m_arrowSpeed;
        gameObject.transform.localScale = new Vector3(1, 1, 1);
        gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
    }
}
