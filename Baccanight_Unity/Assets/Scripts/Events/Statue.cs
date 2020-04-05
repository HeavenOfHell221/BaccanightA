using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Statue : MonoBehaviour
{
    [SerializeField] private PlayerMotion m_playerMotion;
    [SerializeField] private Transform m_groundCheckLeft;
    [SerializeField] private Transform m_groundCheckRight;
    [SerializeField] private LayerMask m_whatIsGround;
    [SerializeField] private Rigidbody2D m_rigidbody2D;
    [SerializeField] private float m_maxSpeedY;

    public void OnEnterPlayer()
    {
        m_playerMotion.IsPushObject = true;
    }

    public void OnExitPlayer()
    {
        m_playerMotion.IsPushObject = false;
    }

    private void FixedUpdate()
    {
        if(m_rigidbody2D.velocity.y > m_maxSpeedY)
        {
            m_rigidbody2D.velocity = new Vector2(m_rigidbody2D.velocity.x, m_maxSpeedY);
        }

        if(!CheckGround())
        {
            m_rigidbody2D.velocity = new Vector2(0f, m_rigidbody2D.velocity.y);
        }
    }

    protected bool CheckGround()
    {
        if (CheckCollider(m_groundCheckLeft.position) || CheckCollider(m_groundCheckRight.position))
        {
            return true;
        }
        return false;
    }

    protected bool CheckCollider(Vector3 groundCheckPosition)
    {
        //calcul de hitbox avec le sol
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckPosition, 0.1f, m_whatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                return true;
            }
        }
        return false;
    }
}
