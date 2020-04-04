using UnityEngine;

/// <summary>
/// Handle the movements of the player.
/// </summary>
public class PlayerMovementControllerGround : MovementControllerGround
{
	#region Inspector
#pragma warning disable 0649

	[SerializeField]
	private PlayerMotion m_PlayerMotion;

    public PlayerMotion Motion { get => m_PlayerMotion; set => m_PlayerMotion = value; }

#pragma warning restore 0649
    #endregion

    override protected void FixedUpdate()
	{
		base.FixedUpdate();

        if (m_PlayerMotion)
        {
            m_PlayerMotion.Motion = Move;
            m_PlayerMotion.IsGrounded = IsGrounded;
            m_PlayerMotion.IsRoofed = IsRoofed;
        }

		m_move = Vector2.zero;
	}

    public override void OnJump()
    {
        if(m_PlayerMotion.CanJump)
        {
            base.OnJump();
        }
    }
}
