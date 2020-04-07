using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Arrow : MonoBehaviour
{
    #region Inspector
#pragma warning disable 0649
    [Header("Arrow")]
    [Space(5)]
    [SerializeField] protected Rigidbody2D m_rigidbody;
    [SerializeField] private float m_arrowSpeedX;
    [SerializeField] private PlayerMotion m_playerMotion;
    [SerializeField] private float m_angleMaxY;
    [SerializeField] [Range(0f, -10f)] protected float m_damage;

#pragma warning restore 0649
    #endregion

    private Vector2 m_speed;

    private void Start()
    {
        ApplySpeed();
    }

    private void OnEnable()
    {
        ApplySpeed();
    }

    private void Flip()
    {
        if (!m_playerMotion.FlipSprite)
        {
            m_speed.x *= -1;
            transform.rotation = new Quaternion(
            transform.rotation.x,
            180f,
            transform.rotation.z,
            transform.rotation.w);
        }
    }

    private void ApplySpeed()
    {
        m_speed = new Vector2(m_arrowSpeedX, Random.Range(-m_angleMaxY, m_angleMaxY));
        m_rigidbody.velocity = m_speed;

        float angle = Mathf.Atan2(m_rigidbody.velocity.y, m_rigidbody.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        StartCoroutine(Disable(10f));
    }

    protected IEnumerator Disable(float time)
    {
        yield return new WaitForSeconds(time);
        m_rigidbody.velocity = Vector2.zero;
        gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
        Flip();
        gameObject.SetActive(false);
        StopAllCoroutines();
    }
}
