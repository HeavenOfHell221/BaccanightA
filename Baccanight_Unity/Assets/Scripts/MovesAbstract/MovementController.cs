using UnityEngine;


/// <summary>
/// Handle the basic movements of a character.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public abstract class MovementController : MonoBehaviour
{
	#region Inspector
#pragma warning disable 0649

	[Header("Movement Controller basic", order = 0)]

	[Range(0f, 20f)]
	[SerializeField]
	private float m_speed = 10f;

    [Range(.1f, 1f)]
	[SerializeField]
	protected float m_smoothSpeed;

#pragma warning restore 0649
	#endregion

	#region Variables

	protected Vector2 m_move;
	protected bool m_canMove = true;
	protected Vector2 m_destination;

	#endregion

	#region Getters / Setters
	public Rigidbody2D Rigidbody { get; protected set; }
	public Vector2 Move { get => m_move; set => m_move = value; }
    public float Speed { get => m_speed; set => m_speed = value; }
	#endregion

	private void Awake()
	{
		Rigidbody = GetComponent<Rigidbody2D>();
	}

	public void ActivateMovement(bool state)
	{
		m_canMove = state;
		Rigidbody.velocity = Vector2.zero;
	}

    abstract public void OnMove(Vector2 motion);

	abstract public void ApplyMovement();
}
