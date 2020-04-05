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
    protected Transform m_roofCheckLeft;

    [SerializeField]
    protected Transform m_roofCheckRight;

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
    protected bool m_isRoofed;
    protected bool m_isRoofedLeft;
    protected bool m_isRoofedRight;
    protected bool m_hasJump = false;
    protected int m_jumpNumberCounter;
	#endregion

	#region Getters / Setters

	public bool IsGrounded { get => m_isGrounded; set => m_isGrounded = value; }
    public bool IsRoofed { get => m_isRoofed; set => m_isRoofed = value; }

    #endregion

    public override void OnMove(Vector2 motion)
    {
        Move = new Vector2(motion.x, Move.y);
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

        if (!Input.GetButton(GameConstants.k_Jump) && m_jumpTrigger)
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
        Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, 0f);
    }

    private void StopJumpSlow()
    {
        m_jumpNumberCounter = 0;
        m_jumpTrigger = false;
    }

    public void ApplyJump()
	{        
        if(m_jumpTrigger)
        {
            if(m_jumpNumberCounter < m_jumpSteps && CheckRoof())
            {
                Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, m_jumpForce);
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
        m_isGroundedLeft = CheckCollider(m_groundCheckLeft.position);
        m_isGroundedRight = CheckCollider(m_groundCheckRight.position);

        // Si aucun des deux pieds est au sol, alors on ne touche pas le sol
        if (!m_isGroundedLeft && !m_isGroundedRight)
        {
            IsGrounded = false;
        }

        // Changement du physicMaterial
        if (!IsGrounded)
		{
			Rigidbody.sharedMaterial = m_AirPhysicMaterial;
		}
		else
		{
			Rigidbody.sharedMaterial = m_GroundPhysicMaterial;
		}

        return IsGrounded;
	}

    protected bool CheckRoof()
    {
        IsRoofed = false;
        m_isRoofedLeft = CheckCollider(m_roofCheckLeft.position);
        m_isRoofedRight = CheckCollider(m_roofCheckRight.position);

        // Si aucun des deux pieds est au sol, alors on ne touche pas le sol
        if (!m_isRoofedLeft || !m_isRoofedRight)
        {
            IsRoofed = true;
        }

        return IsRoofed;
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

	override protected void ApplyMovement()
	{
		if (Mathf.Abs(Move.x) > GameConstants.LimitDicretePosition)
		{
            float velocityX = Mathf.Lerp(Rigidbody.velocity.x, Move.x * m_speed, m_smoothSpeed);
            Rigidbody.velocity = new Vector2(velocityX, 
                Rigidbody.velocity.y < -m_maxFallSpeed ? -m_maxFallSpeed : 
                Rigidbody.velocity.y > m_maxJumpSpeed ? m_maxJumpSpeed : Rigidbody.velocity.y);
        }
		else
		{
            Rigidbody.velocity = new Vector2(0f,
                Rigidbody.velocity.y < -m_maxFallSpeed ? -m_maxFallSpeed :
                Rigidbody.velocity.y > m_maxJumpSpeed ? m_maxJumpSpeed : Rigidbody.velocity.y);
        }
	}
}
