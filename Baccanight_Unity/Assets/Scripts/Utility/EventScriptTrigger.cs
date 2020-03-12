using UnityEngine;


/// <summary>
/// Handle trigger detection for invoke events in response.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class EventScriptTrigger : MonoBehaviour
{
	#region Inspector
#pragma warning disable 0649

	[SerializeField]
	private GameObjectEvent OnEnterDetect;

	[SerializeField]
	private GameObjectEvent OnStayDetect;

	[SerializeField]
	private GameObjectEvent OnExitDetect;

	[SerializeField]
	private LayerMask Layers;

#pragma warning restore 0649
	#endregion

	private void Start()
	{
		GetComponent<Collider2D>().isTrigger = true;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (((1 << other.gameObject.layer) & Layers) != 0)
		{
			OnEnterDetect.Invoke(other.gameObject);
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (((1 << other.gameObject.layer) & Layers) != 0)
		{
			OnExitDetect.Invoke(other.gameObject);
		}
	}
}

