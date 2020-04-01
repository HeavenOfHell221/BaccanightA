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

        m_playerMotion.FlipSprite = m_playerMotion.Motion.x > 0.1f ? 1 : m_playerMotion.Motion.x < -0.1f ? -1 : m_playerMotion.FlipSprite;

        switch(m_playerMotion.FlipSprite)
        {
            case -1: m_player.transform.rotation = new Quaternion(0f, -180f, 0f, m_player.transform.rotation.w); break;
            case 1: m_player.transform.rotation = new Quaternion(0f, 0f, 0f, m_player.transform.rotation.w); break;
            default: break;
        }
	}
}
