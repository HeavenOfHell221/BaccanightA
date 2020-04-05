using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainFireball : MonoBehaviour
{
    #region Inpsector
#pragma warning disable 0649


    [Header("Attributes")]
    [Space(5)]

    [SerializeField] private float m_speed = 3f;
    [SerializeField] private Rigidbody2D m_rigidbody;
    [SerializeField] private float m_timeBeforeMove;

#pragma warning restore 0649
    #endregion

    #region Variables
    #endregion

    private void Start()
    {
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        yield return new WaitForSeconds(0.1f);

        Vector3 direction = Vector3.down;
        m_rigidbody.AddForce(direction * m_speed, ForceMode2D.Impulse);

        yield return new WaitForSeconds(m_timeBeforeMove);
    }

    public void OnEnterPlayer()
    {
        Destroy(gameObject);
    }

    public void SetSpeed(float sp)
    {
        m_speed = sp;
    }
}
