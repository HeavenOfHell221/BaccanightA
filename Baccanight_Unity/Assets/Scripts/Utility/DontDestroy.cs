using UnityEngine;

/// <summary>
/// Simple script for persistent objects.
/// </summary>
public class DontDestroy : MonoBehaviour
{
	public bool Dont_Destroy;
	// Use this for initialization
	void Start()
	{
		if (Dont_Destroy)
		{
            gameObject.transform.SetParent(null);
			DontDestroyOnLoad(gameObject);
		}
	}

}
