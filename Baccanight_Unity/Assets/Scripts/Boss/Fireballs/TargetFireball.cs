using System.Collections;
using UnityEngine;

public class TargetFireball : Fireball
{
    #region Inpsector
#pragma warning disable 0649
    [Header("Move Back", order = 1)]
    [Space(5)]
    [SerializeField] private Transform m_backward;
    [SerializeField] [Range(0.9f, 0.97f)] private float m_DecreaseVelocityPercentage = 0.96f;
    [SerializeField] [Range(5f, 15f)] private float m_forceBack = 10f;
    [SerializeField] [Range(0.4f, 0.8f)] private float m_timeBack = 0.5f;
#pragma warning restore 0649
    #endregion

    #region Variables
    private Transform m_playerPosition;
    private Vector3 m_destination;
    private bool m_lookPlayer = true;
    #endregion

    protected override void Start()
    {
        m_playerPosition = PlayerManager.Instance.PlayerReference.transform;
        StartCoroutine(MoveBack());
        StartCoroutine(LookPlayer());
    }

    private IEnumerator LookPlayer()
    {
        yield return null;
        m_destination = m_playerPosition.position;
        Vector3 direction = transform.position - m_destination;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        if (m_lookPlayer)
        {
            StartCoroutine(LookPlayer());
        }
    }

    private IEnumerator MoveBack()
    {
        yield return new WaitForSecondsRealtime(0.1f);

        Vector3 direction = (m_backward.position - transform.position).normalized;
        Rigidbody.AddForce(direction * m_forceBack, ForceMode2D.Impulse);
        float counter = m_timeBack;

        while (counter > 0f)
        {
            counter -= Time.fixedDeltaTime;
            Rigidbody.velocity *= m_DecreaseVelocityPercentage;
            yield return null;
        }
        m_lookPlayer = false;
        yield return new WaitForSecondsRealtime(m_timeBeforeMove);
        Move();
    }

    protected override void Move()
    {
        Vector3 direction = (m_destination - transform.position).normalized;
        Rigidbody.velocity = direction * Speed;
    }
}
