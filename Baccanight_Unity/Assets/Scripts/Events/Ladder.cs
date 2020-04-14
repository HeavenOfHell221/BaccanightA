using UnityEngine;

public class Ladder : MonoBehaviour
{
    private float m_gravityScale;
    private PlayerMovementControllerAir m_playerMovementControllerAir;
    private PlayerMovementControllerGround m_playerMovementControllerGround;
    private Rigidbody2D m_rigidbody2D;
    [SerializeField] private Collider2D m_collider;
    [SerializeField] private PlayerMotion m_playerMotion;

    public void OnPlayerEnter(GameObject player)
    {
        if (!m_playerMotion.UseWings)
        {
            m_rigidbody2D = player.GetComponent<Rigidbody2D>();
            m_playerMovementControllerAir = player.GetComponent<PlayerMovementControllerAir>();
            m_playerMovementControllerGround = player.GetComponent<PlayerMovementControllerGround>();

            m_gravityScale = m_rigidbody2D.gravityScale;
            m_rigidbody2D.gravityScale = 0;
            m_playerMovementControllerAir.enabled = true;
            m_playerMovementControllerGround.enabled = false;
        }

    }

    public void OnPlayerExit(GameObject player)
    {
        if (!m_playerMotion.UseWings)
        {
            m_rigidbody2D.gravityScale = m_gravityScale;
            m_rigidbody2D.velocity = new Vector2(m_rigidbody2D.velocity.x, 0f);
            m_playerMovementControllerAir.enabled = false;
            m_playerMovementControllerGround.enabled = true;
        }
    }
}
