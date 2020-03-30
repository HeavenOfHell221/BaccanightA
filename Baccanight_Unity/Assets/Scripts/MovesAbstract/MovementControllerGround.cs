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
	protected LayerMask m_whatIsGround;

    [SerializeField]
    private int m_jumpSteps = 20;

    [SerializeField]
    private int m_jumpThreshold = 7;

    [SerializeField]
    [Min(0)]
    private float m_maxFallSpeed = 30f;

    [SerializeField]
    [Min(0)]
    private float m_maxJumpSpeed = 10f;

#pragma warning restore 0649
	#endregion

	#region Variables

	protected bool m_jumpTrigger = false;
	protected bool m_isGrounded;
	protected bool m_isGroundedLeft;
	protected bool m_isGroundedRight;
    protected bool m_hasJump = false;
    protected int m_jumpNumberCounter;
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
        if (IsGrounded)
        {
            m_jumpTrigger = true;
        }
    }

	virtual protected void FixedUpdate()
	{
		if (m_canMove)
		{
			ApplyMovement();
            ApplyJump();
		}
       
	}

    virtual protected void Update()
    {
        CheckGround();

        if(!Input.GetButton(GameConstants.k_Jump) && m_jumpTrigger)
        {
            if(m_jumpNumberCounter < m_jumpSteps && m_jumpNumberCounter > m_jumpThreshold)
            {
                StopJumpSlow();
            }
            else
            {
                StopJumpQuick();
            }
        }
    }

    private void StopJumpQuick()
    {
        m_jumpNumberCounter = 0;
        m_jumpTrigger = false;
        MyRigidbody.velocity = new Vector2(MyRigidbody.velocity.x, 0f);
    }

    private void StopJumpSlow()
    {
        m_jumpNumberCounter = 0;
        m_jumpTrigger = false;
    }

    public void ApplyJump()
	{
        /*if(m_jumpTrigger && IsGrounded && !m_hasJump)
        {
            m_jumpNumberCounter = m_jumpNumber;
            m_hasJump = true;
            m_jumpTrigger = false;
            MyRigidbody.AddForce(Vector2.up * m_jumpForceGround, ForceMode2D.Impulse);
        }

        if (Input.GetButtonUp(GameConstants.k_Jump))
        {
            m_hasJump = false;
        }

        if(Input.GetButton(GameConstants.k_Jump) && m_hasJump)
        {
            if(m_jumpNumberCounter > 0)
            {
                MyRigidbody.AddForce(Vector2.up * m_jumpForceAir, ForceMode2D.Force);
                m_jumpNumberCounter -= 1;
            }
            else
            {
                m_hasJump = false;
            }
        }
        */
        
        if(m_jumpTrigger)
        {
            if(m_jumpNumberCounter < m_jumpSteps)
            {
                MyRigidbody.velocity = new Vector2(MyRigidbody.velocity.x, m_jumpForce);
                m_jumpNumberCounter++;
            }
            else
            {
                StopJumpSlow();
            }
        }
    }

	protected bool CheckGround()
	{
        IsGrounded = true;
        m_isGroundedLeft = CheckGround(m_groundCheckLeft.position);
        m_isGroundedRight = CheckGround(m_groundCheckRight.position);

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
		}

        return IsGrounded;
	}

	protected bool CheckGround(Vector3 groundCheckPosition)
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

	override protected void ApplyMovement()
	{
		if (Mathf.Abs(Move.x) > GameConstants.LimitDicretePosition)
		{
            float velocityX = Mathf.Lerp(MyRigidbody.velocity.x, Move.x * m_speed, m_smoothSpeed);
            MyRigidbody.velocity = new Vector2(velocityX, 
                MyRigidbody.velocity.y < -m_maxFallSpeed ? -m_maxFallSpeed : 
                MyRigidbody.velocity.y > m_maxJumpSpeed ? m_maxJumpSpeed : MyRigidbody.velocity.y);
        }
		else
		{
            MyRigidbody.velocity = new Vector2(0f,
                MyRigidbody.velocity.y < -m_maxFallSpeed ? -m_maxFallSpeed :
                MyRigidbody.velocity.y > m_maxJumpSpeed ? m_maxJumpSpeed : MyRigidbody.velocity.y);
        }
	}
}
