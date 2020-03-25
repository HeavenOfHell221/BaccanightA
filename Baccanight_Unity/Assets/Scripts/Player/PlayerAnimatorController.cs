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

	public Rigidbody2D playerRigidbody;

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
		m_Animator.SetFloat("SpeedOny", playerRigidbody.velocity.y);
		m_Animator.SetFloat("SpeedOnx", Mathf.Abs(m_playerMotion.Motion.x));
        m_Animator.SetBool("IsPushObject", m_playerMotion.IsPushObject);

		if (m_playerMotion.Motion.x > 0.1f)
		{
			sprite.flipX = false;
            PlayerManager.Instance.IsPlayerLookHeadIsLeft = true;
		}

		else if (m_playerMotion.Motion.x < -0.1f)
		{
			sprite.flipX = true;
            PlayerManager.Instance.IsPlayerLookHeadIsLeft = false;
        }
	}
}
