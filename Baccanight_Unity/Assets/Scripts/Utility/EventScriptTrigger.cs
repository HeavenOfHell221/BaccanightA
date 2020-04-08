using UnityEngine;


/// <summary>
/// Handle trigger detection for invoke events in response.
/// </summary>
public class EventScriptTrigger : MonoBehaviour
{
	#region Inspector
#pragma warning disable 0649

	[SerializeField] private GameObjectEvent OnEnterDetect;
    [SerializeField] private GameObjectEvent OnStayDetect;
    [SerializeField] private GameObjectEvent OnExitDetect;
    [SerializeField] private LayerMask Layers;
#pragma warning restore 0649
	#endregion

	private void Start()
	{
        Collider2D[] cols = GetComponents<Collider2D>();
        foreach(var c in cols)
        {
            c.isTrigger = true;
        }
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (((1 << other.gameObject.layer) & Layers) != 0)
		{
			OnEnterDetect.Invoke(other.gameObject);
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (((1 << other.gameObject.layer) & Layers) != 0)
		{
			OnExitDetect.Invoke(other.gameObject);
		}
	}

    private void OnTriggerStay2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & Layers) != 0)
        {
            OnStayDetect.Invoke(other.gameObject);
        }
    }
}

