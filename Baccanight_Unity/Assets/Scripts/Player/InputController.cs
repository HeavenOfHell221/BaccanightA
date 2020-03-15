using UnityEngine;
using UnityEngine.Events;


/// <summary>
/// Handle the input of the players.
/// </summary>
public class InputController : MonoBehaviour
{

	#region Inspector
#pragma warning disable 0649

	[SerializeField]
	private PlayerState m_playerState;

	#region Gameplay Events

	private float m_MoveThreshold = .2f;

	[SerializeField]
	private FloatEvent m_OnMoveHorizontal;

	[SerializeField]
	private UnityEvent m_OnJump;

	[SerializeField]
	private UnityEvent m_OnInteract;

	[SerializeField]
	private UnityEvent m_OnAttackEnter;

	[SerializeField]
	private UnityEvent m_OnAttackExit;

	[SerializeField]
	private UnityEvent m_OnEscape;

	#endregion

	#region Menu Events

	[SerializeField]
	private UnityEvent m_OnCancel;

	[SerializeField]
	private UnityEvent m_OnSubmit;

	#endregion

#pragma warning restore 0649
	#endregion

	#region Getters / Setters

	public FloatEvent OnMoveHorizontal => m_OnMoveHorizontal;
	public UnityEvent OnJump => m_OnJump;
	public UnityEvent OnEscape => m_OnCancel;
	public UnityEvent OnAttackEnter => m_OnAttackEnter;
	public UnityEvent OnAttackExit => m_OnAttackExit;
	public UnityEvent OnSubmit => m_OnSubmit;
	public UnityEvent OnCancel => m_OnCancel;
    public UnityEvent OnInteract => m_OnInteract;
	public static bool HaveGamePad { get; set; }

	#endregion

	public void InitialyseGamePad()
	{
		if (Input.GetJoystickNames().Length > 0)
		{
			foreach (string joystick in Input.GetJoystickNames())
			{
				if (joystick.Length > 0)
				{
					HaveGamePad = true;
					return;
				}
			}
		}
		HaveGamePad = false;
	}

	private void Start()
	{
		InitialyseGamePad();
        PlayerManager.Instance.PlayerReference = gameObject;
        PlayerManager.Instance.PlayerinputController = this;
	}

	void Update()
	{
		switch (m_playerState.State)
		{
			case GameState.inGame:
				GetEscapeDown();
				GetInteractDown();
				GetAttackDown();
				GetAttackUp();
				GetMotion();
				GetJumpDown();
				break;
			case GameState.inMainMenu:
				GetCancelDown();
				GetSubmitDown();
				break;
			default:
				break;
		}
	}

	private void GetMotion()
	{
		float rawMotion = Mathf.Clamp(Input.GetAxisRaw(GameConstants.k_AxisHorizontal), -1, 1);
		//if (m_MoveThreshold < Mathf.Abs(rawMotion))
		//{
			m_OnMoveHorizontal.Invoke(rawMotion);
		//}
	}

	private void GetJumpDown()
	{
		if (Input.GetButton(GameConstants.k_Jump))
		{
			m_OnJump.Invoke();
		}
	}

	private void GetInteractDown()
	{
		if (Input.GetButtonDown(GameConstants.k_Interact))
		{
			m_OnInteract.Invoke();
		}
	}

	private void GetAttackDown()
	{
		if (Input.GetButtonDown(GameConstants.k_Attack))
		{
			m_OnAttackEnter.Invoke();
		}
	}

	private void GetAttackUp()
	{
		if (Input.GetButtonUp(GameConstants.k_Attack))
		{
			m_OnAttackExit.Invoke();
		}
	}
	private void GetEscapeDown()
	{
		if (Input.GetButtonDown(GameConstants.k_Cancel))
		{
			m_OnEscape.Invoke();
		}
	}

	private void GetCancelDown()
	{
		if (Input.GetButtonDown(GameConstants.k_Cancel))
		{
			m_OnCancel.Invoke();
		}
	}

	private void GetSubmitDown()
	{
		if (Input.GetButtonDown(GameConstants.k_Submit))
		{
			m_OnSubmit.Invoke();
		}
	}
}

