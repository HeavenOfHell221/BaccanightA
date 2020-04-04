using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    #region Inpsector
#pragma warning disable 0649

    
    [Header("Attributes")]
    [Space(5)]

    [SerializeField] private float m_speed = 3f;
    [SerializeField] private Rigidbody2D m_rigidbody;
    [SerializeField] private float m_timeBeforeMove;

    [Header("Move Back")]
    [Space(5)]

    [SerializeField] private Transform m_backward;
    [SerializeField] private float m_DecreaseVelocityPercentage = 0.97f;
    [SerializeField] private float m_forceBack;
    [SerializeField] private float m_timeBack;
#pragma warning restore 0649
    #endregion

    #region Variables
    private Transform m_playerPosition;
    private Vector3 m_destination;
    private bool m_lookPlayer = true;
    #endregion

    private void Start()
    {
        m_playerPosition = PlayerManager.Instance.PlayerReference.transform;
        //m_playerPosition = GameObject.FindGameObjectWithTag("Player").transform;
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
        yield return new WaitForSeconds(0.1f);

        Vector3 direction = (m_backward.position - transform.position).normalized;
        m_rigidbody.AddForce(direction * m_forceBack, ForceMode2D.Impulse);
        float counter = m_timeBack;

        while(counter > 0f)
        {
            counter -= Time.fixedDeltaTime;
            m_rigidbody.velocity *= m_DecreaseVelocityPercentage;
            if(counter < m_timeBack / 3f)
            {
                m_lookPlayer = false;
            }
            yield return null;
        }

        
        yield return new WaitForSeconds(m_timeBeforeMove);
        Move();
    }

    private void Move()
    {
        Vector3 direction = (m_destination - transform.position).normalized;
        m_rigidbody.velocity = direction * m_speed;
    }

    public void OnEnterPlayer()
    {
        Destroy(gameObject);
    }
}
