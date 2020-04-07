using System.Collections;
using UnityEngine;

/// <summary>
/// Handle the animations of the player.
/// </summary>
[RequireComponent(typeof(Animator))]
public class PlayerAnimatorController : MonoBehaviour
{

	#region Inspector
#pragma warning disable 0649

	[SerializeField]
	private PlayerMotion m_playerMotion;

    [SerializeField]
	public Rigidbody2D m_playerRigidbody;

    [SerializeField]
    private GameObject m_player;

#pragma warning restore 0649
	#endregion

	#region Variables

	SpriteRenderer sprite;
	private Animator m_Animator;
	#endregion

	void Start()
	{
		m_Animator = GetComponent<Animator>();
		sprite = GetComponent<SpriteRenderer>();
	}

	void Update()
	{
		UpdateAnimator();
	}

	/// <summary>
	/// Update the animator parameters
	/// </summ  
	public void UpdateAnimator()
	{
		m_Animator.SetBool("OnGround", m_playerMotion.IsGrounded);
		m_Animator.SetFloat("SpeedOny", m_playerRigidbody.velocity.y);
		m_Animator.SetFloat("SpeedOnx", Mathf.Abs(m_playerMotion.Motion.x));
        m_Animator.SetBool("IsPushObject", m_playerMotion.IsPushObject);
        m_Animator.SetBool("UseWings", m_playerMotion.UseWings);

        

        GameObject boss = GameObject.FindGameObjectWithTag("Boss");

        if (boss)
        {
            if(boss.transform.position.x < transform.position.x)
            {
                m_player.transform.rotation = new Quaternion(
                   m_player.transform.rotation.x,
                   180f,
                   m_player.transform.rotation.z,
                   m_player.transform.rotation.w);
                m_playerMotion.FlipSprite = false;
            }
            else if(boss.transform.position.x > transform.position.x)
            {
                m_player.transform.rotation = new Quaternion(
                   m_player.transform.rotation.x,
                   0f,
                   m_player.transform.rotation.z,
                   m_player.transform.rotation.w);
                m_playerMotion.FlipSprite = true;
            }
        }
        else
        {
            bool lastFlip = m_playerMotion.FlipSprite;
            m_playerMotion.FlipSprite = m_playerMotion.Motion.x > 0.1f ? true : m_playerMotion.Motion.x < -0.1f ? false : m_playerMotion.FlipSprite;

            if (lastFlip != m_playerMotion.FlipSprite)
            {
                Flip();
            }
        }
	}

    private void Flip()
    {
        m_player.transform.rotation = new Quaternion(
            m_player.transform.rotation.x,
            m_player.transform.rotation.y == 0f ? 180f : 0f,
            m_player.transform.rotation.z,
            m_player.transform.rotation.w);
    }
}
