using UnityEngine;

[CreateAssetMenu(fileName = "PlayerMotion", menuName = "AssetProject/PlayerMotion")]
public class PlayerMotion : ScriptableObject
{
	#region Inspector

	[SerializeField]
	private Vector2 m_motion = Vector2.zero;

	[SerializeField]
	private bool m_isGrounded = false;

    [SerializeField]
    private bool m_canJump;

	#endregion

	#region Getters / Setters

	public Vector2 Motion { get => m_motion; set => m_motion = value; }
	public bool IsGrounded { get => m_isGrounded; set => m_isGrounded = value; }
    public bool CanJump { get => m_canJump; set => m_canJump = value; }


	#endregion

	public void Reset()
	{
		m_motion = Vector2.zero;
		m_isGrounded = false;
	}
}
