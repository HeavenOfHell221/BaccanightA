using System.Collections;
using UnityEngine;

public class ConfinerLayout : MonoBehaviour
{
	static GameObject totalConfiner;

	void Start()
	{
		StartCoroutine(GetTotalConfiner());
	}

	IEnumerator GetTotalConfiner()
	{
		while (totalConfiner == null)
		{
			totalConfiner = GameObject.Find("TotalConfiner");
			yield return null;
		}
		gameObject.transform.SetParent(totalConfiner.transform);
	}
}
