using UnityEngine;


/// <summary>
/// Handle the basic movements of a character.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public abstract class MovementController : MonoBehaviour
{
	#region Inspector
#pragma warning disable 0649

	[Header("Movement Controller", order = 0)]

	[Range(0f, 20f)]
	[SerializeField]
	protected float m_speed = 10f;

    [SerializeField]
    protected float m_jumpForce;

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
	public Rigidbody2D MyRigidbody { get; protected set; }
	public Vector2 Move { get => m_move; protected set => m_move = value; }
	#endregion

	private void Awake()
	{
		MyRigidbody = GetComponent<Rigidbody2D>();
	}

	public void ActivateMovement(bool state)
	{
		m_canMove = state;
		MyRigidbody.velocity = Vector2.zero;
	}

	public void DestinationToMove(Vector2 destination)
	{
		m_destination = destination;
		m_move = (destination - new Vector2(MyRigidbody.transform.position.x, MyRigidbody.transform.position.y));
		if (m_move.magnitude > 1f)
		{
			m_move.Normalize();
		}
	}

	public void OnMove(Vector2 motion)
	{
		Move = motion.normalized;
	}

	abstract protected void ApplyMovement();
}
