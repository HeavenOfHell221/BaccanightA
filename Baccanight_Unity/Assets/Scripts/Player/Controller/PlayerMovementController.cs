using UnityEngine;

/// <summary>
/// Handle the movements of the player.
/// </summary>
public class PlayerMovementController : MovementControllerGround
{
	#region Inspector
#pragma warning disable 0649

	[SerializeField]
	private PlayerMotion m_PlayerMotion;

#pragma warning restore 0649
	#endregion

	#region Variables
	#endregion

	override protected void FixedUpdate()
	{
		base.FixedUpdate();
		
		m_PlayerMotion.Motion = Move;
		m_PlayerMotion.IsGrounded = IsGrounded;
        m_PlayerMotion.IsRoofed = IsRoofed;

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
