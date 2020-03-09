using UnityEngine;


/// <summary>
/// Handle the basic movements of a grounded character.
/// </summary>
public abstract class MovementControllerGround : MovementController
{
    #region Inspector
#pragma warning disable 0649

    [Header("Movement Controller Ground", order = 1)]

	[SerializeField]
	private PhysicsMaterial2D m_GroundPhysicMaterial;

	[SerializeField]
	private PhysicsMaterial2D m_AirPhysicMaterial;

	[SerializeField]
	protected Transform m_groundCheckLeft;

	[SerializeField]
	protected Transform m_groundCheckRight;

	[SerializeField]
	protected LayerMask whatIsGround;

    [SerializeField]
    private float m_jumpCooldown;

#pragma warning restore 0649
	#endregion

	#region Variables

	protected bool m_jumpTrigger = false;
	protected Vector2 m_aimedVelocity;

	protected bool m_isGrounded;
	protected bool m_isGroundedLeft;
	protected bool m_isGroundedRight;
    protected float m_jumpTimer;
    protected bool m_hasJump = false;
	#endregion

	#region Getters / Setters

	public bool IsGrounded { get => m_isGrounded; set => m_isGrounded = value; }

	#endregion

	public void OnMove(float motion)
	{
		m_move.x = motion;
	}

	public virtual void OnJump()
	{
        if (Time.time > m_jumpTimer + m_jumpCooldown)
        {
            if(!m_hasJump)
                m_jumpTrigger = true;
        }
	}

	virtual protected void FixedUpdate()
	{
		CheckGround();
		if (m_canMove)
		{
			ApplyMovement();
			if (m_jumpTrigger && IsGrounded && !m_hasJump)
			{
				ApplyJump();
			}
		}
	}

	public void ApplyJump()
	{
        MyRigidbody.AddForce(Vector2.up * m_jumpForce, ForceMode2D.Impulse);
  		IsGrounded = false;
        m_jumpTrigger = false;
        m_hasJump = true;
    }

	protected void CheckGround()
	{
        IsGrounded = true;
        CheckGroundLeft();
        CheckGroundRight();

        // Si aucun des deux pieds est au sol, alors on ne touche pas le sol
        if (!m_isGroundedLeft && !m_isGroundedRight)
        {
            IsGrounded = false;
        }

        // Changement du physicMaterial
        if (!IsGrounded)
		{
			MyRigidbody.sharedMaterial = m_AirPhysicMaterial;
		}
		else
		{
			MyRigidbody.sharedMaterial = m_GroundPhysicMaterial;
            if(m_hasJump)
            {
                m_hasJump = false;
                m_jumpTimer = Time.time;
            }
		}
	}

	protected void CheckGroundLeft()
	{
		m_isGroundedLeft = false;
		//calcul de hitbox avec le sol
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_groundCheckLeft.position, .05f, whatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				m_isGroundedLeft = true;
				break;
			}
		}
	}
	protected void CheckGroundRight()
	{
		m_isGroundedRight = false;
		//calcul de hitbox avec le sol
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_groundCheckRight.position, .05f, whatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				m_isGroundedRight = true;
				break;
			}
		}
	}

	override protected void ApplyMovement()
	{
		if (Mathf.Abs(Move.x) > GameConstants.LimitDicretePosition)
		{
			m_aimedVelocity = MyRigidbody.velocity;
			m_aimedVelocity.x = Mathf.Lerp(m_aimedVelocity.x, Move.x * m_speed, m_smoothSpeed);
			MyRigidbody.velocity = m_aimedVelocity;
            //Debug.Log(m_aimedVelocity);
		}
		else
		{
			m_aimedVelocity = MyRigidbody.velocity;
			m_aimedVelocity.x = 0f;
			MyRigidbody.velocity = m_aimedVelocity;
		}
	}
}
