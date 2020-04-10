using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionFireball : Fireball
{
    [Header("Range Direction X")]
    [Space(5)]
    [SerializeField] [Range(0f, 3f)] private float m_MinDirectionX;
    [SerializeField] [Range(0f, 3f)] private float m_MaxDirectionX;

    [Header("Range Direction Y")]
    [Space(5)]
    [SerializeField] [Range(0f, 3f)] private float m_MinDirectionY;
    [SerializeField] [Range(0f, 3f)] private float m_MaxDirectionY;

    private bool m_explosionLeft = true;

    protected override void Start()
    {
        GameObject player = PlayerManager.Instance.PlayerReference;
        if(player.transform.position.x > transform.position.x)
        {
            m_explosionLeft = false;
        }
        Move();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        GameObject player = PlayerManager.Instance.PlayerReference;
        if (player.transform.position.x > transform.position.x)
        {
            m_explosionLeft = false;
        }
        Move();
    }

    protected override void Move()
    {
        Rigidbody.velocity = Vector2.zero;
        float directionX;
        float directionY = Random.Range(m_MinDirectionY, m_MaxDirectionY);

        if (!m_explosionLeft)
        {
            directionX = Random.Range(m_MinDirectionX, m_MaxDirectionX);
        }
        else
        {
            directionX = Random.Range(m_MaxDirectionX * -1, m_MinDirectionX * -1);
        }

       
        Vector3 direction = new Vector3(directionX, directionY, 0f);
        Rigidbody.AddForce(direction * Speed, ForceMode2D.Impulse);
    }

    private void Update()
    {
        float angle = Mathf.Atan2(Rigidbody.velocity.y, Rigidbody.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
