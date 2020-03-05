using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
	public float m_timeBeforeDestroy = 1f;

	private void Start()
	{
		Destroy(gameObject, m_timeBeforeDestroy);
	}
}
