using UnityEngine;

[CreateAssetMenu(fileName = "PlayerMotion", menuName = "AssetProject/PlayerMotion")]
public class PlayerMotion : ScriptableObject
{
	#region Inspector

	[SerializeField] private Vector2 m_motion = Vector2.zero;
	[SerializeField] private bool m_isGrounded = false;
    [SerializeField] private bool m_isRoofed = false;
    [SerializeField] private bool m_canJump;
    [SerializeField] private bool m_pushObject;
    [SerializeField] private bool m_flipSprite; // 0 = gauche; 1 = droite
    [SerializeField] private bool m_useWings;

	#endregion

	#region Getters / Setters

	public Vector2 Motion { get => m_motion; set => m_motion = value; }
	public bool IsGrounded { get => m_isGrounded; set => m_isGrounded = value; }
    public bool IsRoofed { get => m_isRoofed; set => m_isRoofed = value; }
    public bool CanJump { get => m_canJump; set => m_canJump = value; }
    public bool IsPushObject { get => m_pushObject; set => m_pushObject = value; }
    public bool FlipSprite { get => m_flipSprite; set => m_flipSprite = value; }
    public bool UseWings { get => m_useWings; set => m_useWings = value; }

	#endregion

	public void Reset()
	{
		m_motion = Vector2.zero;
		m_isGrounded = false;
        m_isRoofed = false;
        m_pushObject = false;
        m_flipSprite = true;
        m_useWings = false;
	}
}
